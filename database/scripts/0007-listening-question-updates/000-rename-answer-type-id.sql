alter table dbo.ListeningQuestions
drop column AnswerTypeId;

alter table dbo.ListeningQuestions
add AnswerType int not null;