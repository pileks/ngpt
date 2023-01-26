create table dbo.PlacementTestInvitations(
    Id int not null identity(1, 1) constraint PK_PlacementTestInvitations primary key,

    Email nvarchar(1000) not null,
    Token nvarchar(50) not null constraint UQ_PlacementTestInvitation_Token unique,

    LanguageId int null constraint FK_PlacementTestInvitation_Language foreign key references dbo.Languages(Id),
    PlacementTestId int null constraint FK_PlacementTestInvitation_PlacementTest foreign key references dbo.PlacementTests(Id)
);