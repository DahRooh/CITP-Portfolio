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


/*SignUp()

signIn()

SignOff()

%viewAListOfFamousMovSer() -> mostviewedMovSer()

ListRelevantTitles() (overload/condition med serie/movie)

list\_relevant\_people() actors/actresses

view\_webpage(wpid)

calculateRating() - trigger

rate()

findPerson()

personKnownFor() -- √

get\_bookmarks()*/

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




























