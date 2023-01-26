alter table dbo.PlacementTests
add ReportedLevelId int null
constraint FK_PlacementTest_ReportedLevel foreign key references dbo.Levels(Id);