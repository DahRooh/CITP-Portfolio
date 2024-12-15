/*

GROUP: cit16, MEMBERS: Andreas Moosdorf, Dagmar Ree, Eray Erkul 

*/



-- view
drop view if exists person_title_webpages; 
drop view if exists person_rated;
drop view if exists title_cast;


-- relations
drop table if exists person_has_a;
drop table if exists person_involved_title;
drop table if exists title_is;
drop table if exists type_award;
drop table if exists mov_title;
drop table if exists ep_title;
drop table if exists rates;

-- entities
drop table if exists genre;
drop table if exists award;
drop table if exists movie;
drop table if exists episode;
drop table if exists title;
drop table if exists profession;
drop table if exists person;


create table profession(
	profession varchar(40) primary key not null
);



create table person(
	p_id varchar(10) primary key,
	name varchar(100),
	birth_year numeric(4,0),
	death_year numeric(4,0),
  person_rating numeric(4,2)
);

create table person_has_a(
	profession varchar(40), 
	p_id varchar(10),
	primary key(p_id, profession),
	foreign key (profession) references profession on update cascade,
	foreign key (p_id) references person
);


create table title(
	t_id varchar(20) primary key,
	title varchar(2000),
	plot varchar,
	rating numeric(4,2) default 0,
	type varchar(20),
	isadult boolean,
	released varchar(15),
	language varchar(1000),
	country varchar(1000),
  runtime numeric(5,0),
  awards varchar(500),
	poster varchar(1000)
);


create table movie(
	t_id varchar (10),
	mov_id varchar (10),
  titletype varchar(20),
  primary key (mov_id),
  foreign key (t_id) references title
);

create table episode(
	t_id varchar (10),
	ep_id varchar (10),
  titletype varchar(20),
	season_num numeric(2,0),
	ep_num numeric(4,0),
  parentId varchar(10), -- index?
  
  primary key (ep_id),
  foreign key (t_id) references title

);

create table person_involved_title(
  pit_id serial,
	p_id varchar(10),
	t_id varchar(10),
	job varchar(30),
  character varchar(500) default null,
  primary key(pit_id, p_id, t_id),
  foreign key(p_id) references person,
  foreign key(t_id) references title
);



create table genre(
	genre varchar(30) primary key
);

create table title_is(
	genre varchar(30),
  t_id varchar(10),
	primary key(genre, t_id),
	foreign key(genre) references genre on update cascade,
	foreign key(t_id) references title
);

