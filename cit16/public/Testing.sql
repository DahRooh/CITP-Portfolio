
/*This is our test file.
  We will be testing our functions and producures determine if the they work correcly 

*/



/*Testing: Sign up*/

-- 1: checking if any users exist
select * from users; 

-- 2: Calling our function
call signup('username1', 'hashed-password', 'mail1@mail.ok', null);

-- 3: checking again if any users exist
select * from users;




/*Testing: Login*/

-- 1: Testing if a user can login
select * from login_user('username1', 'hashed-password'); -- User can login

select * from login_user('username1', 'incorrect-password'); -- User cannot login



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

-- 3: User has bookmarked.
select * from get_bookmarks(1);


-- 1: Check how many users have bookmarked a webpage
select * from get_bookmarks('wptt0108778'); 




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




































