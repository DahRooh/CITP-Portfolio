
drop trigger if exists title_update_trigger on rates;
drop function if exists trigger_update_people_rating;

create function trigger_update_people_rating()
returns trigger as $$
declare per_id record;
begin

  for per_id in (
    select p_id from person_involved_title
    where t_id = new.t_id )

  loop
  update person 
  set person_rating = (
    select round(sum(title.rating) / count(title.title), 2) as rating
    from person 
    join person_involved_title using(p_id)
    join title using(t_id)
    where person.p_id = per_id.p_id
    )
  where person.p_id = per_id.p_id;
  end loop;
  
  return new;
  
end;
$$ language plpgsql;

create trigger title_update_trigger
after insert or update or delete on rates
for each row execute function trigger_update_people_rating();


select * from person 
where p_id = 'nm1580225';


select * from person_involved_title
limit 10;


-- search people
drop function if exists make_search_person;

create function make_search_person(keyword varchar) 
returns table (webpage_id text)
language plpgsql as $$

declare variadic_keyword text[];

begin
		variadic_keyword = string_to_array(keyword, ' ');
    
		return query
    select 'wp'|| searching_algorithm_for_person.p_id as wp_id
    from searching_algorithm_for_person(variadic variadic_keyword);
			
end;
$$;


drop function if exists searching_algorithm_for_person;

create function searching_algorithm_for_person(variadic keywords text[])
returns table (p_id varchar, person_name varchar)
language plpgsql as $$
begin
  return query 
  with exact_match as (
    select p.p_id, p.name as person_name
    from person p
    where p.name ilike any(keywords)
  ), similar_matches as (
    select p.p_id, p.name as person_name
    from person p
    cross join unnest(keywords) as keyword
    where p.name ilike '%' || keyword || '%'
    and not exists (
      select *
      from exact_match em
      where em.p_id = p.p_id
    )
  )
  select * from exact_match
  union all
  select * from similar_matches;
end;
$$;


-- get session
drop function if exists get_user;
create function get_user(in_user_id int)
returns table(
  user_id int, 
  username varchar,
  email varchar
)
language plpgsql as $$
begin 
  return query
    select u_id, username, email 
    from users
    where u_id = in_user_id;
end;
$$;


drop function if exists get_series;
create function get_series(title_id VARCHAR)
returns table(
  e_id VARCHAR,
  ep_name VARCHAR,
  ep_plot VARCHAR,
  ep_rating numeric(3,2),
  ep_type varchar,
  adult boolean,
  release_date varchar,
  language varchar,
  country varchar,
  runtime numeric(5,0),
  awards varchar,
  poster varchar,
  title_type varchar,
  season_number numeric(2,0),
  episode_number numeric(2,0)
)
language plpgsql as $$
begin 
  return query
    select ep.*, episode.season_num, episode.ep_num
    from episode 
    join title ti on parentid = ti.t_id 
    join title ep on episode.t_id = ep.t_id
    where ti.t_id = title_id
    order by episode.season_num, episode.ep_num, title;
end;
$$;



drop function if exists get_all_series;
create function get_all_series()
returns table(
  s_id VARCHAR,
  s_title VARCHAR,
  s_plot VARCHAR,
  s_rating numeric(3,2),
  s_type varchar,
  adult boolean,
  release_date varchar,
  languages varchar,
  country varchar,
  runtime numeric(5,0),
  awards varchar,
  poster varchar,
  title_type varchar
)
language plpgsql as $$
begin 
  return query
    select ti.*
    from episode 
    join title ti on episode.t_id = ti.t_id 
    where ti.titletype = 'series'
    order by ti.rating;
end;
$$;


/* searching titles */

drop function if exists testSearch;
create or replace function testSearch(in_keyword text)
returns table(
  title_text varchar,
  id varchar,
  out_rating numeric(2,3)
)
language plpgsql as $$
declare
  keyword text = lower(in_keyword);

begin 
return query
  select title.title, t_id, rating from title
  where keyword = title.title
  order by rating desc;
end;
$$;

select * from testSearch('1999');


