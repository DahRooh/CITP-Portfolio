/*

GROUP: cit16, MEMBERS: Andreas Moosdorf, Dagmar Ree, Eray Erkul 

*/



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


/* TITLE_IS TABLE */
insert into title_is
select unnest(string_to_array(title_basics.genres, ',')), tconst
from title_basics;

