DELIMITER $$
DROP PROCEDURE IF EXISTS insert_visitor_using_access_code;
CREATE PROCEDURE insert_visitor_using_access_code(IN accessCode VARCHAR(255), IN userId INT)
BEGIN
    DECLARE conference_id INT;
    DECLARE gatheringRole INT;

    SELECT s.gathering_id, ac.gathering_role_id
    INTO conference_id, gatheringRole
    FROM session s
             JOIN event e on s.id = e.session_id
             JOIN online_event oe on e.id = oe.event_id
             JOIN access_code ac on oe.event_id = ac.online_event_event_id
    WHERE accessCode = ac.code;

    INSERT INTO user_gathering_role (gathering_id, user_id, gathering_role_id)
    VALUES (conference_id, userId, gatheringRole);
END$$
DELIMITER ;

# check if user can vote
DELIMITER $$
DROP TRIGGER IF EXISTS before_vote_insert;
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

# check if conference ended before delete
DELIMITER $$
DROP TRIGGER IF EXISTS before_conference_delete;
CREATE TRIGGER before_conference_delete
    BEFORE DELETE
    ON user_gathering_role
    FOR EACH ROW
BEGIN
    DECLARE conferenceEndDate DATETIME;
    SELECT end_date INTO conferenceEndDate FROM conference c WHERE c.gathering_id = OLD.gathering_id;

    IF (conferenceEndDate <= NOW()) THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'Conference has already ended';
    END IF;
END$$
DELIMITER ;

# session procedure
DELIMITER $$
DROP PROCEDURE IF EXISTS is_session_date_range_valid;
CREATE PROCEDURE is_session_date_range_valid(IN conferenceId INT, IN sessionStartDate DATETIME,
                                             IN sessionEndDate DATETIME,
                                             OUT isValid BOOLEAN)
BEGIN
    DECLARE conferenceStartDate DATETIME;
    DECLARE conferenceEndDate DATETIME;

    SELECT c.start_date, c.end_date
    INTO conferenceStartDate,conferenceEndDate
    FROM conference c
    WHERE gathering_id = conferenceId;

    IF (conferenceStartDate <= sessionStartDate AND sessionEndDate <= conferenceEndDate) THEN
        SET isValid = true;
    ELSE
        SET isValid = false;
    END IF;
END$$
DELIMITER ;

# trigger before session insert
DELIMITER $$
DROP TRIGGER IF EXISTS before_session_insert;
CREATE TRIGGER before_session_insert
    BEFORE INSERT
    ON session
    FOR EACH ROW
BEGIN
    DECLARE isValid BOOLEAN;
    SET isValid = false;
    CALL is_session_date_range_valid(NEW.gathering_id, NEW.start_date, NEW.end_date, isValid);
    IF isValid = false THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'session is not in conference datetime range';
    END IF;
END$$
DELIMITER ;

# trigger before session update
DELIMITER $$
DROP TRIGGER IF EXISTS before_session_update;
CREATE TRIGGER before_session_update
    BEFORE UPDATE
    ON session
    FOR EACH ROW
BEGIN
    DECLARE isValid BOOLEAN;
    SET isValid = false;
    CALL is_session_date_range_valid(NEW.gathering_id, NEW.start_date, NEW.end_date, isValid);
    IF isValid = false THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'session is not in conference datetime range';
    END IF;
END$$
DELIMITER ;

# event procedure
DELIMITER $$
DROP PROCEDURE IF EXISTS is_event_date_range_valid;
CREATE PROCEDURE is_event_date_range_valid(IN sessionId INT, IN eventStartDate DATETIME,
                                           IN eventEndDate DATETIME,
                                           OUT isValid BOOLEAN)
BEGIN
    DECLARE sessionStartDate DATETIME;
    DECLARE sessionEndDate DATETIME;

    SELECT s.start_date, s.end_date
    INTO sessionStartDate,sessionEndDate
    FROM session s
    WHERE id = sessionId;

    IF (sessionStartDate <= eventStartDate AND eventEndDate <= sessionEndDate) THEN
        SET isValid = true;
    ELSE
        SET isValid = false;
    END IF;
END$$
DELIMITER ;

# trigger before event insert
DELIMITER $$
DROP TRIGGER IF EXISTS before_event_insert;
CREATE TRIGGER before_event_insert
    BEFORE INSERT
    ON event
    FOR EACH ROW
BEGIN
    DECLARE isValid BOOLEAN;
    SET isValid = false;
    CALL is_event_date_range_valid(NEW.session_id, NEW.start_date, NEW.end_date, isValid);
    IF isValid = false THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'Event is not in session datetime range';
    END IF;
END$$
DELIMITER ;

# trigger before event update
DELIMITER $$
DROP TRIGGER IF EXISTS before_event_insert;
CREATE TRIGGER before_event_insert
    BEFORE UPDATE
    ON event
    FOR EACH ROW
BEGIN
    DECLARE isValid BOOLEAN;
    SET isValid = false;
    CALL is_event_date_range_valid(NEW.session_id, NEW.start_date, NEW.end_date, isValid);
    IF isValid = false THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'Event is not in session datetime range';
    END IF;
END$$
DELIMITER ;

# create trigger before conference insert
DELIMITER $$
DROP TRIGGER IF EXISTS set_conference_start_end_time_on_insert;
CREATE TRIGGER set_conference_start_end_time_on_insert
    BEFORE INSERT
    ON `conference`
    FOR EACH ROW
BEGIN
    SET NEW.start_date = CONCAT(DATE(NEW.start_date), ' 00:00:00');
    SET NEW.end_date = CONCAT(DATE(NEW.end_date), ' 23:59:59');
END$$

# create trigger before conference update
DELIMITER $$
DROP TRIGGER IF EXISTS set_conference_start_end_time_update;
CREATE TRIGGER set_conference_start_end_time_update
    BEFORE UPDATE
    ON `conference`
    FOR EACH ROW
BEGIN
    SET NEW.start_date = CONCAT(DATE(NEW.start_date), ' 00:00:00');
    SET NEW.end_date = CONCAT(DATE(NEW.end_date), ' 23:59:59');
END$$
DELIMITER ;

# get users full name
DELIMITER $$
DROP PROCEDURE IF EXISTS  get_user_fullname;
CREATE PROCEDURE get_user_fullname (IN userId int, OUT full_name varchar(255))
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

DROP VIEW IF EXISTS view_live_event;
CREATE VIEW view_live_event AS
SELECT e.*,
       et.name as event_type_name,
       r.id    as room_id,
       r.room_number,
       l.id    as location_id,
       l.street,
       l.city
FROM event e
         JOIN event_type et on e.event_type_id = et.id
         JOIN live_event le on e.id = le.event_id
         JOIN room r on le.room_id = r.id
         join location l on r.location_id = l.id;
