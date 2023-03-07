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
