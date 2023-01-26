create table dbo.PlacementTestQuestions(
    Id int not null identity(1, 1) constraint PK_PlacementTestQuestions primary key,

    Rating float(53) not null,
    Rd float(53) not null,
    Vol float(53) not null,

    IsAnsweredCorrectly bit not null,

    QuestionId int not null constraint FK_PlacementTestQuestion_Question foreign key references dbo.UseOfLanguageQuestions(Id),
    PlacementTestId int not null constraint FK_PlacementTestQuestion_PlacementTest foreign key references dbo.PlacementTests(Id),
);

alter table dbo.PlacementTests
add StartedOn datetime2 not null
constraint DF_PlacementTest_StartedOn default(N'2022-01-01');

alter table dbo.PlacementTests
drop constraint DF_PlacementTest_StartedOn;

alter table dbo.PlacementTests
add CompletedOn datetime2 null;