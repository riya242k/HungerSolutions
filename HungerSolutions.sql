-- HungerSolutions Database
-- Revision History:
-- John Danez, 2023.12.12: Job, jobDetails changed to NVARCHAR(MAX)
-- John Danez, 2023.11.28: Created


USE master
GO
 
IF DB_ID('HungerSolutions') IS NOT NULL
	DECLARE @killCommand NVARCHAR(MAX) = '';
	SELECT @killCommand += 'KILL ' + CAST(spid AS NVARCHAR) + ';'
	FROM sys.sysprocesses
	WHERE dbid = DB_ID('HungerSolutions');
	EXEC sp_executesql @killCommand;

	DROP DATABASE HungerSolutions
GO


CREATE DATABASE HungerSolutions
GO
 
-- Change to HungerSolutions database 
USE HungerSolutions
GO


 -- Create Tables
DROP TABLE IF EXISTS dbo.Restaurant; -- Dropping table in case it exists
CREATE TABLE Restaurant(
	RestaurantID INT NOT NULL IDENTITY
)


DROP TABLE IF EXISTS dbo.MenuItem; -- Dropping table in case it exists
CREATE TABLE MenuItem(
	MenuItemID INT NOT NULL IDENTITY
)


DROP TABLE IF EXISTS dbo.UserAccount; -- Dropping table in case it exists
CREATE TABLE UserAccount(
	UserAccountID INT NOT NULL IDENTITY
)

-- Add Columns
ALTER TABLE Restaurant 
ADD

	restaurantName VARCHAR (30) NOT NULL,
	rating INT NOT NULL, -- put any random number 1-5 for now
    imageName VARCHAR(50) NOT NULL, -- we will get names once team is done searching for images data
    restaurantLocation VARCHAR(100) NOT NULL, -- try to keep the location length for all records symetrical so we dont have to cut charachters while designing
    phone VARCHAR(10) CHECK (LEN(phone) = 10) -- you can remove it if you feel like
;

ALTER TABLE MenuItem
ADD

	itemName VARCHAR (255) NOT NULL,
	smallDescription VARCHAR (255) NOT NULL,
	category VARCHAR (30) NOT NULL, -- veg/non veg
	price DECIMAL(3,2) NOT NULL,
	imageName VARCHAR(50), -- we will get names once team is done searching for images data
	restaurantID INT NOT NULL -- FK Restaurant table
;

ALTER TABLE UserAccount
ADD

	userName VARCHAR (255) NOT NULL,
	email VARCHAR(100) UNIQUE NOT NULL CHECK (email LIKE '%_@__%.__%'),
	phone VARCHAR(10) CHECK (LEN(phone) = 10),
	passphrase VARCHAR(255) NOT NULL
;

-- Set PKs
ALTER TABLE Restaurant 
ADD 
	CONSTRAINT PK_RestaurantID
    PRIMARY KEY (restaurantID);


ALTER TABLE MenuItem 
ADD 
	CONSTRAINT PK_MenuItemID
    PRIMARY KEY (menuItemID);

ALTER TABLE UserAccount 
ADD 
	CONSTRAINT PK_UserAccountID
    PRIMARY KEY (userAccountID);


-- Set FKs
ALTER TABLE MenuItem 
ADD 
	CONSTRAINT FK_MenuItem_RestaurantID 
	FOREIGN KEY (restaurantID)
	REFERENCES Restaurant(restaurantID);


 -- initialize data
INSERT INTO UserAccount (userName, email, phone, passphrase)
VALUES
    ('Alice Smith','alice.smith@example.com', '9876543210', 'strongpassword'),
    ('Bob Johnson', 'bob.johnson@example.com', '5678901234', 'safeandsecure'),
    ('Emily Jones', 'emily.jones@example.com', '8765432109', 'password123'),
    ('David Brown', 'david.brown@example.com', '2345678901', 'userpass'),
    ('Eva Martin', 'eva.martin@company.com', '0123456789', 'secure'),
    ('Robert White', 'robert.white@company.com', '7890123456', 'secure123'),
    ('Grace Anderson', 'grace.anderson@company.com', '4567890123', 'password');
GO

SELECT * FROM UserAccount;

INSERT INTO Restaurant(restaurantName, rating, imageName, restaurantLocation, phone)
VALUES
    ('Northern Thai Restuarant', '3', 'abc.png', 'Queen St. S, Kitchener', 4567890123); 
GO

SELECT * FROM Restaurant;

INSERT INTO MenuItem(itemName, smallDescription, price, category, imageName, restaurantID)
VALUES
    ('Thai Spring Rolls – Porpai-Tod ', 'A flavorful meat stuffing covered in a light and crispy wrap. Served with our sweet and sour sauce', 6.50, 'Non-Vegeterian', 'test.jpg', 1);
GO

SELECT * FROM MenuItem;



-- update test:
UPDATE Restaurant SET imageName = 'Northern Thai Restaurant.jpg' WHERE RestaurantID = 1 

	--query test: 
SELECT * FROM Restaurant;
SELECT * FROM MenuItem;
SELECT * FROM UserAccount;
GO