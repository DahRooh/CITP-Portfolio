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
	death_year numeric(4,0)
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

create table movie(
	mov_id varchar (10) references title(t_id)
);

create table episode(
	ep_id varchar (10) references title(t_id),
	ep_name varchar(1000),
	season_num numeric(2,0),
	ep_num numeric(4,0)
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



/*
create table award(
	a_id varchar(10) primary key,
	award_type varchar(30),
	won varchar(1)
);
create table title_award(
	a_id varchar(10),
	t_id varchar(10),
	primary key(a_id, t_id),
	foreign key(a_id) references award,
	foreign key (t_id) references title
);

create table mov_title(
	t_id varchar(10) not null,
	mov_id varchar(10),
	primary key(t_id, mov_id),
	foreign key(t_id) references title,
	foreign key(mov_id) references movie
);

create table ep_title(
	ep_id varchar(10),
	t_id varchar(10) not null,
	primary key(ep_id, t_id),
	foreign key(ep_id) references episode, 
	foreign key(t_id) references title 
);*/