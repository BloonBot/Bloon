-- phpMyAdmin SQL Dump
-- version 5.0.2
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Mar 17, 2021 at 03:47 PM
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
-- Database: `bloon_intruder`
--

-- --------------------------------------------------------

--
-- Table structure for table `agents`
--

CREATE TABLE `agents` (
  `intruder_id` int(11) NOT NULL,
  `steam_id` bigint(20) NOT NULL,
  `steam_avatar` varchar(512) DEFAULT NULL,
  `role` enum('Agent','AUG','Moderator','Developer','Demoted') NOT NULL,
  `current_name` varchar(64) CHARACTER SET utf8mb4 NOT NULL,
  `old_agent_name` varchar(32) CHARACTER SET utf8mb4 DEFAULT NULL,
  `matches_won` int(11) NOT NULL,
  `matches_lost` int(11) NOT NULL,
  `rounds_lost` int(11) NOT NULL,
  `rounds_tied` int(11) NOT NULL,
  `rounds_won_elim` int(11) NOT NULL,
  `rounds_won_capture` int(11) NOT NULL,
  `rounds_won_hack` int(11) NOT NULL,
  `rounds_won_timer` int(11) NOT NULL,
  `rounds_won_custom` int(11) NOT NULL,
  `time_played` int(11) NOT NULL,
  `kills` int(11) NOT NULL,
  `team_kills` int(11) NOT NULL,
  `deaths` int(11) NOT NULL,
  `arrests` int(11) NOT NULL,
  `got_arrested` int(11) NOT NULL,
  `captures` int(11) NOT NULL,
  `pickups` int(11) NOT NULL,
  `network_hacks` int(11) NOT NULL,
  `survivals` int(11) NOT NULL,
  `suicides` int(11) NOT NULL,
  `knockdowns` int(11) NOT NULL,
  `got_knocked_down` int(11) NOT NULL,
  `team_knock_down` int(11) NOT NULL,
  `team_damage` int(11) NOT NULL,
  `level` int(11) NOT NULL,
  `level_xp_required` int(11) NOT NULL,
  `level_xp` int(11) NOT NULL,
  `total_xp` int(11) NOT NULL,
  `positive_votes` int(11) NOT NULL,
  `negative_votes` int(11) NOT NULL,
  `received_votes` int(11) NOT NULL,
  `login_count` int(11) NOT NULL,
  `first_login` datetime DEFAULT NULL,
  `last_login` datetime DEFAULT NULL,
  `last_update` datetime NOT NULL,
  `timestamp` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `agents_history`
--

CREATE TABLE `agents_history` (
  `id` int(11) NOT NULL,
  `steam_id` bigint(20) NOT NULL,
  `matches_won` int(11) NOT NULL,
  `matches_lost` int(11) NOT NULL,
  `rounds_lost` int(11) NOT NULL,
  `rounds_tied` int(11) NOT NULL,
  `rounds_won_elims` int(11) NOT NULL,
  `rounds_won_capture` int(11) NOT NULL,
  `rounds_won_hack` int(11) NOT NULL,
  `rounds_won_timer` int(11) NOT NULL,
  `rounds_won_custom` int(11) NOT NULL,
  `time_played` int(11) NOT NULL,
  `kills` int(11) NOT NULL,
  `team_kills` int(11) NOT NULL,
  `deaths` int(11) NOT NULL,
  `arrests` int(11) NOT NULL,
  `got_arrested` int(11) NOT NULL,
  `captures` int(11) NOT NULL,
  `hacks` int(11) NOT NULL,
  `survivals` int(11) NOT NULL,
  `suicides` int(11) NOT NULL,
  `knockdowns` int(11) NOT NULL,
  `got_knockeddown` int(11) NOT NULL,
  `team_knockdowns` int(11) NOT NULL,
  `team_damage` int(11) NOT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp(),
  `login_count` int(11) NOT NULL,
  `level_xp` int(11) NOT NULL,
  `total_xp` int(11) NOT NULL,
  `last_login` datetime NOT NULL,
  `level` int(11) NOT NULL,
  `positive_votes` int(11) NOT NULL,
  `negative_votes` int(11) NOT NULL,
  `total_votes` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `level`
--

CREATE TABLE `level` (
  `level` int(11) NOT NULL,
  `xp_required` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `workshop_maps`
--

CREATE TABLE `workshop_maps` (
  `id` int(11) NOT NULL,
  `file_id` varchar(11) NOT NULL,
  `creator_steam_id` bigint(20) NOT NULL,
  `creator_appid` int(11) NOT NULL,
  `file_size` varchar(512) DEFAULT NULL,
  `preview_url` varchar(256) NOT NULL,
  `title` varchar(128) NOT NULL,
  `short_description` varchar(256) DEFAULT NULL,
  `time_created` datetime NOT NULL,
  `time_updated` datetime NOT NULL,
  `visibility` int(11) NOT NULL,
  `banned` tinyint(1) NOT NULL,
  `ban_reason` varchar(256) DEFAULT NULL,
  `can_subscribe` tinyint(4) NOT NULL,
  `subscriptions` int(11) NOT NULL,
  `favorited` int(11) NOT NULL,
  `followers` int(11) NOT NULL,
  `lifetime_subscriptions` int(11) NOT NULL,
  `lifetime_favorited` int(11) NOT NULL,
  `lifetime_followers` int(11) NOT NULL,
  `lifetime_playtime` varchar(256) NOT NULL,
  `lifetime_playtime_sessions` varchar(11) NOT NULL,
  `views` int(11) NOT NULL,
  `revision_change_number` varchar(256) NOT NULL,
  `revision` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `agents`
--
ALTER TABLE `agents`
  ADD PRIMARY KEY (`intruder_id`);

--
-- Indexes for table `agents_history`
--
ALTER TABLE `agents_history`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `level`
--
ALTER TABLE `level`
  ADD PRIMARY KEY (`level`);

--
-- Indexes for table `workshop_maps`
--
ALTER TABLE `workshop_maps`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `agents_history`
--
ALTER TABLE `agents_history`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `workshop_maps`
--
ALTER TABLE `workshop_maps`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
