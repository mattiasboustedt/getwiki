# getwiki
Fetch random articles from Wikipedia


About
GetWiki is a simple application with the following functionality:

Fetch ten random articles from Wikipedia.
Save the articles in a database.
Present the articles to the user.
Articles can be clicked so that it's content is displayed.
Article content is NOT saved in the database (Someone might update it on Wikipedia).
Displaying contents of an article is done with jQuery & getJSON (We have an ID to the article in the database!).
Articles can be managed and sorted into categories.
Categories are created and controlled by the user. Stored in database.
"Clear database" button to clear database from ALL content. (NO "are you sure")
All database communication is done with stored procedures. (Mapped EF to use stored procedures)
