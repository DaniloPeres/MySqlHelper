# Host: daniloperes.com  (Version 5.7.29-log)
# Date: 2020-06-15 18:38:05
# Generator: MySQL-Front 6.1  (Build 1.26)


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
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

#
# Data for table "books"
#


#
# Structure for table "publishers"
#

DROP TABLE IF EXISTS `publishers`;
CREATE TABLE `publishers` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) NOT NULL DEFAULT '',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

#
# Data for table "publishers"
#

