# Host: localhost  (Version 5.7.23-log)
# Date: 2020-06-29 01:26:52
# Generator: MySQL-Front 6.0  (Build 2.20)


#
# Structure for table "books"
#

DROP TABLE IF EXISTS `books`;
CREATE TABLE `books` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Title` varchar(255) NOT NULL DEFAULT '',
  `Price` decimal(10,2) NOT NULL DEFAULT '0.00',
  `PublisherId` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=latin1;

#
# Data for table "books"
#

INSERT INTO `books` VALUES (1,'Book 1',9.99,1),(2,'Book 2',19.99,2),(3,'Book Test update',3.99,0),(4,'Book Test fields update',3.19,0),(5,'Book Test fields update',3.19,0),(6,'Book Test fields update',3.19,0),(7,'Book Test fields update',3.19,0),(8,'Book Test fields update',3.19,0),(9,'Book Test fields update',3.19,0),(10,'Book Test fields update',3.19,0),(11,'Book Test fields update',3.19,0),(12,'Book Test update',3.99,0),(13,'Book Test update',3.99,0),(14,'Book Test fields update',3.19,0),(15,'Book Test fields',1.19,0),(16,'Book Test fields update',1.19,0);

#
# Structure for table "customer"
#

DROP TABLE IF EXISTS `customer`;
CREATE TABLE `customer` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(20) NOT NULL DEFAULT '',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

#
# Data for table "customer"
#


#
# Structure for table "order"
#

DROP TABLE IF EXISTS `order`;
CREATE TABLE `order` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CustomerId` int(11) NOT NULL DEFAULT '0',
  `TotalPrice` decimal(10,2) NOT NULL DEFAULT '0.00',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

#
# Data for table "order"
#


#
# Structure for table "publishers"
#

DROP TABLE IF EXISTS `publishers`;
CREATE TABLE `publishers` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) NOT NULL DEFAULT '',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1;

#
# Data for table "publishers"
#

INSERT INTO `publishers` VALUES (1,'Publisher 1'),(2,'Publisher 2');
