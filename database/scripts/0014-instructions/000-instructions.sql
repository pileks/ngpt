create table dbo.Instructions(
    Id int not null identity(1,1) constraint PK_Instructions primary key,

    [Text] nvarchar(max) not null,
    QuestionType int not null,

    LanguageId int not null constraint FK_Instruction_Langage foreign key references dbo.Languages(Id)
);