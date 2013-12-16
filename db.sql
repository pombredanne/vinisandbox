CREATE DATABASE  IF NOT EXISTS `vinisandbox` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `vinisandbox`;
-- MySQL dump 10.13  Distrib 5.6.12, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: vinisandbox
-- ------------------------------------------------------
-- Server version	5.6.12

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `comment`
--

DROP TABLE IF EXISTS `comment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `comment` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `source` enum('manual','automatic') DEFAULT NULL,
  `comment` varchar(300) DEFAULT NULL,
  `id_file_detail` int(11) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_comment_file_properties_idx` (`id_file_detail`),
  CONSTRAINT `fk_comment_file_detail` FOREIGN KEY (`id_file_detail`) REFERENCES `file_detail` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `import_function`
--

DROP TABLE IF EXISTS `import_function`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `import_function` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `offset` varchar(20) DEFAULT NULL,
  `name` varchar(45) DEFAULT NULL,
  `import_library_id` int(11) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_import_library_idx` (`import_library_id`),
  CONSTRAINT `fk_import_library` FOREIGN KEY (`import_library_id`) REFERENCES `import_library` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `dns`
--

DROP TABLE IF EXISTS `dns`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `dns` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `domain` varchar(120) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `antivirus_scan`
--

DROP TABLE IF EXISTS `antivirus_scan`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `antivirus_scan` (
  `result` varchar(70) DEFAULT NULL,
  `av_version` varchar(45) DEFAULT NULL,
  `av_last_update` date DEFAULT NULL,
  `id_antivirus` int(11) NOT NULL,
  `id_analysis` int(11) NOT NULL,
  PRIMARY KEY (`id_analysis`,`id_antivirus`),
  KEY `fk_file_properties_idx` (`id_analysis`),
  KEY `fk_antivirus_idx` (`id_antivirus`),
  CONSTRAINT `fk_antivirus_scan_analysis` FOREIGN KEY (`id_analysis`) REFERENCES `analysis` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_antivirus_scan_antivirus` FOREIGN KEY (`id_antivirus`) REFERENCES `antivirus` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `miscellaneous`
--

DROP TABLE IF EXISTS `miscellaneous`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `miscellaneous` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `type` enum('Adobe Malware Classifier','Anomalies/Flags','Anti-VM','Anti-Dbg','Embedded File','URLs') DEFAULT NULL,
  `description` varchar(250) DEFAULT NULL,
  `id_analysis` int(11) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_miscellaneous_file_properties_idx` (`id_analysis`),
  CONSTRAINT `fk_miscellaneous_file_detail` FOREIGN KEY (`id_analysis`) REFERENCES `analysis` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `antivirus`
--

DROP TABLE IF EXISTS `antivirus`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `antivirus` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(45) DEFAULT NULL,
  `email` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `pe_file`
--

DROP TABLE IF EXISTS `pe_file`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `pe_file` (
  `id` int(11) NOT NULL,
  `architecture` varchar(45) DEFAULT NULL,
  `compilation_date` datetime DEFAULT NULL,
  `language` varchar(45) DEFAULT NULL,
  `packer` varchar(70) DEFAULT NULL,
  `entry_point` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_pe_file_file_detail_idx` (`id`),
  CONSTRAINT `fk_pe_file_file_detail` FOREIGN KEY (`id`) REFERENCES `file_detail` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `file`
--

DROP TABLE IF EXISTS `file`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `file` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(255) DEFAULT NULL,
  `source` enum('server_honeypot','client_honeypot','manual') DEFAULT NULL,
  `date` datetime DEFAULT NULL,
  `id_file_detail` int(11) DEFAULT NULL,
  `analyzed` bit(1) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_file_file_properties_idx` (`id_file_detail`),
  CONSTRAINT `fk_file_file_detail` FOREIGN KEY (`id_file_detail`) REFERENCES `file_detail` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `result_file`
--

DROP TABLE IF EXISTS `result_file`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `result_file` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `data` blob,
  `program_name` varchar(45) DEFAULT NULL,
  `id_analysis` int(11) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_result_file_file_properties_idx` (`id_analysis`),
  CONSTRAINT `fk_result_file_analysis` FOREIGN KEY (`id_analysis`) REFERENCES `analysis` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `import_library`
--

DROP TABLE IF EXISTS `import_library`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `import_library` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `file_detail`
--

DROP TABLE IF EXISTS `file_detail`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `file_detail` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `type` varchar(120) DEFAULT NULL,
  `md5` char(32) DEFAULT NULL,
  `sha1` char(40) DEFAULT NULL,
  `sha256` char(64) DEFAULT NULL,
  `sha512` char(128) DEFAULT NULL,
  `crc32` char(8) DEFAULT NULL,
  `ssdeep` varchar(150) DEFAULT NULL,
  `malicious` bit(1) DEFAULT NULL,
  `create_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `data` longblob,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `resource`
--

DROP TABLE IF EXISTS `resource`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `resource` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(100) DEFAULT NULL,
  `size` varchar(20) DEFAULT NULL,
  `language` varchar(30) DEFAULT NULL,
  `id_type` int(11) DEFAULT NULL,
  `id_file_properties` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_resource_file_properties_idx` (`id_file_properties`),
  KEY `fk_resource_type_resource_idx` (`id_type`),
  CONSTRAINT `fk_resource_type_resource` FOREIGN KEY (`id_type`) REFERENCES `resource_type` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_resource_file_properties` FOREIGN KEY (`id_file_properties`) REFERENCES `pe_file` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `import_function_pe_file`
--

DROP TABLE IF EXISTS `import_function_pe_file`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `import_function_pe_file` (
  `id_pe_file` int(11) NOT NULL,
  `id_import_function` int(11) NOT NULL,
  PRIMARY KEY (`id_pe_file`,`id_import_function`),
  KEY `fk_import_function_pe_file_idx` (`id_pe_file`),
  KEY `fk_pe_file_import_function_idx` (`id_pe_file`),
  KEY `fk_import_function_idx` (`id_import_function`),
  CONSTRAINT `fk_import_function` FOREIGN KEY (`id_import_function`) REFERENCES `import_function` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_pe_file_import_function` FOREIGN KEY (`id_pe_file`) REFERENCES `pe_file` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `analysis_dns`
--

DROP TABLE IF EXISTS `analysis_dns`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `analysis_dns` (
  `id_analysis` int(11) NOT NULL,
  `id_dns` int(11) NOT NULL,
  PRIMARY KEY (`id_analysis`,`id_dns`),
  KEY `fk_analysis_dns_analysis_idx` (`id_analysis`),
  KEY `fk_analysis_dns_dns_idx` (`id_dns`),
  CONSTRAINT `fk_analysis_dns_analysis` FOREIGN KEY (`id_analysis`) REFERENCES `analysis` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_analysis_dns_dns` FOREIGN KEY (`id_dns`) REFERENCES `dns` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `computer_event`
--

DROP TABLE IF EXISTS `computer_event`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `computer_event` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `pid` int(11) DEFAULT NULL,
  `process_name` varchar(255) DEFAULT NULL,
  `time_of_day` time DEFAULT NULL,
  `operation` varchar(45) DEFAULT NULL,
  `path` varchar(255) DEFAULT NULL,
  `result` varchar(45) DEFAULT NULL,
  `detail` varchar(1500) DEFAULT NULL,
  `id_analysis` int(11) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_event_file_properties_idx` (`id_analysis`),
  CONSTRAINT `fk_computer_event_analysis` FOREIGN KEY (`id_analysis`) REFERENCES `analysis` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `analysis`
--

DROP TABLE IF EXISTS `analysis`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `analysis` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `start_date` datetime DEFAULT NULL,
  `final_date` datetime DEFAULT NULL,
  `id_file_detail` int(11) NOT NULL,
  `file_name` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_analysis_file_details_idx` (`id_file_detail`),
  CONSTRAINT `fk_analysis_file_detail` FOREIGN KEY (`id_file_detail`) REFERENCES `file_detail` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `export_function`
--

DROP TABLE IF EXISTS `export_function`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `export_function` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(45) DEFAULT NULL,
  `pe_file_id` int(11) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_file_properties_idx` (`pe_file_id`),
  CONSTRAINT `fk_export_function_pe_file` FOREIGN KEY (`pe_file_id`) REFERENCES `pe_file` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `section`
--

DROP TABLE IF EXISTS `section`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `section` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(8) DEFAULT NULL,
  `virtual_address` varchar(20) DEFAULT NULL,
  `virtual_size` varchar(20) DEFAULT NULL,
  `raw_size` varchar(20) DEFAULT NULL,
  `md5` char(32) DEFAULT NULL,
  `suspicious` bit(1) DEFAULT NULL,
  `id_file_properties` int(11) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_section_file_properties_idx` (`id_file_properties`),
  CONSTRAINT `fk_section_file_properties` FOREIGN KEY (`id_file_properties`) REFERENCES `pe_file` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `resource_type`
--

DROP TABLE IF EXISTS `resource_type`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `resource_type` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2013-10-26 23:15:15
