DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `insert_using_access_code`(IN accessCode VARCHAR(255), IN userId INT)
BEGIN
    DECLARE gatheringId INT;
    DECLARE gatheringRoleId INT;

    SELECT s.gathering_id, ac.gathering_role_id INTO gatheringId, gatheringRoleId FROM session s
    JOIN event e on s.id = e.session_id
    JOIN online_event oe on e.id = oe.event_id
    JOIN access_code ac on oe.event_id = ac.online_event_event_id
    WHERE accessCode=ac.code;
    
    INSERT INTO user_gathering_role (gathering_id, user_id, gathering_role_id)
    VALUES (gatheringId, userId, gatheringRoleId);
END$$

DELIMITER ;