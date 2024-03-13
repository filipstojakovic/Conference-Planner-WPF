-- MySQL dump 10.13  Distrib 8.0.29, for Win64 (x86_64)
--
-- Host: localhost    Database: conferencedb
-- ------------------------------------------------------
-- Server version	8.0.29

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Dumping data for table `access_code`
--

LOCK TABLES `access_code` WRITE;
/*!40000 ALTER TABLE `access_code` DISABLE KEYS */;
/*!40000 ALTER TABLE `access_code` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `conference`
--

LOCK TABLES `conference` WRITE;
/*!40000 ALTER TABLE `conference` DISABLE KEYS */;
INSERT INTO `conference` VALUES (2,'2024-03-10 00:00:00','2024-03-20 23:59:59','conference1');
/*!40000 ALTER TABLE `conference` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `conference_has_sponsor`
--

LOCK TABLES `conference_has_sponsor` WRITE;
/*!40000 ALTER TABLE `conference_has_sponsor` DISABLE KEYS */;
/*!40000 ALTER TABLE `conference_has_sponsor` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `event`
--

LOCK TABLES `event` WRITE;
/*!40000 ALTER TABLE `event` DISABLE KEYS */;
INSERT INTO `event` VALUES (3,3,1,'event1','opis event1','2024-03-10 10:00:00','2024-03-10 12:00:00'),(4,3,3,'event2','opis event2','2024-03-10 13:00:00','2024-03-10 15:00:00'),(5,4,2,'event 3','opis eventa 3','2024-03-16 08:00:00','2024-03-16 10:00:00'),(6,4,1,'evet 4','opis eventa 4','2024-03-17 08:00:00','2024-03-17 10:00:00');
/*!40000 ALTER TABLE `event` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `event_type`
--

LOCK TABLES `event_type` WRITE;
/*!40000 ALTER TABLE `event_type` DISABLE KEYS */;
INSERT INTO `event_type` VALUES (1,'lecture'),(3,'meeting'),(2,'presentation');
/*!40000 ALTER TABLE `event_type` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `gathering`
--

LOCK TABLES `gathering` WRITE;
/*!40000 ALTER TABLE `gathering` DISABLE KEYS */;
INSERT INTO `gathering` VALUES (2,'opis conferencije');
/*!40000 ALTER TABLE `gathering` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `gathering_has_resource`
--

LOCK TABLES `gathering_has_resource` WRITE;
/*!40000 ALTER TABLE `gathering_has_resource` DISABLE KEYS */;
/*!40000 ALTER TABLE `gathering_has_resource` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `gathering_role`
--

LOCK TABLES `gathering_role` WRITE;
/*!40000 ALTER TABLE `gathering_role` DISABLE KEYS */;
INSERT INTO `gathering_role` VALUES (3,'Moderator'),(1,'Organizer'),(4,'Presenter'),(2,'Visitor');
/*!40000 ALTER TABLE `gathering_role` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `invitation`
--

LOCK TABLES `invitation` WRITE;
/*!40000 ALTER TABLE `invitation` DISABLE KEYS */;
/*!40000 ALTER TABLE `invitation` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `live_event`
--

LOCK TABLES `live_event` WRITE;
/*!40000 ALTER TABLE `live_event` DISABLE KEYS */;
INSERT INTO `live_event` VALUES (3,3),(4,4),(5,5),(6,6);
/*!40000 ALTER TABLE `live_event` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `live_meeting`
--

LOCK TABLES `live_meeting` WRITE;
/*!40000 ALTER TABLE `live_meeting` DISABLE KEYS */;
/*!40000 ALTER TABLE `live_meeting` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `location`
--

LOCK TABLES `location` WRITE;
/*!40000 ALTER TABLE `location` DISABLE KEYS */;
INSERT INTO `location` VALUES (1,2,'Banja Luka','Patre 5'),(2,2,'Banja Luka','Patre 5'),(3,2,'city1','street name'),(4,2,'city','street name'),(5,2,'city','street name'),(6,2,'city','street name');
/*!40000 ALTER TABLE `location` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `location_type`
--

LOCK TABLES `location_type` WRITE;
/*!40000 ALTER TABLE `location_type` DISABLE KEYS */;
INSERT INTO `location_type` VALUES (1,'hall'),(2,'building'),(3,'hotel');
/*!40000 ALTER TABLE `location_type` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `meeting`
--

LOCK TABLES `meeting` WRITE;
/*!40000 ALTER TABLE `meeting` DISABLE KEYS */;
/*!40000 ALTER TABLE `meeting` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `online_event`
--

LOCK TABLES `online_event` WRITE;
/*!40000 ALTER TABLE `online_event` DISABLE KEYS */;
/*!40000 ALTER TABLE `online_event` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `online_meeting`
--

LOCK TABLES `online_meeting` WRITE;
/*!40000 ALTER TABLE `online_meeting` DISABLE KEYS */;
/*!40000 ALTER TABLE `online_meeting` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `resource`
--

LOCK TABLES `resource` WRITE;
/*!40000 ALTER TABLE `resource` DISABLE KEYS */;
/*!40000 ALTER TABLE `resource` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `role`
--

LOCK TABLES `role` WRITE;
/*!40000 ALTER TABLE `role` DISABLE KEYS */;
INSERT INTO `role` VALUES (4,'admin'),(6,'moderator'),(5,'user');
/*!40000 ALTER TABLE `role` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `room`
--

LOCK TABLES `room` WRITE;
/*!40000 ALTER TABLE `room` DISABLE KEYS */;
INSERT INTO `room` VALUES (1,1,'1110',100),(2,2,'1110',100),(3,3,'10',100),(4,4,'22',100),(5,5,'123',100),(6,6,'456',100);
/*!40000 ALTER TABLE `room` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `session`
--

LOCK TABLES `session` WRITE;
/*!40000 ALTER TABLE `session` DISABLE KEYS */;
INSERT INTO `session` VALUES (3,2,'2024-03-10 10:00:00','2024-03-15 20:00:00','session1','opis session1'),(4,2,'2024-03-16 08:00:00','2024-03-20 20:00:00','session2','opis session2');
/*!40000 ALTER TABLE `session` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `settings`
--

LOCK TABLES `settings` WRITE;
/*!40000 ALTER TABLE `settings` DISABLE KEYS */;
/*!40000 ALTER TABLE `settings` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `sponsor`
--

LOCK TABLES `sponsor` WRITE;
/*!40000 ALTER TABLE `sponsor` DISABLE KEYS */;
/*!40000 ALTER TABLE `sponsor` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `ticket`
--

LOCK TABLES `ticket` WRITE;
/*!40000 ALTER TABLE `ticket` DISABLE KEYS */;
/*!40000 ALTER TABLE `ticket` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `user`
--

LOCK TABLES `user` WRITE;
/*!40000 ALTER TABLE `user` DISABLE KEYS */;
INSERT INTO `user` VALUES (1,'admin','YWRtaW4=','admin','admin','a@a.com'),(2,'user','dXNlcg==','user','user','b@b.com'),(3,'mod','bW9k','moderator','moderator','c@c.com');
/*!40000 ALTER TABLE `user` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `user_gathering_role`
--

LOCK TABLES `user_gathering_role` WRITE;
/*!40000 ALTER TABLE `user_gathering_role` DISABLE KEYS */;
INSERT INTO `user_gathering_role` VALUES (2,1,1),(2,2,3);
/*!40000 ALTER TABLE `user_gathering_role` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `user_has_role`
--

LOCK TABLES `user_has_role` WRITE;
/*!40000 ALTER TABLE `user_has_role` DISABLE KEYS */;
INSERT INTO `user_has_role` VALUES (1,4),(1,5),(2,5),(3,5),(3,6);
/*!40000 ALTER TABLE `user_has_role` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `vote`
--

LOCK TABLES `vote` WRITE;
/*!40000 ALTER TABLE `vote` DISABLE KEYS */;
/*!40000 ALTER TABLE `vote` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-03-09 22:48:05
