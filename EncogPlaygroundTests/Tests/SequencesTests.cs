using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EncogPlayground;

namespace EncogPlaygroundTests.Tests
{
    [TestClass]
    public class SequencesTests
    {
        private Sequences GetSeq()
        {
            return new Sequences();
        }

        [TestMethod]
        public void FibShouldStartAt1()
        {
            Assert.AreEqual(1, GetSeq().Fibbonacci(0, 100).First());
        }

        [TestMethod]
        public void FibShouldStart12358()
        {
            CollectionAssert.AreEqual(new[] { 1, 2, 3, 5, 8 }, GetSeq().Fibbonacci(0, 100).Take(5).ToArray());
        }

        [TestMethod]
        public void FibShouldIncludeMaxIfInSeq()
        {
            CollectionAssert.Contains(GetSeq().Fibbonacci(0, 8).ToArray(), 8);
        }

        [TestMethod]
        public void FibShouldNotIncludeMaxIfNotInSeq()
        {
            Assert.IsTrue(GetSeq().Fibbonacci(0, 9).Max() < 9);
        }

        [TestMethod]
        public void FibShouldIncludeMinIfInSeq()
        {
            Assert.AreEqual(8, GetSeq().Fibbonacci(8, 100).Min());
        }

        [TestMethod]
        public void FibNoResultsWorks()
        {
            Assert.AreEqual(0, GetSeq().Fibbonacci(60, 61).Count());
        }

        [TestMethod]
        public void TenPowersShouldStartAt1()
        {
            CollectionAssert.AreEqual(new[] { 1 }, GetSeq().PowersOf10(0, 8).ToArray());
        }

        [TestMethod]
        public void TenPowersShouldIncludeMax()
        {
            CollectionAssert.AreEqual(new[] { 1, 10 }, GetSeq().PowersOf10(0, 10).ToArray());
        }

        [TestMethod]
        public void TenPowersShouldNotExceedMaxNotInSeq()
        {
            Assert.IsTrue(GetSeq().PowersOf10(0, 11).Max() <= 10);
        }

        [TestMethod]
        public void TenPowersShouldIncludeMinIfPower()
        {
            Assert.AreEqual(10, GetSeq().PowersOf10(10, 11).Min());
        }

        [TestMethod]
        public void TenPowersShouldntIncludeMinIfNotPower()
        {
            Assert.AreEqual(100, GetSeq().PowersOf10(11, 1000).Min());
        }

        [TestMethod]
        public void TenPowersNoResultsWorks()
        {
            Assert.AreEqual(0, GetSeq().PowersOf10(60, 61).Count());
        }
    }
}
