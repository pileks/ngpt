alter table dbo.Users
	add TenantId int not null constraint FK_Users_Tenant foreign key references dbo.Tenants(Id)
go

create index IDX_Users_TenantId
	on dbo.Users (TenantId);
go