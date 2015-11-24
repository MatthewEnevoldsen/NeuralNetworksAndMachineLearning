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
        public void FibShouldntStart11()
        {
            CollectionAssert.AreEqual(new[] { 1 }, GetSeq().FibbonacciLessThan(1).ToArray());
        }

        [TestMethod]
        public void FibShouldStart12358()
        {
            CollectionAssert.AreEqual(new[] { 1, 2, 3, 5, 8 }, GetSeq().FibbonacciLessThan(8).ToArray());
        }

        [TestMethod]
        public void FibShouldIncludeMax()
        {
            CollectionAssert.Contains(GetSeq().FibbonacciLessThan(8).ToArray(), 8);
        }

        [TestMethod]
        public void FibShouldntIncludeAboveMax()
        {
            Assert.IsTrue(GetSeq().FibbonacciLessThan(9).Max() < 9);
        }

        [TestMethod]
        public void TenPowersShouldStartAt1()
        {
            CollectionAssert.AreEqual(new[] { 1 }, GetSeq().PowersOf10(8).ToArray());
        }

        [TestMethod]
        public void TenPowersShouldIncludeMax()
        {
            CollectionAssert.AreEqual(new[] { 1, 10 }, GetSeq().PowersOf10(10).ToArray());
        }

        [TestMethod]
        public void TenPowersShouldNotExceedMax()
        {
            Assert.IsTrue(GetSeq().PowersOf10(10).Max() <= 10);
        }
    }
}
