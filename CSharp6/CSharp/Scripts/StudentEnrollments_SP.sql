USE [University]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[StudentEnrollments] @StudentId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT S.FirstName, S.MiddleName, S.LastName, C.Title, C.Credits 
	FROM Students S, Enrollments E, Courses C 
	WHERE S.StudentId = E.StudentId AND C.CourseId = E.CourseId
END
GO
