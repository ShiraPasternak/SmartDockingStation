DROP FUNCTION IF EXISTS GetLocksManager;
GO
CREATE OR ALTER FUNCTION GetLocksManager(@station_id INT)
RETURNS @Locks Table
(
    -- Lock Data
    lock_id UNIQUEIDENTIFIER NOT NULL,
    lock_name NVARCHAR(MAX) NOT NULL,
    -- Lock Secret Data
    lock_mac NVARCHAR(MAX) NOT NULL,
    lock_secret VARCHAR(MAX) NOT NULL,
    -- Rental Data
    user_id NVARCHAR(MAX),
    -- Soft Delete Flag
    deleted BIT NOT NULL
)
BEGIN
    INSERT INTO @Locks
    SELECT
        -- Lock Data
        id,
        name,
        -- Lock Secret Data
        mac,
        sys.fn_varbintohexstr(secret),
        -- Rental Data
        user_id,
        deleted
    FROM
        Locks
    WHERE
        station_id = @station_id;

    RETURN;
END;
GO