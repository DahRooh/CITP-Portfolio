
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

call login_user('username1', 'incorrect-password', null); -- User cannot login

select * from get_session(1); -- no session started

call login_user('username1', 'hashed-password', null); -- User can login (session start)

select * from get_session(1); -- session has been started
-- skal med i rapport (ikke skrevet om denne)

call sign_off(1); -- user signs off (session stops)
-- skal med i rapport (ikke skrevet om denne)

select * from get_session(1); -- session has ended (as seen my by timeended, not expired)
-- skal med i rapport (ikke skrevet om denne)

call login_user('username1', 'hashed-password', null); -- User logs in again





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

/*Testing: Delete search */

select * from get_user_history(1);

call clear_history(1);

select * from get_user_history(1);

/*Following section is centered upon searching algorithms*/

-- The following is not used in the program:
-- Finds movies in which the following substring is part of either the movie/series title or the plot description.
-- It is a simple function which we expand upon later.
select * from string_search('Godfath');
-- A structured search algorithm. Finds titles.
select * from structured_string_search('monKey', 'blob', 'Bilbo', 'Alfred');
-- Following is based on "string_search". Finds prople instead of titles.
select * from simple_search_person('friends');



/*Testing: Bookmark*/

-- 1: No bookmarks have been made yet by the user created.
select * from get_bookmarks(1);

-- 2: User bookmarks webpages
-- Insert bookmark inserts data regarding the bookmark into three tables(bookmark, wp_bookmarks and user_bookmarks)
call insert_bookmark(1, 'wptt2506874');
call insert_bookmark(1, 'wptt0108778');
call insert_bookmark(1, 'wpnm0000001');


-- 3: User has bookmarked, obtain all its bookmarks. get_bookmarks obtains the data from the bookmark, wp_bookmarks and user_bookmarks relation.
select * from get_bookmarks(1);

-- 4: The function is overloaded and can also check how many users have bookmarked a specific webpage
select * from get_bookmarks('wptt0108778'); 


-- 1: Checking that we can display more than one bookmark per webpage using the overloaded function, creating new user (user 2)
call signup('username2', 'hashed-password', 'mail2m@mail.ok', null);

-- 2: User 2 bookmarks the same webpage as user one
-- if a user tries to bookmark the same webpage, an exception is caught. 
call insert_bookmark(2, 'wptt0108778'); 
call insert_bookmark(2, 'wptt0108778'); 

-- 3: Check that both User 1 & User 2 has bookmarked the same webpage
select * from get_bookmarks('wptt0108778'); 



/*Testing: Delete bookmark*/
select * from get_bookmarks(1);

call delete_bookmark('1wptt2506874', 1);

select * from get_bookmarks(1);





/*Testing: Rating */
-- checking that a user can rate titles.

-- 1 user rating starts off empty
select * from get_user_rating(1);


-- 2 rating two titles should update their rating on title, because a rating is inserted or updated in rate. A trigger takes care of recalculating rating.
call rate('tt2506874', 1, 6, null);
call rate('tt0108778', 1, 7, null);


-- 3 user ratings are inserted
select * from get_user_rating(1);


-- 4 check that rating is updated on titles
select * from title
where t_id in ('tt2506874', 'tt0108778');

-- check that a user can write a review

-- 1 User can write a review
call rate('tt21050232', 1, 2, 'Bad movie');
select * from review
natural join rates;


-- 2 other user can like the review -- fejl
call like_review(1,3,1); -- user 1 likes review 3. 1 for like -1 for dislike.
call like_review(2,3,-1); 

-- 3 a user can change the like to a dislike or remove it
call like_review(1,3,-1); 
select * from review; -- fejl

call like_review(1,3,null); 
select * from review; -- fejl


-- 4 User can edit his old review
call rate('tt21050232', 1, 7, 'Better');














/*Testing: Delete user*/

-- all users still exist
select * from users;

-- deletes a user specified by the user_id. 
call delete_user(1);

select * from users;

-- Check that all the user's preferences are deleted.
select * from get_user_history(1);
select * from get_bookmarks(1);
select * from get_user_rating(1);
select * from get_session(1);


select * from title order by rating desc limit 10;


/*Testing: Person_known_for*/

-- 1: Testing what titles a person has been involved in. 
-- It can be actors, directors, etc.
select person_known_for('Fred Astaire');
select person_known_for('Alfred Hitchcock');


/*Testing: find_coactors. */
-- How many times have an actor/actress worked with another actor/actress.

select * from find_coactors('Ian McKellen');

/*Testing: name_ratings*/
-- Finds average rating of people involved in specified movie/series. 

select * from name_ratings('The Hobbit: The Desolation of Smaug');


/*Testing: co_player_rating*/
-- Finds rating of co-actors to the specified actor/actress
select * from co_players_rating('Ian McKellen');

-- find_similar_titles!!!

/*Testing_ person_words*/
-- Finds most frequent words related to the name passed to the function. 
-- The interger determines the amount of words.
select * from person_words('Ian McKellen', 10);

/*Testing: exact_match*/
-- All movies displayed are in some way related to all passed words. 
-- The fewer the keywords, the more results, and the other way around. 

select * from exact_match('monkey', 'king', 'queen'); -- returns titles
select * from exact_match('monkey', 'king', 'queen', 'dog'); -- does not return titles due to 'dog'







