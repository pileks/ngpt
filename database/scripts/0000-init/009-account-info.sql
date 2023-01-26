create table dbo.AccountInfos
(
    Id int identity(1,1) not null constraint PK_AccountInfos primary key,
    
    Name nvarchar(max) not null,
    UserId int not null constraint FK_AccountInfos_User foreign key references dbo.Users(Id)
)