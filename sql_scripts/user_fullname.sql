DELIMITER $$
CREATE PROCEDURE `get_user_fullname` (IN userId int, OUT full_name varchar(255))
BEGIN
	DECLARE FirstName VARCHAR(50);
    DECLARE LastName VARCHAR(50);
    
SELECT 
    first_name, last_name
INTO FirstName , LastName FROM
    user
WHERE
    id = userId;
    
    SET full_name = CONCAT(FirstName, ' ', LastName);
END$$
DELIMITER ;