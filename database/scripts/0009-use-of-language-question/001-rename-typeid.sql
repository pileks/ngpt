alter table dbo.UseOfLanguageQuestions
drop column TypeId;

alter table dbo.UseOfLanguageQuestions
add [Type] int not null;