﻿using System;

using FluentAssertions.Common;

#if WINRT
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace FluentAssertions.Specs
{
    [TestClass]
    public class TimeSpanConversionExtensionSpecs
    {
        [TestMethod]
        public void When_getting_the_number_of_days_it_should_return_the_correct_time_span_value()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            TimeSpan time = 4.Days();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            Assert.AreEqual(new TimeSpan(4, 0, 0, 0), time);
        }

        [TestMethod]
        public void When_getting_the_number_of_hours_it_should_return_the_correct_time_span_value()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            TimeSpan time = 4.Hours();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            Assert.AreEqual(new TimeSpan(4, 0, 0), time);
        }

        [TestMethod]
        public void When_getting_the_number_of_minutes_it_should_return_the_correct_time_span_value()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            TimeSpan time = 4.Minutes();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            Assert.AreEqual(new TimeSpan(0, 4, 0), time);
        }

        [TestMethod]
        public void When_getting_the_number_of_seconds_it_should_return_the_correct_time_span_value()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            TimeSpan time = 4.Seconds();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            Assert.AreEqual(new TimeSpan(0, 0, 4), time);
        }

        [TestMethod]
        public void When_getting_the_number_of_milliseconds_it_should_return_the_correct_time_span_value()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            TimeSpan time = 4.Milliseconds();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            Assert.AreEqual(new TimeSpan(0, 0, 0, 0, 4), time);
        }

        [TestMethod]
        public void When_combining_fluent_time_methods_it_should_return_the_correct_time_span_value()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            TimeSpan time1 = 23.Hours().And(59.Minutes());
            TimeSpan time2 = 23.Hours(59.Minutes()).And(20.Seconds());
            TimeSpan time3 = 1.Days(2.Hours(33.Minutes(44.Seconds()))).And(99.Milliseconds());

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            Assert.AreEqual(new TimeSpan(23, 59, 0), time1);
            Assert.AreEqual(new TimeSpan(23, 59, 20), time2);
            Assert.AreEqual(new TimeSpan(1, 2, 33, 44, 99), time3);
        }

        [TestMethod]
        public void When_specifying_a_time_before_another_time_it_should_return_the_correct_time()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            DateTime now = 21.September(2011).At(07, 35);

            DateTime twoHoursAgo = 2.Hours().Before(now);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            Assert.AreEqual(new DateTime(2011, 9, 21, 05, 35, 00), twoHoursAgo);
        }

        [TestMethod]
        public void When_specifying_a_time_after_another_time_it_should_return_the_correct_time()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            DateTime now = 21.September(2011).At(07, 35);

            DateTime twoHoursLater = 2.Hours().After(now);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            Assert.AreEqual(new DateTime(2011, 9, 21, 09, 35, 00), twoHoursLater);
        }
    }
}