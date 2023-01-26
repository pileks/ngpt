create table dbo.Languages(
    Id int identity(1,1) not null constraint PK_Languages primary key,
  
    Name nvarchar(max) not null
)

go

insert into dbo.Languages
values
	('English'),
	('Italian'),
	('Spanish'),	
	('German'),	
	('French'),
	('Russian')
go

alter table dbo.ListeningQuestions
	add LanguageId int not null constraint FK_ListeningQuestions_Language foreign key references dbo.Languages(Id)
	constraint DF_ListeningQuestions_Language default 1
go

alter table dbo.ReadingQuestions
	add LanguageId int not null constraint FK_ReadingQuestions_Language foreign key references dbo.Languages(Id)
	constraint DF_ReadingQuestions_Language default 1
go

alter table dbo.UseOfLanguageQuestions
	add LanguageId int not null constraint FK_UseOfLanguageQuestions_Language foreign key references dbo.Languages(Id)
	constraint DF_UseOfLanguageQuestions_Language default 1
go

alter table dbo.ReadingQuestionTexts
	add LanguageId int not null constraint FK_ReadingTexts_Language foreign key references dbo.Languages(Id)
	constraint DF_ReadingQuestionTexts_Language default 1
go

alter table dbo.ListeningQuestionAudios
	add LanguageId int not null constraint FK_ListeningAudios_Language foreign key references dbo.Languages(Id)
	constraint DF_ListeningQuestionAudios_Language default 1
go

alter table dbo.ListeningQuestions
	drop constraint DF_ListeningQuestions_Language
go

alter table dbo.ReadingQuestions
	drop constraint DF_ReadingQuestions_Language
go

alter table dbo.UseOfLanguageQuestions
	drop constraint DF_UseOfLanguageQuestions_Language
go

alter table dbo.ReadingQuestionTexts
	drop constraint DF_ReadingQuestionTexts_Language
go

alter table dbo.ListeningQuestionAudios
	drop constraint DF_ListeningQuestionAudios_Language
go