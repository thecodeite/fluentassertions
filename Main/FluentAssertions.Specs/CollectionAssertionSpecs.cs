﻿using System;
using System.Collections;
using System.Collections.Generic;

#if WINRT
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endif

namespace FluentAssertions.Specs
{
    [TestClass]
    public class CollectionAssertionSpecs
    {
        #region Be Null

        [TestMethod]
        public void When_collection_is_expected_to_be_null_and_it_is_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> someCollection = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            someCollection.Should().BeNull();
        }

        [TestMethod]
        public void When_a_custom_enumerable_implementation_is_expected_not_to_be_null_and_it_is_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var enumerable = new CustomEnumerable();

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            enumerable.Should().NotBeNull();
        }

        [TestMethod]
        public void When_collection_is_expected_to_be_null_and_it_isnt_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> someCollection = new string[0];

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => someCollection.Should().BeNull("because {0} is valid", "null");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected collection to be <null> because null is valid, but found {empty}.");
        }

        [TestMethod]
        public void When_collection_is_not_expected_to_be_null_and_it_isnt_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> someCollection = new string[0];

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            someCollection.Should().NotBeNull();
        }

        [TestMethod]
        public void When_collection_is_not_expected_to_be_null_and_it_is_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> someCollection = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => someCollection.Should().NotBeNull("because {0} should not", "someCollection");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected collection not to be <null> because someCollection should not.");
        }

        #endregion

        #region Have Count

        [TestMethod]
        public void Should_succeed_when_asserting_collection_has_a_count_that_equals_the_number_of_items()
        {
            IEnumerable collection = new [] { 1, 2, 3 };
            collection.Should().HaveCount(3);
        }

        [TestMethod]
        public void Should_fail_when_asserting_collection_has_a_count_that_is_different_from_the_number_of_items()
        {
            IEnumerable collection = new [] { 1, 2, 3 };
            Action act = () => collection.Should().HaveCount(4);

            act.ShouldThrow<AssertFailedException>();
        }

        [TestMethod]
        public void When_collection_has_a_count_that_is_different_from_the_number_of_items_it_should_fail_with_descriptive_message_()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection = new[] { 1, 2, 3 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () =>  collection.Should().HaveCount(4, "because we want to test the failure {0}", "message");
            
            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected collection to contain 4 item(s) because we want to test the failure message, but found 3.");
        }

        [TestMethod]
        public void When_collection_has_a_count_larger_than_the_minimum_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var collection = new [] { 1, 2, 3 };

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            collection.Should().HaveCount(c => c >= 3);
        }

        [TestMethod]
        public void When_collection_has_a_count_that_not_matches_the_predicate_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var collection = new [] { 1, 2, 3 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().HaveCount(c => c >= 4, "a minimum of 4 is required");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected collection {1, 2, 3} to have a count (c >= 4) because a minimum of 4 is required, but count is 3.");
        }

        [TestMethod]
        public void When_collection_count_is_matched_against_a_null_predicate_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var collection = new [] { 1, 2, 3 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().HaveCount(null);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<NullReferenceException>().WithMessage(
                "Cannot compare collection count against a <null> predicate.");
        }

        [TestMethod]
        public void When_collection_count_is_matched_and_collection_is_null_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().HaveCount(1, "we want to test the behaviour with a null subject");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected collection to contain 1 item(s) because we want to test the behaviour with a null subject, but found <null>.");
        }

        [TestMethod]
        public void When_collection_count_is_matched_against_a_predicate_and_collection_is_null_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().HaveCount(c => c < 3, "we want to test the behaviour with a null subject");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected collection to contain (c < 3) items because we want to test the behaviour with a null subject, but found <null>.");
        }

        #endregion

        #region Be Empty

        [TestMethod]
        public void Should_succeed_when_asserting_collection_without_items_is_empty()
        {
            IEnumerable collection = new int[0];
            collection.Should().BeEmpty();
        }

        [TestMethod]
        public void Should_fail_when_asserting_collection_with_items_is_empty()
        {
            IEnumerable collection = new [] { 1, 2, 3 };
            Action act = () => collection.Should().BeEmpty();

            act.ShouldThrow<AssertFailedException>();
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_collection_with_items_is_empty()
        {
            IEnumerable collection = new [] { 1, 2, 3 };
            var assertions = collection.Should();
            assertions.Invoking(x => x.BeEmpty("because we want to test the failure {0}", "message"))
                .ShouldThrow<AssertFailedException>()
                .WithMessage("Expected collection to be empty because we want to test the failure message, but found 3.");
        }

        [TestMethod]
        public void When_asserting_collection_with_items_is_not_empty_it_should_succeed()
        {
            IEnumerable collection = new [] { 1, 2, 3 };
            collection.Should().NotBeEmpty();
        }

        [TestMethod]
        public void When_asserting_collection_with_items_is_not_empty_it_should_enumerate_the_collection_only_once()
        {
            var trackingEnumerable = new TrackingTestEnumerable(1, 2, 3);
            trackingEnumerable.Should().NotBeEmpty();

            trackingEnumerable.Enumerator.LoopCount.Should().Be(1);
        }

        [TestMethod]
        public void When_asserting_collection_without_items_is_not_empty_it_should_fail()
        {
            IEnumerable collection = new int[0];
            Action act = () => collection.Should().NotBeEmpty();
            act.ShouldThrow<AssertFailedException>();
        }

        [TestMethod]
        public void When_asserting_collection_without_items_is_not_empty_it_should_fail_with_descriptive_message_()
        {
            IEnumerable collection = new int[0];
            var assertions = collection.Should();
            assertions.Invoking(x => x.NotBeEmpty("because we want to test the failure {0}", "message"))
                .ShouldThrow<AssertFailedException>()
                .WithMessage("Expected collection not to be empty because we want to test the failure message.");
        }

        [TestMethod]
        public void When_asserting_collection_to_be_empty_but_collection_is_null_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().BeEmpty("because we want to test the behaviour with a null subject");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected collection to be empty because we want to test the behaviour with a null subject, but found <null>.");
        }

        #endregion

        #region Not Be Empty

        [TestMethod]
        public void When_asserting_collection_to_be_not_empty_but_collection_is_null_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().NotBeEmpty("because we want to test the behaviour with a null subject");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected collection not to be empty because we want to test the behaviour with a null subject, but found <null>.");
        }

        #endregion

        #region Be Equal

        [TestMethod]
        public void Should_succeed_when_asserting_collection_is_equal_to_the_same_collection()
        {
            IEnumerable collection1 = new [] { 1, 2, 3 };
            IEnumerable collection2 = new [] { 1, 2, 3 };
            collection1.Should().Equal(collection2);
        }

        [TestMethod]
        public void Should_succeed_when_asserting_collection_is_equal_to_the_same_list_of_elements()
        {
            IEnumerable collection = new [] { 1, 2, 3 };
            collection.Should().Equal(1, 2, 3);
        }

        [TestMethod]
        public void When_two_collections_containing_nulls_are_equal_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new List<string> { "aaa", null };
            var expected = new List<string> { "aaa", null };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => subject.Should().Equal(expected);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldNotThrow();
        }

        [TestMethod]
        public void When_two_collections_are_not_equal_because_one_item_differs_it_should_throw_using_the_reason()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection1 = new [] { 1, 2, 3 };
            IEnumerable collection2 = new [] { 1, 2, 5 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection1.Should().Equal(collection2, "because we want to test the failure {0}", "message");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected collection to be equal to {1, 2, 5} because we want to test the failure message, but {1, 2, 3} differs at index 2.");
        }

        [TestMethod]
        public void When_two_collections_are_not_equal_because_the_actual_collection_contains_more_items_it_should_throw_using_the_reason()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection1 = new[] { 1, 2, 3 };
            IEnumerable collection2 = new[] { 1, 2 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection1.Should().Equal(collection2, "because we want to test the failure {0}", "message");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected collection to be equal to {1, 2} because we want to test the failure message, but {1, 2, 3} contains 1 item(s) too many.");
        }

        [TestMethod]
        public void When_two_collections_are_not_equal_because_the_actual_collection_contains_less_items_it_should_throw_using_the_reason()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection1 = new[] { 1, 2, 3 };
            IEnumerable collection2 = new[] { 1, 2, 3, 4 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection1.Should().Equal(collection2, "because we want to test the failure {0}", "message");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected collection to be equal to {1, 2, 3, 4} because we want to test the failure message, but {1, 2, 3} contains 1 item(s) less.");
        }

        [TestMethod]
        public void When_two_multidimensional_collections_are_not_equal_and_it_should_format_the_collections_properly()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection1 = new [] { new []{1, 2}, new[]{3, 4} };
            IEnumerable collection2 = new [] { new []{5, 6}, new[]{7, 8} };
            
            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection1.Should().Equal(collection2);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected collection to be equal to {{5, 6}, {7, 8}}, but {{1, 2}, {3, 4}} differs at index 0.");
        }

        [TestMethod]
        public void When_asserting_collections_to_be_equal_but_subject_collection_is_null_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection = null;
            IEnumerable collection1 = new [] { 1, 2, 3 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().Equal(collection1, "because we want to test the behaviour with a null subject");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected collection to be equal because we want to test the behaviour with a null subject, but found <null>.");
        }

        [TestMethod]
        public void When_asserting_collections_to_be_equal_but_expected_collection_is_null_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection = new [] { 1, 2, 3 };
            IEnumerable collection1 = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().Equal(collection1, "because we want to test the behaviour with a null subject");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<ArgumentNullException>().WithMessage(
                "Cannot compare collection with <null>.\r\nParameter name: expectation");
        }

        [TestMethod]
        public void When_an_empty_collection_is_compared_for_equality_to_a_non_empty_collection_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var collection1 = new int[0];
            var collection2 = new [] { 1, 2, 3 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection1.Should().Equal(collection2);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected collection to be equal to {1, 2, 3}, but found empty collection.");
        }

        [TestMethod] 
        public void When_all_items_match_according_to_a_predicate_it_should_succeed() 
        { 
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
           var actual = new List<string> { "ONE", "TWO", "THREE", "FOUR" }; 
           var expected = new List<string> { "One", "Two", "Three", "Four" }; 

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => actual.Should().Equal(expected,
                (a, e) => string.Equals(a, e, StringComparison.CurrentCultureIgnoreCase));

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldNotThrow();
        }

        [TestMethod]
        public void When_any_item_does_not_match_according_to_a_predicate_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var actual = new List<string> { "ONE", "TWO", "THREE", "FOUR" };
            var expected = new List<string> { "One", "Two", "Three", "Five" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => actual.Should().Equal(expected,
                (a, e) => string.Equals(a, e, StringComparison.CurrentCultureIgnoreCase));

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action
                .ShouldThrow<AssertFailedException>()
                .WithMessage("Expected*equal to*, but*differs at index 3.", ComparisonMode.Wildcard);
        } 

        #endregion

        #region Not Be Equal

        [TestMethod]
        public void Should_succeed_when_asserting_collection_is_not_equal_to_a_different_collection()
        {
            IEnumerable collection1 = new [] { 1, 2, 3 };
            IEnumerable collection2 = new [] { 3, 1, 2 };
            collection1.Should()
                .NotEqual(collection2);
        }

        [TestMethod]
        public void When_two_equal_collections_are_not_expected_to_be_equal_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection1 = new [] { 1, 2, 3 };
            IEnumerable collection2 = new [] { 1, 2, 3 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection1.Should().NotEqual(collection2);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Did not expect collections {1, 2, 3} and {1, 2, 3} to be equal.");
        }

        [TestMethod]
        public void When_two_equal_collections_are_not_expected_to_be_equal_it_should_report_a_clear_explanation()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection1 = new [] { 1, 2, 3 };
            IEnumerable collection2 = new [] { 1, 2, 3 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection1.Should().NotEqual(collection2, "because we want to test the failure {0}", "message");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Did not expect collections {1, 2, 3} and {1, 2, 3} to be equal because we want to test the failure message.");
        }


        [TestMethod]
        public void When_asserting_collections_not_to_be_equal_subject_but_collection_is_null_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection = null;
            IEnumerable collection1 = new [] { 1, 2, 3 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act =
                () => collection.Should().NotEqual(collection1, "because we want to test the behaviour with a null subject");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected collections not to be equal because we want to test the behaviour with a null subject, but found <null>.");
        }

        [TestMethod]
        public void When_asserting_collections_not_to_be_equal_but_expected_collection_is_null_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection = new [] { 1, 2, 3 };
            IEnumerable collection1 = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act =
                () => collection.Should().NotEqual(collection1, "because we want to test the behaviour with a null subject");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<ArgumentNullException>().WithMessage(
                "Cannot compare collection with <null>.\r\nParameter name: unexpected");
        }

        #endregion

        #region Be Equivalent To

        [TestMethod]
        public void When_two_collections_contain_the_same_elements_it_should_treat_them_as_equivalent()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection1 = new[] { 1, 2, 3 };
            IEnumerable collection2 = new[] { 3, 1, 2 };

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            collection1.Should().BeEquivalentTo(collection2);
        }

        [TestMethod]
        public void When_a_collection_contain_same_elements_it_should_treat_it_as_equivalent()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection = new[] { 1, 2, 3 };

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            collection.Should().BeEquivalentTo(3, 1, 2);
        }

        [TestMethod]
        public void When_character_collections_are_equivalent_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            char[] list1 = ("abc123ab").ToCharArray();
            char[] list2 = ("abc123ab").ToCharArray();

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            list1.Should().BeEquivalentTo(list2);
        }

        [TestMethod]
        public void When_collections_are_not_equivalent_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection1 = new [] { 1, 2, 3 };
            IEnumerable collection2 = new [] { 1, 2 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection1.Should().BeEquivalentTo(collection2, "we treat {0} alike", "all");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected collection {1, 2, 3} to be equivalent to {1, 2} because we treat all alike.");
        }

        [TestMethod]
        public void When_testing_for_equivalence_against_empty_collection_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable subject = new [] { 1, 2, 3 };
            IEnumerable otherCollection = new int[0];

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.Should().BeEquivalentTo(otherCollection);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected collection {1, 2, 3} to be equivalent to {empty}.");
        }
        
        [TestMethod]
        public void When_two_collections_are_both_empty_it_should_treat_them_as_equivalent()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable subject = new int[0];
            IEnumerable otherCollection = new int[0];

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.Should().BeEquivalentTo(otherCollection);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_testing_for_equivalence_against_null_collection_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection1 = new [] { 1, 2, 3 };
            IEnumerable collection2 = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection1.Should().BeEquivalentTo(collection2);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<NullReferenceException>().WithMessage(
                "Cannot verify equivalence against a <null> collection.");
        }

        [TestMethod]
        public void When_asserting_collections_to_be_equivalent_but_subject_collection_is_null_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection = null;
            IEnumerable collection1 = new [] { 1, 2, 3 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act =
                () => collection.Should().BeEquivalentTo(collection1, "because we want to test the behaviour with a null subject");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected collection to be equivalent to {1, 2, 3} because we want to test the behaviour with a null subject, but found <null>.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_collection_is_not_equivalent_to_a_different_collection()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection1 = new [] { 1, 2, 3 };
            IEnumerable collection2 = new [] { 3, 1 };

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            collection1.Should().NotBeEquivalentTo(collection2);
        }

        [TestMethod]
        public void When_collections_are_unexpectingly_equivalent_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection1 = new [] { 1, 2, 3 };
            IEnumerable collection2 = new [] { 3, 1, 2 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection1.Should().NotBeEquivalentTo(collection2);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected collection {1, 2, 3} not be equivalent with collection {3, 1, 2}.");
        }

        [TestMethod]
        public void When_asserting_collections_not_to_be_equivalent_but_subject_collection_is_null_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection = null;
            IEnumerable collection1 = new [] { 1, 2, 3 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act =
                () =>
                    collection.Should().NotBeEquivalentTo(collection1, "because we want to test the behaviour with a null subject");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected collection not to be equivalent because we want to test the behaviour with a null subject, but found <null>.");
        }

        [TestMethod]
        public void When_non_empty_collection_is_not_expected_to_be_equivalent_to_an_empty_collection_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection1 = new [] { 1, 2, 3 };
            IEnumerable collection2 = new int[0];

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection1.Should().NotBeEquivalentTo(collection2);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_testing_collections_not_to_be_equivalent_against_null_collection_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection1 = new [] { 1, 2, 3 };
            IEnumerable collection2 = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection1.Should().NotBeEquivalentTo(collection2);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<NullReferenceException>().WithMessage(
                "Cannot verify inequivalence against a <null> collection.");
        }

        #endregion

        #region Be Subset Of

        [TestMethod]
        public void When_collection_is_subset_of_a_specified_collection_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable subset = new [] { 1, 2 };
            IEnumerable superset = new [] { 1, 2, 3 };

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            subset.Should().BeSubsetOf(superset);
        }

        [TestMethod]
        public void When_collection_is_not_a_subset_of_another_it_should_throw_with_the_reason()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable subset = new [] { 1, 2, 3, 6 };
            IEnumerable superset = new [] { 1, 2, 4, 5 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subset.Should().BeSubsetOf(superset, "because we want to test the failure {0}", "message");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected collection to be a subset of {1, 2, 4, 5} because we want to test the failure message, " +
                    "but items {3, 6} are not part of the superset.");
        }

        [TestMethod]
        public void When_an_empty_collection_is_tested_against_a_superset_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable subset = new int[0];
            IEnumerable superset = new [] { 1, 2, 4, 5 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subset.Should().BeSubsetOf(superset);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_a_subset_is_tested_against_a_null_superset_it_should_throw_with_a_clear_explanation()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable subset = new [] { 1, 2, 3 };
            IEnumerable superset = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subset.Should().BeSubsetOf(superset);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<NullReferenceException>().WithMessage(
                "Cannot verify a subset against a <null> collection.");
        }

        [TestMethod]
        public void When_a_set_is_expected_to_be_not_a_subset_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable subject = new [] { 1, 2, 4 };
            IEnumerable otherSet = new [] { 1, 2, 3 };

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            subject.Should().NotBeSubsetOf(otherSet);
        }
        
        [TestMethod]
        public void When_an_empty_set_is_not_supposed_to_be_a_subset_of_another_set_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable subject = new int[] {};
            IEnumerable otherSet = new [] { 1, 2, 3 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.Should().NotBeSubsetOf(otherSet);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Did not expect collection {empty} to be a subset of {1, 2, 3}.");
        }

        [TestMethod]
        public void Should_fail_when_asserting_collection_is_not_subset_of_a_superset_collection()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable subject = new [] { 1, 2 };
            IEnumerable otherSet = new [] { 1, 2, 3 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.Should().NotBeSubsetOf(otherSet, "because I'm {0}", "mistaken");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Did not expect collection {1, 2} to be a subset of {1, 2, 3} because I'm mistaken.");
        }

        [TestMethod]
        public void When_asserting_collection_to_be_subset_against_null_collection_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection = null;
            IEnumerable collection1 = new [] { 1, 2, 3 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act =
                () => collection.Should().BeSubsetOf(collection1, "because we want to test the behaviour with a null subject");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected collection to be a subset of {1, 2, 3} because we want to test the behaviour with a null subject, but found <null>.");
        }

        #endregion

        #region Contain

        [TestMethod]
        public void Should_succeed_when_asserting_collection_contains_an_item_from_the_collection()
        {
            IEnumerable collection = new [] { 1, 2, 3 };
            collection.Should().Contain(1);
        }

        [TestMethod]
        public void Should_succeed_when_asserting_collection_contains_multiple_items_from_the_collection_in_any_order()
        {
            IEnumerable collection = new [] { 1, 2, 3 };
            collection.Should().Contain(new [] { 2, 1 });
        }

        [TestMethod]
        public void When_a_collection_does_not_contain_single_item_it_should_throw_with_clear_explanation()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection = new [] { 1, 2, 3 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().Contain(4, "because {0}", "we do");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected collection {1, 2, 3} to contain 4 because we do.");
        }

        [TestMethod]
        public void When_a_collection_does_not_contain_another_collection_it_should_throw_with_clear_explanation()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection = new [] { 1, 2, 3 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().Contain(new [] { 3, 4, 5 }, "because {0}", "we do");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected collection {1, 2, 3} to contain {3, 4, 5} because we do, but could not find {4, 5}.");
        }

        [TestMethod]
        public void When_the_contents_of_a_collection_are_checked_against_an_empty_collection_it_should_throw_clear_explanation()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection = new [] { 1, 2, 3 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().Contain(new int[0]);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<ArgumentException>().WithMessage(
                "Cannot verify containment against an empty collection");
        }

        #endregion

        #region Not Contain

        [TestMethod]
        public void Should_succeed_when_asserting_collection_does_not_contain_an_item_that_is_not_in_the_collection()
        {
            IEnumerable collection = new [] { 1, 2, 3 };
            collection.Should().NotContain(4);
        }

        [TestMethod]
        public void When_collection_contains_an_unexpected_item_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection = new [] { 1, 2, 3 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().NotContain(1, "because we {0} like it", "don't");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Collection {1, 2, 3} should not contain 1 because we don't like it, but found it anyhow.");
        }

        [TestMethod]
        public void When_collection_does_contain_an_unexpected_item_matching_a_predicate_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<int> collection = new [] { 1, 2, 3 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().NotContain(item => item == 2, "because {0}s are evil", 2);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Collection {1, 2, 3} should not have any items matching (item == 2) because 2s are evil.");
        }

        [TestMethod]
        public void When_collection_does_not_contain_an_unexpected_item_matching_a_predicate_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<int> collection = new [] { 1, 2, 3 };

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            collection.Should().NotContain(item => item == 4);
        }

        [TestMethod]
        public void When_asserting_collection_does_not_contain_item_against_null_collection_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should()
                .NotContain(1, "because we want to test the behaviour with a null subject");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected collection not to contain element 1 because we want to test the behaviour with a null subject, but found <null>.");
        }

        #endregion

        #region Contain In Order

        [TestMethod]
        public void When_two_collections_contain_the_same_items_in_the_same_order_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection = new [] { 1, 2, 2, 3 };

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            collection.Should().ContainInOrder(1, 2, 3);
        }

        [TestMethod]
        public void When_collection_contains_null_value_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection = new object[] { 1, null, 2, "string" };

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            collection.Should().ContainInOrder(new object[] { 1, null, "string" });
        }

        [TestMethod]
        public void When_the_first_collection_contains_a_duplicate_item_without_affecting_the_order_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection = new [] { 1, 2, 3, 2 };

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            collection.Should().ContainInOrder(1, 2, 3);
        }

        [TestMethod]
        public void When_two_collections_contain_the_same_duplicate_items_in_the_same_order_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection = new [] { 1, 2, 1, 2, 12, 2, 2 };

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            collection.Should().ContainInOrder(1, 2, 1, 2, 12, 2, 2 );
        }

        [TestMethod]
        public void When_a_collection_does_not_contain_a_range_twice_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection = new [] { 1, 2, 1, 3, 12, 2, 2 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().ContainInOrder( 1, 2, 1, 1, 2);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected collection {1, 2, 1, 3, 12, 2, 2} to contain items {1, 2, 1, 1, 2} in order, but 1 (index 3) did not appear (in the right order).");
        }

        [TestMethod]
        public void When_two_collections_contain_the_same_items_but_in_different_order_it_should_throw_with_a_clear_explanation()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => new [] { 1, 2, 3 }.Should().ContainInOrder(new [] { 3, 1 }, "because we said so");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected collection {1, 2, 3} to contain items {3, 1} in order because we said so, but 1 (index 1) did not appear (in the right order).");
        }

        [TestMethod]
        public void When_a_collection_does_not_contain_an_ordered_item_it_should_throw_with_a_clear_explanation()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => new [] { 1, 2, 3 }.Should().ContainInOrder(new [] { 4, 1 }, "we failed");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected collection {1, 2, 3} to contain items {4, 1} in order because we failed, " +
                    "but 4 (index 0) did not appear (in the right order).");
        }

        [TestMethod]
        public void When_passing_in_null_while_checking_for_ordered_containment_it_should_throw_with_a_clear_explanation()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => new [] { 1, 2, 3 }.Should().ContainInOrder(null);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<NullReferenceException>().WithMessage(
                "Cannot verify ordered containment against a <null> collection.");
        }

        [TestMethod]
        public void When_asserting_collection_contains_some_values_in_order_but_collection_is_null_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            string [] strings = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act =
                () => strings.Should().ContainInOrder("string4", "because we're checking how it reacts to a null subject");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected collection to contain \"string4\" in order because we're checking how it reacts to a null subject, but found <null>.");
        }

        #endregion

        #region (Not) be in order

        [TestMethod]
        public void When_asserting_the_items_in_an_ascendingly_ordered_collection_are_ordered_ascending_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------      
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection = new[] { 1, 2, 2, 3 };

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            collection.Should().BeInAscendingOrder();
        }

        [TestMethod]
        public void When_asserting_the_items_in_an_unordered_collection_are_ordered_ascending_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection = new[] { 1, 5, 3 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => collection.Should().BeInAscendingOrder("because numbers are ordered");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected collection to have all items in ascending order because numbers are ordered," +
                    " but found {1, 5, 3} where item at index 2 is in wrong order."); ;
        }

        [TestMethod]
        public void When_asserting_the_items_in_an_unordered_collection_are_not_in_ascending_order_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection = new[] { 1, 5, 3 };

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            collection.Should().NotBeAscendingInOrder();
        }

        [TestMethod]
        public void When_asserting_the_items_in_an_ascendingly_ordered_collection_are_not_in_ascending_order_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection = new[] { 1, 2, 2, 3 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => collection.Should().NotBeAscendingInOrder("because numbers are not ordered");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>()
                .WithMessage("Did not expect collection to have all items in ascending order because numbers are not ordered," +
                    " but found {1, 2, 2, 3}.");
        }

        [TestMethod]
        public void When_asserting_the_items_in_an_descendingly_ordered_collection_are_ordered_descending_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------      
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection = new[] { "z", "y", "x" };

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            collection.Should().BeInDescendingOrder();
        }

        [TestMethod]
        public void When_asserting_the_items_in_an_unordered_collection_are_ordered_descending_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection = new[] { "z", "x", "y" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => collection.Should().BeInDescendingOrder("because letters are ordered");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected collection to have all items in descending order because letters are ordered," +
                    " but found {\"z\", \"x\", \"y\"} where item at index 2 is in wrong order.");
        }

        [TestMethod]
        public void When_asserting_the_items_in_an_unordered_collection_are_not_in_descending_order_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection = new[] { "x", "y", "x" };

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            collection.Should().NotBeDescendingInOrder();
        }

        [TestMethod]
        public void When_asserting_the_items_in_a_descending_ordered_collection_are_not_in_descending_order_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection = new[] { "c", "b", "a" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => collection.Should().NotBeDescendingInOrder("because numbers are not ordered");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>()
                .WithMessage("Did not expect collection to have all items in descending order because numbers are not ordered," +
                    " but found {\"c\", \"b\", \"a\"}.");
        }

        #endregion

        #region (Not) Intersect

        [TestMethod]
        public void When_asserting_the_items_in_an_two_intersecting_collections_intersect_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------      
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection = new[] { 1, 2, 3 };
            IEnumerable otherCollection = new[] { 3, 4, 5 };

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            collection.Should().IntersectWith(otherCollection);
        }

        [TestMethod]
        public void When_asserting_the_items_in_an_two_non_intersecting_collections_intersect_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------      
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection = new[] { 1, 2, 3 };
            IEnumerable otherCollection = new[] { 4, 5 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => collection.Should().IntersectWith(otherCollection, "they should share items");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected collection to intersect with {4, 5} because they should share items," +
                    " but {1, 2, 3} does not contain any shared items.");
        }

        [TestMethod]
        public void When_asserting_the_items_in_an_two_non_intersecting_collections_do_not_intersect_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------      
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection = new[] { 1, 2, 3 };
            IEnumerable otherCollection = new[] { 4, 5 };

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            collection.Should().NotIntersectWith(otherCollection);
        }

        [TestMethod]
        public void When_asserting_the_items_in_an_two_intersecting_collections_do_not_intersect_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------      
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection = new[] { 1, 2, 3 };
            IEnumerable otherCollection = new[] { 2, 3, 4 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => collection.Should().NotIntersectWith(otherCollection, "they should not share items");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>()
                .WithMessage("Did not expect collection to intersect with {2, 3, 4} because they should not share items," +
                    " but found the following shared items {2, 3}.");
        }

        #endregion

        #region Not Contain Nulls

        [TestMethod]
        public void When_collection_does_not_contain_nulls_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection = new [] { 1, 2, 3 };

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            collection.Should().NotContainNulls();
        }

        [TestMethod]
        public void When_collection_contains_nulls_that_are_unexpected_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection = new [] { new object(), null };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().NotContainNulls("because they are {0}", "evil");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected collection not to contain nulls because they are evil, but found one at index 1.");
        }

        [TestMethod]
        public void When_asserting_collection_to_not_contain_nulls_but_collection_is_null_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().NotContainNulls("because we want to test the behaviour with a null subject");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected collection not to contain nulls because we want to test the behaviour with a null subject, but collection is <null>.");
        }

        #endregion

        #region Contain Items Assignable To

        [TestMethod]
        public void Should_succeed_when_asserting_collection_with_all_items_of_same_type_only_contains_item_of_one_type()
        {
            IEnumerable collection = new [] { "1", "2", "3" };
            collection.Should().ContainItemsAssignableTo<string>();
        }

        [TestMethod]
        public void Should_fail_when_asserting_collection_with_items_of_different_types_only_contains_item_of_one_type()
        {
            IEnumerable collection = new List<object>
            {
                1,
                "2"
            };
            
            Action act = () => collection.Should().ContainItemsAssignableTo<string>();

            act.ShouldThrow<AssertFailedException>();
        }

        [TestMethod]
        public void When_a_collection_contains_anything_other_than_strings_it_should_throw_and_report_details()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection = new List<object>
            {
                1,
                "2"
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().ContainItemsAssignableTo<string>();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected collection to contain only items of type System.String, but item 1 at index 0 is of type System.Int32.");
        }

        [TestMethod]
        public void When_a_collection_contains_anything_other_than_strings_it_should_use_the_reason()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection = new List<object>
            {
                1,
                "2"
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().ContainItemsAssignableTo<string>(
                "because we want to test the failure {0}", "message");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "Expected collection to contain only items of type System.String because we want to test the failure message" +
                        ", but item 1 at index 0 is of type System.Int32.");
        }

        [TestMethod]
        public void When_asserting_collection_contains_item_assignable_to_against_null_collection_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should()
                .ContainItemsAssignableTo<string>("because we want to test the behaviour with a null subject");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected collection to contain element assignable to type System.String because we want to test the behaviour with a null subject, but found <null>.");
        }

        #endregion

        #region Only Have Unique Items

        [TestMethod]
        public void Should_succeed_when_asserting_collection_with_unique_items_contains_only_unique_items()
        {
            IEnumerable collection = new [] { 1, 2, 3, 4 };
            collection.Should().OnlyHaveUniqueItems();
        }

        [TestMethod]
        public void When_a_collection_contains_duplicate_items_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection = new [] { 1, 2, 3, 3 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().OnlyHaveUniqueItems("{0} don't like {1}", "we", "duplicates");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected collection to only have unique items because we don't like duplicates, but item 3 is not unique.");
        }

        [TestMethod]
        public void When_asserting_collection_to_only_have_unique_items_but_collection_is_null_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act =
                () => collection.Should().OnlyHaveUniqueItems("because we want to test the behaviour with a null subject");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected collection to only have unique items because we want to test the behaviour with a null subject, but found <null>.");
        }

        #endregion

        #region Have Element At

        [TestMethod]
        public void When_collection_has_expected_element_at_specific_index_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection = new [] { 1, 2, 3 };

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            collection.Should().HaveElementAt(1, 2);
        }

        [TestMethod]
        public void When_collection_does_not_have_the_expected_element_at_specific_index_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection = new [] { 1, 2, 3 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().HaveElementAt(1, 3, "we put it {0}", "there");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected 3 at index 1 because we put it there, but found 2.");
        }

        [TestMethod]
        public void When_collection_does_not_have_an_element_at_the_specific_index_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection = new [] { 1, 2, 3 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().HaveElementAt(4, 3, "we put it {0}", "there");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------            
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected 3 at index 4 because we put it there, but found no element.");
        }

        [TestMethod]
        public void When_asserting_collection_has_element_at_specific_index_against_null_collection_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().HaveElementAt(1, 1,
                "because we want to test the behaviour with a null subject");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected collection to have element at index 1 because we want to test the behaviour with a null subject, but found <null>.");
        }

        #endregion

        #region Miscellaneous

        [TestMethod]
        public void Should_support_chaining_constraints_with_and()
        {
            IEnumerable collection = new [] { 1, 2, 3 };
            collection.Should()
                .HaveCount(3)
                .And
                .HaveElementAt(1, 2)
                .And.NotContain(4);
        }

        #endregion

        #region Have Same Count

        [TestMethod]
        public void When_both_collections_have_the_same_number_elements_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            IEnumerable firstCollection = new [] { 1, 2, 3 };
            IEnumerable secondCollection = new [] { 4, 5, 6 };

            var extensions = firstCollection.Should();

            //-------------------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-------------------------------------------------------------------------------------------------------------------
            extensions.HaveSameCount(secondCollection);
        }

        [TestMethod]
        public void When_both_collections_do_not_have_the_same_number_of_elements_it_should_fail()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            IEnumerable firstCollection = new [] { 1, 2, 3 };
            IEnumerable secondCollection = new [] { 4, 6 };

            var extensions = firstCollection.Should();

            //-------------------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-------------------------------------------------------------------------------------------------------------------
            extensions
                .Invoking(e => e.HaveSameCount(secondCollection))
                .ShouldThrow<AssertFailedException>()
                .WithMessage("Expected collection to have 2 item(s), but found 3.");
        }

        [TestMethod]
        public void When_comparing_item_counts_and_a_reason_is_specified_it_should_it_in_the_exception()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            IEnumerable firstCollection = new [] { 1, 2, 3 };
            IEnumerable secondCollection = new [] { 4, 6 };

            var extensions = firstCollection.Should();

            //-------------------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-------------------------------------------------------------------------------------------------------------------
            extensions
                .Invoking(e => e.HaveSameCount(secondCollection, "we want to test the {0}", "reason"))
                .ShouldThrow<AssertFailedException>()
                .WithMessage("Expected collection to have 2 item(s) because we want to test the reason, but found 3.");
        }

        [TestMethod]
        public void When_asserting_collections_to_have_same_count_against_null_collection_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection = null;
            IEnumerable collection1 = new [] { 1, 2, 3 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().HaveSameCount(collection1,
                "because we want to test the behaviour with a null subject");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected collection to have the same count as {1, 2, 3} because we want to test the behaviour with a null subject, but found <null>.");
        }

        [TestMethod]
        public void When_asserting_collections_to_have_same_count_against_an_other_null_collection_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection = new [] { 1, 2, 3 };
            IEnumerable otherCollection = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().HaveSameCount(otherCollection);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<NullReferenceException>().WithMessage(
                "Cannot verify count against a <null> collection.");
        }

        #endregion
    }

    internal class CustomEnumerable : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            foreach (string s in new [] { "a", "b", "c" })
            {
                yield return s;
            }
        }
    }

    internal class TrackingTestEnumerable : IEnumerable
    {
        private readonly TrackingEnumerator enumerator;
        private readonly int[] values;

        public TrackingTestEnumerable(params int[] values)
        {
            this.values = values;
            enumerator = new TrackingEnumerator(this.values);
        }

        public TrackingEnumerator Enumerator
        {
            get { return enumerator; }
        }

        public IEnumerator GetEnumerator()
        {
            enumerator.IncreaseEnumerationCount();
            enumerator.Reset();
            return enumerator;
        }
    }

    internal class TrackingEnumerator : IEnumerator
    {
        private readonly int[] values;
        private int loopCount;
        private int index;

        public TrackingEnumerator(int[] values)
        {
            index = -1;

            this.values = values;
        }

        public int LoopCount
        {
            get { return loopCount; }
        }

        public void IncreaseEnumerationCount()
        {
            loopCount++;
        }

        public bool MoveNext()
        {
            index++;
            return index < values.Length;
        }

        public void Reset()
        {
            index = -1;
        }

        public object Current
        {
            get { return values[index]; }
        }
    }
}