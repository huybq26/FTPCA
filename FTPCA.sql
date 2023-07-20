-- MySQL dump 10.13  Distrib 8.0.33, for Linux (x86_64)
--
-- Host: localhost    Database: FTPCA
-- ------------------------------------------------------
-- Server version	8.0.33-0ubuntu0.20.04.2

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
-- Table structure for table `Conv_Parti`
--

DROP TABLE IF EXISTS `Conv_Parti`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Conv_Parti` (
  `convid` varchar(36) NOT NULL,
  `userid` int NOT NULL,
  KEY `fk_convid_idx` (`convid`),
  KEY `fk_userid_idx` (`userid`),
  CONSTRAINT `Conv_Parti_ibfk_1` FOREIGN KEY (`convid`) REFERENCES `Conversation` (`convid`),
  CONSTRAINT `Conv_Parti_ibfk_2` FOREIGN KEY (`userid`) REFERENCES `User` (`userid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Conv_Parti`
--

LOCK TABLES `Conv_Parti` WRITE;
/*!40000 ALTER TABLE `Conv_Parti` DISABLE KEYS */;
INSERT INTO `Conv_Parti` VALUES ('ff10eb9d-e684-400d-b921-2f9d9a95048d',4),('ff10eb9d-e684-400d-b921-2f9d9a95048d',5),('ff10eb9d-e684-400d-b921-2f9d9a95048d',3),('7527df0f-6668-4d3b-a4be-204c4d316769',4),('7527df0f-6668-4d3b-a4be-204c4d316769',5),('667e4efb-e117-4355-8e3d-ee7167c3d3cc',6),('667e4efb-e117-4355-8e3d-ee7167c3d3cc',4),('667e4efb-e117-4355-8e3d-ee7167c3d3cc',3),('10248637-22c1-43db-8749-7427f9807d23',4),('10248637-22c1-43db-8749-7427f9807d23',6),('10248637-22c1-43db-8749-7427f9807d23',3),('10248637-22c1-43db-8749-7427f9807d23',5);
/*!40000 ALTER TABLE `Conv_Parti` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Conversation`
--

DROP TABLE IF EXISTS `Conversation`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Conversation` (
  `convid` varchar(36) NOT NULL,
  `convname` varchar(255) NOT NULL,
  `creationtime` datetime NOT NULL,
  `lastmessage` datetime NOT NULL,
  `lastmessagecontent` varchar(1000) DEFAULT NULL,
  `creator` int DEFAULT NULL,
  `lastsender` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`convid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Conversation`
--

LOCK TABLES `Conversation` WRITE;
/*!40000 ALTER TABLE `Conversation` DISABLE KEYS */;
INSERT INTO `Conversation` VALUES ('10248637-22c1-43db-8749-7427f9807d23','cat2','2023-06-24 00:04:17','2023-07-05 00:10:46',NULL,NULL,NULL),('667e4efb-e117-4355-8e3d-ee7167c3d3cc','cat','2023-06-24 00:02:05','2023-07-21 01:21:45','asd',NULL,'test3'),('7527df0f-6668-4d3b-a4be-204c4d316769','secondchat','2023-06-16 22:32:06','2023-07-05 00:11:01',NULL,NULL,NULL),('ff10eb9d-e684-400d-b921-2f9d9a95048d','firstchat','2023-06-16 22:28:24','2023-07-21 01:16:18','good?',NULL,'test1');
/*!40000 ALTER TABLE `Conversation` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `File`
--

DROP TABLE IF EXISTS `File`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `File` (
  `fileid` varchar(36) NOT NULL,
  `filename` varchar(255) NOT NULL,
  `filepath` varchar(255) NOT NULL,
  `filesize` int NOT NULL,
  `filetype` varchar(50) NOT NULL,
  PRIMARY KEY (`fileid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `File`
--

LOCK TABLES `File` WRITE;
/*!40000 ALTER TABLE `File` DISABLE KEYS */;
INSERT INTO `File` VALUES ('0f25757e-3729-4ed3-9af5-664b8464994f','test.txt','/home/bui_quang_huy/Documents/Huy/Internship_and_Projects/FTPCA/FTPCA/server/wwwroot/Uploads/0f25757e-3729-4ed3-9af5-664b8464994f_test.txt',12,'text/plain'),('15c238a2-6b24-42b6-911c-a826233a268c','test.txt','/home/bui_quang_huy/Documents/Huy/Internship_and_Projects/FTPCA/FTPCA/server/Uploads/15c238a2-6b24-42b6-911c-a826233a268c_test.txt',12,'text/plain'),('188e7855-f181-417b-994e-0c844a2812eb','ParaViewGettingStarted-5.11.1.pdf','/home/bui_quang_huy/Documents/Huy/Internship_and_Projects/FTPCA/FTPCA/server/wwwroot/Uploads/188e7855-f181-417b-994e-0c844a2812eb_ParaViewGettingStarted-5.11.1.pdf',1326335,'application/pdf'),('38fea6de-53fa-410b-8ed5-ca5b54596946','test.txt','/home/bui_quang_huy/Documents/Huy/Internship_and_Projects/FTPCA/FTPCA/server/wwwroot/Uploads/38fea6de-53fa-410b-8ed5-ca5b54596946_test.txt',12,'text/plain'),('3ddc5a6b-3b4f-403f-90d8-eef7a7dc5105','lec7.pdf_3ddc5a6b-3b4f-403f-90d8-eef7a7dc5105','/home/bui_quang_huy/Documents/Huy/Internship_and_Projects/FTPCA/FTPCA/server/Uploads/lec7.pdf_3ddc5a6b-3b4f-403f-90d8-eef7a7dc5105',3839219,'application/pdf'),('615a9b57-412d-4bf0-b414-f1587ff18362','test.txt','/home/bui_quang_huy/Documents/Huy/Internship_and_Projects/FTPCA/FTPCA/server/Uploads/615a9b57-412d-4bf0-b414-f1587ff18362_test.txt',12,'text/plain'),('6808ca93-c038-4d52-8e72-09c7067784dd','test.txt','/home/bui_quang_huy/Documents/Huy/Internship_and_Projects/FTPCA/FTPCA/server/Uploads/6808ca93-c038-4d52-8e72-09c7067784dd_test.txt',12,'text/plain'),('7a1e03e8-0a09-422b-a4f3-fb0c00e8c273','test.txt','/home/bui_quang_huy/Documents/Huy/Internship_and_Projects/FTPCA/FTPCA/server/Uploads/7a1e03e8-0a09-422b-a4f3-fb0c00e8c273_test.txt',12,'text/plain'),('7ac7f03b-84ef-4df5-aca9-2911a4107c9f','test.txt','/home/bui_quang_huy/Documents/Huy/Internship_and_Projects/FTPCA/FTPCA/server/Uploads/7ac7f03b-84ef-4df5-aca9-2911a4107c9f_test.txt',12,'text/plain'),('7b4babcc-6569-497b-9172-ec091348b24e','test.txt','/home/bui_quang_huy/Documents/Huy/Internship_and_Projects/FTPCA/FTPCA/server/Uploads/7b4babcc-6569-497b-9172-ec091348b24e_test.txt',12,'text/plain'),('7d27a834-3221-4b1e-972a-653ef2953045','test.txt','/home/bui_quang_huy/Documents/Huy/Internship_and_Projects/FTPCA/FTPCA/server/Uploads/7d27a834-3221-4b1e-972a-653ef2953045_test.txt',12,'text/plain'),('8b98ae80-c702-455d-bf5e-bef6068ae4bd','test.txt','/home/bui_quang_huy/Documents/Huy/Internship_and_Projects/FTPCA/FTPCA/server/Uploads/8b98ae80-c702-455d-bf5e-bef6068ae4bd_test.txt',12,'text/plain'),('93bffd2a-cb79-4596-b43a-388c0300d647','Screenshot from 2023-07-10 10-56-15.png','/home/bui_quang_huy/Documents/Huy/Internship_and_Projects/FTPCA/FTPCA/server/Uploads/93bffd2a-cb79-4596-b43a-388c0300d647_Screenshot from 2023-07-10 10-56-15.png',133379,'image/png'),('9d671538-930b-4259-9af3-79fc7ab5a009','snakeGame.mp4','/home/bui_quang_huy/Documents/Huy/Internship_and_Projects/FTPCA/FTPCA/server/Uploads/9d671538-930b-4259-9af3-79fc7ab5a009_snakeGame.mp4',615357,'video/mp4'),('a95be82f-5b76-40ca-a397-4760b831bf62','test.txt','/home/bui_quang_huy/Documents/Huy/Internship_and_Projects/FTPCA/FTPCA/server/Uploads/a95be82f-5b76-40ca-a397-4760b831bf62_test.txt',12,'text/plain'),('c9b6975f-a333-4ae0-8cf3-9c62184b7490','test.txt','/home/bui_quang_huy/Documents/Huy/Internship_and_Projects/FTPCA/FTPCA/server/Uploads/c9b6975f-a333-4ae0-8cf3-9c62184b7490_test.txt',12,'text/plain'),('dc26117a-4ce9-494b-b59a-4d2873d4ffdc','test.txt','/home/bui_quang_huy/Documents/Huy/Internship_and_Projects/FTPCA/FTPCA/server/Uploads/dc26117a-4ce9-494b-b59a-4d2873d4ffdc_test.txt',12,'text/plain'),('e1518a92-f6bb-4729-a5aa-1eeb514977ca','bscan-test.png','/home/bui_quang_huy/Documents/Huy/Internship_and_Projects/FTPCA/FTPCA/server/wwwroot/Uploads/e1518a92-f6bb-4729-a5aa-1eeb514977ca_bscan-test.png',82487,'image/png'),('ed22bcd7-3226-4e11-892b-ba1ac73234bf','test.txt','/home/bui_quang_huy/Documents/Huy/Internship_and_Projects/FTPCA/FTPCA/server/Uploads/ed22bcd7-3226-4e11-892b-ba1ac73234bf_test.txt',12,'text/plain'),('f906c716-95bf-4faa-846b-fbdbce078935','test.txt','/home/bui_quang_huy/Documents/Huy/Internship_and_Projects/FTPCA/FTPCA/server/Uploads/f906c716-95bf-4faa-846b-fbdbce078935_test.txt',12,'text/plain'),('ff526ff9-1194-48be-8628-06c7109f16f2','test.txt','/home/bui_quang_huy/Documents/Huy/Internship_and_Projects/FTPCA/FTPCA/server/Uploads/ff526ff9-1194-48be-8628-06c7109f16f2_test.txt',12,'text/plain');
/*!40000 ALTER TABLE `File` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `FriendRequest`
--

DROP TABLE IF EXISTS `FriendRequest`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `FriendRequest` (
  `senderid` int NOT NULL,
  `receiverid` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `FriendRequest`
--

LOCK TABLES `FriendRequest` WRITE;
/*!40000 ALTER TABLE `FriendRequest` DISABLE KEYS */;
INSERT INTO `FriendRequest` VALUES (4,3),(5,3);
/*!40000 ALTER TABLE `FriendRequest` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Friendship`
--

DROP TABLE IF EXISTS `Friendship`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Friendship` (
  `userid1` int NOT NULL,
  `userid2` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Friendship`
--

LOCK TABLES `Friendship` WRITE;
/*!40000 ALTER TABLE `Friendship` DISABLE KEYS */;
INSERT INTO `Friendship` VALUES (4,5),(6,4),(6,5);
/*!40000 ALTER TABLE `Friendship` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Message`
--

DROP TABLE IF EXISTS `Message`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Message` (
  `messageid` varchar(36) NOT NULL,
  `convid` varchar(36) NOT NULL,
  `senderid` int NOT NULL,
  `content` varchar(1000) NOT NULL,
  `timestampt` datetime NOT NULL,
  `fileid` varchar(36) DEFAULT NULL,
  PRIMARY KEY (`messageid`),
  KEY `fk_convid_idx` (`convid`),
  KEY `fk_senderid_idx` (`senderid`),
  CONSTRAINT `Message_ibfk_1` FOREIGN KEY (`convid`) REFERENCES `Conversation` (`convid`),
  CONSTRAINT `Message_ibfk_2` FOREIGN KEY (`senderid`) REFERENCES `User` (`userid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Message`
--

LOCK TABLES `Message` WRITE;
/*!40000 ALTER TABLE `Message` DISABLE KEYS */;
INSERT INTO `Message` VALUES ('1089443f-0a94-4d85-aa89-a3b954142c3e','667e4efb-e117-4355-8e3d-ee7167c3d3cc',6,'asd','2023-07-21 01:21:45','null'),('15a71986-30ef-4bb7-a01e-5166feaaf885','667e4efb-e117-4355-8e3d-ee7167c3d3cc',6,'Anyone here','2023-07-21 01:16:35','null'),('197cd3e2-2a5e-4f51-9c29-3392d85953d0','667e4efb-e117-4355-8e3d-ee7167c3d3cc',6,'I am here','2023-07-21 01:16:45','null'),('1d22ee12-7eab-4f86-97f0-2525f51528cc','ff10eb9d-e684-400d-b921-2f9d9a95048d',4,'asdfasd','2023-07-21 00:59:37','null'),('2580169c-8c64-4b13-816f-2bd892884c61','ff10eb9d-e684-400d-b921-2f9d9a95048d',4,'Sent a file','2023-07-20 22:45:11','e1518a92-f6bb-4729-a5aa-1eeb514977ca'),('2ebf8074-45d5-4919-ba38-634cd668d3b2','ff10eb9d-e684-400d-b921-2f9d9a95048d',4,'sdafsd','2023-07-20 22:24:00','null'),('31240e08-48bd-45c6-8311-f44a5efbad2d','ff10eb9d-e684-400d-b921-2f9d9a95048d',5,'hi','2023-07-07 01:16:08',NULL),('32b26974-5c12-4ecf-9e31-c29f969b5ca5','ff10eb9d-e684-400d-b921-2f9d9a95048d',4,'hi guys','2023-07-21 00:59:47','null'),('380d8eff-b38b-40b3-a54a-592452880f6f','667e4efb-e117-4355-8e3d-ee7167c3d3cc',4,'fisrt','2023-07-05 00:10:52',NULL),('38a15baa-6e5d-4cb1-b11e-e8bcaa3afb07','ff10eb9d-e684-400d-b921-2f9d9a95048d',3,'Hello guys I am John Doe','2023-06-16 22:56:36',NULL),('48a6350d-431e-47dc-860d-26c9ff8fbc94','ff10eb9d-e684-400d-b921-2f9d9a95048d',4,'JUne 22 checking!','2023-06-22 22:29:28',NULL),('523146f4-004c-4e31-bdca-8ded0ebed995','ff10eb9d-e684-400d-b921-2f9d9a95048d',4,'\"Chao moi nguoi\"','2023-06-16 22:55:12',NULL),('6a44aa07-6d24-49c5-95cd-8162ac6b4d26','ff10eb9d-e684-400d-b921-2f9d9a95048d',4,'asdfasd','2023-07-05 00:05:22',NULL),('7a11dc82-2288-4c94-b7e8-66c70d4a1071','ff10eb9d-e684-400d-b921-2f9d9a95048d',4,'lo guys','2023-07-05 00:09:40',NULL),('895caa51-84de-467f-ba66-a167f8395313','ff10eb9d-e684-400d-b921-2f9d9a95048d',4,'Hello','2023-06-16 22:50:22',NULL),('9df99e4f-3f95-4971-a076-a45bb130e520','ff10eb9d-e684-400d-b921-2f9d9a95048d',4,'helo','2023-07-21 01:16:01','null'),('ab8ea587-5ba6-451a-be6a-8be304650e21','ff10eb9d-e684-400d-b921-2f9d9a95048d',4,'hi guys','2023-07-05 00:07:58',NULL),('b363691d-1be0-48db-92b3-6859aef56f85','667e4efb-e117-4355-8e3d-ee7167c3d3cc',6,'Sent a file','2023-07-21 01:17:58','0f25757e-3729-4ed3-9af5-664b8464994f'),('b5c7ed63-4744-4dd4-badb-96bcbc1809d3','7527df0f-6668-4d3b-a4be-204c4d316769',4,'asdf','2023-07-05 00:11:01',NULL),('bb3863d5-6899-4534-9c72-8cfa049f6646','667e4efb-e117-4355-8e3d-ee7167c3d3cc',6,'vz','2023-07-21 01:18:05','null'),('c15da506-07a6-43e7-b0c0-2d83d2a45420','10248637-22c1-43db-8749-7427f9807d23',4,'dfsgdf','2023-07-05 00:10:46',NULL),('cb92b14d-bf31-4c87-a4f4-84592d06cd77','ff10eb9d-e684-400d-b921-2f9d9a95048d',4,'Sent a file','2023-07-20 23:02:03','188e7855-f181-417b-994e-0c844a2812eb'),('cc301eb6-25c2-4bfc-8ddf-3c8c6a4f0de8','ff10eb9d-e684-400d-b921-2f9d9a95048d',4,'j;kasjdf;as','2023-06-22 22:44:02',NULL),('d1883ca8-4aff-48b6-a388-a9c6fb2ce5de','ff10eb9d-e684-400d-b921-2f9d9a95048d',4,'fasdfasdfasdfasd','2023-06-22 22:44:09',NULL),('d8c75774-90b2-4f45-b1fa-852ca656efa5','667e4efb-e117-4355-8e3d-ee7167c3d3cc',6,'asd','2023-07-21 01:17:44','null'),('dbef86ec-9882-498b-b4ba-2841116283ed','ff10eb9d-e684-400d-b921-2f9d9a95048d',4,'Sent a file','2023-07-20 22:24:13','615a9b57-412d-4bf0-b414-f1587ff18362'),('dc59dc60-8bf1-4143-9376-167a6fe18383','ff10eb9d-e684-400d-b921-2f9d9a95048d',4,'asdfasdfasdfasdfasdf','2023-06-22 22:44:21',NULL),('e852bd4b-8cb5-44d5-97af-c9fc7ad9d447','ff10eb9d-e684-400d-b921-2f9d9a95048d',4,'good?','2023-07-21 01:16:18','null'),('f3d310e9-0b97-48db-832b-be1b80528bc8','ff10eb9d-e684-400d-b921-2f9d9a95048d',4,'Hello hesly','2023-06-16 22:55:28',NULL),('f52f6a9f-42d6-4940-ab67-bb133917b8fb','ff10eb9d-e684-400d-b921-2f9d9a95048d',5,'Hello test1 from test2','2023-06-16 22:56:04',NULL),('fc06ef76-c9ed-4c0b-b0f4-93e22eae1508','ff10eb9d-e684-400d-b921-2f9d9a95048d',4,'asdfas','2023-07-21 00:33:17','null');
/*!40000 ALTER TABLE `Message` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `User`
--

DROP TABLE IF EXISTS `User`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `User` (
  `userid` int NOT NULL AUTO_INCREMENT,
  `username` varchar(255) NOT NULL,
  `phonenumber` varchar(255) NOT NULL,
  `email` varchar(255) NOT NULL,
  `name` varchar(255) NOT NULL,
  `hashedpassword` varchar(255) NOT NULL,
  PRIMARY KEY (`userid`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `User`
--

LOCK TABLES `User` WRITE;
/*!40000 ALTER TABLE `User` DISABLE KEYS */;
INSERT INTO `User` VALUES (1,'admin2','12345678','admin@gmail.com','Admin2','admin'),(2,'john_doe','123456789','john_doe@example.com','John Doe','$2a$11$nPQDCsQiQSG61IbHb4KKOuQVyoEkVXVw.BbDIITRX08t71GfUK.UG'),(3,'contributor','123456789','john_doe@example.com','John Doe','$2a$11$eOLjTN5mUsDbuD/aWM0DKOxHXDUhYtBzTZIOqMqvaPAP1bGrKFsKy'),(4,'test1','2123412','sfsdf@gmail.com','test1','$2a$11$is0FVJj4aVBZ9KS02EdwCOqL7AEG.QDMGN9OiSaDLwKX1HQk6nlfu'),(5,'test2','2123412','sfsdf@gmail.com','test1','$2a$11$NDBqMb8KF/7KUa2sq.oVuuAJOm7Q4LYfWXbyMWm64IvcOKyL9oUja'),(6,'test3','2123412','sfsdf@gmail.com','test3','$2a$11$G8/rsvmhZ8coyrFXdeHbI.cAdbjucH19KXESifH1qCrXhIcfjFmMK');
/*!40000 ALTER TABLE `User` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2023-07-21  1:31:02
