/*
Finding co-players: 
Make a function that, given the name of an actor, will return a list of actors that are the most frequent co-players to the given actor. 

For the actors found, return their nconst, primaryname and the frequency (number of titles in which they have co-
played).


Hint: You may for this as well as for other purposes find a view helpful to make query
expressions easier (to express and to read). 
An example of such a view could be one that 
collects the most important columns from title, principals and name in a single virtual table.*/

select * from title natural join person_involved_title;
select * from person_involved_title;