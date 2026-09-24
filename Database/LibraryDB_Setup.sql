/* =====================================================================
   LibraryDB - إنشاء كامل + بيانات تجريبية
   الملف ده ممكن يتشغل أكتر من مرة (بيمسح الجداول القديمة الأول).
   ⚠ بيمسح البيانات الموجودة في الجداول دي!
   ===================================================================== */

IF DB_ID('LibraryDB') IS NULL
    CREATE DATABASE LibraryDB;
GO

USE LibraryDB;
GO

/* ---------- مسح الجداول القديمة (بالترتيب الصح) ---------- */
IF OBJECT_ID('Payments', 'U') IS NOT NULL DROP TABLE Payments;
IF OBJECT_ID('Fines', 'U') IS NOT NULL DROP TABLE Fines;
IF OBJECT_ID('Loans', 'U') IS NOT NULL DROP TABLE Loans;
IF OBJECT_ID('MembershipHistory', 'U') IS NOT NULL DROP TABLE MembershipHistory;
IF OBJECT_ID('Members', 'U') IS NOT NULL DROP TABLE Members;
GO

/* =========================================
   1. جداول الأعضاء
   ========================================= */
CREATE TABLE Members(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId NVARCHAR(450) NOT NULL UNIQUE,
    MembershipDate DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    Status NVARCHAR(20) NOT NULL DEFAULT 'Active',
    Address NVARCHAR(250) NULL,
    Phone NVARCHAR(20) NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,

    -- تم التعطيل مؤقتاً لتجنب خطأ AspNetUsers
    -- CONSTRAINT FK_Members_AspNetUsers_UserId
    --     FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE CASCADE,

    CONSTRAINT CHK_Member_Status CHECK (Status IN ('Active', 'Suspended', 'Expired'))
);
GO

CREATE TABLE MembershipHistory(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    MemberId INT NOT NULL,
    OldStatus NVARCHAR(20) NOT NULL,
    NewStatus NVARCHAR(20) NOT NULL,
    ChangedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    Reason NVARCHAR(250) NULL,

    CONSTRAINT FK_MembershipHistory_Members_MemberId
        FOREIGN KEY (MemberId) REFERENCES Members(Id) ON DELETE CASCADE
);
GO

/* =========================================
   2. جدول الإعارات (Loans)
   ========================================= */
CREATE TABLE Loans (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    MemberId INT NOT NULL,
    BookCopyId INT NOT NULL,
    BorrowDate DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    DueDate DATETIME2 NOT NULL,
    ReturnDate DATETIME2 NULL,
    Status INT NOT NULL DEFAULT 0,
    RenewalCount INT NOT NULL DEFAULT 0
);
GO

CREATE INDEX IX_Loans_MemberId ON Loans(MemberId);
CREATE INDEX IX_Loans_BookCopyId ON Loans(BookCopyId);
CREATE INDEX IX_Loans_Status ON Loans(Status);
GO

/* =========================================
   3. جداول الغرامات والمدفوعات
   ========================================= */
CREATE TABLE Fines (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    LoanId INT NOT NULL,
    MemberId INT NOT NULL,
    Amount DECIMAL(10,2) NOT NULL,
    Reason NVARCHAR(250) NOT NULL,
    Status NVARCHAR(20) NOT NULL DEFAULT 'Unpaid',
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),

    CONSTRAINT CK_Fines_Amount CHECK (Amount > 0),
    CONSTRAINT CK_Fines_Status CHECK (Status IN ('Unpaid', 'PartiallyPaid', 'Paid')),

    CONSTRAINT FK_Fines_Loans FOREIGN KEY (LoanId) REFERENCES Loans(Id),
    CONSTRAINT FK_Fines_Members FOREIGN KEY (MemberId) REFERENCES Members(Id)
);
GO

CREATE TABLE Payments (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FineId INT NOT NULL,
    Amount DECIMAL(10,2) NOT NULL,
    PaymentMethod NVARCHAR(20) NOT NULL,
    PaidAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),

    CONSTRAINT CK_Payments_Amount CHECK (Amount > 0),
    CONSTRAINT CK_Payments_Method CHECK (PaymentMethod IN ('Cash', 'Card', 'Online')),

    CONSTRAINT FK_Payments_Fines FOREIGN KEY (FineId) REFERENCES Fines(Id)
);
GO

CREATE INDEX IX_Fines_LoanId ON Fines(LoanId);
CREATE INDEX IX_Fines_MemberId ON Fines(MemberId);
CREATE INDEX IX_Fines_Status ON Fines(Status);
CREATE INDEX IX_Payments_FineId ON Payments(FineId);
GO

/* =========================================
   4. بيانات تجريبية
   ========================================= */
INSERT INTO Members (UserId, Phone, Address) VALUES
('test-user-1', '01000000001', 'Cairo'),
('test-user-2', '01000000002', 'Mansoura');
GO

INSERT INTO Loans (MemberId, BookCopyId, DueDate) VALUES
(1, 101, DATEADD(DAY, -10, GETUTCDATE())),
(1, 102, DATEADD(DAY,  -5, GETUTCDATE())),
(2, 103, DATEADD(DAY,  -3, GETUTCDATE()));
GO

INSERT INTO Fines (LoanId, MemberId, Amount, Reason, Status) VALUES
(1, 1,  50.00, 'Late Return',  'Paid'),
(2, 1, 100.00, 'Damaged Book', 'PartiallyPaid'),
(3, 2,  75.00, 'Late Return',  'Unpaid');
GO

INSERT INTO Payments (FineId, Amount, PaymentMethod) VALUES
(1, 50.00, 'Cash'),
(2, 50.00, 'Card');
GO

SELECT * FROM Fines;
SELECT * FROM Payments;
GO
