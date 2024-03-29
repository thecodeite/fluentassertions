﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FluentAssertions.Common;

#if WINRT
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace FluentAssertions.Specs
{
    [TestClass]
    public class SimpleTimeSpanAssertionSpecs
    {
        private readonly TimeSpan OneSecondNegative = 1.Seconds().Negate();
        private readonly TimeSpan OneSecond = 1.Seconds();
        private readonly TimeSpan TwoSeconds = 2.Seconds();

        [TestMethod]
        public void Should_succeed_when_asserting_positive_value_to_be_positive()
        {
            OneSecond.Should().BePositive();
        }

        [TestMethod]
        public void Should_fail_when_asserting_negative_value_to_be_positive()
        {
            Action act = () => OneSecondNegative.Should().BePositive();
            act.ShouldThrow<AssertFailedException>();

        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_negative_value_to_be_positive()
        {
            var assertions = (OneSecondNegative).Should();
            assertions.Invoking(x => x.BePositive("because we want to test the failure {0}", "message"))
                .ShouldThrow<AssertFailedException>()
                .WithMessage("Expected positive value because we want to test the failure message, but found -1s");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_negative_value_to_be_negative()
        {
            OneSecondNegative.Should().BeNegative();
        }

        [TestMethod]
        public void Should_fail_when_asserting_positive_value_to_be_negative()
        {
            Action act = () => OneSecond.Should().BeNegative();
            act.ShouldThrow<AssertFailedException>();

        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_positive_value_to_be_negative()
        {
            var assertions = (OneSecond).Should();
            assertions.Invoking(x => x.BeNegative("because we want to test the failure {0}", "message"))
                .ShouldThrow<AssertFailedException>()
                .WithMessage("Expected negative value because we want to test the failure message, but found 1s");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_value_to_be_equal_to_same_value()
        {
            OneSecond.Should().Be(TimeSpan.FromSeconds(1));
        }

        [TestMethod]
        public void Should_fail_when_asserting_value_to_be_equal_to_different_value()
        {
            Action act = () => OneSecond.Should().Be(TwoSeconds);
            act.ShouldThrow<AssertFailedException>();

        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_value_to_be_equal_to_different_value()
        {
            var assertions = OneSecond.Should();
            assertions.Invoking(x => x.Be(TwoSeconds, "because we want to test the failure {0}", "message"))
                .ShouldThrow<AssertFailedException>()
                .WithMessage(@"Expected 2s because we want to test the failure message, but found 1s.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_value_to_be_not_equal_to_different_value()
        {
            OneSecond.Should().NotBe(TwoSeconds);
        }

        [TestMethod]
        public void Should_fail_when_asserting_value_to_be_not_equal_to_the_same_value()
        {
            Action act = () => OneSecond.Should().NotBe(OneSecond);
            act.ShouldThrow<AssertFailedException>();

        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_value_to_be_not_equal_to_the_same_value()
        {
            var assertions = OneSecond.Should();
            assertions.Invoking(x => x.NotBe(OneSecond, "because we want to test the failure {0}", "message"))
                .ShouldThrow<AssertFailedException>()
                .WithMessage(@"Did not expect 1s because we want to test the failure message.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_value_to_be_greater_than_smaller_value()
        {
            TwoSeconds.Should().BeGreaterThan(OneSecond);
        }

        [TestMethod]
        public void Should_fail_when_asserting_value_to_be_greater_than_greater_value()
        {
            Action act = () => OneSecond.Should().BeGreaterThan(TwoSeconds);
            act.ShouldThrow<AssertFailedException>();

        }

        [TestMethod]
        public void Should_fail_when_asserting_value_to_be_greater_than_same_value()
        {
            Action act = () => TwoSeconds.Should().BeGreaterThan(TwoSeconds);
            act.ShouldThrow<AssertFailedException>();

        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_value_to_be_greater_than_greater_value()
        {
            var assertions = OneSecond.Should();
            assertions.Invoking(x => x.BeGreaterThan(TwoSeconds, "because we want to test the failure {0}", "message"))
                .ShouldThrow<AssertFailedException>()
                .WithMessage(@"Expected a value greater than 2s because we want to test the failure message, but found 1s.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_value_to_be_greater_or_equal_to_smaller_value()
        {
            TwoSeconds.Should().BeGreaterOrEqualTo(OneSecond);
        }

        [TestMethod]
        public void Should_succeed_when_asserting_value_to_be_greater_or_equal_to_same_value()
        {
            TwoSeconds.Should().BeGreaterOrEqualTo(TwoSeconds);
        }

        [TestMethod]
        public void Should_fail_when_asserting_value_to_be_greater_or_equal_to_greater_value()
        {
            Action act = () => OneSecond.Should().BeGreaterOrEqualTo(TwoSeconds);
            act.ShouldThrow<AssertFailedException>();

        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_value_to_be_greater_or_equal_to_greater_value()
        {
            var assertions = OneSecond.Should();
            assertions.Invoking(x => x.BeGreaterOrEqualTo(TwoSeconds, "because we want to test the failure {0}", "message"))
                .ShouldThrow<AssertFailedException>()
                .WithMessage(@"Expected a value greater or equal to 2s because we want to test the failure message, but found 1s.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_value_to_be_less_than_greater_value()
        {
            OneSecond.Should().BeLessThan(TwoSeconds);
        }

        [TestMethod]
        public void Should_fail_when_asserting_value_to_be_less_than_smaller_value()
        {
            Action act = () => TwoSeconds.Should().BeLessThan(OneSecond);
            act.ShouldThrow<AssertFailedException>();

        }

        [TestMethod]
        public void Should_fail_when_asserting_value_to_be_less_than_same_value()
        {
            Action act = () => TwoSeconds.Should().BeLessThan(TwoSeconds);
            act.ShouldThrow<AssertFailedException>();

        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_value_to_be_less_than_smaller_value()
        {
            var assertions = TwoSeconds.Should();
            assertions.Invoking(x => x.BeLessThan(OneSecond, "because we want to test the failure {0}", "message"))
                .ShouldThrow<AssertFailedException>()
                .WithMessage(@"Expected a value less than 1s because we want to test the failure message, but found 2s.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_value_to_be_less_or_equal_to_greater_value()
        {
            OneSecond.Should().BeLessOrEqualTo(TwoSeconds);
        }

        [TestMethod]
        public void Should_succeed_when_asserting_value_to_be_less_or_equal_to_same_value()
        {
            TwoSeconds.Should().BeLessOrEqualTo(TwoSeconds);
        }

        [TestMethod]
        public void Should_fail_when_asserting_value_to_be_less_or_equal_to_smaller_value()
        {
            Action act = () => TwoSeconds.Should().BeLessOrEqualTo(OneSecond);
            act.ShouldThrow<AssertFailedException>();

        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_value_to_be_less_or_equal_to_smaller_value()
        {
            var assertions = TwoSeconds.Should();
            assertions.Invoking(x => x.BeLessOrEqualTo(OneSecond, "because we want to test the failure {0}", "message"))
                .ShouldThrow<AssertFailedException>()
                .WithMessage(@"Expected a value less or equal to 1s because we want to test the failure message, but found 2s.");
        }


    }
}
