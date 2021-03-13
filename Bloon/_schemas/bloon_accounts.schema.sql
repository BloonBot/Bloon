-- phpMyAdmin SQL Dump
-- version 5.0.2
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Mar 13, 2021 at 02:07 AM
-- Server version: 10.4.11-MariaDB
-- PHP Version: 7.4.4

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `bloon_accounts`
--

-- --------------------------------------------------------

--
-- Table structure for table `package_account`
--

CREATE TABLE `package_account` (
  `steam_id` bigint(20) UNSIGNED NOT NULL,
  `discord_id` bigint(20) UNSIGNED NOT NULL,
  `pin` int(6) NOT NULL,
  `permissions` enum('Basic','PR','Mod','Admin') NOT NULL DEFAULT 'Basic',
  `private_profile` tinyint(1) NOT NULL,
  `package_created` datetime NOT NULL,
  `last_login` datetime NOT NULL,
  `steam_profile_url` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `room_history`
--

CREATE TABLE `room_history` (
  `id` int(11) NOT NULL,
  `room_id` int(11) NOT NULL,
  `name` varchar(255) NOT NULL,
  `passworded` tinyint(1) NOT NULL,
  `region` varchar(6) NOT NULL,
  `official` tinyint(1) NOT NULL,
  `ranked` tinyint(1) NOT NULL,
  `version` int(11) NOT NULL,
  `position` int(11) NOT NULL,
  `agent_count` int(11) NOT NULL,
  `creator_intruder_id` int(11) NOT NULL,
  `creator_steam_id` bigint(20) NOT NULL,
  `last_update` datetime NOT NULL,
  `current_map` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `package_account`
--
ALTER TABLE `package_account`
  ADD PRIMARY KEY (`steam_id`);

--
-- Indexes for table `room_history`
--
ALTER TABLE `room_history`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `room_history`
--
ALTER TABLE `room_history`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
