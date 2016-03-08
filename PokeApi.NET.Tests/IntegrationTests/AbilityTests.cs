﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PokeAPI.Tests.IntegrationTests
{
    public class AbilityTests
    {
        [Theory]
        [InlineData(1)][InlineData(2)][InlineData(3)][InlineData(4)][InlineData(5)][InlineData(6)][InlineData(7)][InlineData(8)][InlineData(9)][InlineData(10)][InlineData(11)][InlineData(12)][InlineData(13)][InlineData(14)][InlineData(15)][InlineData(16)][InlineData(17)][InlineData(18)][InlineData(19)][InlineData(20)][InlineData(21)][InlineData(22)][InlineData(23)][InlineData(24)][InlineData(25)][InlineData(26)][InlineData(27)][InlineData(28)][InlineData(29)][InlineData(30)][InlineData(31)][InlineData(32)][InlineData(33)][InlineData(34)][InlineData(35)]
        [InlineData(36)][InlineData(37)][InlineData(38)][InlineData(39)][InlineData(40)][InlineData(41)][InlineData(42)][InlineData(43)][InlineData(44)][InlineData(45)][InlineData(46)][InlineData(47)][InlineData(48)][InlineData(49)][InlineData(50)][InlineData(51)][InlineData(52)][InlineData(53)][InlineData(54)][InlineData(55)][InlineData(56)][InlineData(57)][InlineData(58)][InlineData(59)][InlineData(60)][InlineData(61)][InlineData(62)][InlineData(63)][InlineData(64)][InlineData(65)][InlineData(66)][InlineData(67)][InlineData(68)][InlineData(69)][InlineData(70)]
        [InlineData(71)][InlineData(72)][InlineData(73)][InlineData(74)][InlineData(75)][InlineData(76)][InlineData(77)][InlineData(78)][InlineData(79)][InlineData(80)][InlineData(81)][InlineData(82)][InlineData(83)][InlineData(84)][InlineData(85)][InlineData(86)][InlineData(87)][InlineData(88)][InlineData(89)][InlineData(90)][InlineData(91)][InlineData(92)][InlineData(93)][InlineData(94)][InlineData(95)][InlineData(96)][InlineData(97)][InlineData(98)][InlineData(99)][InlineData(100)][InlineData(101)][InlineData(102)][InlineData(103)][InlineData(104)][InlineData(105)]
        [InlineData(106)][InlineData(107)][InlineData(108)][InlineData(109)][InlineData(110)][InlineData(111)][InlineData(112)][InlineData(113)][InlineData(114)][InlineData(115)][InlineData(116)][InlineData(117)][InlineData(118)][InlineData(119)][InlineData(120)][InlineData(121)][InlineData(122)][InlineData(123)][InlineData(124)][InlineData(125)][InlineData(126)][InlineData(127)][InlineData(128)][InlineData(129)][InlineData(130)][InlineData(131)][InlineData(132)][InlineData(133)][InlineData(134)][InlineData(135)][InlineData(136)][InlineData(137)][InlineData(138)][InlineData(139)][InlineData(140)]
        [InlineData(141)][InlineData(142)][InlineData(143)][InlineData(144)][InlineData(145)][InlineData(146)][InlineData(147)][InlineData(148)][InlineData(149)][InlineData(150)][InlineData(151)][InlineData(152)][InlineData(153)][InlineData(154)][InlineData(155)][InlineData(156)][InlineData(157)][InlineData(158)][InlineData(159)][InlineData(160)][InlineData(161)][InlineData(162)][InlineData(163)][InlineData(164)][InlineData(165)][InlineData(166)][InlineData(167)][InlineData(168)][InlineData(169)][InlineData(170)][InlineData(171)][InlineData(172)][InlineData(173)][InlineData(174)][InlineData(175)]
        [InlineData(176)][InlineData(177)][InlineData(178)][InlineData(179)][InlineData(180)][InlineData(181)][InlineData(182)][InlineData(183)][InlineData(184)][InlineData(185)][InlineData(186)][InlineData(187)][InlineData(188)][InlineData(189)][InlineData(190)][InlineData(191)]
        [InlineData(10001)][InlineData(10002)][InlineData(10003)][InlineData(10004)][InlineData(10005)][InlineData(10006)][InlineData(10007)][InlineData(10008)][InlineData(10009)][InlineData(10010)]
        [InlineData(10011)][InlineData(10012)][InlineData(10013)][InlineData(10014)][InlineData(10015)][InlineData(10016)][InlineData(10017)][InlineData(10018)][InlineData(10019)][InlineData(10020)][InlineData(10021)][InlineData(10022)][InlineData(10023)][InlineData(10024)][InlineData(10025)][InlineData(10026)][InlineData(10027)][InlineData(10028)][InlineData(10029)][InlineData(10030)][InlineData(10031)][InlineData(10032)][InlineData(10033)][InlineData(10034)][InlineData(10035)][InlineData(10036)][InlineData(10037)][InlineData(10038)][InlineData(10039)][InlineData(10040)][InlineData(10041)][InlineData(10042)][InlineData(10043)][InlineData(10044)][InlineData(10045)]
        [InlineData(10046)][InlineData(10047)][InlineData(10048)][InlineData(10049)][InlineData(10050)][InlineData(10051)][InlineData(10052)][InlineData(10053)][InlineData(10054)][InlineData(10055)][InlineData(10056)][InlineData(10057)][InlineData(10058)][InlineData(10059)][InlineData(10060)]
        public async Task GetsAbility(int abilityID)
        {
            var t = DataFetcher.GetApiObject<Ability>(abilityID);
            Assert.NotNull(t);
        }
    }
}
