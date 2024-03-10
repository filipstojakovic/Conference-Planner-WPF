-- -----------------------------------------------------
-- Schema conferencedb
-- -----------------------------------------------------
DROP SCHEMA IF EXISTS `conferencedb` ;

-- -----------------------------------------------------
-- Schema conferencedb
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `conferencedb` DEFAULT CHARACTER SET utf8mb3 ;
USE `conferencedb` ;

-- -----------------------------------------------------
-- Table `conferencedb`.`gathering`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `conferencedb`.`gathering` ;

CREATE TABLE IF NOT EXISTS `conferencedb`.`gathering` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `description` VARCHAR(255) NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb3;


-- -----------------------------------------------------
-- Table `conferencedb`.`conference`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `conferencedb`.`conference` ;

CREATE TABLE IF NOT EXISTS `conferencedb`.`conference` (
  `gathering_id` INT NOT NULL,
  `start_date` DATETIME NOT NULL,
  `end_date` DATETIME NOT NULL,
  `name` VARCHAR(255) NOT NULL,
  PRIMARY KEY (`gathering_id`),
  CONSTRAINT `fk_CONFERENCE_EVENT1`
    FOREIGN KEY (`gathering_id`)
    REFERENCES `conferencedb`.`gathering` (`id`)
    ON DELETE CASCADE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb3;


-- -----------------------------------------------------
-- Table `conferencedb`.`sponsor`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `conferencedb`.`sponsor` ;

CREATE TABLE IF NOT EXISTS `conferencedb`.`sponsor` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(255) NULL DEFAULT NULL,
  `contact_email` VARCHAR(255) NULL DEFAULT NULL,
  `web_page_url` VARCHAR(255) NULL DEFAULT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb3;


-- -----------------------------------------------------
-- Table `conferencedb`.`conference_has_sponsor`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `conferencedb`.`conference_has_sponsor` ;

CREATE TABLE IF NOT EXISTS `conferencedb`.`conference_has_sponsor` (
  `gathering_id` INT NOT NULL,
  `sponsor_id` INT NOT NULL,
  PRIMARY KEY (`gathering_id`, `sponsor_id`),
  INDEX `fk_CONFERENCE_has_SPONSOR_SPONSOR1_idx` (`sponsor_id` ASC) VISIBLE,
  INDEX `fk_CONFERENCE_has_SPONSOR_CONFERENCE1_idx` (`gathering_id` ASC) VISIBLE,
  CONSTRAINT `fk_CONFERENCE_has_SPONSOR_CONFERENCE1`
    FOREIGN KEY (`gathering_id`)
    REFERENCES `conferencedb`.`conference` (`gathering_id`),
  CONSTRAINT `fk_CONFERENCE_has_SPONSOR_SPONSOR1`
    FOREIGN KEY (`sponsor_id`)
    REFERENCES `conferencedb`.`sponsor` (`id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb3;


-- -----------------------------------------------------
-- Table `conferencedb`.`gathering_role`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `conferencedb`.`gathering_role` ;

CREATE TABLE IF NOT EXISTS `conferencedb`.`gathering_role` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `name_UNIQUE` (`name` ASC) VISIBLE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb3;


-- -----------------------------------------------------
-- Table `conferencedb`.`event_type`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `conferencedb`.`event_type` ;

CREATE TABLE IF NOT EXISTS `conferencedb`.`event_type` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `name_UNIQUE` (`name` ASC) VISIBLE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb3;


-- -----------------------------------------------------
-- Table `conferencedb`.`session`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `conferencedb`.`session` ;

CREATE TABLE IF NOT EXISTS `conferencedb`.`session` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `gathering_id` INT NOT NULL,
  `start_date` DATETIME NOT NULL,
  `end_date` DATETIME NOT NULL,
  `name` VARCHAR(255) NOT NULL,
  `description` VARCHAR(255) NULL DEFAULT '',
  PRIMARY KEY (`id`),
  INDEX `fk_SESSION_CONFERENCE1_idx` (`gathering_id` ASC) VISIBLE,
  CONSTRAINT `fk_SESSION_CONFERENCE1`
    FOREIGN KEY (`gathering_id`)
    REFERENCES `conferencedb`.`conference` (`gathering_id`)
    ON DELETE RESTRICT)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb3;


-- -----------------------------------------------------
-- Table `conferencedb`.`event`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `conferencedb`.`event` ;

CREATE TABLE IF NOT EXISTS `conferencedb`.`event` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `session_id` INT NOT NULL,
  `event_type_id` INT NOT NULL,
  `name` VARCHAR(255) NOT NULL,
  `description` VARCHAR(255) NULL DEFAULT '',
  `start_date` DATETIME NOT NULL,
  `end_date` DATETIME NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_EVENT_EVENT_TYPE1_idx` (`event_type_id` ASC) VISIBLE,
  INDEX `fk_EVENT_SESSION1_idx` (`session_id` ASC) VISIBLE,
  CONSTRAINT `fk_EVENT_EVENT_TYPE1`
    FOREIGN KEY (`event_type_id`)
    REFERENCES `conferencedb`.`event_type` (`id`),
  CONSTRAINT `fk_EVENT_SESSION1`
    FOREIGN KEY (`session_id`)
    REFERENCES `conferencedb`.`session` (`id`)
    ON DELETE RESTRICT)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb3;


-- -----------------------------------------------------
-- Table `conferencedb`.`resource`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `conferencedb`.`resource` ;

CREATE TABLE IF NOT EXISTS `conferencedb`.`resource` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(255) NOT NULL,
  `capacity` INT NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `name_UNIQUE` (`name` ASC) VISIBLE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb3;


-- -----------------------------------------------------
-- Table `conferencedb`.`gathering_has_resource`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `conferencedb`.`gathering_has_resource` ;

CREATE TABLE IF NOT EXISTS `conferencedb`.`gathering_has_resource` (
  `resource_id` INT NOT NULL,
  `gathering_id` INT NOT NULL,
  `required_capacity` INT NULL DEFAULT NULL,
  PRIMARY KEY (`resource_id`, `gathering_id`),
  INDEX `fk_RESOURCE_has_GEATHERING_GEATHERING1_idx` (`gathering_id` ASC) VISIBLE,
  INDEX `fk_RESOURCE_has_GEATHERING_RESOURCE1_idx` (`resource_id` ASC) VISIBLE,
  CONSTRAINT `fk_RESOURCE_has_GEATHERING_GEATHERING1`
    FOREIGN KEY (`gathering_id`)
    REFERENCES `conferencedb`.`gathering` (`id`)
    ON DELETE CASCADE,
  CONSTRAINT `fk_RESOURCE_has_GEATHERING_RESOURCE1`
    FOREIGN KEY (`resource_id`)
    REFERENCES `conferencedb`.`resource` (`id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb3;


-- -----------------------------------------------------
-- Table `conferencedb`.`meeting`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `conferencedb`.`meeting` ;

CREATE TABLE IF NOT EXISTS `conferencedb`.`meeting` (
  `gathering_id` INT NOT NULL,
  `date` DATETIME NOT NULL,
  PRIMARY KEY (`gathering_id`),
  CONSTRAINT `fk_MEETING_EVENT1`
    FOREIGN KEY (`gathering_id`)
    REFERENCES `conferencedb`.`gathering` (`id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb3;


-- -----------------------------------------------------
-- Table `conferencedb`.`user`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `conferencedb`.`user` ;

CREATE TABLE IF NOT EXISTS `conferencedb`.`user` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `username` VARCHAR(255) NOT NULL,
  `password` VARCHAR(255) NOT NULL,
  `first_name` VARCHAR(255) NOT NULL,
  `last_name` VARCHAR(255) NOT NULL,
  `email` VARCHAR(255) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `username_UNIQUE` (`username` ASC) VISIBLE,
  UNIQUE INDEX `email_UNIQUE` (`email` ASC) VISIBLE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb3;


-- -----------------------------------------------------
-- Table `conferencedb`.`invitation`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `conferencedb`.`invitation` ;

CREATE TABLE IF NOT EXISTS `conferencedb`.`invitation` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `gathering_id` INT NOT NULL,
  `user_id` INT NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_INVITATION_MEETING1_idx` (`gathering_id` ASC) VISIBLE,
  INDEX `fk_INVITATION_USER1_idx` (`user_id` ASC) VISIBLE,
  CONSTRAINT `fk_INVITATION_MEETING1`
    FOREIGN KEY (`gathering_id`)
    REFERENCES `conferencedb`.`meeting` (`gathering_id`),
  CONSTRAINT `fk_INVITATION_USER1`
    FOREIGN KEY (`user_id`)
    REFERENCES `conferencedb`.`user` (`id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb3;


-- -----------------------------------------------------
-- Table `conferencedb`.`location_type`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `conferencedb`.`location_type` ;

CREATE TABLE IF NOT EXISTS `conferencedb`.`location_type` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(255) NULL DEFAULT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb3;


-- -----------------------------------------------------
-- Table `conferencedb`.`location`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `conferencedb`.`location` ;

CREATE TABLE IF NOT EXISTS `conferencedb`.`location` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `location_type_id` INT NOT NULL,
  `city` VARCHAR(255) NOT NULL,
  `street` VARCHAR(255) NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_LOCATION_LOCATION_TYPE1_idx` (`location_type_id` ASC) VISIBLE,
  CONSTRAINT `fk_LOCATION_LOCATION_TYPE1`
    FOREIGN KEY (`location_type_id`)
    REFERENCES `conferencedb`.`location_type` (`id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb3;


-- -----------------------------------------------------
-- Table `conferencedb`.`room`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `conferencedb`.`room` ;

CREATE TABLE IF NOT EXISTS `conferencedb`.`room` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `location_id` INT NOT NULL,
  `room_number` VARCHAR(255) NULL DEFAULT NULL,
  `capacity` INT NULL DEFAULT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_ROOM_LOCATION1_idx` (`location_id` ASC) VISIBLE,
  CONSTRAINT `fk_ROOM_LOCATION1`
    FOREIGN KEY (`location_id`)
    REFERENCES `conferencedb`.`location` (`id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb3;


-- -----------------------------------------------------
-- Table `conferencedb`.`live_event`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `conferencedb`.`live_event` ;

CREATE TABLE IF NOT EXISTS `conferencedb`.`live_event` (
  `event_id` INT NOT NULL,
  `room_id` INT NOT NULL,
  PRIMARY KEY (`event_id`),
  INDEX `fk_LIVE_EVENT_ROOM1_idx` (`room_id` ASC) VISIBLE,
  CONSTRAINT `fk_LIVE_EVENT_EVENT1`
    FOREIGN KEY (`event_id`)
    REFERENCES `conferencedb`.`event` (`id`)
    ON DELETE CASCADE,
  CONSTRAINT `fk_LIVE_EVENT_ROOM1`
    FOREIGN KEY (`room_id`)
    REFERENCES `conferencedb`.`room` (`id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb3;


-- -----------------------------------------------------
-- Table `conferencedb`.`live_meeting`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `conferencedb`.`live_meeting` ;

CREATE TABLE IF NOT EXISTS `conferencedb`.`live_meeting` (
  `gathering_id` INT NOT NULL,
  `room_id` INT NOT NULL,
  PRIMARY KEY (`gathering_id`),
  INDEX `fk_LIVE_MEETING_ROOM1_idx` (`room_id` ASC) VISIBLE,
  CONSTRAINT `fk_LIVE_MEETING_MEETING1`
    FOREIGN KEY (`gathering_id`)
    REFERENCES `conferencedb`.`meeting` (`gathering_id`)
    ON DELETE CASCADE,
  CONSTRAINT `fk_LIVE_MEETING_ROOM1`
    FOREIGN KEY (`room_id`)
    REFERENCES `conferencedb`.`room` (`id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb3;


-- -----------------------------------------------------
-- Table `conferencedb`.`online_event`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `conferencedb`.`online_event` ;

CREATE TABLE IF NOT EXISTS `conferencedb`.`online_event` (
  `event_id` INT NOT NULL,
  `url` VARCHAR(255) NULL DEFAULT NULL,
  PRIMARY KEY (`event_id`),
  CONSTRAINT `fk_ONLINE_EVENT_EVENT1`
    FOREIGN KEY (`event_id`)
    REFERENCES `conferencedb`.`event` (`id`)
    ON DELETE CASCADE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb3;


-- -----------------------------------------------------
-- Table `conferencedb`.`online_meeting`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `conferencedb`.`online_meeting` ;

CREATE TABLE IF NOT EXISTS `conferencedb`.`online_meeting` (
  `gathering_id` INT NOT NULL,
  `url` VARCHAR(255) NULL DEFAULT NULL,
  PRIMARY KEY (`gathering_id`),
  CONSTRAINT `fk_ONLINE_MEETING_MEETING1`
    FOREIGN KEY (`gathering_id`)
    REFERENCES `conferencedb`.`meeting` (`gathering_id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb3;


-- -----------------------------------------------------
-- Table `conferencedb`.`role`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `conferencedb`.`role` ;

CREATE TABLE IF NOT EXISTS `conferencedb`.`role` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `name_UNIQUE` (`name` ASC) VISIBLE)
ENGINE = InnoDB
AUTO_INCREMENT = 4
DEFAULT CHARACTER SET = utf8mb3;


-- -----------------------------------------------------
-- Table `conferencedb`.`ticket`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `conferencedb`.`ticket` ;

CREATE TABLE IF NOT EXISTS `conferencedb`.`ticket` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `price` DECIMAL(10,2) NULL DEFAULT NULL,
  `gathering_id` INT NOT NULL,
  `user_id` INT NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_TICKET_CONFERENCE1_idx` (`gathering_id` ASC) VISIBLE,
  INDEX `fk_TICKET_USER1_idx` (`user_id` ASC) VISIBLE,
  CONSTRAINT `fk_TICKET_CONFERENCE1`
    FOREIGN KEY (`gathering_id`)
    REFERENCES `conferencedb`.`conference` (`gathering_id`),
  CONSTRAINT `fk_TICKET_USER1`
    FOREIGN KEY (`user_id`)
    REFERENCES `conferencedb`.`user` (`id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb3;


-- -----------------------------------------------------
-- Table `conferencedb`.`user_has_role`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `conferencedb`.`user_has_role` ;

CREATE TABLE IF NOT EXISTS `conferencedb`.`user_has_role` (
  `user_id` INT NOT NULL,
  `role_id` INT NOT NULL,
  PRIMARY KEY (`user_id`, `role_id`),
  INDEX `fk_User_has_Role_Role1_idx` (`role_id` ASC) VISIBLE,
  INDEX `fk_User_has_Role_User_idx` (`user_id` ASC) VISIBLE,
  CONSTRAINT `fk_User_has_Role_Role1`
    FOREIGN KEY (`role_id`)
    REFERENCES `conferencedb`.`role` (`id`),
  CONSTRAINT `fk_User_has_Role_User`
    FOREIGN KEY (`user_id`)
    REFERENCES `conferencedb`.`user` (`id`)
    ON DELETE CASCADE
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb3;


-- -----------------------------------------------------
-- Table `conferencedb`.`vote`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `conferencedb`.`vote` ;

CREATE TABLE IF NOT EXISTS `conferencedb`.`vote` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `user_id` INT NOT NULL,
  `gathering_id` INT NULL,
  `session_id` INT NULL,
  `event_id` INT NULL,
  `rating` INT NULL DEFAULT NULL,
  `comment` VARCHAR(255) NULL DEFAULT NULL,
  `vote_date` DATETIME NULL DEFAULT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_VOTE_GEATHERING1_idx` (`gathering_id` ASC) VISIBLE,
  INDEX `fk_VOTE_USER1_idx` (`user_id` ASC) VISIBLE,
  INDEX `fk_vote_session1_idx` (`session_id` ASC) VISIBLE,
  INDEX `fk_vote_event1_idx` (`event_id` ASC) VISIBLE,
  CONSTRAINT `fk_VOTE_GEATHERING1`
    FOREIGN KEY (`gathering_id`)
    REFERENCES `conferencedb`.`gathering` (`id`),
  CONSTRAINT `fk_VOTE_USER1`
    FOREIGN KEY (`user_id`)
    REFERENCES `conferencedb`.`user` (`id`),
  CONSTRAINT `fk_vote_session1`
    FOREIGN KEY (`session_id`)
    REFERENCES `conferencedb`.`session` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_vote_event1`
    FOREIGN KEY (`event_id`)
    REFERENCES `conferencedb`.`event` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb3;


-- -----------------------------------------------------
-- Table `conferencedb`.`settings`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `conferencedb`.`settings` ;

CREATE TABLE IF NOT EXISTS `conferencedb`.`settings` (
  `user_id` INT NOT NULL,
  `language` VARCHAR(45) NULL DEFAULT 'en',
  `theme` VARCHAR(45) NULL DEFAULT 'default',
  PRIMARY KEY (`user_id`),
  CONSTRAINT `fk_settings_user1`
    FOREIGN KEY (`user_id`)
    REFERENCES `conferencedb`.`user` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `conferencedb`.`access_code`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `conferencedb`.`access_code` ;

CREATE TABLE IF NOT EXISTS `conferencedb`.`access_code` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `code` VARCHAR(45) NULL,
  `online_event_event_id` INT NULL,
  `online_meeting_gathering_id` INT NULL,
  `gathering_role_id` INT NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_access_code_online_event1_idx` (`online_event_event_id` ASC) VISIBLE,
  INDEX `fk_access_code_online_meeting1_idx` (`online_meeting_gathering_id` ASC) VISIBLE,
  INDEX `fk_access_code_geathering_role1_idx` (`gathering_role_id` ASC) VISIBLE,
  CONSTRAINT `fk_access_code_online_event1`
    FOREIGN KEY (`online_event_event_id`)
    REFERENCES `conferencedb`.`online_event` (`event_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_access_code_online_meeting1`
    FOREIGN KEY (`online_meeting_gathering_id`)
    REFERENCES `conferencedb`.`online_meeting` (`gathering_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_access_code_geathering_role1`
    FOREIGN KEY (`gathering_role_id`)
    REFERENCES `conferencedb`.`gathering_role` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `conferencedb`.`user_gathering_role`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `conferencedb`.`user_gathering_role` ;

CREATE TABLE IF NOT EXISTS `conferencedb`.`user_gathering_role` (
  `gathering_id` INT NOT NULL,
  `user_id` INT NOT NULL,
  `gathering_role_id` INT NOT NULL,
  PRIMARY KEY (`gathering_id`, `user_id`),
  INDEX `fk_gathering_has_user_user1_idx` (`user_id` ASC) VISIBLE,
  INDEX `fk_gathering_has_user_gathering1_idx` (`gathering_id` ASC) VISIBLE,
  INDEX `fk_gathering_has_user_gathering_role1_idx` (`gathering_role_id` ASC) VISIBLE,
  CONSTRAINT `fk_gathering_has_user_gathering1`
    FOREIGN KEY (`gathering_id`)
    REFERENCES `conferencedb`.`gathering` (`id`)
    ON DELETE CASCADE
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_gathering_has_user_user1`
    FOREIGN KEY (`user_id`)
    REFERENCES `conferencedb`.`user` (`id`)
    ON DELETE RESTRICT
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_gathering_has_user_gathering_role1`
    FOREIGN KEY (`gathering_role_id`)
    REFERENCES `conferencedb`.`gathering_role` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb3;

