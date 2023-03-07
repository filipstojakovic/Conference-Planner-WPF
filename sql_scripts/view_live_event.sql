CREATE 
    ALGORITHM = UNDEFINED 
    DEFINER = `root`@`localhost` 
    SQL SECURITY DEFINER
VIEW `view_live_event` AS
    SELECT 
        `e`.`id` AS `id`,
        `e`.`session_id` AS `session_id`,
        `e`.`event_type_id` AS `event_type_id`,
        `e`.`name` AS `name`,
        `e`.`description` AS `description`,
        `e`.`start_date` AS `start_date`,
        `e`.`end_date` AS `end_date`,
        `et`.`name` AS `event_type_name`,
        `r`.`room_number` AS `room_number`,
        `l`.`street` AS `street`
    FROM
        ((((`event` `e`
        JOIN `event_type` `et` ON ((`e`.`event_type_id` = `et`.`id`)))
        JOIN `live_event` `le` ON ((`e`.`id` = `le`.`event_id`)))
        JOIN `room` `r` ON ((`le`.`room_id` = `r`.`id`)))
        JOIN `location` `l` ON ((`r`.`location_id` = `l`.`id`)))