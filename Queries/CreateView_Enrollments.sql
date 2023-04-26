CREATE VIEW dbo.StudentEnrollment_View 
AS 
SELECT 
	e.Grade, 
	s.LastName, 
	s.MiddleName, 
	s.FirstName, 
	s.Gender, 
	s.EnrollmentDate, 
	c.Title, 
	c.Credits
FROM
	 dbo.Enrollment AS e
JOIN dbo.Student AS s
	ON e.StudentID = s.ID 
JOIN dbo.Course AS c
	ON e.CourseID = c.ID;