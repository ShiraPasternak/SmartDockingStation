DROP FUNCTION IF EXISTS GetCurrentRentals;
GO
CREATE OR ALTER FUNCTION GetCurrentRentals(@user_id NVARCHAR(MAX))
RETURNS @CurrentRentals Table
(
    -- Station Data
    station_name NVARCHAR(MAX) NOT NULL,
    latitude DECIMAL(9,6) NOT NULL,
    longitude DECIMAL(9,6) NOT NULL,
    -- Lock Data
    lock_id UNIQUEIDENTIFIER NOT NULL,
    lock_name NVARCHAR(MAX) NOT NULL,
    -- Rental Data
    hourly_rate DECIMAL(4,2) NOT NULL,
    start_time DATETIME NOT NULL
)
BEGIN
    INSERT INTO @CurrentRentals
    SELECT
        -- Station Data
        station_name,
        latitude,
        longitude,
        -- Lock Data
        id,
        name,
        -- Rental Data
        hourly_rate,
        start_time
    FROM
        Locks
    WHERE
        user_id = @user_id;

    RETURN;
END;
GO