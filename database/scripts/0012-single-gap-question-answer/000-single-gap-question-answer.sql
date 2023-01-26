create table dbo.SingleGapQuestionAnswers(
    Id int not null identity(1, 1) constraint PK_SingleGapQuestionAnswers primary key,

    [Text] nvarchar(200) not null,
    IsCaseSensitive bit not null,

    SingleGapQuestionId int not null 
        constraint FK_SingleGapQuestionAnswer_SingleGapQuestion foreign key references dbo.SingleGapQuestions(Id),

    UserId int not null constraint FK_SingleGapQuestionAnswers_User foreign key references dbo.Users(Id),
	TenantId int not null constraint FK_SingleGapQuestionAnswers_Tenant foreign key references dbo.Tenants(Id)
);
go

insert into dbo.SingleGapQuestionAnswers([Text], IsCaseSensitive, SingleGapQuestionId, UserId, TenantId)
select s.Answer, s.CaseSensitive, s.Id, s.UserId, s.TenantId from dbo.SingleGapQuestions s;
go

alter table dbo.SingleGapQuestions
drop column Answer;

alter table dbo.SingleGapQuestions
drop column CaseSensitive;