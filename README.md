# Invoice


this is a project for determining how to solve the `invoice_id` increament.

> **_Note_**: in the time of making this repo, I was learning `asp.net`

so basically I have number that I want to increment on each request, and I have the ability to reset the number if it reaches a certain threshold. all this with a very heavy usage.

I tried three approaches: 

## First Approach: 
the first appraoch I tried to set the starting number as `1001` in a database field, and increment the number on each request, and reset the number to be `2001` when I want to reset it. but this solution is so complicated and heavy on the database. so this solution is not considered as the solution I want.


## Second Approach:
in this approach I created a table `MetaData` that contains one record to store the last number. and reset or increment with each request, using a `transaction` with isolation level `Serializable` so any type of interaction with this table is not possible if any one is changing the value. but this got me some errors when testing it. it was because that sometimes the same connection is trying to change the value while it is on a `transaction` already.
so this solution is not a viable solution too.


## Third Approach: 
this was the best approach, I used a class `Numbering` that holds a value that cannot be changed only by a thread at a time using `lock`, and used a `DI` to inject the class as a `Singleton` so there is only one instance of this class. no errors incountered.


