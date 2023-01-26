alter table dbo.UploadedResources
drop constraint FK_UploadedResources_Tenant
go

alter table dbo.UploadedResources
drop column TenantId