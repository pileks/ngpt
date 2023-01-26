alter table dbo.AuthTokens
	add TenantId int not null constraint FK_AuthTokens_Tenant foreign key references dbo.Tenants(Id)
go

create index IDX_AuthTokens_TenantId
	on dbo.AuthTokens (TenantId);
go