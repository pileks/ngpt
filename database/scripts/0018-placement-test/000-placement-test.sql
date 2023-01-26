create table dbo.PlacementTests(
    Id int not null identity(1, 1) constraint PK_PlacementTests primary key,

    Rating float(53) not null,
    Rd float(53) not null,
    Vol float(53) not null,

    LanguageId int not null constraint FK_PlacementTest_Language foreign key references dbo.Languages(Id)
);