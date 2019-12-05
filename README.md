# CS4173CompSecurityProject
### Authors: Ryan Hernandez and Treyson Martin

## How to run the website and application.
First you will have to set up the database and update the connection strings.

Below is query to create a DB in SQL Server and populate the tables.

CREATE Database CSKADB

Create Table CarInfo (  
   Id int primary Key,  
   Date_Created DateTime,  
   PhoneNum int,  
   Make varchar(30),  
   Model varchar(30),  
   [Owner] varchar(50)  
);

Create Table KeyInfo (  
   Id int primary key,  
   Date_Registered DateTime,  
   Car_Id int foreign key references CarInfo(Id),  
   Times_Called int,  
   Times_Successful int  
);

Then insert some values into CarInfo table.  
Ie.  
INSERT INTO CarInfo VALUES (id, datetime, phonenum, make, model, owner)  

Now you have to update the connection string the in appsetting for the web and console applications.

Creation of Twilio Account.  
Go to: https://www.twilio.com/try-twilio  

Once this is done you will have to update the twilio phone number in line 133 on CarKeyAuthenticationService.cs

You will have to upate the LINQ for the web.  
Line 30 in the CarInfoController. Change the owner to you to see your registered cars.