alter table dbo.Users
	add HasVerifiedEmail bit not null constraint DF_Users_HasVerifiedEmail default (0)
go

alter table dbo.Users
	drop constraint DF_Users_HasVerifiedEmail
go
