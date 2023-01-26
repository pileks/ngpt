create table dbo.Tenants(
    Id int identity(1,1) not null constraint PK_Tenants primary key,
	
    Name nvarchar(max) not null,
    DisplayName nvarchar(max) not null
)