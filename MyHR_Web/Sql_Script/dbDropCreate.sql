USE [master]
GO
IF EXISTS (select * from sys.databases where name = 'dbMyCompany')
BEGIN
	ALTER DATABASE [dbMyCompany] SET SINGLE_USER WITH ROLLBACK IMMEDIATE	
	ALTER DATABASE [dbMyCompany] SET SINGLE_USER;	
END

DROP DATABASE IF EXISTS [dbMyCompany]
GO
CREATE DATABASE [dbMyCompany]
GO