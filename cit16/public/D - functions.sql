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


/*
SignUp()

signIn()

SignOff()

*/


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
do $$
declare 
  results boolean;
begin 
  call signup('username', 'hashed-password', 'mail@mail.ok', results);
END;
$$;

-- check signup
select * from users;





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


select * from login_user('username', 'hashed-password'); -- returns true if correct else false




-- logout function  (maybe if we create a session for the users)



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
    return;
end; $$
language plpgsql;


create trigger rate_title -- the trigger (calling the trigger function)
after insert on rates
for each row execute procedure rate_trigger(); -- for each new row
  

insert into rates values ('tt2506874', 1, 7.7);

select * from title where t_id = 'tt2506874';

/*




ListRelevantTitles() (overload/condition med serie/movie)

viewcount + rating of people/mov/serie

*/

create or replace function List_relevant_titles()
returns table (
  t_id varchar(10),
  title varchar(100),
  total_views numeric(9,0)
)
language plpgsql as $$
begin 
  return query
  select title.t_id, title.title, sum(webpage.wp_view_count) as total_views
  from title
  left join webpage on title.t_id = webpage.p_t_id
  left join wp_bookmarks on webpage.wp_id = wp_bookmarks.wp_id
  group by title.t_id, title.title
  order by total_views desc; 

end;
$$;

select List_relevant_titles();

update webpage
set wp_view_count = 1
where wp_id = 'wptt2506874';


/*

list\_relevant\_people() actors/actresses


view\_webpage(wpid)





get\_bookmarks()


findPerson()

personKnownFor() -- √

create or replace function List_relevant_titles()
returns table (
  t_id varchar(10),
  title varchar(100),
  total_views numeric(9,0)
)
language plpgsql as $$
begin 
  return query
  select title.t_id, title.title, sum(webpage.wp_view_count) as total_views
  from title
  left join webpage on title.t_id = webpage.p_t_id
  left join wp_bookmarks on webpage.wp_id = wp_bookmarks.wp_id
  group by title.t_id, title.title
  order by total_views desc; 

end;
$$;

select List_relevant_titles();

update webpage
set wp_view_count = 1
where wp_id = 'wptt2506874';




calculateRating() - trigger

*/

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


drop function if exists string_search;

create function string_search(search_string varchar(100))
returns table(id varchar(20), title varchar(2000))
language plpgsql as 
$$

declare search_key varchar(100) := concat('%', search_string, '%');

begin
		select t_id, title from title
		where title like search_key or plot like search_key;
end;

return search_key;
$$;


select string_search('and');




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




























