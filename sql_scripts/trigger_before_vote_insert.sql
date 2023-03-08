DROP TRIGGER IF EXISTS before_vote_insert
DELIMITER $$
CREATE TRIGGER before_vote_insert
    BEFORE INSERT
    ON vote
    FOR EACH ROW
BEGIN
    DECLARE participant INT DEFAULT 0;
    DECLARE conferenceEndDate DATETIME;

    SELECT COUNT(1)
    INTO participant
    FROM user_gathering_role ugr
    WHERE ugr.user_id = NEW.user_id
      AND ugr.gathering_id = NEW.gathering_id;

    IF (participant = 0) THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'User is not a participant in this conference';
    END IF;

    SELECT end_date INTO conferenceEndDate FROM conference c WHERE c.gathering_id = NEW.gathering_id;

    IF (conferenceEndDate >= NOW()) THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'Conference did not end';
    END IF;
END$$
DELIMITER ;