

LOCK TABLES `event_type` WRITE;
/*!40000 ALTER TABLE `event_type` DISABLE KEYS */;
INSERT INTO `event_type` VALUES (1,'lecture'),(3,'meeting'),(2,'presentation');
/*!40000 ALTER TABLE `event_type` ENABLE KEYS */;
UNLOCK TABLES;


LOCK TABLES `gathering_role` WRITE;
/*!40000 ALTER TABLE `gathering_role` DISABLE KEYS */;
INSERT INTO `gathering_role` VALUES (3,'Moderator'),(1,'Organizer'),(4,'Presenter'),(2,'Visitor');
/*!40000 ALTER TABLE `gathering_role` ENABLE KEYS */;
UNLOCK TABLES;

LOCK TABLES `role` WRITE;
/*!40000 ALTER TABLE `role` DISABLE KEYS */;
INSERT INTO `role` VALUES (4,'admin'),(6,'moderator'),(5,'user');
/*!40000 ALTER TABLE `role` ENABLE KEYS */;
UNLOCK TABLES;

LOCK TABLES `user` WRITE;
/*!40000 ALTER TABLE `user` DISABLE KEYS */;
INSERT INTO `user` VALUES (1,'admin','YWRtaW4=','admin','admin','a@a.com'),(2,'user','dXNlcg==','user','user','b@b.com'),(3,'mod','bW9k','moderator','moderator','c@c.com');
/*!40000 ALTER TABLE `user` ENABLE KEYS */;
UNLOCK TABLES;

LOCK TABLES `user_has_role` WRITE;
/*!40000 ALTER TABLE `user_has_role` DISABLE KEYS */;
INSERT INTO `user_has_role` VALUES (1,4),(1,5),(2,5),(3,5),(3,6);
/*!40000 ALTER TABLE `user_has_role` ENABLE KEYS */;
UNLOCK TABLES;
