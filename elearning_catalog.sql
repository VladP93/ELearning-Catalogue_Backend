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
			i.Degree,
			i.CreatedAt
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

		INSERT INTO Instructor(InstructorId, FirstName, LastName, Degree, CreatedAt)
		VALUES(@InstructorId, @FirstName, @LastName, @Degree, GETUTCDATE())

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
			Degree = @Degree,
			CreatedAt = GETUTCDATE()
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
		Degree,
		CreatedAt
	FROM Instructor WHERE
		InstructorId = @InstructorId

	END
;

CREATE PROCEDURE usp_get_course_pagination(
	@courseTitle nvarchar(500),
	@pageNumber int,
	@quantity int,
	@columnSort nvarchar(500),
	@totalRecords int OUTPUT,
	@totalPages int OUTPUT
)
AS
	BEGIN

		SET NOCOUNT ON
		SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

		DECLARE @start int
		DECLARE @end int

		SET @end = @pageNumber * @quantity

		IF @pageNumber = 1
			BEGIN
				SET @start = (@pageNumber * @quantity) -@quantity
			END
		ELSE
			BEGIN
				SET @start = ( (@pageNumber * @quantity) - @quantity) + 1
			END

		CREATE TABLE #TMP(
			rowNumber INT IDENTITY(1,1),
			Id UNIQUEIDENTIFIER
		)

		DECLARE @SQL nvarchar(max)
		SET @SQL = 'SELECT CourseId FROM Course'
		
		IF @courseTitle IS NOT NULL
			BEGIN 
				SET @SQL = @SQL + ' WHERE Title LIKE ''%' + @courseTitle + '%'' '
			END

		IF @columnSort IS NOT NULL
			BEGIN
				SET @SQL = @SQL + ' ORDER BY ' + @columnSort
			END

		INSERT INTO #TMP(Id)
		EXEC sp_executesql @SQL

		SELECT @totalRecords = COUNT(*) FROM #TMP

		IF @totalRecords > @quantity
			BEGIN
				SET @totalPages = @totalRecords / @quantity
				IF (@totalRecords % @quantity) > 0
					BEGIN
						SET @totalPages = @totalPages + 1
					END
			END
		ELSE
			BEGIN
				SET @totalPages = 1
			END

		SELECT 
			c.CourseId,
			c.Title,
			c.CourseDescription,
			c.PublicationDate,
			c.CoverPhoto,
			c.CreatedAt,
			p.ActualPrice,
			p.PromotionPrice
		FROM #TMP t INNER JOIN Course c
			ON t.Id = c.CourseId
		LEFT JOIN Price p
			ON p.CourseId = c.CourseId
		WHERE t.rowNumber >= @start AND t.rowNumber <= @end

	END
	;