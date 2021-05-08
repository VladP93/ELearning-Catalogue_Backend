CREATE DATABASE elearning_catalog;
GO

USE elearning_catalog;
GO

CREATE TABLE Course(
	CourseId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	Title NVARCHAR(500) NOT NULL,
	CourseDescription NVARCHAR(1000) NOT NULL,
	PublicationDate DATETIME NOT NULL,
	CoverPhoto VARBINARY(MAX) NOT NULL
);

CREATE TABLE Price(
	PriceId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	ActualPrice MONEY NOT NULL,
	PromotionPrice MONEY,
	CourseId INT NOT NULL,
	FOREIGN KEY (CourseId) REFERENCES Course(CourseId)
);

CREATE TABLE Commentary(
	CommentaryId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	Student NVARCHAR(500) NOT NULL,
	Score INT NOT NULL,
	CommentaryText NVARCHAR(MAX),
	CourseId INT NOT NULL,
	FOREIGN KEY (CourseId) REFERENCES Course(CourseId)
);

CREATE TABLE Instructor(
	InstructorId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	FirstName VARCHAR(200) NOT NULL,
	LastName VARCHAR(200) NOT NULL,
	Degree VARCHAR(200) NOT NULL,
	ProfilePhoto VARBINARY(MAX)
);

CREATE TABLE CourseInstructor(
	CourseId INT NOT NULL,
	InstructorId INT NOT NULL,
	PRIMARY KEY(CourseId, InstructorId),
	FOREIGN KEY (CourseId) REFERENCES Course(CourseId),
	FOREIGN KEY (InstructorId) REFERENCES Instructor(InstructorId)
);