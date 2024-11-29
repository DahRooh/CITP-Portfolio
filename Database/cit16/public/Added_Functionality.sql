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
  e_id VARCHAR, episode_name VARCHAR, ti_id VARCHAR, series_name VARCHAR
)
language plpgsql as $$
begin 
  return query
    select ep_id, 
    t2.title as episode_title, 
    t1.t_id, t1.title as series_title 
    from episode 
    join title t1 on parentid = t1.t_id 
    join title t2 on episode.t_id = t2.t_id
    where t1.t_id = title_id;
end;
$$;

