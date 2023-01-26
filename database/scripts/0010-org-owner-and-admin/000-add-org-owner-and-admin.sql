alter table dbo.Tenants
	add OrgOwnerId int null constraint FK_Tenants_OrgOwner foreign key references dbo.Users(Id)
go

alter table dbo.Users
	add IsOrgAdmin bit not null
go