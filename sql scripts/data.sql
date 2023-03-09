-- Insert some roles
INSERT INTO role (name)
VALUES ('admin'),
       ('user'),
       ('moderator');

-- Insert some users with roles
INSERT INTO user (first_name, last_name, username, email, password)
VALUES ('admin', 'admin', 'admin', 'a@a.com', 'YWRtaW4='),
       ('user', 'user', 'user', 'b@b.com', 'dXNlcg=='),
       ('moderator', 'moderator', 'mod', 'c@c.com', 'bW9k');

-- Assign user roles
INSERT INTO user_has_role (user_id, role_id)
VALUES ((SELECT id FROM user WHERE username = 'admin'), (SELECT id FROM role WHERE name = 'admin')),
       ((SELECT id FROM user WHERE username = 'admin'), (SELECT id FROM role WHERE name = 'user')),
       ((SELECT id FROM user WHERE username = 'user'), (SELECT id FROM role WHERE name = 'user')),
       ((SELECT id FROM user WHERE username = 'mod'), (SELECT id FROM role WHERE name = 'moderator')),
       ((SELECT id FROM user WHERE username = 'mod'), (SELECT id FROM role WHERE name = 'user'));

-- Insert some events
INSERT INTO event_type (name)
VALUES ('lecture'),
       ('presentation'),
       ('meeting');

-- Insert some location types
INSERT INTO gathering_role (name)
VALUES ('Organizer'),
       ('Visitor'),
       ('Moderator'),
       ('Presenter');
       
INSERT INTO location_type (name)
VALUES ('hall'),('building'),('hotel');

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
        JOIN `location` `l` ON ((`r`.`location_id` = `l`.`id`)));