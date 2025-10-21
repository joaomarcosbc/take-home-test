IF DB_ID('FundoDb') IS NOT NULL
BEGIN
    ALTER DATABASE FundoDb SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE FundoDb;
END
GO

CREATE DATABASE FundoDb;
GO

USE FundoDb;
GO

IF OBJECT_ID('dbo.Loans', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Loans (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Amount DECIMAL(18,2) NOT NULL,
        CurrentBalance DECIMAL(18,2) NOT NULL,
        ApplicantName NVARCHAR(100) NOT NULL,
        Status INT NOT NULL,
        DateCreated DATETIME NOT NULL DEFAULT GETDATE(),
        DateModified DATETIME NULL
    );
END
GO

IF OBJECT_ID('dbo.LoanPayments', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.LoanPayments (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        LoanId INT NOT NULL,
        Amount DECIMAL(18,2) NOT NULL,
        DateCreated DATETIME NOT NULL DEFAULT GETDATE(),
        DateModified DATETIME NULL,
        CONSTRAINT FK_LoanPayments_Loans FOREIGN KEY (LoanId) REFERENCES dbo.Loans(Id)
    );
END
GO

INSERT INTO dbo.Loans (Amount, CurrentBalance, ApplicantName, Status, DateCreated)
VALUES
(10000, 10000, 'João Silva', 0, GETDATE()),
(20000, 15000, 'Maria Oliveira', 0, GETDATE()),
(5000, 0, 'Carlos Souza', 1, GETDATE()),
(12000, 4000, 'Fernanda Lima', 0, GETDATE()),
(30000, 0, 'Rafael Santos', 1, GETDATE()),
(15000, 7500, 'Beatriz Almeida', 0, GETDATE()),
(8000, 0, 'Lucas Pereira', 1, GETDATE()),
(25000, 25000, 'Juliana Costa', 0, GETDATE()),
(6000, 0, 'Paulo Henrique', 1, GETDATE()),
(18000, 18000, 'Tatiane Melo', 0, GETDATE()),
(9000, 0, 'Roberto Dias', 1, GETDATE()),
(22000, 11000, 'Amanda Rocha', 0, GETDATE()),
(17000, 0, 'Fábio Moura', 1, GETDATE()),
(11000, 5500, 'Camila Ferreira', 0, GETDATE()),
(14000, 0, 'Renato Gonçalves', 1, GETDATE());
GO

INSERT INTO dbo.LoanPayments (LoanId, Amount, DateCreated)
VALUES
(2, 5000, GETDATE()),
(4, 1000, GETDATE()),
(6, 5000, GETDATE()),
(12, 10000, GETDATE()),
(14, 5500, GETDATE());
GO
