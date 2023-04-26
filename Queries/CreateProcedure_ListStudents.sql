SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[usp_GetAllStudents]
AS
BEGIN
	SELECT [ID],[LastName],[MiddleName],[FirstName],[Gender],[EnrollmentDate] FROM [dbo].[Student];
END
GO
