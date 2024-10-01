/*D.1. Basic framework functionality: 
Consider what is needed to support the framework anddevelop functions for that. 
You will need functions for managing users and for bookmarking names and titles. 
You could also consider developing functions for adding notes to titles and names and for retrieving bookmarks as well as search history and rating history for users.*/


--we got:
-- signup
-- login
-- inserts for. bookmark, search (relations)
-- known for
-- 


/* Views */

drop view if exists person_title_webpages;

create view person_title_webpages as (
  select lower(name) as name_title, wp_id, url, null as plot from webpage join person on p_id = p_t_id
  union
  select lower(title) as name_title, wp_id, url, lower(plot) from webpage join title on t_id = p_t_id 
  order by wp_id
 );


drop view if exists title_cast;
create view title_cast as (
    select p_id, name, t_id, title, character, rating 
    from person_involved_title natural join title natural join person
    where character is not null
);

--------------------------------------------------------------------------------
-- sign up procedure 
drop procedure if exists signup;

create procedure signup(in username varchar(20), in password varchar(256), in email varchar(50), out results boolean)
LANGUAGE plpgsql
as $$
declare results boolean;
begin
  begin 
    insert into users (username, password, email) values(username, password, email);
    results := true;
    raise notice 'Sign up successful';
  exception
    when others then -- catches all exceptions
      raise notice 'Cannot sign up';
      results := false;
  end;
end;
$$;

-- call signup procedure example
call signup('username1', 'hashed-password', 'mail1@mail.ok', null);


-- check signup
select * from users;




--------------------------------------------------------------------------------
-- sign in function 
drop function if exists login_user;

create function login_user(p_username varchar, p_password varchar)
returns boolean
language plpgsql as $$
declare 
  results int; 

begin 
  with login_try as (
    select * from users 
    where username = p_username and password = p_password )
  
  select count(*) into results from login_try;
  return results;
end;
$$;


select * from login_user('username1', 'hashed-password'); -- User can login
select * from login_user('username1', 'incorrect-password'); -- User cannot login



--------------------------------------------------------------------------------
-- Procedure for insert_search

drop procedure insert_search;

create procedure insert_search(in keyword varchar, in user_id int)
language plpgsql as $$
declare
	now_timestamp timestamp := current_timestamp;
	search_id varchar := concat(keyword, now_timestamp);

begin
	insert into search values (search_id, keyword, now_timestamp);
	insert into history values (search_id, user_id);
	insert into wp_search
	select search_id, webp_id from user_search(keyword)
	limit 100;
end;
$$;

-- insert into wp_search
call insert_search('Zombies of Oz: Tin', 1);

select * from history 
natural join wp_search
natural join search;



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
	select keyword, search_timestamp from history natural join search
	where u_id = user_id
	order by search_timestamp desc;
end;
$$;

call insert_search('The Godfather', 1);
call insert_search('Friends', 1);
call insert_search('asd', 1);
call insert_search('', 1);

select * from get_user_history(1);



--------------------------------------------------------------------------------
-- procedure for inserting into wp and bookmarks

drop procedure insert_bookmark;

create procedure insert_bookmark(in user_id int, in webpage_id varchar)
language plpgsql as $$
declare 
  bookmark_id varchar := concat(user_id, webpage_id);
begin
  insert into bookmark values (bookmark_id, current_timestamp);
  insert into bookmarks values (bookmark_id, user_id);
  insert into wp_bookmarks values (bookmark_id, webpage_id);
end;
$$;


call insert_bookmark(1, 'wptt2506874');


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
           bookmarks.u_id, bookmark_timestamp
    from bookmark natural join wp_bookmarks natural join bookmarks
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
           bookmarks.u_id, bookmark_timestamp
    from bookmark natural join wp_bookmarks natural join bookmarks
    where p_u_id = bookmarks.u_id;
end;
$$;

select * from get_bookmarks('wptt2506874');
select * from get_bookmarks(1);



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

select person_known_for('Fred Astaire');
select person_known_for('Alfred Hitchcock');







--to make
-- delete user (cascades on ddl)
-- delete bookmark (cascades)
-- clear history (cascades)
-- get user ratings
-- logout?? if needed (session)










/*D.2. Simple search: Develop a simple search function for instance called string_search(). This
function should, given a search sting S as parameter, find all movies where S is a substring
of the title or a substring of the plot description. For the movies found return id and title
(tconst and primarytitle, if you kept the attribute names from the provided dataset). Make
sure to bring the framework into play, such that the search history is updated as a side
effect of the call of the search function.*/


-- not made









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
drop procedure rate;
create procedure rate(in title_id varchar(10), in user_id int, in user_rating numeric(4,2))
language plpgsql as $$
begin
  if title_id in (select t_id from rates where u_id = user_id) then
    update rates 
    set rating = user_rating
    where t_id = title_id;
  else 
    insert into rates values (title_id, user_id, user_rating);
  end if;
end;
$$;


/* Rates trigger after insert */
-- rate trigger. when a row is inserted into rates, then we update the rating for that title.;
drop trigger if exists rate_title on rates;
drop function rate_trigger;

create function rate_trigger() -- the trigger function
returns trigger as $$
begin
    update title
    set rating = (
      select avg(rating) 
      from rates where t_id = new.t_id)
      where t_id = new.t_id;
      return null; -- need to return something
end; $$
language plpgsql;


create trigger rate_title -- the trigger (calling the trigger function)
after insert or update on rates
for each row execute procedure rate_trigger(); -- for each new row
  

truncate rates;

call rate('tt0903624', 1, 6);
call rate('tt21832076', 1, 7);
call rate('tt21824192', 1, 8);
call rate('tt1170358', 1, 9);
call rate('tt2506874', 1, 10);
-- Check rating
select * from title 
order by rating desc;







/*
D.4. Structured string search: Develop a search function, for instance called
structured_string_search(), that take 4 string parameters and return titles that match these on the title, the plot, the characters and the person names involved respectively. Make the function flexible in the sense that it don’t care about case of letters and argument values are treated as substrings (to match they should just be included in the column value in question). For the movies found, return id and title (in the source data called tconst and primarytitle). Make sure to bring the framework into play, such that the search history is stored as a side effect of the call of the search function.
*/

-- no













/*
D.5. Finding names: The above search functions are focused on finding titles. Try to add to these by developing one or two functions aimed at finding names (of for instance actors).
*/










/*
D.6. Finding co-players: Make a function that, given the name of an actor, will return a list of actors that are the most frequent co-players to the given actor. For the actors found, return their nconst, primaryname and the frequency (number of titles in which they have co-
played).

Hint: You may for this as well as for other purposes find a view helpful to make query expressions easier (to express and to read). An example of such a view could be one that collects the most important columns from title, principals and name in a single virtual table.
*/


drop view if exists title_cast;
create view title_cast as (
    select p_id, name, t_id, title, character, rating
    from person_involved_title natural join title natural join person
    where character is not null
);



drop function if exists find_coactors;
create function find_coactors(actor varchar)
returns table (
      person_id varchar,
      co_actor varchar,
      counted bigint
      )
language plpgsql as $$
begin
  return query
    select t1.p_id, t1.name, count(distinct t2.title) 
    from title_cast t1 
    join title_cast t2 on t1.title = t2.title
    where t2.name = actor and t1.name <> actor
    group by t1.p_id, t1.name
    order by count desc;
end;
$$;


select * from find_coactors('Ian McKellen');





/*
D.7. Name rating: 
Derive a rating of names (just actors or all names, as you prefer) based on ratings of the titles they are related to. 

Modify the database to store also these name ratings.

-- not made yet v
Make sure to give higher influence to titles with more votes in the calculation.
You can do this by calculating a weighted average of the averagerating for the titles, where the numvotes is used as weight.
*/

create view person_rated as 
	(with temp_test as (
	select distinct name, title, rating
	from title_cast)
		select name, round(sum(rating) / count(title), 3) as rating
		from temp_test
		group by name
		order by rating desc
);

alter table person
add person_rating numeric(8,3);

update person
set person_rating = rating
from person_rated
where person.name = person_rated.name;

select * from person
where person_rating is not null
order by person_rating desc;




/*
D.8. Popular actors: Suggest and implement a function that makes use of the name ratings. One
example could be a function that takes a movie and lists the actors of the movie in order of
decreasing popularity. Another could be a similar function that takes an actor and lists the
co-players in order of decreasing popularity.
*/


-- 1)
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

select * from name_ratings('The Hobbit: The Desolation of Smaug');

-- 2)

drop function if exists co_players_rating;
create function co_players_rating(actor varchar)
returns table (co_players_name varchar, avg_rating numeric)
language plpgsql as $$

begin
	return query
	
	select name, person_rating 
	from find_coactors(actor)
	join person on person_id = p_id
	order by person_rating desc;

end;
$$;

select * from co_players_rating('Ian McKellen');
select * from co_players_rating('Ken Stott');






/*
D.9. Similar movies: Discuss and suggest a notion of similarity among movies. Design and implement a function that, given a movie as input, will provide a list of other movies that are similar.

*/

-- Finds all the movies/series that have the same genre as the input_title_id and user_id
-- Finds all the movies/series that have the same genre as the input_title
drop function if exists find_similar_titles(varchar); 
create or replace function find_similar_titles(input_title varchar)
returns table(
  similar_title varchar,
  genre_of_similar_title varchar
)
language plpgsql as $$
begin
  return query
  select title as similar_title,
         genre as genre_of_similar_title
  from title
  join title_is using(t_id)
  where genre in (
      select genre
      from title_is
      join title using(t_id)
      where title = input_title)
  
  and title <> input_title;
end;
$$;





-- Finds all the movies/series that have the same genre as the input_title_id
drop function if exists find_similar_titles(varchar); 
create or replace function find_similar_titles(input_title_id varchar)
returns table(
  similar_title varchar,
  genre_of_similar_title varchar
)
language plpgsql as $$
begin
  return query
  select title as similar_title,
         genre as genre_of_similar_title
  from title
  join title_is using(t_id)
  where genre in (
      select genre
      from title_is
      where t_id = input_title_id)
  and t_id <> input_title_id;
end;
$$;



-- Finds all the movies/series that have the same genre as the input_title_id and user_id
drop function if exists find_similar_titles(varchar, int); 

create or replace function find_similar_titles(input_title_id varchar, user_id int)
returns table(
  similar_title_id varchar,
  similar_title varchar,
  is_bookmarked boolean,
  multiple_same_genre numeric
)
language plpgsql as $$
begin
  return query
  select distinct t_id as similar_title_id,
         title as similar_title,
         case when bookmark_id is not null then true else false end as is_bookmarked,
         sum(case when (select count(distinct genre) from title_is) > 1 
         then 1 else 0 end)::numeric as multiple_same_genre
  from title
  join title_is using(t_id)
  join webpage on t_id = p_t_id
  left join wp_bookmarks using(wp_id)
  left join bookmarks using(bookmark_id)
  where genre in (
      select genre
      from title_is
      where t_id = input_title_id)
  and t_id <> input_title_id
  and (u_id = user_id or u_id is null)
  group by similar_title_id, similar_title, is_bookmarked
  order by multiple_same_genre desc, is_bookmarked desc;
end;
$$;

select * from find_similar_titles('tt0108778', 1);






/*
D.10. Frequent person words: The wi table provides an inverted index for titles using the 4 columns: primarytitle, plot and, from persons involved in the title, characters and primaryname. So, given a title, we can from wi get a lot of words, that are somehow characteristic for the title. To retrieve a list of words that are characteristic for a person we can do the following: find the titles the person has been involved in, and find all words associated with these titles (using wi). To get a list of unique words, you can just group by word in an aggregation by count(). Thereby you'll get a list of words together with their frequencies in all titles the person has been involved in. Use this principle in a function person_words() that takes a person name as parameter and returns a list of words in decreasing frequency order, limited to fixed length (e.g. 10). 

Optionally, add a parameter to the function to set a maximum for the length of the list. You can consider the frequency to be a weight, where higher weight means more importance in the characteristics of the person.
*/







/*
D.11. Exact-match querying: 
Introduce an exact-match querying function that takes one or more keywords as arguments and returns posts that match all of these. 

Use the inverted index wi for this purpose. You can find inspiration on how to do that in the slides on Textual Data and IR.
*/









/*
D.12. Best-match querying: 
Develop a refined function similar to D.11, but now with a “best-match” ranking and ordering of objects in the answer. 

A best-match ranking simply means: the more keywords that match, the higher the rank. Titles in the answer should be ordered by decreasing rank. 

See also the Textual Data and IR slides for hints.
*/












/*
D.13. Word-to-words querying: An alternative, to providing search results as ranked lists of posts, is to provide answers in the form of ranked lists of words. These would then be weighted keyword lists with weights indicating relevance to the query. Develop functionality to provide such lists as answer. 

One option to do this is the following:
1) Evaluate the keyword query and derive the set of all matching titles, 

2) count word frequencies over all matching titles (for all matching titles collect the words they are indexed by in the inverted index wi), 

3) provide the most frequent words (in decreasing order) as an answer (the frequency is thus the weight here)
*/









/*
Consider, if time allows, the following issues.
D.14. Weighted indexing [OPTIONAL]: Build a new inverted index for weighted indexing similar to the wi index, but now with added weights. A weight for an entry in the index should indicate the relevance of the title to the word. As weighting strategy, a good choice would probably be a variant of TFIDF.

Ranked weighted querying: Develop a refined function similar to D.12, but now with a ranking based on a relevance weighting (TFIDF or similar) provided by the
weighted indexing.

Finally, feel free to elaborate.
*/









/*
D.15. Own ideas [OPTIONAL] 
If you have some ideas of your own, you can plug them in here.
*/




























