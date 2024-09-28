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
Simple search: 
Develop a simple search function for instance called string_search(). 
This function should, given a search sting S as parameter, find all movies where S is a substring
of the title or a substring of the plot description. 

For the movies found return id and title
(tconst and primarytitle, if you kept the attribute names from the provided dataset). Make
sure to bring the framework into play, such that the search history is updated as a side
effect of the call of the search function.
*/

