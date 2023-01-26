alter table dbo.ListeningQuestionAudios
add 
    Approved bit not null constraint DF_ListeningQuestionAudio_Approved default(0),
    ApproverId int null constraint FK_ListeningQuestionAudio_User foreign key references dbo.Users(Id);

go

alter table dbo.ListeningQuestionAudios
drop constraint DF_ListeningQuestionAudio_Approved;

go