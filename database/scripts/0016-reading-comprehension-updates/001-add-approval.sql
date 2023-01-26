alter table dbo.ReadingQuestionTexts
add 
    Approved bit not null constraint DF_ReadingQuestionText_Approved default(0),
    ApproverId int null constraint FK_ReadingQuestionText_User foreign key references dbo.Users(Id);

go

alter table dbo.ReadingQuestionTexts
drop constraint DF_ReadingQuestionText_Approved;

go