alter table dbo.UseOfLanguageQuestions
add InstructionId int null
constraint FK_UseOfLangaugeQuestion_Instruction foreign key references dbo.Instructions(Id);

go

update dbo.UseOfLanguageQuestions
set InstructionId = (select top 1 Id from dbo.Instructions);

go

alter table dbo.UseOfLanguageQuestions
alter column InstructionId int not null;