create table dbo.UploadedResources(
    Id int identity(1,1) not null constraint PK_UploadedResources primary key,
	
    Name nvarchar(max) not null,
	Guid nvarchar(max) not null,
	MimeType nvarchar(max) not null,
	CreatedOn datetime2 not null,
	IsInUse bit not null,
	
	TenantId int not null constraint FK_UploadedResources_Tenant foreign key references dbo.Tenants(Id)
)