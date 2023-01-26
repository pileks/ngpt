create table dbo.Users(
    Id int identity(1,1) not null constraint PK_Users primary key,

    Email nvarchar(200) not null,
	Password nvarchar(200) not null,
	IsActive bit not null,
	IsSuperAdmin bit not null
)

go