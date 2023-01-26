alter table dbo.AccountInfos
	add HasAcceptedTermsAndPrivacyPolicy bit not null constraint DF_AccountInfos_HasAcceptedTermsAndPrivacyPolicy default(1)  
go

alter table dbo.AccountInfos
	drop constraint DF_AccountInfos_HasAcceptedTermsAndPrivacyPolicy
go