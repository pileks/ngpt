create table dbo.SingleAnswerQuestions(
    Id int identity(1,1) not null constraint PK_SingleAnswerQuestions primary key,
	
    [QuestionText] nvarchar(max) not null,
    AnswerType int not null,

	UserId int not null constraint FK_SingleAnswerQuestion_User foreign key references dbo.Users(Id),
	TenantId int not null constraint FK_SingleAnswerQuestion_Tenant foreign key references dbo.Tenants(Id)
);

go

create table dbo.SingleAnswerQuestionAnswers(
    Id int identity(1,1) not null constraint PK_SingleAnswerQuestionAnswers primary key,

    IsCorrect bit not null,
    [Text] nvarchar(max) null,
    
    ImageId int null constraint FK_SingleAnswerQuestionAnswer_Image foreign key references dbo.UploadedResources(Id),
    QuestionId int not null constraint FK_SingleAnswerQuestionAnswer_Question foreign key references dbo.SingleAnswerQuestions(Id),
);

go