
/*This is our test file.
  We will be testing our functions and producures determine if the they work correcly 

*/



/*Testing: Sign up*/

-- 1: no users exist
select * from users; 

-- 2: after calling our procedure
call signup('username1', 'hashed-password', 'mail1@mail.ok', null);

-- 3: one user exists
select * from users;


/*Testing: Login*/

-- 1: Testing if a user can login

select * from login_user('username1', 'incorrect-password'); -- User cannot login

select * from get_session(1); -- no session started

select * from login_user('username1', 'hashed-password'); -- User can login (session start)

select * from get_session(1); -- session has been started
-- skal med i rapport (ikke skrevet om denne)

call sign_off(1); -- user signs off (session stops)
-- skal med i rapport (ikke skrevet om denne)

select * from get_session(1); -- session has ended (as seen my by timeended, not expired)
-- skal med i rapport (ikke skrevet om denne)

select * from login_user('username1', 'hashed-password'); -- User logs in again





-- TO-DO:
/*Testing: Search*/

-- 1: Check if user has any search history
select * from get_user_history(1);

-- 2: User searches
call insert_search('Zombies of Oz: Tin', 1);
call insert_search('Friends', 1);
call insert_search('The Godfather', 1);

-- 3: Check if the functionality works
select * from get_user_history(1);




/*Testing: Bookmark*/

-- 1: Check if user has bookmarked any webpages.
select * from get_bookmarks(1);

-- 2: User bookmarks webpages
call insert_bookmark(1, 'wptt2506874');
call insert_bookmark(1, 'wptt0108778');
call insert_bookmark(1, 'wpnm0000001');

-- 3: User has bookmarked.
select * from get_bookmarks(1);

-- 1: Check how many users have bookmarked a webpage
select * from get_bookmarks('wptt0108778'); 

-- 2: Create new user (user 2)
call signup('username2', 'hashed-password', 'mail2m@mail.ok', null);

-- 3: User 2 creates a bookmark
call insert_bookmark(2, 'wptt0108778');

-- 3: User 2 has bookmarked
select * from get_bookmarks(2);



/*Testing: Rating */                  -- FEJL!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

select * from get_user_rating(1);


call rate('tt2506874', 1, 6);
call rate('tt0108778', 1, 7);


select * from get_user_rating(1);


select * from title
where t_id in ('tt2506874', 'tt0108778');


/*Testing: Delete search */

select * from get_user_history(1);

call clear_history(1);

select * from get_user_history(1);



/*Testing: Delete bookmark*/
select * from get_bookmarks(1);

call delete_bookmark('1wptt2506874');

select * from get_bookmarks(1);


abe

/*Testing: Delete user*/

select * from users;

call delete_user(1);

select * from users;

-- Check that all the user's preferences are deleted.
select * from get_user_history(1);
select * from get_bookmarks(1);
select * from get_user_rating(1);



select * from webpage
left join wp_search using(wp_id) 
natural join search; 

select * from search;




































