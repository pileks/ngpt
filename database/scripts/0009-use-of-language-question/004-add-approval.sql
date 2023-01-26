alter table dbo.UseOfLanguageQuestions
add 
    Approved bit not null constraint DF_UseOfLanguageQuestion_Approved default(0),
    ApproverId int null constraint FK_UseOfLanguageQuestion_User foreign key references dbo.Users(Id);

go

alter table dbo.UseOfLanguageQuestions
drop constraint DF_UseOfLanguageQuestion_Approved;

go