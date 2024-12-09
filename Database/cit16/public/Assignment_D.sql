/*

GROUP: cit16, MEMBERS: Andreas Moosdorf, Dagmar Ree, Eray Erkul 

*/




/*D.1. Basic framework functionality: 
Consider what is needed to support the framework anddevelop functions for that. 
You will need functions for managing users and for bookmarking names and titles. 
You could also consider developing functions for adding notes to titles and names and for retrieving bookmarks as well as search history and rating history for users.*/


/*
drop table title_akas;
drop table title_basics;
drop table title_count;
drop table title_crew;
drop table title_episode;
drop table title_principals;
drop table title_ratings;
drop table omdb_data;
drop table name_basics;*/


/*
WEBPAGE creation
*/
do $$
begin
  if (select count(*) from webpage) = 0 then

    insert into webpage (wp_id, t_id)
    select concat('wp', t_id), t_id from title;
    
    insert into webpage (wp_id, p_id)
    select concat('wp', p_id), p_id from person;
  end if;
end;
$$ language plpgsql;



/* Views */

drop view if exists person_title_webpages;

create view person_title_webpages as (
  select lower(name) as name_title, wp_id, url, null as plot from webpage join person using(p_id)
  union
  select lower(title) as name_title, wp_id, url, lower(plot) from webpage join title using(t_id)
  order by wp_id
 );
 
drop view if exists person_rated;
drop view if exists title_cast;
create view title_cast as (
    select p_id, name, t_id, title, character, rating 
    from person_involved_title natural join title natural join person
    where character is not null
);

drop view person_rated;
create view person_rated as 
    (with temp_test as (
    select distinct name, title, rating
    from title_cast)
        select name, round(sum(rating) / count(title), 2) as rating
        from temp_test
        group by name
        order by rating desc
);

select * from person_rated;





--------------------------------------------------------------------------------
-- sign up procedure 
drop procedure if exists signup;

create procedure signup(in user_id int, in username varchar(20), in password varchar(256), in email varchar(50), in salt varchar)
LANGUAGE plpgsql
as $$
begin
  begin 
    insert into users values(user_id, username, password, email, salt);
    raise notice 'Sign up successful';
  exception
    when others then -- catches all exceptions
      raise notice 'Cannot sign up';
  end;
end;
$$;

--------------------------------------------------------------------------------
-- sign in procedure 
drop procedure if exists login_user;

create procedure login_user(in p_username varchar, in p_password varchar, out logged_in boolean)
language plpgsql as $$
declare 
  results int;
  user_id int;
begin 
  select u_id into user_id
  from users 
  where username = p_username;
  

  select count(*) into results
  from users 
  where username = p_username and password = p_password;

  
  if results > 0 then 
    call start_session(user_id); 
    logged_in := true;
  else 
    logged_in := false;
  end if;
end;
$$;







-- get session
drop function if exists get_session;
create function get_session(p_user_id int)
returns table(
  session_id int, 
  user_id int,
  timecreated timestamp,
  timeended timestamp,
  expired varchar
)
language plpgsql as $$
begin 
  return query
    select session.session_id, u_id, session_start, session_end, expiration from session natural join user_session
    where u_id = p_user_id 
    order by session_start desc;
end;
$$;







-- start session
drop procedure if exists start_session;
create procedure start_session(in user_id int)
language plpgsql as $$
declare 
  sess_id int;
  last_session_ended timestamp;
  last_session_id int;
  
begin
  select timeended, session_id 
  into last_session_ended, last_session_id
  from get_session(user_id);
  

  if last_session_ended is null then 
    call sign_off(user_id);
  end if;

  insert into session (session_start, session_end, expiration) 
  values(default, default, default) returning session_id into sess_id;
  
  insert into user_session values (user_id, sess_id);
end;
$$;




-- sign off session
drop procedure if exists sign_off; -- end session thats it
create procedure sign_off(in user_id int)
language plpgsql as $$
declare 
  sess_id int;

  
begin
  select session_id into sess_id from get_session(1) limit 1;
  
  update session 
  set session_end = current_timestamp
  where session_id = sess_id;
end;
$$;


--------------------------------------------------------------------------------
-- procedure for inserting into wp and bookmarks

drop procedure if exists insert_bookmark;

create procedure insert_bookmark(in user_id int, in webpage_id varchar)
language plpgsql as $$
declare 
  bookmark_id varchar := concat(user_id, webpage_id);
begin
  begin
    insert into bookmark values (bookmark_id, current_timestamp);
    insert into user_bookmarks values (bookmark_id, user_id);
    insert into wp_bookmarks values (bookmark_id, webpage_id);

    exception
      when others then
        raise notice 'Error: %', sqlerrm;
        raise notice 'Already bookmarked.';
  end;
end;
$$;

------------------------------------------
-- get bookmarks

-- one function accepts varchars the other integers, the only difference.
-- ints are for users, varchar is for webpages.
drop function if exists get_bookmarks(varchar);
create function get_bookmarks(p_wp_id varchar) 
returns table (
  bookmark_id varchar,
  wp_id varchar,
  u_id int,
  created_at timestamp
)
language plpgsql as $$
begin 
  return query
    select bookmark.bookmark_id, 
           wp_bookmarks.wp_id, 
           user_bookmarks.u_id, bookmarked_at
    from bookmark natural join wp_bookmarks natural join user_bookmarks
    where p_wp_id = wp_bookmarks.wp_id;
end;
$$;


------------------
-- overloaded

drop function if exists get_bookmarks(int);
create function get_bookmarks(p_u_id int) 
returns table (
  bookmark_id varchar,
  wp_id varchar,
  u_id int,
  created_at timestamp
)
language plpgsql as $$
begin 
  return query
    select bookmark.bookmark_id, 
           wp_bookmarks.wp_id, 
           user_bookmarks.u_id, bookmarked_at
    from bookmark natural join wp_bookmarks natural join user_bookmarks
    where p_u_id = user_bookmarks.u_id;
end;
$$;



--------------------------------------------------------------------------------
-- Person known for

drop function if exists person_known_for;

create function person_known_for (search_name varchar(100))
returns table (title varchar(2000))
language plpgsql as $$

begin
		return query
				select distinct title.title from title
				natural join person_involved_title join person using (p_id)
				where person.name = search_name;
			
end;
$$;






--------------------------------------------------------------------------------

-- delete user
drop procedure if exists delete_user;
create procedure delete_user(in user_id int)
language plpgsql as $$
begin
  delete from users
  where u_id = user_id;
end;
$$;




--------------------------------------------------------------------------------

-- delete bookmark
drop procedure if exists delete_bookmark;

create procedure delete_bookmark(in user_bookmark_id varchar, in user_id int)
language plpgsql as $$
begin
  delete from bookmark
  where bookmark_id = user_bookmark_id;
end;
$$;







/*D.2. Simple search: 
Develop a simple search function for instance called string_search(). 
This function should, given a search sting S as parameter, find all movies where S is a substring of the title or a substring of the plot description. 

For the movies found return id and title (tconst and primarytitle, if you kept the attribute names from the provided dataset). 
Make sure to bring the framework into play, such that the search history is updated as a side effect of the call of the search function.*/

-- return id title. show that update history after search is already done in trigger
drop function if exists string_search;

create function string_search(search_string varchar)
returns table (
  id varchar,
  title_name varchar
)
language plpgsql as $$
declare 
  search_word varchar := concat('%', lower(search_string), '%');
begin
  return query
    select mov_id, title
    from title join movie on mov_id = title.t_id
    where lower(title) like search_word or (plot is not null and lower(plot) like search_word);
end;
$$;








------------------------------------------
-- get_user_history 

drop function if exists get_user_history;
create function get_user_history(user_id int)
returns table
(
	search_word varchar, time_searched timestamp
)
language plpgsql as 
$$
begin
	return query
	select keyword, searched_at from history natural join search
	where u_id = user_id
	order by searched_at desc;
end;
$$;



--------------------------------------------------------------------------------

-- clear_history

drop procedure if exists clear_history;
create procedure clear_history(in user_id int)
language plpgsql as $$
begin
delete from 
  search where search_id in (
                      select search_id 
                      from search natural join history
                      where u_id = 1
                      );
end;
$$;


/*
D.3. Title rating: Introduce functionality for rating by a function, called for instance rate(), that
takes a title and a rate as an integer value between 1 and 10, where 10 is best (see more
detailed interpretation at What are IMDb ratings?). 

The function should update the (average-)rating appropriately taking the new vote into consideration. Make sure to bring the
framework into play, such that the rating history is updated as a side effect of the call of
the rate function. Also make sure to treat multiple calls of rate() by the same user
consistently. This could be for instance by ignoring or blocking an attempt to rate, in case a
rating of the same movie by the same user is already registered. Alternatively, an update
with the new rate can be preceded by a “redrawing” of the previous rating, recalculating the
average rating appropriately.
*/

/* User rate */
drop procedure if exists rate;
create procedure rate(in title_id varchar(10), in user_id int, in user_rating int, in review_id int, in in_review varchar(256) default null)
language plpgsql as $$
begin

  if title_id in 
  (select t_id from rates where u_id = user_id) 
  then -- update current rating and review
    raise notice 'update current rating';
    update rates 
    set rating = user_rating, rated_at = current_timestamp
    where t_id = title_id and u_id = user_id;
    if in_review is not null then
      raise notice 'update review as well';

      update review
      set review = in_review
      where rev_id = review_id;
    end if;
    
  else -- create new rating/review
    raise notice 'creating new rating/review';

    insert into review (rev_id, review, likes)
    values (review_id, in_review, default);

    insert into rates 
    values (title_id, user_id, review_id, user_rating, current_timestamp);
    
  end if;
end;
$$;





-- liking reviews
drop procedure if exists like_review;
create procedure like_review(in user_id int, in in_rev_id int, in in_liked int)
language plpgsql as $$
begin
  if (select count(*) 
      from likes l 
      where l.u_id = user_id and l.rev_id = in_rev_id) > 0
      then 
        update likes
          set liked = in_liked
          where u_id = user_id and rev_id = in_rev_id;
          raise notice 'update like';
      else 
        raise notice 'insert like';
        insert into likes values (user_id, in_rev_id, in_liked);
      end if;
end;
$$;



-- trigger for calculating likes:
drop trigger if exists update_likes on likes;
drop function if exists calculate_likes;

create function calculate_likes() 
returns trigger as $$
declare
  in_rev_id int;
begin
    raise notice 'recalculate likes on review: %', new.rev_id;
    update review
    set likes = (
      select coalesce(sum(liked), 0)
      from likes 
      where rev_id = new.rev_id)
      where rev_id = new.rev_id;

    return new; 
end; $$
language plpgsql;

create trigger update_likes
after insert or update on likes
for each row execute procedure calculate_likes();



/* Rates trigger after insert */
-- rate trigger. when a row is inserted into rates, then we update the rating for that title.;

drop trigger if exists rate_title on rates;
drop function if exists rate_trigger;

create function rate_trigger() -- the trigger function
returns trigger as $$
begin
    raise notice 'recalculate likes on %', new.rev_id;
    update title
    set rating = (
        select avg(rating) 
        from rates 
        where rev_id = new.rev_id
        ) 
      where t_id = new.t_id;
      
    return new;
end; $$
language plpgsql;


create trigger rate_title -- the trigger (calling the trigger function)
after insert or update on rates
for each row execute procedure rate_trigger(); -- for each new row run function


--------------------------------------------------------------------------------
-- get user rating

drop function if exists get_user_rating;
create function get_user_rating(user_id int)
returns table (
    title_name varchar(2000),
    user_rating numeric(4,2),
    rating_timestamp timestamp
)
language plpgsql as $$

begin
    return query
        select title.title, rates.rating, rated_at
        from title
        join rates using (t_id)
    order by rated_at desc;
end;
$$;




/*

D.4. Structured string search: 

Develop a search function, for instance called structured_string_search(), that take 4 string parameters and return titles that match these on the 
title, the plot, the characters and the person names involved respectively. 

Make the function flexible in the sense that it don’t care about case of letters and argument values are treated as substrings (to match they should just be included in the column value in question). 

-- For the movies found, return id and title (in the source data called tconst and primarytitle). 
-- Make sure to bring the framework into play, such that the search history is stored as a side effect of the call of the search function.

*/


drop function if exists structured_string_search;
create function structured_string_search(_title varchar, _plot varchar, _characters varchar, _person varchar)
returns table(
  id varchar,
  title_name varchar
)
language plpgsql as $$
declare 
  title_search varchar := concat('%', lower(_title), '%');
  plot_search varchar := concat('%', lower(_plot), '%');
  character_search varchar := concat('%', lower(_characters), '%');
  person_search varchar := concat('%', lower(_person), '%');

begin
  return query
    select distinct t_id, title
      from person 
      natural join person_involved_title 
      natural join title
      where lower(title) like title_search
      or lower(plot) like plot_search
      or lower(character) like character_search
      or lower(name) like person_search;
end;
$$;

select * from structured_string_search('lord of the rings');


/*
D.5. Finding names: The above search functions are focused on finding titles. Try to add to these by developing one or two functions aimed at finding names (of for instance actors).
*/

drop function if exists simple_search_person(varchar);
create function simple_search_person(search_string varchar)
returns table(
  person_id varchar,
  person_name varchar
)
language plpgsql as $$
declare 
  search_word varchar := concat('%', lower(search_string), '%');

begin
  if 
  
  return query
    select distinct p_id, name
    from person_involved_title
    natural join person
    natural join title    
    where lower(title) like search_word or person."name" like search_word or (plot is not null and lower(plot) like search_word);
    
end;
$$;


select * from simple_search_person('ian mckellen');


/*
D.6. Finding co-players: Make a function that, given the name of an actor, will return a list of actors that are the most frequent co-players to the given actor. For the actors found, return their nconst, primaryname and the frequency (number of titles in which they have co-
played).

Hint: You may for this as well as for other purposes find a view helpful to make query expressions easier (to express and to read). An example of such a view could be one that collects the most important columns from title, principals and name in a single virtual table.
*/

drop function if exists find_coactors_with_skip;
create function find_coactors_with_skip(actor_id varchar, skip int,take int)
returns table (
      person_id varchar,
      co_actor varchar,
      title_name varchar,
      person_rating numeric(8,2),
      counted bigint
      
      )
language plpgsql as $$
begin
  return query
    select t1.p_id, t1.name, t1.title, t1.rating, count(distinct t2.title) 
    from title_cast t1 
    join title_cast t2 on t1.title = t2.title
    where t2.p_id = actor_id and t1.p_id <> actor_id
    group by t1.p_id, t1.name, t1.title, t1.rating
    order by count desc
    limit take offset skip;
end;
$$;

drop function if exists find_coactors;
create function find_coactors(actor_id varchar)
returns table (
      person_id varchar,
      co_actor varchar,
      title_name varchar,
      person_rating numeric(8,2),
      counted bigint
      
      )
language plpgsql as $$
begin
  return query
    select t1.p_id, t1.name, t1.title, t1.rating, count(distinct t2.title) 
    from title_cast t1 
    join title_cast t2 on t1.title = t2.title
    where t2.p_id = actor_id and t1.p_id <> actor_id
    group by t1.p_id, t1.name, t1.title, t1.rating
    order by count desc;
end;
$$;


/*
D.7. Name rating: 
Derive a rating of names (just actors or all names, as you prefer) based on ratings of the titles they are related to. 

Modify the database to store also these name ratings.

-- not made yet, will be added in next revision.
Make sure to give higher influence to titles with more votes in the calculation.
You can do this by calculating a weighted average of the averagerating for the titles, where the numvotes is used as weight.
*/

do $$
declare rating_exists boolean;

begin
select exists ( -- check if column has been made
    select 1 
    from information_schema.columns 
    where table_name = 'person' 
      and column_name = 'person_rating'
) into rating_exists;

if not rating_exists then -- if not, make it
  alter table person
  add person_rating numeric(8,3);
  
end if;
end;
$$ language plpgsql;


drop procedure if exists update_all_people_rating;
create procedure update_all_people_rating()
language plpgsql as $$
begin
  update person
  set person_rating = rating
  from person_rated
  where person.name = person_rated.name;
end;
$$;

call update_all_people_rating();

select * from person limit 1;

/*
D.8. Popular actors: Suggest and implement a function that makes use of the name ratings. One
example could be a function that takes a movie and lists the actors of the movie in order of
decreasing popularity. Another could be a similar function that takes an actor and lists the
co-players in order of decreasing popularity.
*/


-- 1) find all names' rating based on their respective rating in a title
drop function if exists name_ratings;
create function name_ratings(movie_title varchar)
returns table (person_name varchar, avg_rating numeric)
language plpgsql as $$

begin

	return query

	select distinct name, person_rating
	from person
	natural join person_involved_title 
	natural join title
	where person_rating is not null
	and title = movie_title
	order by person_rating desc;
end;
$$;


-- 2) find all co-actors rating based on their respective rating
drop function if exists co_players_rating;
create function co_players_rating(actor_id varchar)
returns table (co_players_id varchar, avg_rating numeric)
language plpgsql as $$

begin
	return query
	
	select p_id, person_rating 
	from find_coactors(actor_id)
	join person on person_id = p_id
	order by person_rating desc;

end;
$$;


/*
D.9. Similar movies: Discuss and suggest a notion of similarity among movies. Design and implement a function that, given a movie as input, will provide a list of other movies that are similar.

*/
-- Find all the movies/series that have the same genre as the input_title_id, rank by how many

drop function if exists find_similar_titles; 
create or replace function find_similar_titles(input_title_id varchar)
returns table(
  similar_title_id varchar,
  similar_title varchar,
  multiple_same_genre numeric
)
language plpgsql as $$
begin
  return query
  select distinct t_id as similar_title_id,
         title as similar_title,
         sum(case when (select count(genre) from title_is where title_is.genre = genre) > 1 then 1 else 0 end)::numeric as multiple_same_genre
  from title
  join title_is using(t_id)
  where genre in (
      select genre
      from title_is
      where t_id = input_title_id)
  and t_id <> input_title_id
  group by similar_title_id, similar_title 
  order by multiple_same_genre desc, title.t_id
  limit 15;
end;
$$;





/*
D.10. Frequent person words: 
The wi table provides an inverted index for titles using the 4 columns: 
primarytitle, plot and, character and primaryname.

So, given a title, we can from wi get a lot of words, that are somehow characteristic for the title. 

To retrieve a list of words that are characteristic for a person we can do the following: 

find the titles the person has been involved in, and find all words associated with these titles (using wi). 
To get a list of unique words, you can just group by word in an aggregation by count(). 

Thereby you'll get a list of words together with their frequencies in all titles the person has been involved in. Use this principle in a function person_words() that takes a person name as parameter and returns a list of words in decreasing frequency order, limited to fixed length (e.g. 10). 

Optionally, add a parameter to the function to set a maximum for the length of the list. You can consider the frequency to be a weight, where higher weight means more importance in the characteristics of the person.
*/

drop function if exists person_words;
create function person_words(person_name varchar, list_length int)
returns table (wi_word text, frequency bigint)
language plpgsql as $$
begin
  return query
    select word, count(word)
      from person
      natural join person_involved_title 
      natural join title
      join wi on t_id = tconst
      where name = person_name
      group by word
      order by count desc
      limit list_length;
end;
$$;
  

select * from person_words('Ian McKellen', 30);



/*
D.11. Exact-match querying: 
Introduce an exact-match querying function that takes one or more keywords as arguments and returns posts that match all of these. 

Use the inverted index wi for this purpose. You can find inspiration on how to do that in the slides on Textual Data and IR.
*/

drop function if exists exact_match;
create function exact_match(variadic keywords text[])
returns table (t_title varchar)
language plpgsql as $$

declare
  query varchar := '';
  keyword varchar;

begin
  foreach keyword in array keywords
  loop
    
    query := query || ' select title
    from title join wi on t_id = tconst
    where lower(word) like '''||keyword||'''
    intersect';

  end loop;
  if query != '' then
        query := left(query, length(query) - 9); 
  end if;
  
  query := ' select title as title from ('||query||') as foo';
  

  return query execute query;
end;
$$;






/*
D.12. Best-match querying: 
Develop a refined function similar to D.11, but now with a “best-match” ranking and ordering of objects in the answer. 

A best-match ranking simply means: the more keywords that match, the higher the rank. Titles in the answer should be ordered by decreasing rank. 

See also the Textual Data and IR slides for hints.
*/

drop function if exists best_match;
create function best_match(variadic keywords text[])
returns table (title varchar, wp_id varchar, frequency bigint)
language plpgsql as $$
declare 
query text := '';
keyword text;

begin
  if (array_length(keywords, 1) > 0 and keywords[1] <> '') then
    foreach keyword in array keywords
    loop

      query := query || ' select title, wp_id, count(title.t_id)
      from title join wi on title.t_id = tconst 
      join webpage on tconst = webpage.t_id
      where lower(word) like '''||lower(keyword)||'''
      group by title, wp_id
      union all';

      
    end loop;
    

    query := left(query, length(query) - 10);   
    
    query := 'with key_words_match as ('||query||')
    select title, wp_id, count as frequency 
    from key_words_match
    order by frequency desc';
    


    return query execute query;
  end if;
end;
$$;





/*
D.13. Word-to-words querying: An alternative, to providing search results as ranked lists of posts, is to provide answers in the form of ranked lists of words. These would then be weighted keyword lists with weights indicating relevance to the query. 

Develop functionality to provide such lists as answer. 

One option to do this is the following:
1) Evaluate the keyword query and derive the set of all matching titles, 

2) count word frequencies over all matching titles (for all matching titles collect the words they are indexed by in the inverted index wi), 

3) provide the most frequent words (in decreasing order) as an answer (the frequency is thus the weight here)
*/

drop function if exists word_to_words;
create function word_to_words(keyword varchar)
returns table (wi_word text, counter bigint)
language plpgsql as $$
begin
  return query
    with titles_from_keyword as (
      select t_id from wi join title on tconst = t_id
      where lower(word) like keyword
    )
    select word, count(word) 
    from titles_from_keyword join wi on tconst = t_id
    GROUP BY word
    order by count desc;
end;
$$;

/*
Consider, if time allows, the following issues.
D.14. Weighted indexing [OPTIONAL]: Build a new inverted index for weighted indexing similar to the wi index, but now with added weights. A weight for an entry in the index should indicate the relevance of the title to the word. As weighting strategy, a good choice would probably be a variant of TFIDF.

Ranked weighted querying: Develop a refined function similar to D.12, but now with a ranking based on a relevance weighting (TFIDF or similar) provided by the
weighted indexing.

Finally, feel free to elaborate.
*/




drop function if exists searching_algorithm;
create function searching_algorithm(variadic keywords text[])
returns table (id varchar, title varchar,keyword text, appears numeric, results numeric)
language plpgsql as $$
declare 
  keyword text; 
  keyword_appears numeric; -- used in tf
  total_words numeric;  -- used in tf
  total_webpages numeric; -- used in idf
  title_with_keyword numeric; -- used in idf 
  
  web_page record;
  
  results numeric; -- result of equation tf-idf
  
begin
  -- total amount of documents
  select count(wp_id) into total_webpages from webpage;

  foreach keyword in array keywords
  loop
    keyword := lower(keyword);
    
    -- title_with_keyword
    select count(distinct t_id) into title_with_keyword
    from wi 
    join title on title.t_id = tconst
    where lower(word) = keyword;
    
    if title_with_keyword > 0 then
    
        for web_page in
          select distinct t_id, title.title
          from wi 
          join title on t_id = tconst
          where lower(word) = keyword
          loop
        
            -- total words in document
            select count(word) into total_words 
            from wi 
            join title on tconst = title.t_id
            where title.t_id = web_page.t_id;
            
            -- keyword_appears
            select count(word) into keyword_appears
            from wi 
            join title on title.t_id = tconst
            where lower(word) = keyword 
            and title.t_id = web_page.t_id;
  
  
            if total_words > 0 and title_with_keyword > 0 then
              -- equation
              results := ((keyword_appears / total_words) 
                    * log(total_webpages / title_with_keyword));
            else 
              results := 0;
            end if;
            
            return query 
                select web_page.t_id, web_page.title, keyword, keyword_appears, round(results, 10);
          end loop;
        end if; 
      end loop;
      
  end;
$$;




/*
D.15. Own ideas [OPTIONAL] 
If you have some ideas of your own, you can plug them in here.

session -> get session, start session

sign off

rate includes comment

users can like or dislike a comment

*/




--------------------------------------------------------------------------------
-- Procedure for insert_search
  
-- insert search into all the tables related
drop procedure if exists insert_search;
create procedure insert_search(in keyword text, in user_id int, out new_search_id varchar)
language plpgsql as $$
declare
	now_timestamp timestamp := current_timestamp;
	search_id varchar := concat(keyword, now_timestamp);
  variadic_keyword text[] := string_to_array(keyword, ' ');
  
begin
  new_search_id := search_id;
	insert into search values (search_id, keyword, now_timestamp);
	insert into history values (search_id, user_id);

end;
$$;

-- temp function to call insert and obtain the result
drop function if exists make_search(varchar, int, int);
create function make_search(keyword varchar, take int, skip int default 0) 
returns table (webpage_id text, relevance numeric)
language plpgsql as $$

declare new_search_id varchar; variadic_keyword text[];

begin
		
		variadic_keyword = string_to_array(keyword, ' ');

		return query
			select wp_id, frequency 
			from 
			(	
				select 'wp'|| id as wp_id, sum(results) as frequency
				from searching_algorithm(variadic variadic_keyword)
				group by wp_id
				order by frequency desc
				limit take offset skip		
			) as results
			
			order by frequency desc;
end;
$$;

drop function if exists make_search(varchar);
create function make_search(keyword varchar) 
returns table (webpage_id text, relevance numeric)
language plpgsql as $$

declare new_search_id varchar; variadic_keyword text[];

begin
		
		variadic_keyword = string_to_array(keyword, ' ');

		return query
			select wp_id, frequency 
			from 
			(	
				select 'wp'|| id as wp_id, sum(results) as frequency
				from searching_algorithm(variadic variadic_keyword)
				group by wp_id
				order by frequency desc
			) as results
			
			order by frequency desc;
end;
$$;

