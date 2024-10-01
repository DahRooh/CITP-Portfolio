/*
D.6

Finding co-players: 
Make a function that, given the name of an actor, will return a list of actors that are the most frequent co-players to the given actor. 

For the actors found, return their nconst, primaryname and the frequency (number of titles in which they have co-played).


Hint: You may for this as well as for other purposes find a view helpful to make query
expressions easier (to express and to read). 
An example of such a view could be one that 
collects the most important columns from title, principals and name in a single virtual table.*/

drop view if exists title_cast;
create view title_cast as (
    select p_id, name, t_id, title, character, rating
    from person_involved_title natural join title natural join person
    where character is not null
);

select t1.name, t2.name, count(distinct t2.title) 
from title_cast t1 
join title_cast t2 on t1.title = t2.title
where t2.name = 'Ian McKellen' and t1.name <> 'Ian McKellen'
GROUP BY t1.name, t2.name
order by count desc;


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
D.8. 
Popular actors: Suggest and implement a function that makes use of the name ratings. 

One example could be a function that takes a movie and lists the actors of the movie in order of decreasing popularity. 

Another could be a similar function that takes an actor and lists the co-players in order of decreasing popularity.
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