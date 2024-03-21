-- MySQL dump 10.13  Distrib 8.0.29, for Win64 (x86_64)
--
-- Host: localhost    Database: conferencedb
-- ------------------------------------------------------
-- Server version	8.0.29

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `conference`
--

DROP TABLE IF EXISTS `conference`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `conference` (
                              `gathering_id` int NOT NULL,
                              `start_date` datetime NOT NULL,
                              `end_date` datetime NOT NULL,
                              `name` varchar(255) NOT NULL,
                              PRIMARY KEY (`gathering_id`),
                              CONSTRAINT `fk_CONFERENCE_EVENT1` FOREIGN KEY (`gathering_id`) REFERENCES `gathering` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `conference`
--

LOCK TABLES `conference` WRITE;
/*!40000 ALTER TABLE `conference` DISABLE KEYS */;
/*!40000 ALTER TABLE `conference` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `event`
--

DROP TABLE IF EXISTS `event`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `event` (
                         `id` int NOT NULL AUTO_INCREMENT,
                         `session_id` int NOT NULL,
                         `event_type_id` int NOT NULL,
                         `name` varchar(255) NOT NULL,
                         `description` varchar(255) DEFAULT '',
                         `start_date` datetime NOT NULL,
                         `end_date` datetime NOT NULL,
                         PRIMARY KEY (`id`),
                         KEY `fk_EVENT_EVENT_TYPE1_idx` (`event_type_id`),
                         KEY `fk_EVENT_SESSION1_idx` (`session_id`),
                         CONSTRAINT `fk_EVENT_EVENT_TYPE1` FOREIGN KEY (`event_type_id`) REFERENCES `event_type` (`id`) ON DELETE RESTRICT,
                         CONSTRAINT `fk_EVENT_SESSION1` FOREIGN KEY (`session_id`) REFERENCES `session` (`id`) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `event`
--

LOCK TABLES `event` WRITE;
/*!40000 ALTER TABLE `event` DISABLE KEYS */;
/*!40000 ALTER TABLE `event` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `event_type`
--

DROP TABLE IF EXISTS `event_type`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `event_type` (
                              `id` int NOT NULL AUTO_INCREMENT,
                              `name` varchar(45) NOT NULL,
                              PRIMARY KEY (`id`),
                              UNIQUE KEY `name_UNIQUE` (`name`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `event_type`
--

LOCK TABLES `event_type` WRITE;
/*!40000 ALTER TABLE `event_type` DISABLE KEYS */;
INSERT INTO `event_type` VALUES (1,'lecture'),(3,'meeting'),(2,'presentation');
/*!40000 ALTER TABLE `event_type` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `gathering`
--

DROP TABLE IF EXISTS `gathering`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `gathering` (
                             `id` int NOT NULL AUTO_INCREMENT,
                             `description` varchar(255) DEFAULT NULL,
                             PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `gathering`
--

LOCK TABLES `gathering` WRITE;
/*!40000 ALTER TABLE `gathering` DISABLE KEYS */;
/*!40000 ALTER TABLE `gathering` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `gathering_role`
--

DROP TABLE IF EXISTS `gathering_role`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `gathering_role` (
                                  `id` int NOT NULL AUTO_INCREMENT,
                                  `name` varchar(45) NOT NULL,
                                  PRIMARY KEY (`id`),
                                  UNIQUE KEY `name_UNIQUE` (`name`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `gathering_role`
--

LOCK TABLES `gathering_role` WRITE;
/*!40000 ALTER TABLE `gathering_role` DISABLE KEYS */;
INSERT INTO `gathering_role` VALUES (3,'Moderator'),(1,'Organizer'),(4,'Presenter'),(2,'Visitor');
/*!40000 ALTER TABLE `gathering_role` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `live_event`
--

DROP TABLE IF EXISTS `live_event`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `live_event` (
                              `event_id` int NOT NULL,
                              `city` varchar(255) NOT NULL,
                              `address` varchar(255) NOT NULL,
                              PRIMARY KEY (`event_id`),
                              CONSTRAINT `fk_LIVE_EVENT_EVENT1` FOREIGN KEY (`event_id`) REFERENCES `event` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `live_event`
--

LOCK TABLES `live_event` WRITE;
/*!40000 ALTER TABLE `live_event` DISABLE KEYS */;
/*!40000 ALTER TABLE `live_event` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `online_event`
--

DROP TABLE IF EXISTS `online_event`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `online_event` (
                                `event_id` int NOT NULL,
                                `url` varchar(255) DEFAULT NULL,
                                PRIMARY KEY (`event_id`),
                                CONSTRAINT `fk_ONLINE_EVENT_EVENT1` FOREIGN KEY (`event_id`) REFERENCES `event` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `online_event`
--

LOCK TABLES `online_event` WRITE;
/*!40000 ALTER TABLE `online_event` DISABLE KEYS */;
/*!40000 ALTER TABLE `online_event` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `role`
--

DROP TABLE IF EXISTS `role`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `role` (
                        `id` int NOT NULL AUTO_INCREMENT,
                        `name` varchar(45) NOT NULL,
                        PRIMARY KEY (`id`),
                        UNIQUE KEY `name_UNIQUE` (`name`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `role`
--

LOCK TABLES `role` WRITE;
/*!40000 ALTER TABLE `role` DISABLE KEYS */;
INSERT INTO `role` VALUES (4,'admin'),(5,'user');
/*!40000 ALTER TABLE `role` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `session`
--

DROP TABLE IF EXISTS `session`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `session` (
                           `id` int NOT NULL AUTO_INCREMENT,
                           `gathering_id` int NOT NULL,
                           `start_date` datetime NOT NULL,
                           `end_date` datetime NOT NULL,
                           `name` varchar(255) NOT NULL,
                           `description` varchar(255) DEFAULT '',
                           PRIMARY KEY (`id`),
                           KEY `fk_SESSION_CONFERENCE1_idx` (`gathering_id`),
                           CONSTRAINT `fk_SESSION_CONFERENCE1` FOREIGN KEY (`gathering_id`) REFERENCES `conference` (`gathering_id`) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `session`
--

LOCK TABLES `session` WRITE;
/*!40000 ALTER TABLE `session` DISABLE KEYS */;
/*!40000 ALTER TABLE `session` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `settings`
--

DROP TABLE IF EXISTS `settings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `settings` (
                            `user_id` int NOT NULL,
                            `language` varchar(45) DEFAULT 'en',
                            `theme` varchar(45) DEFAULT 'default',
                            PRIMARY KEY (`user_id`),
                            CONSTRAINT `fk_settings_user1` FOREIGN KEY (`user_id`) REFERENCES `user` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `settings`
--

LOCK TABLES `settings` WRITE;
/*!40000 ALTER TABLE `settings` DISABLE KEYS */;
INSERT INTO `settings` VALUES (1,'en','default');
/*!40000 ALTER TABLE `settings` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user`
--

DROP TABLE IF EXISTS `user`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `user` (
                        `id` int NOT NULL AUTO_INCREMENT,
                        `username` varchar(255) NOT NULL,
                        `password` varchar(255) NOT NULL,
                        `first_name` varchar(255) NOT NULL,
                        `last_name` varchar(255) NOT NULL,
                        `email` varchar(255) NOT NULL,
                        PRIMARY KEY (`id`),
                        UNIQUE KEY `username_UNIQUE` (`username`),
                        UNIQUE KEY `email_UNIQUE` (`email`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user`
--

LOCK TABLES `user` WRITE;
/*!40000 ALTER TABLE `user` DISABLE KEYS */;
INSERT INTO `user` VALUES (1,'admin','YWRtaW4=','admin','admin','a@a.com'),(2,'user','dXNlcg==','user','user','b@b.com');
/*!40000 ALTER TABLE `user` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user_gathering_role`
--

DROP TABLE IF EXISTS `user_gathering_role`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `user_gathering_role` (
                                       `gathering_id` int NOT NULL,
                                       `user_id` int NOT NULL,
                                       `gathering_role_id` int NOT NULL,
                                       PRIMARY KEY (`gathering_id`,`user_id`),
                                       KEY `fk_gathering_has_user_user1_idx` (`user_id`),
                                       KEY `fk_gathering_has_user_gathering1_idx` (`gathering_id`),
                                       KEY `fk_gathering_has_user_gathering_role1_idx` (`gathering_role_id`),
                                       CONSTRAINT `fk_gathering_has_user_gathering1` FOREIGN KEY (`gathering_id`) REFERENCES `gathering` (`id`) ON DELETE CASCADE,
                                       CONSTRAINT `fk_gathering_has_user_gathering_role1` FOREIGN KEY (`gathering_role_id`) REFERENCES `gathering_role` (`id`),
                                       CONSTRAINT `fk_gathering_has_user_user1` FOREIGN KEY (`user_id`) REFERENCES `user` (`id`) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user_gathering_role`
--

LOCK TABLES `user_gathering_role` WRITE;
/*!40000 ALTER TABLE `user_gathering_role` DISABLE KEYS */;
/*!40000 ALTER TABLE `user_gathering_role` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user_has_role`
--

DROP TABLE IF EXISTS `user_has_role`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `user_has_role` (
                                 `user_id` int NOT NULL,
                                 `role_id` int NOT NULL,
                                 PRIMARY KEY (`user_id`,`role_id`),
                                 KEY `fk_User_has_Role_Role1_idx` (`role_id`),
                                 KEY `fk_User_has_Role_User_idx` (`user_id`),
                                 CONSTRAINT `fk_User_has_Role_Role1` FOREIGN KEY (`role_id`) REFERENCES `role` (`id`),
                                 CONSTRAINT `fk_User_has_Role_User` FOREIGN KEY (`user_id`) REFERENCES `user` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user_has_role`
--

LOCK TABLES `user_has_role` WRITE;
/*!40000 ALTER TABLE `user_has_role` DISABLE KEYS */;
INSERT INTO `user_has_role` VALUES (1,4),(1,5),(2,5);
/*!40000 ALTER TABLE `user_has_role` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping routines for database 'conferencedb'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-03-21 20:09:03
