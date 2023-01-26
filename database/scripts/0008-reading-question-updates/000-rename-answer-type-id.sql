alter table dbo.ReadingQuestions
drop column AnswerTypeId;

alter table dbo.ReadingQuestions
add AnswerType int not null;