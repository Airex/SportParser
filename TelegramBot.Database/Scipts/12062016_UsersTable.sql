IF not EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.Users') AND type in (N'U'))
Begin
CREATE TABLE dbo.Users (
	 ID				int				NOT NULL IDENTITY(1, 1)
	,userIdRef			int				NOT NULL	
	,CONSTRAINT PK_Users PRIMARY KEY CLUSTERED (ID)
)
end;
GO

IF not EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.Settings') AND type in (N'U'))
Begin
CREATE TABLE dbo.Settings (
	 ID				int				NOT NULL IDENTITY(1, 1)
	,userId			int				NOT NULL
	,name			nvarchar(100)	NOT NULL
	,value			nvarchar(max)	NULL
	,CONSTRAINT PK_Settings PRIMARY KEY CLUSTERED (ID)
	,CONSTRAINT FK_Settings_Users FOREIGN KEY (userId) REFERENCES dbo.Users(ID)
)
end;
GO