/*

GROUP: cit16, MEMBERS: Andreas Moosdorf, Dagmar Ree, Eray Erkul 

*/



call signup('username1', 'hashed-password', 'mail1@mail.ok', null);

call login_user('username1', 'hashed-password', null);

select * from get_session(1); 

select * from string_search('Godfath');

select * from structured_string_search('monKey', 'blob', 'Bilbo', 'Alfred') limit 100;

select * from simple_search_person('friends') limit 100;

select * from exact_match('monkey', 'king', 'queen');

select * from best_match('cat', 'mouse') limit 100;

select * from word_to_words('monkey') limit 100;

select id, sum(results) as frequency
from searching_algorithm('lord', 'of', 'the', 'rings', 'towers')
group by id
order by frequency desc
limit 5;

select title, relevance from make_search('hermione granger', 1) 
join title on t_id = substring(webpage_id, 3,10);

select * from get_user_history(1);

select * from make_search('Friends', 1);

call clear_history(1);

call insert_bookmark(1, 'wptt2506874');

select * from get_bookmarks(1);

select * from get_bookmarks('wptt0108778'); 

call delete_bookmark('1wptt2506874', 1);

call rate('tt21050232', 1, 2, 'Bad movie');

call like_review(1, 1, -1);

select likes from review;

select rating from title where t_id = 'tt21050232';

call insert_search('king', 1, null);

select * from get_user_rating(1);

call like_review(1,1,1); 

call sign_off(1);

call start_session(1);

call delete_user(1);

select person_known_for('Fred Astaire');

select * from find_coactors('Ian McKellen');

select * from co_players_rating('Ian McKellen');

select * from find_similar_titles('tt2506874');

select * from person_words('Ian McKellen', 10);

call update_all_people_rating();

select * from name_ratings('The Hobbit: The Desolation of Smaug');

















