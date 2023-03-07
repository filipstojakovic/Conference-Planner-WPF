CREATE 
    ALGORITHM = UNDEFINED 
    DEFINER = `root`@`localhost` 
    SQL SECURITY DEFINER
VIEW `select_events_with_event_type` AS
    SELECT 
        `e`.`id` AS `id`,
        `e`.`session_id` AS `session_id`,
        `e`.`event_type_id` AS `event_type_id`,
        `e`.`name` AS `name`,
        `e`.`description` AS `description`,
        `e`.`start_date` AS `start_date`,
        `e`.`end_date` AS `end_date`,
        `et`.`name` AS `event_type_name`
    FROM
        (`event` `e`
        JOIN `event_type` `et` ON ((`et`.`id` = `e`.`event_type_id`)))