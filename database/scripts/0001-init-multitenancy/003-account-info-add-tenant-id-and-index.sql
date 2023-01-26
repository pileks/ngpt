alter table dbo.AccountInfos
	add TenantId int not null constraint FK_AccountInfos_Tenant foreign key references dbo.Tenants(Id)
go

create index IDX_AccountInfos_TenantId
	on dbo.AccountInfos (TenantId);
go