alter table dbo.ListeningQuestionAudios
add LevelId int not null 
constraint DF_LQA_Level default(1)
constraint FK_ListeningQuestionAudio_Level foreign key references dbo.Levels(Id);
go

alter table dbo.ListeningQuestionAudios
drop constraint DF_LQA_Level;
go

update dbo.ListeningQuestions
set LevelId = 5
where Id = 6;

go

update dbo.ListeningQuestionAudios
set ListeningQuestionAudios.LevelId = (
    select top 1 dbo.ListeningQuestions.LevelId 
    from dbo.ListeningQuestions
    where ListeningQuestions.AudioId = ListeningQuestionAudios.Id
);

go

alter table dbo.ListeningQuestions
drop constraint FK_ListeningQuestions_Language;

alter table dbo.ListeningQuestions
drop column LanguageId;

alter table dbo.ListeningQuestions
drop constraint FK_ListeningQuestions_Levels;

alter table dbo.ListeningQuestions
drop column LevelId;