-- phpMyAdmin SQL Dump
-- version 5.0.2
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Mar 13, 2021 at 02:03 AM
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
-- Table structure for table `censor`
--

CREATE TABLE `censor` (
  `id` int(11) NOT NULL,
  `pattern` varchar(128) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

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
(1, '(what|how).*(switch|change|move|swap).*(maps?)', 'To change the map ingame make yourself an admin & then use `/changemap mapName` (You must be the admin to do this).'),
(2, '(how|what).*(can)?.*(make|give|become|be) (admini?s?t?r?a?t?o?r?)', 'To make yourself an admin use `/makeadmin` this only works if you are the original room creator.'),
(3, '(is).*(map|list).*(map|list)', 'The Official maps are Riverside, Mountainside, Cliffside, and Oceanside; there are many unofficial maps out there too, check them out at: <https://steamcommunity.com/app/518150/workshop/>'),
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
(15, '(is|how|can).*(show|change).*(fps)', 'You can show your current FPS on your hud simply by doing `/fps`. You can change your FPS and Vsync settings from the options'),
(16, '(((what).*(is).*(default|normal|original).*(gravity))|((how|can|do).*(change|modify).*(gravity)))', 'Gravity by default is at `-9.81`. To change the gravity you do `/gravity #`. ***REMINDER YOU MUST BE AN ADMIN TO DO THIS***'),
(17, '(((how|can).*(change|retrieve).*(passw?o?r?d?))|((i).*(forgot).*(password)))', 'You may reset your old user password at: <https://intruderfps.com/reset-password>'),
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
(5, 'Twitter', 0),
(6, 'Twitch', 0),
(7, 'Totally Not Spam', 1),
(8, 'SBA Inactivity Pruning', 0),
(9, 'Reddit', 0),
(10, 'Random Responses', 1),
(11, 'Fetch Youtube Videos', 1),
(12, 'PackageAccounts', 0),
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
(5, 'Looking for links? Check out <#892796013759303760>'),
(6, 'Our wiki is home to a lot of useful information. Run `.help -wiki` to get started today.'),
(7, 'The developer of this bot really likes pizza. You can buy him one if you also really enjoy pizza. `.donate`'),
(8, 'Check out <#106405837810995200> for the latest suggestions and bugs discovered by fellow players.'),
(9, 'Want to help squash a bug you found? Use .bugs'),
(10, 'Old Intruder profiles are no longer being updated and will **not** be linked to corresponding Steam profiles.'),
(11, 'Follow Superboss Games on Twitter! https://twitter.com/SuperbossGames/'),
(12, 'Have a video of Intruder? Post it in <#268062505761243137>'),
(13, 'Join our `Looking to Play` role using the command `/ltp`'),
(14, 'Want to leave the `Looking to Play` role? Type `/ltp`'),
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
(25, 'Want to change the map? Use `/changemap {mapName}`'),
(26, 'Need to know the console commands? .wiki commands'),
(27, 'The default key to pull out the radio is F. Once in your hand, you need to hold MOUSE1 to transmit. All players on your team hear the transmission, and any nearby enemies will also hear you talking into your radio.'),
(28, 'There are two forms of voice chat in the game. The first is 3D voice chat (C by default), which can be heard by ALL nearby players.'),
(29, 'Players deemed to have a negative impact on the good of the community are demoted and can not access all servers.'),
(30, 'You can activate some hats ingame with certain commands. These commands need to be used like /{hatName} 1|0. Check out /santa, /goof, /alert, /huh, /box, /jack, /rudolph. Some hats are for AUG players only.'),
(31, 'Activating goof mode by typing /goof 1 in chat prevents people being punished for TKing you, and puts a goofy cone on your head. You can disable it with /goof 0.'),
(32, 'Advanced User Group is a select group of elite agents that have proven themselves to be quality members of the community.'),
(33, 'The most likely cause of game crashes are missing or corrupt game files. In some cases antivirus software may remove files by mistake, and without telling you. If you use Norton, AVG or AVAST, uninstall and get something decent.'),
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

-- --------------------------------------------------------

--
-- Table structure for table `sba_inactivity_tracking`
--

CREATE TABLE `sba_inactivity_tracking` (
  `user_id` bigint(20) UNSIGNED NOT NULL,
  `last_message` datetime NOT NULL,
  `warning_timestamp` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

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
-- Indexes for table `censor`
--
ALTER TABLE `censor`
  ADD PRIMARY KEY (`id`);

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
-- AUTO_INCREMENT for table `censor`
--
ALTER TABLE `censor`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

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
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `message`
--
ALTER TABLE `message`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=38;

--
-- AUTO_INCREMENT for table `role_member`
--
ALTER TABLE `role_member`
  MODIFY `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `social_item`
--
ALTER TABLE `social_item`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
