create table dbo.AuthTokens(
    Id int identity(1,1) not null constraint PK_AuthTokens primary key,
	
    Guid nvarchar(max) not null,
	
    UserId int not null constraint FK_AuthTokens_User foreign key references dbo.Users(Id)
)

go