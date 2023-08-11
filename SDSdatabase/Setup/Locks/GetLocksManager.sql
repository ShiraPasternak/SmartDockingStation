DROP FUNCTION IF EXISTS GetLocksManager;
GO
CREATE OR ALTER FUNCTION GetLocksManager(@station_id INT)
RETURNS @Locks Table
(
    -- Lock Data
    lock_id UNIQUEIDENTIFIER NOT NULL,
    lock_name NVARCHAR(MAX) NOT NULL,
    -- Rental Data
    user_id NVARCHAR(MAX)
)
BEGIN
    INSERT INTO @Locks
    SELECT
        -- Lock Data
        id,
        name,
        -- Rental Data
        user_id
    FROM
        Locks
    WHERE
        station_id = @station_id;

    RETURN;
END;
GO