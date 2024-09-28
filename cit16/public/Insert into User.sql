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


insert into search (search_timestamp) values (CURRENT_TIMESTAMP);
insert into history values (2, 1);
insert into wp_search values (2, 'wptt2506874');




/*
BOOKMARK
*/
/*
BOOKMARKS
*/
/*
WP_BOOKMARKS
*/

insert into bookmark (bookmark_timestamp) values (CURRENT_TIMESTAMP);
insert into bookmarks values (1, 1);
insert into wp_bookmarks values (1, 'wptt2506874');


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








