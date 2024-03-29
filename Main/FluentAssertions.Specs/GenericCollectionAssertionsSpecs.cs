﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#if WINRT
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace FluentAssertions.Specs
{
    [TestClass]
    public class GenericCollectionAssertionsSpecs
    {
        #region (Not) Contain

        [TestMethod]
        public void When_collection_does_not_contain_an_expected_item_matching_a_predicate_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<int> collection = new[] { 1, 2, 3 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().Contain(item => item > 3, "at least {0} item should be larger than 3", 1);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Collection {1, 2, 3} should have an item matching (item > 3) because at least 1 item should be larger than 3.");
        }

        [TestMethod]
        public void When_collection_does_contain_an_expected_item_matching_a_predicate_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<int> collection = new[] { 1, 2, 3 };

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            collection.Should().Contain(item => item == 2);
        }

        [TestMethod]
        public void When_a_collection_of_strings_contains_the_expected_string_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var strings = new[] { "string1", "string2", "string3" };

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            strings.Should().Contain("string2");
        }

        [TestMethod]
        public void When_a_collection_of_strings_does_not_contain_the_expected_string_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var strings = new[] { "string1", "string2", "string3" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => strings.Should().Contain("string4", "because {0} is required", "4");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected collection {\"string1\", \"string2\", \"string3\"} to contain \"string4\" because 4 is required.");
        }
        
        [TestMethod]
        public void When_a_collection_does_not_contain_the_combination_of_a_collection_and_a_single_item_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var strings = new List<string> { "string1", "string2" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => strings.Should().Contain(strings, "string3");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected collection {\"string1\", \"string2\"} to contain {\"string1\", \"string2\", \"string3\"}, but could not find {\"string3\"}.");
        }

        [TestMethod]
        public void When_asserting_collection_contains_some_values_but_collection_is_null_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            string[] strings = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => strings.Should().Contain("string4", "because we're checking how it reacts to a null subject");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected collection to contain \"string4\" because we're checking how it reacts to a null subject, but found <null>.");
        }

        [TestMethod]
        public void When_asserting_collection_contains_values_according_to_predicate_but_collection_is_null_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            List<string> strings = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => strings.Should().Contain(x => x == "xxx", "because we're checking how it reacts to a null subject");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected collection to contain (x == \"xxx\") because we're checking how it reacts to a null subject, but found <null>.");
        }

        [TestMethod]
        public void When_asserting_collection_doesnt_contain_values_according_to_predicate_but_collection_is_null_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            List<string> strings = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => strings.Should().NotContain(x => x == "xxx", "because we're checking how it reacts to a null subject");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected collection not to contain (x == \"xxx\") because we're checking how it reacts to a null subject, but found <null>.");
        }

        #endregion

        #region Only Contain

        [TestMethod]
        public void When_a_collection_contains_items_not_matching_a_predicate_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var collection = new List<int> { 2, 12, 3, 11, 2 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().OnlyContain(i => i <= 10, "10 is the maximum");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected collection to contain only items matching (i <= 10) because 10 is the maximum, but {12, 11} do(es) not match.");
        }

        [TestMethod]
        public void When_a_collection_is_empty_and_should_contain_only_items_matching_a_predicate_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var strings = new string[0];

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => strings.Should().OnlyContain(e => e.Length > 0);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected collection to contain only items matching (e.Length > 0), but the collection is empty.");
        }
        
        [TestMethod]
        public void When_a_collection_contains_only_items_matching_a_predicate_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var collection = new List<int> { 2, 9, 3, 8, 2};

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().OnlyContain(i => i <= 10);

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow(); ;
        }

        #endregion

        #region Contain Single

        [TestMethod]
        public void When_a_collection_contains_a_single_item_matching_a_predicate_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<int> collection = new[] { 1, 2, 3 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().ContainSingle(item => (item == 2));

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_an_empty_collection_contains_a_single_item_matching_a_predicate_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<int> collection = new int[0];
            Expression<Func<int, bool>> expression = (item => (item == 2));

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().ContainSingle(expression);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            string expectedMessage =
                string.Format("Expected collection to contain a single item matching {0}, " +
                    "but the collection is empty.", expression.Body);

            act.ShouldThrow<AssertFailedException>().WithMessage(expectedMessage);
        }

        [TestMethod]
        public void When_asserting_a_null_collection_contains_a_single_item_matching_a_predicate_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<int> collection = null;
            Expression<Func<int, bool>> expression = (item => (item == 2));

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().ContainSingle(expression);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            string expectedMessage =
                string.Format("Expected collection to contain a single item matching {0}, " +
                    "but found <null>.", expression.Body);

            act.ShouldThrow<AssertFailedException>().WithMessage(expectedMessage);
        }

        [TestMethod]
        public void When_non_empty_collection_does_not_contain_a_single_item_matching_a_predicate_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<int> collection = new[] { 1, 3 };
            Expression<Func<int, bool>> expression = (item => (item == 2));

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().ContainSingle(expression);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            string expectedMessage =
                string.Format("Expected collection to contain a single item matching {0}, " +
                    "but no such item was found.", expression.Body);

            act.ShouldThrow<AssertFailedException>().WithMessage(expectedMessage);
        }

        [TestMethod]
        public void When_non_empty_collection_contains_more_than_a_single_item_matching_a_predicate_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<int> collection = new[] { 1, 2, 2, 2, 3 };
            Expression<Func<int, bool>> expression = (item => (item == 2));

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().ContainSingle(expression);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            string expectedMessage =
                string.Format("Expected collection to contain a single item matching {0}, " +
                    "but 3 such items were found.", expression.Body);

            act.ShouldThrow<AssertFailedException>().WithMessage(expectedMessage);
        }

        #endregion
    }
}
