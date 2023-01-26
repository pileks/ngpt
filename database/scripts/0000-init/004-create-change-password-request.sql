create table dbo.ChangePasswordRequests(
    Id int identity(1,1) not null constraint PK_ChangePasswordRequests primary key,
	
	[Uid] nvarchar(max) not null,
	
    UserId int not null constraint FK_ChangePasswordRequests_User foreign key references dbo.Users(Id),
    SentEmailId int not null constraint FK_ChangePasswordRequests_SentEmail foreign key references dbo.SentEmails(Id)
)

go