DELETE FROM [dbo].[Loan]
DBCC CHECKIDENT ('Checkpoint_IT_1stPass.dbo.Loan',RESEED, 0)

DELETE FROM [dbo].[LoanAttachment
DBCC CHECKIDENT ('Checkpoint_IT_1stPass.dbo.LoanAttachment',RESEED, 0)
DELETE FROM [dbo].[LoanCreditHistory]
DBCC CHECKIDENT ('Checkpoint_IT_1stPass.dbo.LoanCreditHistory',RESEED, 0)
DELETE FROM [dbo].[LoanCustomerChildren]
DBCC CHECKIDENT ('Checkpoint_IT_1stPass.dbo.LoanCustomerChildren',RESEED, 0)
DELETE FROM [dbo].[LoanPersonalProperty]
DBCC CHECKIDENT ('Checkpoint_IT_1stPass.dbo.LoanPersonalProperty',RESEED, 0)
DELETE FROM [dbo].[LoanUnitDesired]
DBCC CHECKIDENT ('Checkpoint_IT_1stPass.dbo.LoanUnitDesired',RESEED, 0)

DELETE FROM [dbo].[LoanUnitDesiredTC]
DBCC CHECKIDENT ('Checkpoint_IT_1stPass.dbo.LoanUnitDesiredTC',RESEED, 0)
