CREATE DATABASE elearning_catalog;
GO

USE elearning_catalog;
GO

CREATE TABLE Course(
	CourseId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	Title NVARCHAR(500) NOT NULL,
	CourseDescription NVARCHAR(1000) NOT NULL,
	PublicationDate DATETIME NOT NULL,
	CoverPhoto VARBINARY(MAX)
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

-- Creating user stored procedures

CREATE PROCEDURE usp_get_instructors
AS
	BEGIN
		SET NOCOUNT ON
		SELECT
			i.InstructorId,
			i.FirstName,
			i.LastName,
			i.Degree
		FROM Instructor i

	END
;

CREATE PROCEDURE usp_create_instructor(
	@InstructorId uniqueIdentifier,
	@FirstName nvarchar(500),
	@LastName nvarchar(500),
	@Degree nvarchar(100)
)
AS 
	BEGIN

		INSERT INTO Instructor(InstructorId, FirstName, LastName, Degree)
		VALUES(@InstructorId, @FirstName, @LastName, @Degree)

	END
;

CREATE PROCEDURE usp_update_instructor(
	@InstructorId uniqueIdentifier,
	@FirstName nvarchar(500),
	@LastName nvarchar(500),
	@Degree nvarchar(100)
)
AS
	BEGIN
		UPDATE Instructor SET
			FirstName = @FirstName,
			LastName = @LastName,
			Degree = @Degree
		WHERE
			InstructorId = @InstructorId

	END
;

CREATE PROCEDURE usp_delete_instructor(
	@InstructorId uniqueIdentifier
)
AS
	BEGIN

		DELETE FROM CourseInstructor WHERE InstructorId = @InstructorId

		DELETE FROM Instructor WHERE InstructorId = @InstructorId

	END
;

CREATE PROCEDURE usp_get_instructor_by_id(
	@InstructorId uniqueIdentifier
)
AS
	BEGIN

	SELECT
		InstructorId,
		FirstName,
		LastName,
		Degree
	FROM Instructor WHERE
		InstructorId = @InstructorId

	END
;