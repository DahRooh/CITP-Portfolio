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

