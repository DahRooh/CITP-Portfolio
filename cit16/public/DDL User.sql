-- view
drop view if exists person_title_webpages; 
drop view if exists person_rated;
drop view if exists title_cast;
-- relations
drop table if exists bookmarks;
drop table if exists history;
drop table if exists rates;
drop table if exists wp_bookmarks;
drop table if exists wp_search;

-- entities
drop table if exists webpage;
drop table if exists users;
drop table if exists search;
drop table if exists bookmark;


create table webpage
(
	wp_id varchar(12) primary key,
  p_t_id varchar(10),
  url varchar(255) default null,
  wp_view_count numeric(9,0) default 0
);

/*
select * from title
union 
select concat('wp', t_id) from title;
*/
create table users
(
		u_id serial primary key,
		username varchar(30) unique, 
		password varchar(255),
		email varchar(100) unique
);

create table search
(
		search_id varchar primary key,
    keyword varchar,
    search_timestamp timestamp	
);

create table bookmark
(
  bookmark_id varchar primary key,
  bookmark_timestamp timestamp
);

create table bookmarks
(
	bookmark_id varchar,
  u_id int not null,
  primary key(bookmark_id, u_id),
  foreign key (bookmark_id) references bookmark on delete cascade,
  foreign key (u_id) references users on delete cascade
);

create table history
(
	search_id varchar,
  u_id int not null,
  primary key(search_id, u_id),
  foreign key (search_id) references search on delete cascade,
  foreign key (u_id) references users on delete cascade
);

create table wp_bookmarks
(
  bookmark_id varchar,
  wp_id varchar(12) not null,
  primary key(bookmark_id, wp_id),
  foreign key (bookmark_id) references bookmark on delete cascade,
  foreign key (wp_id) references webpage on delete cascade

);


create table wp_search
(
  search_id varchar,
  wp_id varchar(12),
	primary key (search_id, wp_id),
	foreign key (search_id) references search on delete cascade,  
  foreign key (wp_id) references webpage on delete cascade
);

create table rates
(
  t_id varchar(10),
  u_id int,
  rating numeric(4,2),
  timestamps timestamp,
  primary key(t_id, u_id),
  foreign key (t_id) references title on delete cascade,
  foreign key (u_id) references users on delete cascade
);









