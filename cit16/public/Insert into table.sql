/* 
ENTITIES CREATION
*/




/* PERSON TABLE */
-- insert person data
insert into person
  select nconst, primaryname, cast(nullif(birthyear, '') as integer), cast(nullif(deathyear, '') as integer) from name_basics;
  
  
  
  
/* PROFESSION TABLE */
-- insert each profession, found in their person table
-- we split up to make a profession out of each
insert into profession
select distinct split_part(primaryprofession, ',' , 1) as profession from name_basics
 where split_part(primaryprofession, ',' , 1) <> '';
 

 
 
 
 /* TITLE TABLE */
insert into title
select tconst, primarytitle, null, isadult, released, language, country, poster
from title_basics left join omdb_data using(tconst);





select *
from title_episode join title_basics on parenttconst = title_basics.tconst;



/* MOVIE TABLE */

-- all "movies"
insert into movie
select tconst, titletype, runtimeminutes
from title_basics left join omdb_data using(tconst)
where titletype in ('tvShort', 'movie', 'tvMovie', 'video', 'short');




/* EPISODE TABLE */

-- all "episodes"
insert into episode
select tconst, primarytitle, cast(nullif(season, 'N/A') as numeric),  cast(nullif(episode, 'N/A') as numeric), runtimeminutes, null
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

/* PERSON_IS_A TABLE */

insert into person_is_a
select profession, p_id
from person
join name_basics on person.p_id = name_basics.nconst
join profession on name_basics.primaryprofession = profession.profession;

/* PERSON_INVOLVED_TITLE TABLE */

insert into person_involved_title (p_id, t_id, job)
select nconst, tconst, category
from title_principals;


select * from person_involved_title
natural join person
NATURAL join title 
where name = 'Samuel L. Jackson';

select *
from title_basics
join title_principals using (tconst)
where nconst = 'nm0000168';

select * from name_basics
where primaryname = 'Samuel L. Jackson';

/* TITLE_IS TABLE */

insert into title_is
select unnest(string_to_array(title_basics.genres, ',')), tconst
from title_basics;

select * from title_is
where t_id = 'tt21050232';

select unnest(string_to_array(title_basics.genres, ',')), tconst
from title_basics
where primarytitle = 'Bugs Bunny Builders';

select unnest(string_to_array(title_basics.genres, ',')), tconst
from title_basics
where tconst = 'tt21050232';


select * from profession;



select parenttconst, primarytitle, 
from title_basics join omdb_data using(tconst) 
join title_episode on parenttconst = title_basics.tconst;




