/*
Navicat SQL Server Data Transfer

Source Server         : MSSQL_localhost
Source Server Version : 110000
Source Host           : localhost:1433
Source Database       : newareshare
Source Schema         : dbo

Target Server Type    : SQL Server
Target Server Version : 110000
File Encoding         : 65001

Date: 2019-03-19 13:15:55
*/


-- ----------------------------
-- Table structure for t_data
-- ----------------------------
DROP TABLE [dbo].[t_data]
GO
CREATE TABLE [dbo].[t_data] (
[Id] int NULL ,
[key] nvarchar(100) NULL ,
[value] nvarchar(2000) NULL ,
[status] int NULL ,
[desc] nvarchar(255) NULL 
)


GO

-- ----------------------------
-- Records of t_data
-- ----------------------------
INSERT INTO [dbo].[t_data] ([Id], [key], [value], [status], [desc]) VALUES (N'1', N'chargeCodes', N'{"trayCode":"code0001","batteryCodes":"code001,code002,...,code032"}', N'1', N'充电位电芯条码信息')
GO
GO
INSERT INTO [dbo].[t_data] ([Id], [key], [value], [status], [desc]) VALUES (N'2', N'dischargeCodes', N'{"trayCode":"code0001","batteryCodes":"code001,code002,...,code032"}', N'1', N'放电位电芯条码信息')
GO
GO
INSERT INTO [dbo].[t_data] ([Id], [key], [value], [status], [desc]) VALUES (N'3', N'sortingResults', N'{"trayCode":"code0001","results":"1,2,2,3,4,...,8"}', N'1', N'分选结果')
GO
GO
