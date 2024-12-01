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
  title_type varchar
)
language plpgsql as $$
begin 
  return query
    select ep.*
    from episode 
    join title ti on parentid = ti.t_id 
    join title ep on episode.t_id = ep.t_id
    where ti.t_id = title_id;
end;
$$;

select * from get_series('tt20854604');

select ep.*
from episode 
join title ti on parentid = ti.t_id 
join title ep on episode.t_id = ep.t_id
where ti.t_id = 'tt20854604';