-- relations
drop table if exists bookmarks;
drop table if exists history;
drop table if exists rates;

-- entities
drop table if exists webpage;
drop table if exists users;
drop table if exists user_hist;
drop table if exists user_bookmark;

create table webpage
(
	w 
);

select * from title
union 
select concat('wp', t_id) from title;

create table users
(
		u_id serial primary key,
		username varchar(30) unique, 
		password varchar(255),
		email varchar(100)
);

create table user_hist
(
		u_id numeric(10),
		cur_search 
		
		time_viewed timestamp
		
);


create table user_bookmark
(

);


create table bookmarks
(
	
);

create table history
(

);

create table rates
(

);