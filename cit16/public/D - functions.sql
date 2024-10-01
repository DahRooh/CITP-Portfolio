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


drop function if exists find_entertainment_and_rates; -- WHAT IF IT IS SPELLED WRONG?

create function find_entertainment_and_rates (search_for_entertainment varchar(100))
returns table (
  id varchar(10),
  title_name varchar(100),
  title_rating numeric(4,2),
  total_views numeric(9,0)
)
language plpgsql as $$

declare title_key varchar(100) := replace(concat('%', lower(search_for_entertainment), '%'), ' ', '');
begin
	raise notice '%', title_key;
	raise notice 'test';
	return query

		select t_id, title, rating, webpage.wp_view_count as total_views
		from title
		left join webpage on t_id = p_t_id
		where replace(lower(title.title), ' ', '') like title_key
		order by title.rating desc, total_views desc; 
	
end;
$$;

select find_entertainment_and_rates('Friends');




select * from find_people_and_rates('red');
select find_people_and_rates('staire');
select find_people_and_rates('Fred Astaire');
select find_people_and_rates('red Astai');

--------------------------------------------------
--------------------------------------------------
--------------------------------------------------















/*Alternative search*/

-- find_person (halfway done)

drop function if exists find_person_and_rates;

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




-------------------------------------------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------------------------------


select find_entertainment('odfather');
select find_entertainment('?');
select find_entertainment('he godfat');


-------------------------------------------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------------------------------


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
------------------------------------------------------------