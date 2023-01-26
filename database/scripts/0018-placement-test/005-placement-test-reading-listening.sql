alter table dbo.PlacementTests
add ShouldTestReading bit not null
constraint DF_ShouldTestReading default(0);

alter table dbo.PlacementTests
add ShouldTestListening bit not null
constraint DF_ShouldTestListening default(0);

alter table dbo.PlacementTests
add ReadingTextCorrectAnswers int null;

alter table dbo.PlacementTests
add ReadingTextTotalAnswers int null;

alter table dbo.PlacementTests
add ListeningAudioCorrectAnswers int null;

alter table dbo.PlacementTests
add ListeningAudioTotalAnswers int null;

alter table dbo.PlacementTests
add ReadingQuestionTextId int null
constraint FK_PlacementTest_ReadingQuestionText foreign key references dbo.ReadingQuestionTexts(Id);

alter table dbo.PlacementTests
add ListeningQuestionAudioId int null
constraint FK_PlacementTest_ListeningQuestionAudio foreign key references dbo.ListeningQuestionAudios(Id);
