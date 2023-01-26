delete from dbo.ReadingQuestionAnswers;
delete from dbo.ReadingQuestions;
go

alter table dbo.ReadingQuestionTexts
add LevelId int not null 
constraint DF_RQT_Level default(1)
constraint FK_ReadingQuestionText_Level foreign key references dbo.Levels(Id);
go

alter table dbo.ReadingQuestionTexts
drop constraint DF_RQT_Level;
go

alter table dbo.ReadingQuestions
drop constraint FK_ReadingQuestions_Language;

alter table dbo.ReadingQuestions
drop column LanguageId;

alter table dbo.ReadingQuestions
drop constraint FK_ReadingQuestions_Level;

alter table dbo.ReadingQuestions
drop column LevelId;