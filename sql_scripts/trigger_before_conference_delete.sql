DROP TRIGGER IF EXISTS before_conference_delete;
DELIMITER $$
CREATE TRIGGER before_conference_delete
    BEFORE DELETE
    ON user_gathering_role
    FOR EACH ROW
BEGIN
    DECLARE conferenceEndDate DATETIME;
    SELECT end_date INTO conferenceEndDate FROM conference c WHERE c.gathering_id = OLD.gathering_id;

    IF(conferenceEndDate<=NOW()) THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'Conference has already ended';
    END IF;
END$$
DELIMITER ;