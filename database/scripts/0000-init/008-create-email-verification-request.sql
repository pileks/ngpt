create table dbo.EmailVerificationRequests(
    Id int identity(1,1) not null constraint PK_EmailVerificationRequests primary key,
	
	Code nvarchar(max) not null,
	NumberOfVerificationAttempts int not null,
	CreatedOn datetime2 not null,

    UserId int not null constraint FK_EmailVerificationRequests_User foreign key references dbo.Users(Id),
    SentEmailId int not null constraint FK_EmailVerificationRequests_SentEmail foreign key references dbo.SentEmails(Id)
)

go