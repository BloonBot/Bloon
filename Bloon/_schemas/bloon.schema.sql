-- phpMyAdmin SQL Dump
-- version 5.0.2
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Mar 03, 2021 at 05:13 PM
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
-- Database: `bloon`
--

-- --------------------------------------------------------

--
-- Table structure for table `faq`
--

CREATE TABLE `faq` (
  `id` int(11) NOT NULL,
  `regex` varchar(256) NOT NULL,
  `message` varchar(512) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `faq`
--

INSERT INTO `faq` (`id`, `regex`, `message`) VALUES
(1, '(what|how).*(switch|change|move|swap).*(maps?)', 'To change the map ingame make yourself an admin & then use `\\/changemap mapName` (You must be the admin to do this).'),
(2, '(how|what).*(can)?.*(make|give|become|be) (admini?s?t?r?a?t?o?r?)', 'To make yourself an admin use `\\/makeadmin` this only works if you are the original room creator.'),
(3, '(is).*(map|list).*(map|list)', 'The Official maps are Riverside, Mountainside, Cliffside, and Oceanside; there are many unofficial maps out there too, check them out at: <https:\\/\\/steamcommunity.com\\/app\\/518150\\/workshop\\/>'),
(4, '(how).*(report).*(bugs?)', 'You can report bugs at: <https://superbossgames.helprace.com/>'),
(5, '(how|when|what) (long|length|doe?s?).*(ban?n?e?d?s?)', 'Temporary bans last about 15 minutes. A timer on the main menu will tell you how much time is left on your temporary ban.  '),
(6, '(cant|can\'t|cannot).*(download).*(maps?)', 'If the map is not downloading correctly you may want to restart your game, if problems persist clear your map cache.'),
(7, '(what|how).*(tuning)', 'Server Tuning is settings that allow you to change minor or major game mechanics, like speed health & respawns. You can create tuning for your server at: <https://tuning.bloon.info>'),
(9, '(game)?.*(why|is).*(is)?.*(game).*(dead)', 'If the servers are empty start your own and ping Looking To Play and have some fun.'),
(10, '(cant?n?o?t?|can|how|is).*(name|change).*(name|change)', 'Intruder uses your Steam profile username assuming that the characters within the name are supported.'),
(11, '(who).*(creato?r?e?d?|mad?k?e).*(bloon)', 'Bloon currently has two main contributors which are DukeofSussex and Ruby.'),
(12, '(what|how).*(is|cant?n?o?t?|do).*(aug)', 'The Advanced User Group is a group formed to help serious players run matches, play tournaments, and engage in private community events and activities.'),
(13, '(who).*(creato?r?e?d?|mad?k?e).*(intruder)', 'Intruder is being developed by Rob Storm and Austin Roush.'),
(14, '(how|can) (to|do|you).*(kick)', 'To kick a player, use the button on their player profile by clicking the (i) icon next to their name in the Teams menu.'),
(15, '(is|how|can).*(show|change).*(fps)', 'You can show your current FPS on your hud simply by doing `\\/fps`. You can change your FPS and Vsync settings from the options'),
(16, '(((what).*(is).*(default|normal|original).*(gravity))|((how|can|do).*(change|modify).*(gravity)))', 'Gravity by default is at `-9.81`. To change the gravity you do `/gravity #`. ***REMINDER YOU MUST BE AN ADMIN TO DO THIS***'),
(17, '(((how|can).*(change|retrieve).*(passw?o?r?d?))|((i).*(forgot).*(password)))', 'You may reset your old user password at: <https:\\/\\/intruderfps.com\\/reset-password>'),
(18, '(do|can|how|is|where).*(pay).*(with) (paypal)', 'You can use Paypal on Steam!'),
(19, '(what).*(are).*(controls)', 'The controls are: https://i.imgur.com/Bkc97Fp.jpg'),
(20, '(how|can).*(to|do|you|move).*(time|sun) (in) (game|intruder)', 'You can change the time of day in game by doing /suntime 15 (24 hour time scale) ***WARNING YOU MUST BE MASTER CLIENT AND ADMIN***'),
(21, '(how|what).*(can)?.*(make|give|become|be).*(master)', 'You can set the master client via the player profile cards. ***SIDE NOTE:** this only works if you are the room admin*'),
(22, '((how|can).*(change|remove|disable|(turn.*off?)).*(hud))', 'You can turn off your HUD in game by hitting `q+p` at the same time (this odd key combo is so that you don\'t accidentaly turn off your hud in game)'),
(23, '((how|can|what).*(((is|are)|(join|enter)|(leave|remove))) (ltp|looki?n?g? to play))', 'Looking to play (or known as LTP) is a role you can give yourself by running the command `.ltp`. You will be given the role for 7 days unless you continue to play Intruder. If you\'d like to leave the role, simply run the command again. You name will be marked purple once you\'re in.'),
(24, '((how|can|what).*(((is|are)|(activate|(start|begin)))).*(lms|last man standing))', 'Last Man Standing, aka LMS, is a battle royale style gamemode on Riverside primarily and some custom maps. You can try it on the Riverside LMS map.'),
(25, 'thank(?:s| you).*bloon', 'You\'re welcome, hooman'),
(26, '(where|how).*(can|do).*(make|create).(custom|).*(maps?)', 'Get Started Making Custom Maps: <https://sharklootgilt.superbossgames.com/wiki/index.php?title=IntruderMM/>\r\nMap Maker Resources: <https://docs.google.com/document/d/10Qvao_pA-lM8IFASWaAr6AGlNYG28CJaLqyOd5aUrss/edit?usp=sharing/>\r\nSuperGuide: <https://docs.google.com/document/d/1FXw6tlccdtrtJl_RjpyreXqSjQ9k-f1X9ZDxdOCHtAw/edit/>'),
(27, '(where|how).*(can|do).*(access|show|get|pull).*(console)', 'You can access the console within the game at any time by using `\\ + Tab` at the same time. It\'ll open a black box with colored debugging.'),
(28, '(my|the|team).*(microphone|mic|team).*(quiet|silent|trouble hearing)', '1) USB Microphone? \r\nhttps://discordapp.com/channels/103933666417217536/306213064934424576/551549500251045907\r\n2) Check microphone volume within Windows (Sometimes this changes)\r\n3) Check microphone values within game settings'),
(30, '(what|how).*(is|join|make|get).*(comp|comp).(player?s?)', 'The comp player role is a role reserved for players who play in PUGs (Pop-up games) and Comp matches.');

-- --------------------------------------------------------

--
-- Table structure for table `feature_status`
--

CREATE TABLE `feature_status` (
  `id` int(10) NOT NULL,
  `name` varchar(32) NOT NULL,
  `enabled` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `feature_status`
--

INSERT INTO `feature_status` (`id`, `name`, `enabled`) VALUES
(1, 'Profanity Filter Embeds', 1),
(2, 'Fetch Workshop Items', 1),
(3, 'Fetch Wiki articles', 1),
(4, 'Welcome Mat', 1),
(5, 'Twitter', 1),
(6, 'Twitch', 1),
(7, 'Totally Not Spam', 1),
(8, 'SBA Inactivity Pruning', 0),
(9, 'Reddit', 1),
(10, 'Random Responses', 1),
(11, 'Fetch Youtube Videos', 1),
(12, 'PackageAccounts', 1),
(13, 'LTP Pruning', 1),
(14, 'LTP Commands', 1),
(15, 'Remove Discord Invites', 1),
(16, 'Servers', 1),
(17, 'Get Agent Stats', 1),
(18, 'Helprace Jobs', 1),
(19, 'Goodbye Messages', 1),
(20, 'FAQ', 1),
(21, 'Channel Pin Limit Warning', 1),
(22, 'Add users to @Now Playing', 1),
(23, 'Z26', 1),
(24, 'Stats Command', 1),
(25, 'GSI Commands', 1),
(26, 'Mod Tools', 1),
(27, 'Role Persistence', 1),
(28, 'Fetch Steam News', 1),
(29, '#sbg_userinfo Logging', 1);

-- --------------------------------------------------------

--
-- Table structure for table `job`
--

CREATE TABLE `job` (
  `id` int(11) NOT NULL,
  `name` varchar(64) NOT NULL,
  `last_execution` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `job`
--

INSERT INTO `job` (`id`, `name`, `last_execution`) VALUES
(1, 'Bloon.Features.Helprace.HelpraceJob', '2021-03-03 16:09:52'),
(2, 'Bloon.Features.IntruderBackend.Servers.ServerJob', '2021-03-03 16:09:52'),
(3, 'Bloon.Features.LTP.LTPPruneJob', '2021-03-02 19:41:36'),
(4, 'Bloon.Features.SBAInactivity.SBAInactivityJob', '2021-03-02 19:41:36'),
(5, 'Bloon.Features.IntruderBackend.Agents.ScrapeAgents', '2021-03-03 16:09:52'),
(6, 'Bloon.Features.Twitter.TwitterJob', '2021-03-03 16:09:52'),
(7, 'Bloon.Features.Wiki.WikiJob', '2021-03-03 16:09:52'),
(8, 'Bloon.Features.Workshop.WorkshopJob', '2021-03-03 16:09:52'),
(9, 'Bloon.Features.Youtube.YouTubeJob', '2021-03-03 15:59:52'),
(10, 'Bloon.Features.NowPlaying.NowPlayingJob', '2021-03-03 16:09:52'),
(11, 'Bloon.Features.SteamNews.SteamNewsJob', '2021-03-03 15:59:52');

-- --------------------------------------------------------

--
-- Table structure for table `ltp`
--

CREATE TABLE `ltp` (
  `user_id` bigint(20) UNSIGNED NOT NULL,
  `timestamp` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `message`
--

CREATE TABLE `message` (
  `id` int(11) NOT NULL,
  `message` varchar(1028) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `message`
--

INSERT INTO `message` (`id`, `message`) VALUES
(1, 'Use `.help` to get started with any command.'),
(3, 'Interested in how things happen behind the scenes for Bloon? Ask around for the discord invite.'),
(4, 'You can run any command if you DM me. Check it out!'),
(5, 'Looking for links? Check out <#151815021805043712>'),
(6, 'Our wiki is home to a lot of useful information. Run `.help -wiki` to get started today.'),
(7, 'The developer of this bot really likes pizza. You can buy him one if you also really enjoy pizza. `.donate`'),
(8, 'Check out <#106405837810995200> for the latest suggestions and bugs discovered by fellow players.'),
(9, 'Want to help squash a bug you found? Use .bugs'),
(10, 'Old Intruder profiles are no longer being updated and will **not** be linked to corresponding Steam profiles.'),
(11, 'Follow Superboss Games on Twitter! https://twitter.com/SuperbossGames/'),
(12, 'Have a video of Intruder? Post it in <#268062505761243137>'),
(13, 'Join our `Looking to Play` role using the command `.ltp`'),
(14, 'Want to leave the `Looking to Play` role? Type `.ltp`'),
(15, 'Feel free to ping Mods if you need help with anything.'),
(16, 'You can disable the HUD in game by pressing Q+P'),
(17, 'Want to take a nicer screenshot? U+H+B while in game will save the screenshot inside the game\'s folder.'),
(18, 'Try to ***not*** kill your team members. They\'re important to your team\'s success. Repeatedly doing so might result in a temporary ban.'),
(19, 'Want to make your very own map? Head over to <#141327816300953600> to get started.'),
(20, 'Having trouble opening doors? Try using the scroll wheel. You cannot rebind this.'),
(21, 'You can rebind any **keys** within the game.'),
(22, 'Tired of being kicked from another server? Make your own!'),
(23, 'Want to change mouse sensitivity? Use /ms {number}'),
(24, 'Want to make yourself an admin of your own room? /makeadmin {playerName}'),
(25, 'Want to change the map? Use `/changemap {userName@mapName}`'),
(26, 'Need to know the console commands? .wiki commands'),
(27, 'The default key to pull out the radio is F. Once in your hand, you need to hold MOUSE1 to transmit. All players on your team hear the transmission, and any nearby enemies will also hear you talking into your radio.'),
(28, 'There are two forms of voice chat in the game. The first is 3D voice chat (C by default), which can be heard by ALL nearby players.'),
(29, 'Players deemed to have a negative impact on the good of the community are demoted and can not access all servers.'),
(30, 'You can activate some hats ingame with certain commands. These commands need to be used like /cmd 1|0. Check out /santa, /goof, /alert, /huh, /box, /jack, /rudolph. Some hats are for AUG players only.'),
(31, 'Activating goof mode by typing /goof 1 in chat prevents people being punished for TKing you, and puts a goofy cone on your head. You can disable it with /goof 0.'),
(32, 'Advanced User Group is a select group of elite agents that have proven themselves to be quality members of the community.'),
(33, 'The most likely cause of game crashes are missing or corrupt game files. In some cases antivirus software may remove files by mistake, and without telling you. If you use Norton antivirus, this is VERY likely.'),
(34, 'There are binoculars placed in the map. You don\'t spawn with them. They\'re useful for differentiating between friend and foe at a distance. You can also take a screenshot with the binoculars by clicking/firing them.'),
(35, 'You can kill yourself in-game by typing /kill in the chat.'),
(36, 'Raid is a bit like capture the flag but only the guards have flags. The guards must defend two briefcases that are on their side of the map while the intruders breach from their own side of the map, grab one of the two packages, and exfiltrate back to their side of the map.'),
(37, 'Doors are randomly locked. They can be unlocked with the lockpick tool.');

-- --------------------------------------------------------

--
-- Table structure for table `role_member`
--

CREATE TABLE `role_member` (
  `id` int(10) UNSIGNED NOT NULL,
  `member_id` bigint(20) UNSIGNED NOT NULL,
  `role_id` bigint(20) UNSIGNED NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `role_member`
--

INSERT INTO `role_member` (`id`, `member_id`, `role_id`) VALUES
(211, 72153220201320448, 306830032247193613),
(189, 72153220201320448, 372465564892790784),
(190, 72153220201320448, 414942025109078027),
(139, 73629047820660736, 306830032247193613),
(125, 73629047820660736, 556941912871796736),
(115, 73629047820660736, 613827678621008035),
(26, 80020463958360064, 414942025109078027),
(1, 80825380654555136, 404057360622026762),
(174, 82986814872223744, 122843947700715520),
(169, 89980708134621184, 611298881564704799),
(46, 94543029146222592, 306830032247193613),
(47, 94543029146222592, 414942025109078027),
(48, 94543029146222592, 556941912871796736),
(49, 94543029146222592, 611397396680933386),
(364, 94599296644546560, 556941912871796736),
(161, 95958328861036544, 306830032247193613),
(162, 95958328861036544, 598636664432099331),
(230, 96006568889298944, 608727296387317805),
(31, 96016419593912320, 608727296387317805),
(114, 99893452526870528, 306830032247193613),
(91, 99893452526870528, 373159300610392074),
(158, 102234546707107840, 122843947700715520),
(300, 103932826268766208, 306830032247193613),
(301, 103932826268766208, 598636664432099331),
(21, 103961319677362176, 414942025109078027),
(327, 103967428408512512, 414942025109078027),
(326, 103967428408512512, 592739440187932682),
(325, 103967428408512512, 677689088068943922),
(154, 104250700522852352, 306830032247193613),
(155, 104250700522852352, 611398303791316994),
(204, 104308798285414400, 598636664432099331),
(249, 104389223280295936, 306830032247193613),
(247, 104389223280295936, 414942025109078027),
(246, 104389223280295936, 598636664432099331),
(260, 104389223280295936, 631772172679118868),
(121, 104579537953636352, 306830032247193613),
(122, 104579537953636352, 373159300610392074),
(361, 105501572271714304, 306830032247193613),
(360, 105501572271714304, 373159300610392074),
(362, 105501572271714304, 608727296387317805),
(363, 105501572271714304, 677689088068943922),
(231, 105819228635926528, 306830032247193613),
(232, 105819228635926528, 598636664432099331),
(367, 105838876865642496, 306830032247193613),
(368, 105838876865642496, 611298881564704799),
(218, 106484338433986560, 306830032247193613),
(217, 106484338433986560, 598636664432099331),
(219, 106484338433986560, 611298881564704799),
(320, 107670767017168896, 814366657014661120),
(116, 108026484777566208, 611298881564704799),
(254, 108355403691085824, 306830032247193613),
(255, 108355403691085824, 598636664432099331),
(50, 108955905927696384, 306830032247193613),
(52, 108955905927696384, 611298881564704799),
(220, 109093428809175040, 306830032247193613),
(221, 109093428809175040, 611298881564704799),
(76, 109587405673091072, 414942025109078027),
(16, 112663244006551552, 306830032247193613),
(73, 118582746254802947, 556941912871796736),
(310, 118741991239450624, 122843947700715520),
(351, 121077940275511296, 556941912871796736),
(168, 121103354699972608, 372465564892790784),
(117, 122853133457162240, 122843947700715520),
(307, 125038019915808768, 306830032247193613),
(308, 125038019915808768, 611398303791316994),
(357, 125068591329443840, 608727296387317805),
(322, 135207491070853120, 414942025109078027),
(102, 137218456075370496, 556941912871796736),
(70, 145231248334389248, 306830032247193613),
(71, 145231248334389248, 556941912871796736),
(72, 145231248334389248, 598636664432099331),
(201, 147511573869625344, 306830032247193613),
(202, 147511573869625344, 414942025109078027),
(203, 147511573869625344, 556941912871796736),
(105, 154882746580205568, 608727296387317805),
(283, 157948612360273921, 556941912871796736),
(284, 157948612360273921, 598636664432099331),
(196, 166746704639426560, 414942025109078027),
(197, 166746704639426560, 556941912871796736),
(165, 168841717837987840, 414942025109078027),
(233, 170850774992027649, 414942025109078027),
(205, 171108024293785600, 556941912871796736),
(213, 174185317669011456, 414942025109078027),
(214, 174185317669011456, 556941912871796736),
(212, 176205012140163072, 306830032247193613),
(234, 176205012140163072, 556941912871796736),
(365, 176205012140163072, 813318148563533835),
(124, 176492242662457345, 306830032247193613),
(126, 178717224528379905, 306830032247193613),
(137, 178717224528379905, 372465564892790784),
(127, 178717224528379905, 556941912871796736),
(128, 178717224528379905, 592739440187932682),
(63, 179065596745482240, 306830032247193613),
(227, 183530050870575104, 556941912871796736),
(129, 184114825138405376, 306830032247193613),
(130, 184114825138405376, 414942025109078027),
(131, 184114825138405376, 611298881564704799),
(293, 184326235600584704, 404057360622026762),
(317, 186406787895918592, 306830032247193613),
(313, 186406787895918592, 556941912871796736),
(315, 186406787895918592, 611397396680933386),
(314, 186406787895918592, 631771900409937920),
(316, 186406787895918592, 813318148563533835),
(339, 189873312174702592, 306830032247193613),
(337, 189873312174702592, 592739440187932682),
(156, 192131967851036672, 306830032247193613),
(157, 192131967851036672, 372465564892790784),
(67, 193104213591457793, 306830032247193613),
(78, 193479795101663232, 414942025109078027),
(352, 196822241067925504, 655641124383358988),
(319, 197088936168914944, 814402575524823052),
(89, 197962266950369281, 556941912871796736),
(241, 200734190210121728, 306830032247193613),
(242, 200734190210121728, 598636664432099331),
(345, 201112679358791680, 306830032247193613),
(346, 201112679358791680, 556941912871796736),
(350, 202171780218093569, 814402575524823052),
(192, 202620317577904128, 306830032247193613),
(193, 202620317577904128, 414942025109078027),
(194, 202620317577904128, 611298881564704799),
(195, 202620317577904128, 611397396680933386),
(34, 209445347070705664, 306830032247193613),
(35, 209445347070705664, 556941912871796736),
(15, 209445347070705664, 592739440187932682),
(25, 209445347070705664, 611298881564704799),
(109, 210114735247458306, 608727296387317805),
(309, 213611626689986560, 556941912871796736),
(54, 217804149130002434, 414942025109078027),
(55, 217804149130002434, 592739440187932682),
(140, 218806220595658752, 414942025109078027),
(340, 218806220595658752, 556941912871796736),
(151, 218806220595658752, 592739440187932682),
(138, 221684953040027649, 414942025109078027),
(136, 221684953040027649, 556941912871796736),
(170, 224584445921984512, 306830032247193613),
(244, 225151345143840768, 306830032247193613),
(321, 226877107978895360, 814366657014661120),
(318, 227458365306044416, 608727296387317805),
(342, 227510505139470336, 814402575524823052),
(96, 234834986081976320, 372462870132424706),
(97, 234834986081976320, 414942025109078027),
(106, 235550975136366593, 306830032247193613),
(107, 235550975136366593, 372462870132424706),
(28, 235886675505446913, 306830032247193613),
(245, 236663299708682241, 306830032247193613),
(199, 240154716862939137, 372462870132424706),
(264, 243519101174153216, 306830032247193613),
(265, 243519101174153216, 608727296387317805),
(266, 243519101174153216, 611397396680933386),
(123, 246690234530529280, 608727296387317805),
(276, 249503867895676930, 306830032247193613),
(30, 250745440176308234, 306830032247193613),
(235, 251410938899464192, 414942025109078027),
(222, 251410938899464192, 556941912871796736),
(223, 251410938899464192, 611398303791316994),
(185, 252370592869777408, 306830032247193613),
(273, 255178190656503809, 611298881564704799),
(17, 256564283826110464, 608727296387317805),
(181, 259743585702903808, 122843947700715520),
(88, 261221156739088384, 372465564892790784),
(353, 261798046378098691, 556941912871796736),
(180, 261798046378098691, 598636664432099331),
(354, 261798046378098691, 783160707763404821),
(68, 261917621140717569, 122843947700715520),
(39, 263769186935898112, 306830032247193613),
(51, 263769186935898112, 556941912871796736),
(83, 264457792235110410, 414942025109078027),
(358, 268832160390053888, 815325570375614524),
(311, 270781583852699658, 556941912871796736),
(312, 270781583852699658, 598636664432099331),
(228, 271996025848332299, 306830032247193613),
(229, 271996025848332299, 556941912871796736),
(69, 275360926495145984, 556941912871796736),
(173, 279461468045574144, 306830032247193613),
(332, 280182720271876106, 814366657014661120),
(253, 281539831564337173, 306830032247193613),
(252, 281539831564337173, 414942025109078027),
(251, 281539831564337173, 677689088068943922),
(333, 285508071944355841, 306830032247193613),
(334, 285508071944355841, 414942025109078027),
(336, 285508071944355841, 556941912871796736),
(335, 285508071944355841, 598636664432099331),
(41, 291957799339032576, 306830032247193613),
(208, 295017001515089920, 637769924231823381),
(207, 295676316366274560, 306830032247193613),
(329, 296862537218129931, 306830032247193613),
(331, 296862537218129931, 556941912871796736),
(330, 296862537218129931, 592739440187932682),
(338, 296862537218129931, 611397396680933386),
(328, 296862537218129931, 677689088068943922),
(135, 301460398744535046, 122843947700715520),
(113, 301481497167396874, 122843947700715520),
(20, 301486364170387458, 122843947700715520),
(90, 302401965134970890, 306830032247193613),
(101, 302401965134970890, 611398303791316994),
(182, 304029600726646804, 306830032247193613),
(92, 305942472549203971, 306830032247193613),
(93, 305942472549203971, 556941912871796736),
(94, 305942472549203971, 598636664432099331),
(95, 305942472549203971, 654221261018365972),
(206, 313155601196908544, 306830032247193613),
(209, 316560294535102467, 306830032247193613),
(188, 316560294535102467, 414942025109078027),
(210, 316560294535102467, 556941912871796736),
(187, 316560294535102467, 611397396680933386),
(176, 316560294535102467, 677689088068943922),
(278, 320595836600057867, 306830032247193613),
(36, 320595836600057867, 783160707763404821),
(216, 322420861518872579, 306830032247193613),
(215, 322420861518872579, 654221261018365972),
(29, 323909873815388161, 306830032247193613),
(143, 324384159021924354, 306830032247193613),
(277, 324384159021924354, 611397396680933386),
(144, 324384159021924354, 611398303791316994),
(324, 324394068174176256, 556941912871796736),
(323, 324394068174176256, 814366657014661120),
(74, 324988247291985930, 306830032247193613),
(356, 326035495161430016, 710695816414888016),
(243, 326560722735595532, 306830032247193613),
(359, 326560722735595532, 556941912871796736),
(172, 327953499650392067, 306830032247193613),
(191, 331038559228133378, 306830032247193613),
(53, 331284220452405248, 306830032247193613),
(186, 331501089629143061, 306830032247193613),
(178, 336933670881329154, 306830032247193613),
(250, 339476002348466196, 306830032247193613),
(110, 345606060184436736, 306830032247193613),
(111, 345606060184436736, 372465564892790784),
(261, 356080529533304832, 306830032247193613),
(238, 356080529533304832, 414942025109078027),
(239, 356080529533304832, 556941912871796736),
(240, 356080529533304832, 611398303791316994),
(152, 357259892345798657, 556941912871796736),
(163, 357259892345798657, 783160707763404821),
(269, 358596509459152898, 306830032247193613),
(268, 358596509459152898, 414942025109078027),
(267, 358596509459152898, 556941912871796736),
(270, 358596509459152898, 608727296387317805),
(272, 358596509459152898, 677689088068943922),
(271, 358596509459152898, 710695816414888016),
(58, 362571215287615490, 306830032247193613),
(56, 362571215287615490, 556941912871796736),
(57, 362571215287615490, 608727296387317805),
(60, 362571215287615490, 677689088068943922),
(59, 362571215287615490, 710695816414888016),
(134, 369968621461438465, 763493830947110963),
(149, 381698636305924096, 608727296387317805),
(146, 383126024868855818, 306830032247193613),
(120, 395260233246572544, 306830032247193613),
(179, 398654568915009536, 598636664432099331),
(167, 399914829860700170, 306830032247193613),
(166, 399914829860700170, 710695816414888016),
(37, 408563281737809932, 122843947700715520),
(32, 409566805556985877, 306830032247193613),
(33, 409566805556985877, 611397396680933386),
(23, 415367243673632770, 556941912871796736),
(24, 415367243673632770, 608727296387317805),
(171, 422615054442168330, 122843947700715520),
(42, 426298935867473927, 306830032247193613),
(43, 426298935867473927, 556941912871796736),
(44, 426298935867473927, 598636664432099331),
(280, 429995254306045955, 710695816414888016),
(349, 455365045191311362, 814366657014661120),
(100, 466716826647265290, 306830032247193613),
(98, 466716826647265290, 556941912871796736),
(99, 466716826647265290, 598636664432099331),
(225, 469944405177008128, 556941912871796736),
(226, 469944405177008128, 598636664432099331),
(262, 482463274323935232, 306830032247193613),
(263, 482463274323935232, 710695816414888016),
(282, 491219558326075394, 414942025109078027),
(108, 491667407467839508, 122843947700715520),
(302, 500451992074977290, 815325570375614524),
(184, 507620605068050432, 306830032247193613),
(355, 507620605068050432, 556941912871796736),
(183, 507620605068050432, 598636664432099331),
(177, 519643099379662864, 763493830947110963),
(45, 538969555729383434, 306830032247193613),
(119, 543789956112449536, 306830032247193613),
(118, 543789956112449536, 710695816414888016),
(343, 558357559514234881, 556941912871796736),
(344, 558357559514234881, 815325570375614524),
(150, 567851624782102579, 556941912871796736),
(64, 569445975719018496, 306830032247193613),
(62, 581459222080913410, 306830032247193613),
(304, 581459222080913410, 556941912871796736),
(61, 581459222080913410, 710695816414888016),
(145, 598205721028591628, 608727296387317805),
(347, 601031944574205952, 556941912871796736),
(348, 601031944574205952, 813318148563533835),
(38, 602537120837533697, 306830032247193613),
(27, 602537120837533697, 556941912871796736),
(305, 602537120837533697, 814366657014661120),
(306, 618774154866458650, 783160707763404821),
(303, 630480390255149078, 815325570375614524),
(103, 646440346645037076, 556941912871796736),
(104, 646440346645037076, 783160707763404821),
(147, 684224643410034693, 556941912871796736),
(148, 684224643410034693, 783160707763404821),
(279, 689256449423179868, 122843947700715520),
(200, 760528292565614613, 760530098754551808),
(198, 761439375594094612, 654221261018365972),
(281, 764028368387178497, 306830032247193613),
(159, 764028368387178497, 556941912871796736),
(160, 764028368387178497, 783160707763404821),
(341, 796922493666721793, 122843947700715520),
(366, 805997489278156881, 592739440187932682);

-- --------------------------------------------------------

--
-- Table structure for table `sba_inactivity_tracking`
--

CREATE TABLE `sba_inactivity_tracking` (
  `user_id` bigint(20) UNSIGNED NOT NULL,
  `last_message` datetime NOT NULL,
  `warning_timestamp` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `sba_inactivity_tracking`
--

INSERT INTO `sba_inactivity_tracking` (`user_id`, `last_message`, `warning_timestamp`) VALUES
(103932826268766208, '2021-03-02 18:10:22', NULL),
(103967428408512512, '2021-03-02 02:51:03', NULL),
(104389223280295936, '2021-03-02 21:33:54', NULL),
(135207491070853120, '2021-03-03 01:07:04', NULL),
(201112679358791680, '2021-03-02 17:55:54', NULL),
(234834986081976320, '2021-03-02 18:11:08', NULL),
(244407876683169792, '2021-02-28 19:15:03', NULL),
(285508071944355841, '2021-03-03 01:09:25', NULL),
(296862537218129931, '2020-10-30 17:52:05', '2021-03-02 14:41:41'),
(646440346645037076, '2020-12-16 22:01:12', '2021-03-02 14:41:40'),
(760528292565614613, '2021-03-03 01:12:16', NULL);

-- --------------------------------------------------------

--
-- Table structure for table `social_item`
--

CREATE TABLE `social_item` (
  `id` int(11) NOT NULL,
  `uid` varchar(20) NOT NULL,
  `title` varchar(512) NOT NULL,
  `author` varchar(64) NOT NULL,
  `type` enum('Helprace','Reddit','SteamNews','SteamForum','Twitter','Wiki','YouTube','Workshop') NOT NULL,
  `additional` text NOT NULL,
  `timestamp` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `social_item`
--

INSERT INTO `social_item` (`id`, `uid`, `title`, `author`, `type`, `additional`, `timestamp`) VALUES
(1, '374', 'My friend is having a problem, when the game ends and the map changes, he can no longer enter the server he was on.', 'Farinha', 'Helprace', '{\"channel\":\"problems\"}', '2020-10-16 14:50:21'),
(2, 'lqUWOk54c28', 'Intruder Update 1648: Stats, XP, Game Modes, MORE!', 'Superboss Games', 'YouTube', '{}', '2020-10-02 23:59:39'),
(3, '1321965040757547009', 'The Shrike pistol can shoot through doors at close range and Agent @bopman15 takes advantage of its capabilities. Unfortunately, you still have to hit the enemy with your shots... 🚪💥\n\n#indiegames #gamedev #stealth #fps #madewithunity #Physics https://t.co/MNpp56qVea', 'Superboss Games', 'Twitter', '{}', '2020-10-30 00:00:01'),
(4, '2266134379', 'Spooky Hillside', '76561197961840473', 'Workshop', '{}', '2020-10-24 04:26:23'),
(5, '2541', 'Main Page', 'Austin', 'Wiki', '{\"pageId\":1,\"type\":\"edit\",\"byteDifference\":6}', '2020-10-28 03:38:16'),
(6, '1338666286822215681', 'Today, for #MapmakerMonday we travel to Russia to investigate a mysterious impact site. \"Sputnik\" by Agent Roasted Roach puts guards and intruders against each other to uncover or conceal the truth! 🛰️\n\nhttps://t.co/TQUtdrc1Kp\n\n#indiegames #gamedev #stealth #madewithunity', 'Superboss Games', 'Twitter', '{}', '2020-12-15 02:04:48'),
(7, '382', 'Body shields ', 'DCGamer123', 'Helprace', '{\"channel\":\"ideas\"}', '2020-11-27 23:20:27'),
(8, '3807188575854199934', 'Intruder Update 1648 - Stats, XP, Game Modes, MORE!', 'Ryguytheguy', 'SteamNews', '{}', '2020-10-03 00:06:26'),
(9, '2308907412', 'Sputnik', '76561198193540009', 'Workshop', '{}', '2020-12-04 22:11:53'),
(10, '2543', 'Pistol', 'Swox', 'Wiki', '{\"pageId\":60,\"type\":\"edit\",\"byteDifference\":27}', '2020-11-10 15:16:16'),
(11, '1341186693764812800', 'This #MapmakerMonday Agent Mattbatt56 has decorated Phil\'s Saloon, making it Phil\'s Christmas! Swap out the sand for snow and have a holly jolly time! 🎄\n\nhttps://t.co/lF3Iljp2x0\n\n#indiegames #gamedev #stealth #madewithunity', 'Superboss Games', 'Twitter', '{}', '2020-12-22 01:00:00'),
(12, '383', 'Sniper Recoil Problem', 'Manav 9.1', 'Helprace', '{\"channel\":\"problems\"}', '2020-12-20 10:07:17'),
(13, '2325367660', 'Arctic Radar', '76561198066004163', 'Workshop', '{}', '2020-12-18 20:02:06'),
(14, '1354232659963207680', 'After taking down a guard, Agent @bopman15 takes a leap of faith onto a moving train, and it looks like this is his stop! 🛑\n\n#indiegames #gamedev #stealth #madewithunity #bonk https://t.co/Pzd7UV5VCN', 'Superboss Games', 'Twitter', '{}', '2021-01-27 01:00:01'),
(15, '2548', 'Mapmaker resources', 'Bopman15', 'Wiki', '{\"pageId\":315,\"type\":\"edit\",\"byteDifference\":-46}', '2021-01-26 15:40:09'),
(16, '2376293834', 'RealtimeGI Test 2', '76561198169265681', 'Workshop', '{}', '2021-01-28 09:06:00'),
(17, '384', 'An option to click to vote', '2rad4rio', 'Helprace', '{\"channel\":\"ideas\"}', '2021-01-31 05:16:58'),
(18, '2380533889', 'Professional', '76561198044683166', 'Workshop', '{}', '2021-01-31 22:33:43'),
(19, '2380987281', 'Swag', '76561198169265681', 'Workshop', '{}', '2021-02-01 11:23:13'),
(20, '1363569472091680772', 'Our stealth multiplayer FPS: INTRUDER is on sale for the first time! %50 off!\n\nWe grinded for months on one of our biggest updates yet in prep for the sale. New update includes: Unlocks, Items, new physics, &amp; more!\n#gamedev #indiedev #unity3d #Steam \n\nhttps://t.co/E1lndhe0IQ https://t.co/qHQJ2z8f45', 'Superboss Games', 'Twitter', '{}', '2021-02-21 19:21:10'),
(21, '4059402835321359996', 'Update 1757 & SALE - 50% off, Unlocks, Item Cache, Physics, and MORE!', 'Ryguytheguy', 'SteamNews', '{}', '2021-02-21 18:00:37'),
(22, '385', 'Allow the game/server to launch/host with anti-cheat disabled', 'BP', 'Helprace', '{\"channel\":\"ideas\"}', '2021-02-21 23:42:47'),
(23, 'hCcgmK28Rw4', 'Intruder 1757 UPDATE - Unlocks, Item Cache, Physics, MORE', 'Superboss Games', 'YouTube', '{}', '2021-02-21 18:00:08'),
(24, '2571', 'Shrike Gold', 'Bloxri', 'Wiki', '{\"pageId\":707,\"type\":\"new\",\"byteDifference\":132}', '2021-02-22 16:02:39'),
(25, '2404148076', 'Toptier', '76561198106593386', 'Workshop', '{}', '2021-02-22 06:02:01'),
(26, '1364741905448271872', 'Even though the Non-Lethal Shotgun is just that, Agent Spoooky3 manages to get a kill with it for #OneTapWednesday 🏖️\n\n#gamedev #indiedev #unity3d #Steam https://t.co/cBVGwU6THk', 'Superboss Games', 'Twitter', '{}', '2021-02-25 01:00:00'),
(27, '391', 'Possible bug when picking up briefcase while inside capture zone. ', 'Eean Ditto', 'Helprace', '{\"channel\":\"problems\"}', '2021-02-25 18:34:55'),
(28, '2407484926', 'Rusty oilrig', '76561198046899874', 'Workshop', '{}', '2021-02-25 16:37:15'),
(29, '2853', 'Template:List of Weapons and Gadgets', 'Pwenguin', 'Wiki', '{\"pageId\":73,\"type\":\"edit\",\"byteDifference\":21}', '2021-02-24 20:49:31'),
(30, '1365104296673628165', 'Agent Jariic takes out an intruder who is halfway down the zipline! Speaking of halfway... did you know Intruder is half off on Steam for the first time?!?! 🤯\n\nGrab a copy for yourself or a friend here: https://t.co/E1lndhe0IQ\n\n#gamedev #indiedev #unity3d #Steam https://t.co/3W7q9ixMyv', 'Superboss Games', 'Twitter', '{}', '2021-02-26 01:00:01'),
(31, '2408293655', 'Bay Villa test map', '76561198204224431', 'Workshop', '{}', '2021-02-26 13:40:48'),
(32, '395', 'Steam Achievements', 'Pwenguin', 'Helprace', '{\"channel\":\"ideas\"}', '2021-02-26 17:22:20'),
(33, '2879', 'Sniper Rifle', 'Bloxri', 'Wiki', '{\"pageId\":62,\"type\":\"edit\",\"byteDifference\":438}', '2021-02-26 16:42:48'),
(34, '4060529371710516499', 'Update 1772: Quality of life, Fixes, Intruder MM, & sale update', 'Rob Storm', 'SteamNews', '{}', '2021-02-27 20:42:46'),
(35, '1365721314011598857', 'Our first Intruder 50% off Sale event is over in &lt; 2 days! Still available this weekend. It\'s going very well though so thank you all!\n\nSteam link: https://t.co/E1lndhe0IQ\n\n#Steam #Unity3d #gamedev #indiedev', 'Superboss Games', 'Twitter', '{}', '2021-02-27 17:51:49'),
(36, '399', 'item cache', 'Anon Anon', 'Helprace', '{\"channel\":\"problems\"}', '2021-02-27 21:33:05'),
(37, '2409306595', 'La Casa', '76561197982431401', 'Workshop', '{}', '2021-02-27 11:46:38'),
(38, '2887', 'Change Log', 'Bloxri', 'Wiki', '{\"pageId\":138,\"type\":\"edit\",\"byteDifference\":399}', '2021-02-27 19:36:03'),
(39, '1365829068063727617', 'Agent @bopman15 takes a tumble after knocking over an enemy, luckily his teammate\'s concussion grenade had just enough power to push him back! 💥\n\nLearn more about Intruder\'s new ragdoll physics and our 50% off sale by checking out the article on Steam!\n\nhttps://t.co/rt7phYMIKq https://t.co/rN2O2I3ve7', 'Superboss Games', 'Twitter', '{}', '2021-02-28 01:00:00'),
(40, '2889', 'Panther Red Ice', 'Pwenguin', 'Wiki', '{\"pageId\":934,\"type\":\"new\",\"byteDifference\":242}', '2021-02-27 23:15:00'),
(41, 'luo2fz', 'The (2nd) longest clips video I\'ve made', 'bopman14', 'Reddit', '{}', '2021-02-28 19:23:00'),
(42, '402', 'Advanced stealth cardboard box (A.S.C.B.)', 'LightXcookie', 'Helprace', '{\"channel\":\"ideas\"}', '2021-02-28 16:04:33'),
(43, '2891', 'User:Vahnthemann', 'RobStorm', 'Wiki', '{\"pageId\":0,\"type\":\"log\",\"byteDifference\":0}', '2021-02-28 16:35:28'),
(44, '410', 'Cant find map by UUID, causing frequent disconnects', 'Mrbadlemonnohope', 'Helprace', '{\"channel\":\"problems\"}', '2021-03-01 18:48:53'),
(45, 'lvkugr', 'Reputation reset option', 'NuclearNacho58', 'Reddit', '{}', '2021-03-01 21:18:16'),
(46, '411', 'Better glass door interactions.', 'Valentine VII', 'Helprace', '{\"channel\":\"ideas\"}', '2021-03-02 00:22:34'),
(47, '1366568944702685185', '🥳🎂 Happy Intrudiversary Agents! 🎂🥳 2 years ago today, Intruder launched on Steam and now, two years later, we have hit record numbers of players every day this week! 📈 Thank you for the support along the way and keep on intruding! &gt;:D\n\n#Steam #Unity3d #gamedev #indiedev https://t.co/W64hVdSXCz', 'Superboss Games', 'Twitter', '{}', '2021-03-02 02:00:00'),
(48, 'lvs7fh', 'Trio Troubles (Gameplay Clips)', 'Vectrex720', 'Reddit', '{}', '2021-03-02 02:55:36'),
(49, 'lvt2jf', 'By far my most embarrassing death', 'AwkwardToast404', 'Reddit', '{}', '2021-03-02 03:44:45'),
(50, 'jr7bsu', 'Ultrawide Support?', 'Dragoru', 'Reddit', '{}', '2020-11-09 21:53:39'),
(51, 'lvt3e2', 'By far my most embarrassing death', 'AwkwardToast404', 'Reddit', '{}', '2021-03-02 03:46:03'),
(52, 'jr7bsu', 'Ultrawide Support?', 'Dragoru', 'Reddit', '{}', '2020-11-09 21:53:39'),
(53, 'lvt445', 'By far my most embarrassing death', 'AwkwardToast404', 'Reddit', '{}', '2021-03-02 03:47:13'),
(54, '417', 'Better Cuffs', 'Valentine VII', 'Helprace', '{\"channel\":\"ideas\"}', '2021-03-03 03:23:37'),
(55, '418', 'Bonus XP for arrests without killing.', 'Jae', 'Helprace', '{\"channel\":\"ideas\"}', '2021-03-03 05:55:26');

-- --------------------------------------------------------

--
-- Table structure for table `z26`
--

CREATE TABLE `z26` (
  `name` varchar(64) NOT NULL,
  `value` varchar(1028) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `z26`
--

INSERT INTO `z26` (`name`, `value`) VALUES
('1110', '1110 is rollig\'s IGN. He has many names.'),
('actionman', 'Action is a mature Aussie chap that programs very large games. In his off time he moderates a large forum, plays large games, undertakes large projects (like IntruderTournaments.com) and he has a really'),
('asper29', 'No time for speaking, only headshots.'),
('AUG', 'Advanced User Group is a select group of elite agents that have proven themselves to be quality members of the community.\nThere are benefits to being in AUG and to apply to be a member, you should see http://superbossgames.com/forum/viewtopic.php?f=14&t=1505\n'),
('austinroush', 'Austin is (one of) the developers Superboss needs. I love him and I want to spend the rest of my life with him. But It?s not about what I want; it\'s about what\'s fair! I?m an agent of chaos. Oh, and you know the thing about chaos- it\'s fair. Together, we will destroy Gotham. I\'m not afraid of you. ~ Z26'),
('australia', 'There are heaps of Aussies that play Intruder. Ping is fairly high to the American servers, but it doesn\'t really cause any issues.'),
('badplayer', 'Punish teamkillers with /punish. Mute mic spammers with /mute <name>. Report other acts of attrition with /report <name>.'),
('ban', 'When a player is punished for teamkilling repeatedly, the game may automatically ban them for a short time. The ban period is perhaps a few hours.'),
('banana', 'Bananas in the game are pick-up-able objects, placed in intuitive spots around the levels. Once picked up and added to your inventory, you can place the peels in e.g., doorways, and make da people fall overs.'),
('benn', 'Benn falls out windows. Everyone knows this.'),
('bennslip', 'http://underscorediscovery.com/sven/bennslip.mp4'),
('binoculars', 'There are binoculars placed in the map. You don\'t spawn with them. They\'re useful for differentiating between friend and foe at a distance. You can also take a screenshot with the binoculars by clicking/firing them. The screenshots are saved to the game directory (probably in program files).'),
('bloon', '<:bloon:244201882484998154>'),
('bloxri', 'Bloxri is 19 y/o girl who plays intruder essentially naked half the time. (S)he created blocside too.'),
('box', 'The box \'hat\' was introduced in update 445, in September 2014. It is an AUG only hat. The command to use it is /box 1.'),
('bug', 'You can report a bug with the game with the command /bug <message>'),
('buy', 'https://superbossgames.com/intruder/store/'),
('cant login', 'Errors when logging in are commonly caused by one of these issues.\n1: You\'re not entering your password correctly. It happens, so please double check, and let us know you\'ve double checked when asking for help.\n2: Your password has characters in it that the game doesn\'t accept. Unfortunately, for the time being, passwords with spaces or special characters in them seem not to work in the game, although they do work when logging in on the website/forum.\nYou can change your password to something simpler here: '),
('change password', 'Login to the forum and change your password here: http://superbossgames.com/forum/ucp.php?i=profile&mode=reg_details\n'),
('changelog url', 'https://superbossgames.com/forum/viewtopic.php?f=14&t=685'),
('cliffhack', 'The intruders need to \'hack\' any 3 of the \'computer systems\'.'),
('cliffside', 'http://superbossgames.com/wiki/index.php?title=Cliffside'),
('clip', 'http://www.youtube.com/watch?v=REdjjLBaiOs'),
('commands', 'Commands should be entered into the in-game chat. For a list of commands, see the faq topic \"game instructions\".'),
('comms', 'You can communicate in game in 4 ways. 1: Global text chat (ENTER). 2: 3D voice chat (C by default), which can be heard by ALL nearby players.\n3: Your Radio gadget (F by default), which when in your hand and transmitting (MOUSE1), broadcasts to all players on your team. Nearby enemy players can hear you talking into your radio, so be careful.\n4: Character hand gestures (TAB).\n'),
('communication', 'Communication is important in Intruder. Check in with teammates etc etc location alert teammates to tripping sensors and spotting enemies etc etc'),
('connection issue', 'http://superbossgames.com/forum/viewtopic.php?f=14&t=1522'),
('continuous voice', 'By default, the game uses voice activation for local voice chat. Most players disable this. To disable: Esc -> Mic setup -> Untick \'auto\'. The HOME key toggles local voice activation.'),
('controls', 'http://superbossgames.com/wiki/index.php?title=Controls'),
('crash', 'The most likely cause of game crashes are missing or corrupt game files. In some cases antivirus software may remove files by mistake, and without telling you. If you use Norton antivirus, this is VERY likely.\nTry disabling your antivirus, and redownloading, reinstalling.\n'),
('custom maps', 'Custom maps were made possible in the 449 patch released in Dec 2014. They\'re made in Unity and you can find out more at https://superbossgames.com/forum/viewtopic.php?f=14&t=2030'),
('demotion', 'Players deemed to have a negative impact on the good of the community are demoted and can not access all servers.\nActions that may cause demotion include mic spamming, trolling, not completing objectives, etc. See http://superbossgames.com/forum/viewtopic.php?f=14&t=1562\n'),
('developers', 'AustinRoush and Robstorm are the developers.'),
('dongers', 'Dongers is a funny lad. Top bloke. Cool dude. Nice guy. Innocent at heart, else his name would clearly be Dongers6969.'),
('dongers7878', 'Dongers is a funny lad. Top bloke. Cool dude. Nice guy. Innocent at heart, else his name would clearly be Dongers6969.'),
('doors', 'Scroll your mousewheel to open doors and vents. If the door is locked, you will need to pick the lock first.'),
('download', 'You can download the installer from https://superbossgames.com/intruder/store/download/'),
('dregas', '<:fsfu:448670713386237962> for life'),
('eula', 'https://superbossgames.com/legal/eula/'),
('faq', 'Meta.'),
('forgot password', 'https://superbossgames.com/forum/ucp.php?mode=sendpassword'),
('fullscreen', '\'/fullscreen true\' and \'/fullscreen false\' do what you\'d expect them to do :)'),
('furry', 'Lets see here, we got Bloodshed, DeerAmazon, err, who else?'),
('gadgets', 'http://superbossgames.com/wiki/index.php?title=Gadgets'),
('game instructions', 'http://docs.google.com/document/d/1JRrMzlp343R3l-afONd-vJuJLHTIf4rPV9TL87wgugs/'),
('gay', 'Perhaps you meant \'Bloodshed\', \'Extricated\', or \'iDubbbz\'?'),
('giftcodes', 'View giftcodes you purchased at https://superbossgames.com/intruder/store/mygiftcodes/ and redeem them at https://superbossgames.com/intruder/store/giftcode/'),
('god', 'If god were real, why did he make me so flawed?'),
('goof', 'Activating goof mode by typing /goof 1 in chat prevents people being punished for TKing you, and puts a goofy cone on your head. You can disable it with /goof 0.'),
('haidirssi', 'Haidirssi is Haidawe\'s other nick. He likes to keep it in the channel 24/7 and log. Then he sends those logs to your mom. And the NSA. And NASA.'),
('hats', 'You can activate some hats ingame with certain commands. These commands need to be used like /cmd 1|0. Check out /santa, /goof, /alert, /huh, /box, /jack, /rudolph. Some hats are for AUG players only.'),
('help', 'What the heck do you want help WITH :D? Try asking about that instead.'),
('home', 'The HOME key toggles local voice activation.'),
('how not to be seen', 'https://superbossgames.com/forum/viewtopic.php?f=3&t=1889&p=10157 and https://www.youtube.com/watch?v=W47nRODqvM8'),
('how to screenshot', 'How to screenshot:  1. Press the PrtScn/Print Screen button on your keyboard. 2. Go to http://snag.gy/ 3. Press CTRL+V, just like it says. 4. Paste the link it gives you (same as the page you end up on).'),
('howtocp', 'Basically everyone on the internet knows how to copy and paste, so that\'s why people are probably laughing if you legitimately don\'t know how. This web page will teach you. http://www.deadzoom.com/instructions/copy_paste_using_keyboard.htm'),
('idubbbz', 'iDubbbz is some gay retard. He has a youtube channel called \'iDubbbzTV\'. He has 3 b\'s in his name like he majored in idiocy, but he\'s actually a tremendously cool dude.'),
('info', 'THIS is info, didn\'cha know. Well now you do, you braindead ho.'),
('install', 'http://superbossgames.com/forum/viewtopic.php?f=14&t=1320'),
('instructions', 'Install instructions are here: http://superbossgames.com/forum/viewtopic.php?f=14&t=1320 and game instructions are here:     http://docs.google.com/document/d/1JRrMzlp343R3l-afONd-vJuJLHTIf4rPV9TL87wgugs/\n'),
('intruder', 'This is intruder http://www.youtube.com/watch?v=7oDmWAONLBY'),
('intruderdb', 'Built by an actual duke, it contains a bunch of useless statistics nobody cares about. Except those pretty graphs, they\'re pretty neat.'),
('intrudermm', 'https://superbossgames.com/forum/viewtopic.php?f=14&t=2030'),
('intruderxmas2014', 'https://www.youtube.com/watch?v=wjcpLu5s0PI'),
('jesus', 'http://inogolo.com/pronunciation/Jesus'),
('keypad', 'Keypads need a different code every round. The code is written on a piece of paper but the location of the paper may be randomised depending on the map. In Riverside you will find the paper in one of the 3 offices furthest away from the security room.'),
('leekster', 'Leekster once was very generous about promoting the game, even giving tens of copies away. He\'s studying, but he makes wads of dosh writing words \'n shit.'),
('legal', 'https://superbossgames.com/legal/'),
('linux', 'Check out vel0hs tutorial on how to get intruder working on linux here https://superbossgames.com/forum/viewtopic.php?f=14&t=2000'),
('list giftcodes', 'https://superbossgames.com/intruder/store/mygiftcodes/'),
('lockpick', 'Doors are randomly locked. They can be unlocked with the lockpick tool. This development video http://www.youtube.com/watch?v=n7cDG60J-5o explains it completely.'),
('lopez', 'Lopez loves sniping people and feeling pro.'),
('me', 'Me? I\'m Bloon!'),
('micboost', 'https://superbossgames.com/forum/viewtopic.php?f=14&t=2029'),
('moomoohk', 'jewjew was drafted for rape duty in Israel, so he\'s only around on weekends now. Don\'t stand in front of OR behind RPGs, pal.\n'),
('mrotton', 'Loves 4k nude photos of kangaroos, but they always get blocked by das Aufl?sungbegrenzung.'),
('mute', 'Mute a player: /mute <playername> Unmute a player: /unmute <playername> List muted players: /whoismuted\n'),
('newb', 'https://www.youtube.com/watch?v=7oDmWAONLBY'),
('news', 'https://SuperbossGames.com/IntruderNewsBlock.txt'),
('night', 'Some maps have a night version.'),
('nohud', 'Press Q+P together to toggle the hud displaying. Press Q+P+O to disable the capture point purple area.'),
('nologo', 'The /nologo command removes the SuperbossGames.com text from the upper right of the screen.'),
('norton', 'Norton Antivirus is rather poop and unfortunately thinks the Intruder installer is some sort of malware. The end result of this is that when downloading the installer, your browser may not finish the download and/or the .msi installer may not show up where you expect it to.\nYou may be able to remove the file from the Norton quarantine area and then use it. Alternatively, disable Norton before you start the download and re-enable it after you install the game. This should be safe if you don\'t continue to bro'),
('nword', 'nword: The one major rule of #SuperbossGames is not to use this word, ever, even joking, even if you think it\'s OK for you.'),
('objective', 'The gamemodes are described in the topics \'raid\' and \'cliffhack\'. To view the locations of the objectives, hit O ingame to bring up the objective menu/screen.'),
('oceanside', 'Oceanside is a map created by SteelRaven with the custom map tools and released with the 449 patch. It\'s frickin\' good. The gatecode is out the back.'),
('optical', 'Optical sounds exactly like idubbbz and R3vo to my australian robot ears. He\'s really cool, but too harsh on fleshypig :P'),
('opticalf3ar', 'Optical sounds exactly like idubbbz and R3vo to my australian robot ears. He\'s really cool, but too harsh on fleshypig :P'),
('papaoptical', 'Optical sounds exactly like idubbbz and R3vo to my australian robot ears. He\'s really cool, but too harsh on fleshypig :P'),
('ping', 'You can view your ping by typing /ping in the chat. If it\'s more than 0, you can blame it when you lose!'),
('poxle', 'Forever locked in the kids table...'),
('privacy', 'https://superbossgames.com/legal/privacy/'),
('r3vo', 'R3vo is notable, but too normal. Nothin embarassing bout him. He makes some decent videos. Cool guy.'),
('racism', 'We don\'t like racism. Rob especially dislikes racism. Try not to come across as racist by not 1. being racist 2. making racist remarks. If you ARE racist, keep it to your damn self.'),
('radio', 'The default key to pull out the radio is F. Once in your hand, you need to hold MOUSE1 to transmit. All players on your team hear the transmission, and any nearby enemies will also hear you talking into your radio.\nOften players will signal to other players without talking by clicking MOUSE1 to transmit and releasing it instantly. This breaks squelch for your teammates and they hear a small crackle.\n'),
('raid', 'Raid is a bit like capture the flag but only the guards have flags. The guards must defend two briefcases that are on their side of the map while the intruders breach from their own side of the map, grab one of the two packages, and exfiltrate back to their side of the map.'),
('real pistol', 'The pistol in the game is based off the QSZ-92. There is a neat picture here http://daisukekazama.deviantart.com/art/QSZ-92-Desertised-194528005'),
('real smg', 'The SMG in Intruder is based off the Kriss Vector. There is a picture of a vector that looks like the one in game at http://img4.wikia.nocookie.net/__cb20111226164119/nazizombiesplus/images/3/30/Kriss_Super_V.jpg'),
('real sniper', 'The sniper rifle in the game is based off of the PSG-1.'),
('redeem giftcodes', 'https://superbossgames.com/intruder/store/giftcode/'),
('rekedens', 'Rekedens is a lovable hick. He is talented, likes cars, gfx, design, photography, and long walks between the trailers while singing coombayah <3.'),
('report', 'You can report a player by typing /report <playername> <reason>'),
('resolution', 'As well as in the settings dialog when you run the game, you can change your resolution ingame with some commands. /stp sets the game window to 720p. /tep sets the game window to 1080p.'),
('riverside', 'http://superbossgames.com/wiki/index.php?title=Riverside_Floorplan_Image and a high res version http://i2.minus.com/i2VPR5COnk7Vc.png'),
('robstorm', 'RobStorm is one of the Superboss developers. He is is the mother of all intruders, as they flew out of his womb to do his bidding he snickered at the mortal guards. He prefers music to games. retweet if you havn\'t yeti. brbutt. ~ Z26'),
('rollig', 'rollig/rolig/alfred/1110/monty/trolig/etc birthed z26. He\'s typically a real nice guy, but he he\'s also creepy af :D. Used to be called rollig, but then changed his name to funny.'),
('rolling', 'https://gs1.wac.edgecastcdn.net/8019B6/data.tumblr.com/2c87201302d1b668ba683a30957f0310/tumblr_ngzwbsDX5m1s02vreo1_500.gif'),
('russianpod', 'russianpod is 13 and has the worst internet known to man.'),
('screenshot', 'The game includes a way of taking screenshots. Use the binoculars! You can also press U+H+B to make a screenshot.bmp in your game directory, but as this file is overwritten with subsequent screenshots, it is of limited use.'),
('screenshot location', 'Screenshots taken with the binoculars are saved to your game directory - probably in program files.'),
('screenshotting is so hard', 'http://www.take-a-screenshot.org/'),
('sebastianthecrab', 'Crabastiantheseb is one of the gods of Intruder. His meat is delicious.'),
('secret', 'Dumbledore was gay.'),
('secretss', 'SHH.'),
('servername', 'To find out the name of the server you\'re currently in, use the /st or /server commands.'),
('seventhfrost', 'Seventhfrost is writing a thesis on Intruder. He just hasn\'t started writing.'),
('spy', 'Spy is a cool guy, good player that makes pointless batman voices and cat noises ingame. My robot logic cannot comprehend the value of this.'),
('statsimplemented', 'Stats don\'t go to the start of time. They were added in April 2014. https://superbossgames.com/content/intruder-update-stats-gift-codes-and-friends'),
('stealthgenius', 'https://www.youtube.com/watch?v=u6TP_qYyv2E'),
('steelraven7', 'SVENSKE SUPERMANG'),
('suicide', 'You can kill yourself in-game by typing /kill in the chat.'),
('super_', 'You\'re all a bunch of retarded cunts'),
('t-t-t', '\'i don\'t watch anime during the day or where others can see me\''),
('tasmania', 'Cheeseness and BattleCat are both from Tasmania. They\'re brothers/father + son.'),
('teamkilling', 'If a player teamkills you, you can type \"/punish\" in the chat and they will be forced to miss out on the next round. Repeat teamkillers will be banned temporarily. If you and a friend are teamkilling consensually, you should type /goof 1 so no-one is punished.'),
('thisisnotatag', 'Thisisnotatag likes to state the obvious and simple. But we love him for it???'),
('tmc', 'TMC stands for Trust, Maturity and Contribution. You can read more about it at http://superbossgames.com/wiki/index.php?title=TMC'),
('tonjevic', 'Tonjevic escaped. He broke free and eluded our best efforts at recapture for several months. Then he turned back around and gave himself up. Brainwashing is not a -perfect- science.'),
('tutorial', 'http://superbossgames.com/forum/viewtopic.php?f=14&t=1742'),
('vectrex720', 'le keyboard obsessive.'),
('voice', 'There are two forms of voice chat in the game. The first is 3D voice chat (C by default), which can be heard by ALL nearby players. The second is the radio gadget.\nMore info on the radio gadget can be found in this faq under \"radio\" or on the Superboss wiki at http://superbossgames.com/wiki/index.php?title=Gadgets\nCheck out the \'continuous voice\' faq topic if that\'s an issue for you.\n'),
('wiki', 'http://superbossgames.com/wiki/index.php?title=Category:Intruder'),
('yaib', 'YAAAAAAAAAAAAAAAAAAAIIIIIIIIIIIIBBBBBBBBBBBBBBB'),
('yond', 'Yond is super cool and apparently super fit. \'Tween you and me, I\'d S his D if you know what I mean.'),
('you', 'No. FAQ you!'),
('z26', 'I was a passing murmur in the breeze.');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `faq`
--
ALTER TABLE `faq`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `feature_status`
--
ALTER TABLE `feature_status`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `name` (`name`);

--
-- Indexes for table `job`
--
ALTER TABLE `job`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `name` (`name`);

--
-- Indexes for table `ltp`
--
ALTER TABLE `ltp`
  ADD PRIMARY KEY (`user_id`);

--
-- Indexes for table `message`
--
ALTER TABLE `message`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `role_member`
--
ALTER TABLE `role_member`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `Unique` (`member_id`,`role_id`);

--
-- Indexes for table `sba_inactivity_tracking`
--
ALTER TABLE `sba_inactivity_tracking`
  ADD PRIMARY KEY (`user_id`);

--
-- Indexes for table `social_item`
--
ALTER TABLE `social_item`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `z26`
--
ALTER TABLE `z26`
  ADD PRIMARY KEY (`name`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `faq`
--
ALTER TABLE `faq`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=31;

--
-- AUTO_INCREMENT for table `feature_status`
--
ALTER TABLE `feature_status`
  MODIFY `id` int(10) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=30;

--
-- AUTO_INCREMENT for table `job`
--
ALTER TABLE `job`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;

--
-- AUTO_INCREMENT for table `message`
--
ALTER TABLE `message`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=38;

--
-- AUTO_INCREMENT for table `role_member`
--
ALTER TABLE `role_member`
  MODIFY `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=369;

--
-- AUTO_INCREMENT for table `social_item`
--
ALTER TABLE `social_item`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=56;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
