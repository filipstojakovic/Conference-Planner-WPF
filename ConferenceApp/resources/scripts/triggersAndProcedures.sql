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
