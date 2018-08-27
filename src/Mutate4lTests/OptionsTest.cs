﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mutate4l.Cli;
using Mutate4l.Core;
using Mutate4l.Dto;
using System.Collections.Generic;

namespace Mutate4lTests
{
    class OptionsClassOne
    {
        [OptionInfo(groupId: 1, type: OptionType.AllOrSpecified)]
        public bool GroupOneToggleOne { get; set; }

        [OptionInfo(groupId: 1, type: OptionType.AllOrSpecified)]
        public bool GroupOneToggleTwo { get; set; }

        [OptionInfo(groupId: 2, type: OptionType.AllOrSpecified)]
        public bool GroupTwoToggleOne { get; set; }

        [OptionInfo(groupId: 2, type: OptionType.AllOrSpecified)]
        public bool GroupTwoToggleTwo { get; set; }

        public decimal DecimalValue { get; set; }

        public bool SimpleBoolFlag { get; set; }

        [OptionInfo(min: 1, max: 100)]
        public int IntValue { get; set; }
    }

    enum TestEnum
    {
        EnumValue1,
        EnumValue2
    }

    class OptionsClassTwo
    {
        public decimal[] DecimalValue { get; set; }

        public int[] IntValue { get; set; }

        public TestEnum EnumValue { get; set; }
    }

    [TestClass]
    public class OptionsTest
    {
        [TestMethod]
        public void TestToggleGroups()
        {
            var options = new Dictionary<TokenType, List<Token>>();
            options[TokenType.GroupOneToggleOne] = new List<Token>();
            var parsedOptions = OptionParser.ParseOptions<OptionsClassOne>(new Command { Options = options });
            Assert.IsTrue(parsedOptions.GroupOneToggleOne);
            Assert.IsFalse(parsedOptions.GroupOneToggleTwo);
            Assert.IsTrue(parsedOptions.GroupTwoToggleOne);
            Assert.IsTrue(parsedOptions.GroupTwoToggleTwo);
        }

        [TestMethod]
        public void TestValues()
        {
            var options = new Dictionary<TokenType, List<Token>>();
            options[TokenType.DecimalValue] = new List<Token>() { new Token(TokenType.MusicalDivision, "1/8", 0) };
            options[TokenType.IntValue] = new List<Token>() { new Token(TokenType.Number, "14", 0) };
            options[TokenType.SimpleBoolFlag] = new List<Token>();
            var parsedOptions = OptionParser.ParseOptions<OptionsClassOne>(new Command { Options = options });
            Assert.AreEqual(14, parsedOptions.IntValue);
            Assert.AreEqual(.5m, parsedOptions.DecimalValue);
            Assert.IsTrue(parsedOptions.SimpleBoolFlag);
        }

        [TestMethod]
        public void TestMinMaxValue()
        {
            var options = new Dictionary<TokenType, List<Token>>();
            options[TokenType.IntValue] = new List<Token>() { new Token(TokenType.Number, "1000", 0) };
            var parsedOptions = OptionParser.ParseOptions<OptionsClassOne>(new Command { Options = options });
            Assert.AreEqual(100, parsedOptions.IntValue);
            options[TokenType.IntValue] = new List<Token>() { new Token(TokenType.Number, "0", 0) };
            parsedOptions = OptionParser.ParseOptions<OptionsClassOne>(new Command { Options = options });
            Assert.AreEqual(1, parsedOptions.IntValue);
        }

        [TestMethod]
        public void TestListValues()
        {
            var options = new Dictionary<TokenType, List<Token>>();
            options[TokenType.DecimalValue] = new List<Token>() { new Token(TokenType.MusicalDivision, "1/8", 0), new Token(TokenType.MusicalDivision, "1/16", 0) };
            options[TokenType.IntValue] = new List<Token>() { new Token(TokenType.Number, "14", 0), new Token(TokenType.Number, "2", 0), new Token(TokenType.Number, "900", 0) };
            var parsedOptions = OptionParser.ParseOptions<OptionsClassTwo>(new Command { Options = options });
            Assert.AreEqual(2, parsedOptions.DecimalValue.Length);
            Assert.AreEqual(3, parsedOptions.IntValue.Length);
        }

        [TestMethod]
        public void TestEnumValues()
        {
            var options = new Dictionary<TokenType, List<Token>>();
            options[TokenType.EnumValue] = new List<Token>() { new Token(TokenType.EnumValue, "enumvalue2", 0) };
            var parsedOptions = OptionParser.ParseOptions<OptionsClassTwo>(new Command { Options = options });
            Assert.AreEqual(TestEnum.EnumValue2, parsedOptions.EnumValue);
        }
    }
}
