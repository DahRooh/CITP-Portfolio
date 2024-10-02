/* 
ENTITIES CREATION
*/





/*
WEBPAGE
*/
insert into webpage (wp_id, p_t_id)
select concat('wp', t_id), t_id from title
union
select concat('wp', p_id), p_id from person;





/*
USERS
*/

insert into users (username, password, email)
values ('Bob', '"hashed"', 'abemanden@ruc.dk');

insert into users (username, password, email)
values ('Bob1', '"hashed"', 'abemanden1@ruc.dk');


/* 
SEQUENCES CREATION
*/



/*
HISTORY

WP_SEARCH

SEARCH 
*/
select * from users;

insert into search values (concat('1', CURRENT_TIMESTAMP), 'Zombies of Oz: Tin' ,CURRENT_TIMESTAMP);
insert into history values ('12024-10-02 17:15:01.784764+02', 2);
insert into wp_search values (2, 'wptt2506874');

select * from title where t_id = 'tt2506874';
select * from title where title = 'Friends';



/*
Nothing Like Dreaming : tt2506874
Friends   :   tt0108778
Zombies of Oz: Tin   : tt2506874
*/
select * from search;
select * from history;



select * from title
where type = 'movie';




/*
BOOKMARK
*/
/*
BOOKMARKS
*/
/*
WP_BOOKMARKS
*/

insert into bookmark values (concat('1', 'wptt0108778'), CURRENT_TIMESTAMP);
insert into bookmarks values ('1wptt2506874', 2);
insert into wp_bookmarks values (1, 'wptt2506874');

select * from bookmark;
select * from bookmarks;







-- all user bookmarks
select username, wp_id from users natural join wp_bookmarks;


-- count of webpage bookmarks
select wp_id, count(bookmark_id)
from webpage left join wp_bookmarks using(wp_id) 
GROUP BY wp_id
order by count desc;


-- update webpage
update webpage
set wp_view_count = (
  select count(*) from wp_search where wp_id = 'wptt2506874'
)
where wp_id = 'wptt2506874';

/*
RATES
*/


-- insert into
insert into rates values ('tt2506874', 1, 9.8);
select * from rates;

insert into rates values ('tt2506874', 2, 7.7);
select * from rates;


-- update: find avg rating of a particular title
update title
set rating = (
select avg(rating) 
from rates where t_id = 'tt2506874')
where t_id = 'tt2506874';





select awards from omdb_data where awards is not null and length(awards) > 10;








