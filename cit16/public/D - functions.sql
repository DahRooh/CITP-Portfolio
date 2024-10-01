/*
create function "NAME" ("ARGS type"...)
returns DATATYPE
language plpgsql as $$
DECLARE VARIABLES;
BEGIN STATEMENTS
---- LOGIC;
END;
$$; -- END CREATE FUNCTION
*/

/*  D.1. 


Basic framework functionality: Consider what is needed to support the framework and
develop functions for that. You will need functions for managing users and for bookmarking
names and titles. You could also consider developing functions for adding notes to titles and
names and for retrieving bookmarks as well as search history and rating history for users.*/




-- * sign up procedure *
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





-- * sign in function *
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




/* logout */
-- logout function  (maybe if we create a session for the users)
-- to be done if at all


/*User search*/
drop function if exists user_search; 
create function user_search(keyword varchar)
  returns table (
      displayname varchar(1000),
      webp_id varchar(20)
  )
	
language plpgsql as $$
declare 
  search_key varchar := concat('%',lower(keyword),'%');
begin
  return query
    select name_title::varchar(1000), wp_id::varchar(1000)
    from person_title_webpages
    where name_title like search_key 
    or (plot is not null and 
        plot like search_key);
end;
$$;

select * from user_search('Zombies of Oz: Tin');











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






 
/* get_bookmarks from user or wepage 
insert_bookmark from user */


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









/* User rate */
drop procedure rate;
create procedure rate(in t_id varchar(10), in u_id int, in rating numeric(4,2))
language plpgsql as $$
begin
insert into rates values (t_id, u_id, rating);
end;
$$;





/* Rates trigger after insert */
-- rate trigger. when a row is inserted into rates, then we update the rating for that title.;
drop trigger if exists rate_title on rates;
drop function rate_trigger;

create function rate_trigger() -- the trigger function
returns trigger as $$
begin
  raise notice '%', new;
    update title
    set rating = (
      select avg(rating) 
      from rates where t_id = new.t_id)
      where t_id = new.t_id;
      return null; -- need to return something
end; $$
language plpgsql;


create trigger rate_title -- the trigger (calling the trigger function)
after insert on rates
for each row execute procedure rate_trigger(); -- for each new row
  

truncate rates;
call rate('tt2506874', 1, 2);
-- Check rating
select * from title 
order by rating desc;




/* get_user_history */

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





/*


ListRelevantTitles() (overload/condition med serie/movie)

viewcount + rating of people/mov/serie

*/
/* order of relevance: 
    name -> how close is the search key to an actual value
         -> plot vs title
    rating -> (how many have voted, what rating)
    viewcount -> from website
*/

drop function if exists list_relevant_titles(); 
create or replace function list_relevant_titles()  -- REMEMBER RELEVANT PEOPLE
returns table (
  id varchar(10),
  title_name varchar(100),
  title_rating numeric(4,2),
  total_views numeric(9,0)
)
language plpgsql as $$
begin 
  return query
  select t_id, title, rating, webpage.wp_view_count as total_views
  from title
  left join webpage on t_id = p_t_id
  order by title.rating desc, total_views desc; 

end;
$$;

select * from list_relevant_titles(); -- a list of "popular" titles





/* Person known for */

drop function if exists person_known_for;

create function person_known_for (search_name varchar(100))
returns table (title varchar(2000))
language plpgsql as $$

declare 
	selected_name varchar(100) := search_name;

begin
		return query
				
				select distinct title.title from title
				natural join person_involved_title join person using (p_id)
				where person.name = initcap(selected_name);
			
end;
$$;

select person_known_for('Alan Ladd');

























/* D.2
Simple search: 
Develop a simple search function for instance called string_search(). 
This function should, given a search sting S as parameter, find all movies where S is a substring
of the title or a substring of the plot description. 

For the movies found return id and title
(tconst and primarytitle, if you kept the attribute names from the provided dataset). Make
sure to bring the framework into play, such that the search history is updated as a side
effect of the call of the search function.
*/




/*
D.3
Title rating: Introduce functionality for rating by a function, called for instance rate(), that
takes a title and a rate as an integer value between 1 and 10, where 10 is best (see more
detailed interpretation at What are IMDb ratings?). The function should update the (average-
)rating appropriately taking the new vote into consideration. Make sure to bring the
framework into play, such that the rating history is updated as a side effect of the call of
the rate function. Also make sure to treat multiple calls of rate() by the same user
consistently. This could be for instance by ignoring or blocking an attempt to rate, in case a
rating of the same movie by the same user is already registered. Alternatively, an update
with the new rate can be preceded by a “redrawing” of the previous rating, recalculating the
average rating appropriately
*/





-- Maybe use
/* update viewcount */ -- SKAL BRUGE MERE IFHT SEARCH FØR VI KAN KØRE PÅ HER
drop trigger view_count on wp_search;
drop function update_view_count;

create function update_view_count() -- VIRKER IKKE SKAL KIGGE PÅ SEARCH WP_ID ER DER IKKE MERE, HVORDAN KLIKKER VI PÅ EN WEBPAGE? HVORDAN BLIVER DET REGISTRERET? NY TABLE MED PAGE_VIEWED EFTER ET KLIK PÅ ET RESULTAT I WP_SEARCH???
returns trigger as $$
begin
    if new.wp_id in (select wp_id from webpage) then
      update webpage
      set wp_view_count = (
      select count(*) from wp_search
      where wp_id = new.wp_id);
    end if;
    return null; -- need to return something
end; $$
language plpgsql;

create trigger view_count 
after insert on wp_search
for each row execute procedure update_view_count();












/*Alternative search*/


-- find_person (halfway done)

drop function if exists find_person;

create function find_person (search_for_person varchar(100))
returns table(id varchar(10), name varchar(100))
language plpgsql as $$

declare person_key varchar(100) := replace(concat('%', lower(search_for_person), '%'), ' ', '');

begin
	raise notice '%', person_key;
	raise notice 'test';

	return query
		-- select part of name
		select person.p_id, person.name from person
		where replace(lower(person.name), ' ', '') like person_key;
		-- second part: what if the name of the person isn't spelled correctly?
end;
$$; 

select * from find_person('red');
select find_person('staire');
select find_person('Fred Astaire');
select find_person('red Astai');

-- find_entertainment (halfway done)

drop function if exists find_entertainment;

create function find_entertainment (search_for_entertainment varchar(100))
returns table(id varchar(10), title varchar(2000))
language plpgsql as $$

declare title_key varchar(100) := replace(concat('%', lower(search_for_entertainment), '%'), ' ', '');
begin
	return query
		-- select part of name
		select title.t_id, title.title from title
		where replace(lower(title.title), ' ', '') like title_key;
		-- second part: what if the name of the title isn't spelled correctly?
end;
$$; 

select find_entertainment('odfather');
select find_entertainment('?');
select find_entertainment('he godfat');






/* Views */

drop view if exists person_title_webpages;

create view person_title_webpages as (
  select lower(name) as name_title, wp_id, url, null as plot from webpage join person on p_id = p_t_id
  union
  select lower(title) as name_title, wp_id, url, lower(plot) from webpage join title on t_id = p_t_id 
  order by wp_id
 );


select * from person_title_webpages;




