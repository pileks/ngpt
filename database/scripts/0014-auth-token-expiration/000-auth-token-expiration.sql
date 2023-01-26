delete from dbo.AuthTokens
go

alter table dbo.AuthTokens
add ExpirationDate datetime2 not null
go
