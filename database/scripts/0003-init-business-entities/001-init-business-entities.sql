create table dbo.Levels(
    Id int identity(1,1) not null constraint PK_Levels primary key,
  
    Title nvarchar(max) not null,
    FromRating int not null,
    RepresentativeRating int not null
)

go

create table dbo.MultipleChoiceQuestions(
    Id int identity(1,1) not null constraint PK_MultipleChoiceQuestions primary key,
	
		
	UserId int not null constraint FK_MultipleChoiceQuestions_User foreign key references dbo.Users(Id),
	TenantId int not null constraint FK_MultipleChoiceQuestions_Tenant foreign key references dbo.Tenants(Id)
)

go
create table dbo.MultipleChoiceQuestionAnswerParts(
    Id int identity(1,1) not null constraint PK_MultipleChoiceQuestionAnswerParts primary key,
	
	UserId int not null constraint FK_MultipleChoiceQuestionAnswerParts_User foreign key references dbo.Users(Id),
	TenantId int not null constraint FK_MultipleChoiceQuestionAnswerParts_Tenant foreign key references dbo.Tenants(Id)
)

go
create table dbo.MultipleChoiceQuestionTextParts(
    Id int identity(1,1) not null constraint PK_MultipleChoiceQuestionTextParts primary key,
  
    Text nvarchar(max) not null,
	
	UserId int not null constraint FK_MultipleChoiceQuestionTextParts_User foreign key references dbo.Users(Id),
	TenantId int not null constraint FK_MultipleChoiceQuestionTextParts_Tenant foreign key references dbo.Tenants(Id)
)

go

create table dbo.SingleGapQuestions(
    Id int identity(1,1) not null constraint PK_SingleGapQuestions primary key,
  
    TextBefore nvarchar(max) not null,
    TextAfter nvarchar(max) not null,
    Answer nvarchar(max) not null,
    CaseSensitive bit not null,
	
	UserId int not null constraint FK_SingleGapQuestions_User foreign key references dbo.Users(Id),
	TenantId int not null constraint FK_SingleGapQuestions_Tenant foreign key references dbo.Tenants(Id)
)

go

create table dbo.DragDropQuestions(
    Id int identity(1,1) not null constraint PK_DragDropQuestions primary key,
	
	UserId int not null constraint FK_DragDropQuestions_User foreign key references dbo.Users(Id),
	TenantId int not null constraint FK_DragDropQuestions_Tenant foreign key references dbo.Tenants(Id)
)

go

create table dbo.DragDropQuestionParts(
    Id int identity(1,1) not null constraint PK_DragDropQuestionParts primary key,
  
    Ordinal int not null,
    Text nvarchar(max) not null,
    IsDraggable bit not null,
    QuestionId int not null constraint FK_DragDropQuestionParts_Question foreign key references dbo.DragDropQuestions(Id),
	
	UserId int not null constraint FK_DragDropQuestionParts_User foreign key references dbo.Users(Id),
	TenantId int not null constraint FK_DragDropQuestionParts_Tenant foreign key references dbo.Tenants(Id)
)

go

create table dbo.MultipleChoiceQuestionParts(
    Id int identity(1,1) not null constraint PK_MultipleChoiceQuestionParts primary key,
  
    Ordinal int not null,
    TextPartId int not null constraint FK_MultipleChoiceQuestionParts_TextPart foreign key references dbo.MultipleChoiceQuestionTextParts(Id),
    AnswerPartId int not null constraint FK_MultipleChoiceQuestionParts_AnswerPart foreign key references dbo.MultipleChoiceQuestionAnswerParts(Id),
	QuestionId int not null constraint FK_MultipleChoiceQuestionParts_Question foreign key references dbo.MultipleChoiceQuestions(Id),
	
	UserId int not null constraint FK_MultipleChoiceQuestionParts_User foreign key references dbo.Users(Id),
	TenantId int not null constraint FK_MultipleChoiceQuestionParts_Tenant foreign key references dbo.Tenants(Id)
)

go
create table dbo.MutlipleChoiceQuestionAnswerPartOptions(
    Id int identity(1,1) not null constraint PK_MutlipleChoiceQuestionAnswerPartOptions primary key,
  
    Text nvarchar(max) not null,
    IsCorrect bit not null,
    AnswerPartId int not null constraint FK_MutlipleChoiceQuestionAnswerPartOptions_AnswerPart foreign key references dbo.MultipleChoiceQuestionAnswerParts(Id),
	
	UserId int not null constraint FK_MutlipleChoiceQuestionAnswerPartOptions_User foreign key references dbo.Users(Id),
	TenantId int not null constraint FK_MutlipleChoiceQuestionAnswerPartOptions_Tenant foreign key references dbo.Tenants(Id)
)

go
create table dbo.UseOfEnglishQuestions(
    Id int identity(1,1) not null constraint PK_UseOfEnglishQuestions primary key,
  
    LevelId int not null constraint FK_UseOfEnglishQuestions_Level foreign key references dbo.Levels(Id),
    TypeId int not null,
    MultipleChoiceQuestionId int not null constraint FK_UseOfEnglishQuestions_MultipleChoiceQuestion foreign key references dbo.MultipleChoiceQuestions(Id),
    SingleGapQuestionId int not null constraint FK_UseOfEnglishQuestions_SingleGapQuestion foreign key references dbo.SingleGapQuestions(Id),
    DragDropQuestionId int not null constraint FK_UseOfEnglishQuestions_DragDropQuestion foreign key references dbo.DragDropQuestions(Id),
	
	UserId int not null constraint FK_UseOfEnglishQuestions_User foreign key references dbo.Users(Id),
	TenantId int not null constraint FK_UseOfEnglishQuestions_Tenant foreign key references dbo.Tenants(Id)
)

go

create table dbo.ReadingQuestionTexts(
    Id int identity(1,1) not null constraint PK_ReadingQuestionTexts primary key,
  
    Title nvarchar(max) not null,
    Text nvarchar(max) not null,
	
	UserId int not null constraint FK_ReadingQuestionTexts_User foreign key references dbo.Users(Id),
	TenantId int not null constraint FK_ReadingQuestionTexts_Tenant foreign key references dbo.Tenants(Id)
)

go

create table dbo.ReadingQuestions(
    Id int identity(1,1) not null constraint PK_ReadingQuestions primary key,
  
    LevelId int not null constraint FK_ReadingQuestions_Level foreign key references dbo.Levels(Id),
    TextId int not null constraint FK_ReadingQuestions_Text foreign key references dbo.ReadingQuestionTexts(Id),
	QuestionText nvarchar(max) not null,
    AnswerTypeId int not null,
	
	UserId int not null constraint FK_ReadingQuestions_User foreign key references dbo.Users(Id),
	TenantId int not null constraint FK_ReadingQuestions_Tenant foreign key references dbo.Tenants(Id)
)

go
create table dbo.ReadingQuestionAnswers(
    Id int identity(1,1) not null constraint PK_ReadingQuestionAnswers primary key,
  
    Text nvarchar(max) not null,
    IsCorrect bit not null,
    Ordinal int not null,
    ImageId int not null constraint FK_ReadingQuestionAnswers_Image foreign key references dbo.UploadedResources(Id),
    QuestionId int not null constraint FK_ReadingQuestionAnswers_Question foreign key references dbo.ReadingQuestions(Id),
	
	UserId int not null constraint FK_ReadingQuestionAnswers_User foreign key references dbo.Users(Id),
	TenantId int not null constraint FK_ReadingQuestionAnswers_Tenant foreign key references dbo.Tenants(Id)
)

go

create table dbo.ListeningQuestionAudios(
    Id int identity(1,1) not null constraint PK_ListeningQuestionAudios primary key,
  
    Title nvarchar(max) not null,
    ResourceId int not null constraint FK_ListeningQuestionAudios_Resource foreign key references dbo.UploadedResources(Id),
	
	UserId int not null constraint FK_ListeningQuestionAudios_User foreign key references dbo.Users(Id),
	TenantId int not null constraint FK_ListeningQuestionAudios_Tenant foreign key references dbo.Tenants(Id)
)

go


create table dbo.ListeningQuestions(
    Id int identity(1,1) not null constraint PK_ListeningQuestions primary key,
  
    LevelId int not null constraint FK_ListeningQuestions_Levels foreign key references dbo.Levels(Id),
    QuestionText nvarchar(max) not null,
    AnswerTypeId int not null,
    AudioId int not null constraint FK_ListeningQuestions_Audio foreign key references dbo.ListeningQuestionAudios(Id),
	
	UserId int not null constraint FK_ListeningQuestions_User foreign key references dbo.Users(Id),
	TenantId int not null constraint FK_ListeningQuestions_Tenant foreign key references dbo.Tenants(Id)
)

go
create table dbo.ListeningQuestionAnswers(
    Id int identity(1,1) not null constraint PK_ListeningQuestionAnswers primary key,
  
    IsCorrect bit not null,
    Ordinal int not null,
    Text nvarchar(max) not null,
    ImageId int not null constraint FK_ListeningQuestionAnswers_Image foreign key references dbo.UploadedResources(Id),
    QuestionId int not null constraint FK_ListeningQuestionAnswers_Question foreign key references dbo.ListeningQuestions(Id),
	
	UserId int not null constraint FK_ListeningQuestionAnswers_User foreign key references dbo.Users(Id),
	TenantId int not null constraint FK_ListeningQuestionAnswers_Tenant foreign key references dbo.Tenants(Id)
)

go
