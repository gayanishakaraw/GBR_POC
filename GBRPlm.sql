USE [master]
GO

/****** Object:  Database [GBRPLM]    Script Date: 8/5/2017 11:21:34 AM ******/
CREATE DATABASE [GBRPLM]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'GBRPLM', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\GBRPLM.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'GBRPLM_log', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\GBRPLM_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO

ALTER DATABASE [GBRPLM] SET COMPATIBILITY_LEVEL = 110
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [GBRPLM].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [GBRPLM] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [GBRPLM] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [GBRPLM] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [GBRPLM] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [GBRPLM] SET ARITHABORT OFF 
GO

ALTER DATABASE [GBRPLM] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [GBRPLM] SET AUTO_CREATE_STATISTICS ON 
GO

ALTER DATABASE [GBRPLM] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [GBRPLM] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [GBRPLM] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [GBRPLM] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [GBRPLM] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [GBRPLM] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [GBRPLM] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [GBRPLM] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [GBRPLM] SET  DISABLE_BROKER 
GO

ALTER DATABASE [GBRPLM] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [GBRPLM] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [GBRPLM] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [GBRPLM] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [GBRPLM] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [GBRPLM] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [GBRPLM] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [GBRPLM] SET RECOVERY SIMPLE 
GO

ALTER DATABASE [GBRPLM] SET  MULTI_USER 
GO

ALTER DATABASE [GBRPLM] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [GBRPLM] SET DB_CHAINING OFF 
GO

ALTER DATABASE [GBRPLM] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [GBRPLM] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO

ALTER DATABASE [GBRPLM] SET  READ_WRITE 
GO

