create table dbo.SentEmails(
    Id int identity(1,1) not null constraint PK_SentEmails primary key,
	
	[From] nvarchar(max) not null,
	[To] nvarchar(max) not null,
	
	[Subject] nvarchar(max) not null,
	[HtmlBody] nvarchar(max) not null,
	[PlainTextBody] nvarchar(max) not null,
	
	[Date] datetime2 not null
)

go