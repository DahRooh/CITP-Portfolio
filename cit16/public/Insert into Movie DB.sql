/*
DO $$
declare rec record;
month_num int;
date int;
begin
for rec in select tconst, SUBSTR(released, 1,2) as "d", SUBSTR(released, 4,3) as "m", SUBSTR(released, 8,4) as "y" from omdb_data
loop
  
  case 
    when rec."m" = 'Jan' then month_num := 1;
    when rec."m" = 'Feb' then month_num := 2;
    when rec."m" = 'Mar' then month_num := 3;
    when rec."m" = 'Apr' then month_num := 4;
    when rec."m" = 'May' then month_num := 5;
    when rec."m" = 'Jun' then month_num := 6;
    when rec."m" = 'Jul' then month_num := 7;
    when rec."m" = 'Aug' then month_num := 8;
    when rec."m" = 'Sep' then month_num := 9;
    when rec."m" = 'Oct' then month_num := 10;
    when rec."m" = 'Nov' then month_num := 11;
    when rec."m" = 'Dec' then month_num := 12;
    else month_num := 00;
    end case;
    raise notice '% %', rec.tconst, month_num;

end loop;
END;
$$;*/

/* 
ENTITIES CREATION
*/

select * from session;


/* PERSON TABLE */
-- insert person data
insert into person
  select nconst, primaryname, cast(nullif(birthyear, '') as integer), cast(nullif(deathyear, '') as integer) from name_basics;
  
  
  
  
/* PROFESSION TABLE */
-- insert each profession, found in their person table
-- we split up to make a profession out of each
insert into profession
select distinct unnest(string_to_array(name_basics.primaryprofession, ',')) from name_basics;
 
 
 /* TITLE TABLE */
insert into title
select tconst, primarytitle, nullif(plot, 'N/A'), 0, titletype, isadult, released, language, country, runtimeminutes, nullif(awards, 'N/A'), poster
from title_basics left join omdb_data using(tconst);



/* MOVIE TABLE */

-- all "movies"
insert into movie
select distinct tconst
from title_basics
full join omdb_data using(tconst)
where titletype in ('tvShort', 'movie', 'tvMovie', 'short') or type in ('movie');



/* EPISODE TABLE */

-- all "episodes"
insert into episode
select tconst, primarytitle, cast(nullif(season, 'N/A') as numeric),  cast(nullif(episode, 'N/A') as numeric)
from title_basics left join omdb_data using(tconst)
where titletype not in ('tvShort', 'movie', 'tvMovie', 'video', 'short', 'videoGame');


/*
-- all 'games' if usable
insert into games????
select tconst, primarytitle, null, isadult, released, language, country, poster
from title_basics left join omdb_data using(tconst)
where titletype = 'videoGame';
*/



/* GENRE TABLE */ 
insert into genre
(select distinct split_part(genres, ',', 1) as genre from title_basics
where genres <> '' and genres <> 'N/A'

union

select distinct split_part(genre, ',', 1) as genre from omdb_data
where genre <> '' and genre <> 'N/A');


/* 
RELATION CREATION
*/


/* PERSON_HAS_A TABLE */
insert into person_has_a
select unnest(string_to_array(name_basics.primaryprofession, ',')), nconst
from name_basics;





/* PERSON_INVOLVED_TITLE TABLE */
insert into person_involved_title (p_id, t_id, job, character)
select nconst, tconst, category, nullif(characters, '')
from title_principals;



/*select * from person_involved_title
natural join person
NATURAL join title 
where name = 'Samuel L. Jackson';

select *
from title_basics
join title_principals using (tconst)
where nconst = 'nm0000168';*/



/* TITLE_IS TABLE */
insert into title_is
select unnest(string_to_array(title_basics.genres, ',')), tconst
from title_basics;



/*
select unnest(string_to_array(title_basics.genres, ',')), tconst
from title_basics
where tconst = 'tt21050232';*/



















-- old

/* EP_TITLE relation 

--insert into ep_title
insert into ep_title
select tconst, parenttconst
from title_episode join episode on ep_id = tconst;

--select title, count(*) 
--from ep_title join title using(t_id)
--GROUP BY title;

*/

/* casting relation

-- ep cast
insert into casting (p_id, ep_id, character)
select nconst, ep_id, characters
from episode join title_principals on ep_id = tconst
where category in ('actor', 'actress');
--where ep_name = 'The One with the Ick Factor' and category in ('actor', 'actress');

select * from casting;


select characters from title_principals
ORDER BY length(characters) desc;


--mov cast
--insert into casting (p_id, t_id, character)
select nconst, mov_id, characters
from movie
join title_principals on mov_id = tconst
where category in ('actor', 'actress');


select * from casting
where t_id is not null;

select * from title_principals
where nconst = 'nm0891514 ';


 */






