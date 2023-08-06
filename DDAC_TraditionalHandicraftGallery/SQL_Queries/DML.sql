DELETE FROM QuoteRequests;
IF EXISTS (SELECT *
FROM sys.identity_columns
WHERE OBJECT_NAME(OBJECT_ID) = 'QuoteRequests' AND last_value IS NOT NULL) 
    DBCC CHECKIDENT ('QuoteRequests', RESEED, 0);

DELETE FROM Handicrafts;
IF EXISTS (SELECT *
FROM sys.identity_columns
WHERE OBJECT_NAME(OBJECT_ID) = 'Handicrafts' AND last_value IS NOT NULL) 
    DBCC CHECKIDENT ('Handicrafts', RESEED, 0);

DELETE FROM HandicraftTypes;
IF EXISTS (SELECT *
FROM sys.identity_columns
WHERE OBJECT_NAME(OBJECT_ID) = 'HandicraftTypes' AND last_value IS NOT NULL) 
    DBCC CHECKIDENT ('HandicraftTypes', RESEED, 0);

DELETE FROM AspNetUserRoles;
IF EXISTS (SELECT *
FROM sys.identity_columns
WHERE OBJECT_NAME(OBJECT_ID) = 'AspNetUserRoles' AND last_value IS NOT NULL) 
    DBCC CHECKIDENT ('AspNetUserRoles', RESEED, 0);

DELETE FROM AspNetUsers;
IF EXISTS (SELECT *
FROM sys.identity_columns
WHERE OBJECT_NAME(OBJECT_ID) = 'AspNetUsers' AND last_value IS NOT NULL) 
    DBCC CHECKIDENT ('AspNetUsers', RESEED, 0);

DELETE FROM AspNetRoles;
IF EXISTS (SELECT *
FROM sys.identity_columns
WHERE OBJECT_NAME(OBJECT_ID) = 'AspNetRoles' AND last_value IS NOT NULL) 
    DBCC CHECKIDENT ('AspNetRoles', RESEED, 0);

GO


-- Inserting data into the AspNetUsers table
-- admin, Admin@123
-- user, User@123
INSERT INTO AspNetUsers
    (Id, AccessFailedCount, ConcurrencyStamp, CreatedAt, Email, EmailConfirmed, IsAdmin, LockoutEnabled, LockoutEnd, NormalizedEmail, NormalizedUserName, PasswordHash, PhoneNumber, PhoneNumberConfirmed, SecurityStamp, TwoFactorEnabled, UpdatedAt, UserName)
VALUES
    ('1', 0, '59e98dd3-6736-4158-b424-886f9583a109', '2023-08-03T21:53:22.819', 'admin@example.com', 1, 1, 0, NULL, 'ADMIN@EXAMPLE.COM', 'ADMIN', 'AQAAAAIAAYagAAAAEIvbeG3TOZArRMBB4xIIZYkQgMW07PmxYvvS4Y0WOS4LOBO1r6P1+jN7f0nilBY98w==', NULL, 0, '', 0, '2023-08-03T21:53:22.819', 'admin'),
    ('2', 0, '5c315835-ebad-41cf-8310-7147f48578ff', '2023-08-03T21:53:22.938', 'user@example.com', 1, 0, 0, NULL, 'USER@EXAMPLE.COM', 'USER', 'AQAAAAIAAYagAAAAEFaMBG0Ni3GIyuKmtOAMh5pMYg1l8ifPkGjaDj4S+O2/kc5zUyP2if1fm/xBAN2iyA==', NULL, 0, '', 0, '2023-08-03T21:53:22.938', 'user');

-- Inserting data into the AspNetRoles table
INSERT INTO AspNetRoles
    (Id, Name, NormalizedName)
VALUES
    ('2c5e174e-3b0e-446f-86af-483d56fd7210', 'Admin', 'ADMIN'),
    ('8e445865-a24d-4543-a6c6-9443d048cdb9', 'User', 'USER');

INSERT INTO AspNetUserRoles
    (RoleId, UserId)
VALUES
    ('2c5e174e-3b0e-446f-86af-483d56fd7210', '1'),
    ('8e445865-a24d-4543-a6c6-9443d048cdb9', '2');

-- Inserting data into the HandicraftTypes table
SET IDENTITY_INSERT HandicraftTypes ON;
INSERT INTO HandicraftTypes
    (Id, CreatedAt, Name, UpdatedAt)
VALUES
    (1, '2023-08-03T21:53:22.737', 'Pottery', '2023-08-03T21:53:22.737'),
    (2, '2023-08-03T21:53:22.737', 'Woodwork', '2023-08-03T21:53:22.737');
SET IDENTITY_INSERT HandicraftTypes OFF;

-- Inserting data into the Handicrafts table
SET IDENTITY_INSERT Handicrafts ON;
INSERT INTO Handicrafts
    (Id, AuthorEmail, AuthorName, CreatedAt, Description, ImageURL, IsHidden, Name, TypeId, UpdatedAt)
VALUES
    (1, 'johnsmith@example.com', 'John Smith', '2023-08-03T21:53:22.737', 'A beautifully crafted glazed pot.', 'http://placehold.it/500x500', 0, 'Glazed Pot', 1, '2023-08-03T21:53:22.737'),
    (2, 'janedoe@example.com', 'Jane Doe', '2023-08-03T21:53:22.737', 'A sturdy oak table, hand-crafted with precision.', 'http://placehold.it/500x500', 0, 'Oak Table', 2, '2023-08-03T21:53:22.737');
SET IDENTITY_INSERT Handicrafts OFF;
