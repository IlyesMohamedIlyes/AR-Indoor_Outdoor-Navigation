# AR-Indoor_Outdoor-Navigation
This is an AR Project.

## Description
This project was made to be presented to get my bachlor degree in the University of Science and Technology Houari Boumedienne (**USTHB**) at Beb Ezzouar, Algiers, Algeria. 
The application is made with Unity engine. I used android mobiles to test the app, which were : Xperia Z3, Xperia Z5 and Xperia C5 Ultra.
The scripts wrote with C# are not quite commented. Will do my best to perfom it.


## ATENTION
**DO NOT** MODIFY SQLite Database. If you are not from USTHB, Algeria and you want to test in your building, you can add a DB with the same name and data you may want.
**DO NOT** migrate to MapBox SDK version 1.4 in master branch, the new one has a lot of primary changes. It will take some time to pass to new version.


## TODO
- [ ] Enhance indoor localisation with [IndoorAtlas SDK](https://www.indooratlas.com/) ;
- [ ] Verify deplacement calculation, on [StepCounter.cs](Assets/Scripts/StepCounter.cs) ;
- [ ] Eliminate useless direction calculations when outdoor navigation.
        
## Screenshots
* <b>Choose where you want to go<b>
<img width="250" src="Screenshots/Screenshot_list-of-rooms-in-CSdepartment.png">


        
## Tools & languages
* Main language **C#** ; 
* [Unity3d](https://unity.com) ; 
* [MapBox](https://www.mapbox.com) v1.3 : Its Unity3d SDK gived the chance to fulfil a complete mission in visualising maps, getting directions ; 
* [SQLite](https://sqlite.org) : for the database creation. This database contains GPS coordinates of professors/classes/checkpoints ; 
* [Vuforia](https://developer.vuforia.com) : Its Unity3d SDK helped in recognizing checkpoints patterns.
