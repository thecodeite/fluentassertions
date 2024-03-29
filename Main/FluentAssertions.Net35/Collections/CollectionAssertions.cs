﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

using FluentAssertions.Common;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

#if WINRT
using System.Reflection;
#endif

namespace FluentAssertions.Collections
{
    /// <summary>
    /// Contains a number of methods to assert that an <see cref="IEnumerable"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public abstract class CollectionAssertions<TSubject, TAssertions> : ReferenceTypeAssertions<TSubject, TAssertions>
        where TAssertions : CollectionAssertions<TSubject, TAssertions>
        where TSubject : IEnumerable
    {
        /// <summary>
        /// Asserts that the number of items in the collection matches the supplied <paramref name="expected" /> amount.
        /// </summary>
        /// <param name="expected">The expected number of items in the collection.</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<TAssertions> HaveCount(int expected, string reason = "", params object[] reasonArgs)
        {
            if (ReferenceEquals(Subject, null))
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected {context:collection} to contain {0} item(s){reason}, but found <null>.", expected);
            }

            int actualCount = Subject.Cast<object>().Count();

            Execute.Verification
                .ForCondition(actualCount == expected)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected {context:collection} to contain {0} item(s){reason}, but found {1}.", expected, actualCount);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the number of items in the collection matches a condition stated by the <paramref name="countPredicate"/>.
        /// </summary>
        /// <param name="countPredicate">A predicate that yields the number of items that is expected to be in the collection.</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<TAssertions> HaveCount(Expression<Func<int, bool>> countPredicate, string reason = "",
            params object[] reasonArgs)
        {
            if (countPredicate == null)
            {
                throw new NullReferenceException("Cannot compare collection count against a <null> predicate.");
            }

            if (ReferenceEquals(Subject, null))
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected {context:collection} to contain {0} items{reason}, but found {1}.", countPredicate.Body, Subject);
            }

            Func<int, bool> compiledPredicate = countPredicate.Compile();

            int actualCount = Subject.Cast<object>().Count();

            if (!compiledPredicate(actualCount))
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected {context:collection} {0} to have a count {1}{reason}, but count is {2}.",
                        Subject, countPredicate.Body, actualCount);
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the collection does not contain any items.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<TAssertions> BeEmpty(string reason = "", params object[] reasonArgs)
        {
            if (ReferenceEquals(Subject, null))
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected {context:collection} to be empty{reason}, but found {0}.", Subject);
            }

            IEnumerable<object> enumerable = Subject.Cast<object>();

            Execute.Verification
                .ForCondition(!enumerable.Any())
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected {context:collection} to be empty{reason}, but found {0}.", enumerable.Count());

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the collection contains at least 1 item.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<TAssertions> NotBeEmpty(string reason = "", params object[] reasonArgs)
        {
            if (ReferenceEquals(Subject, null))
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected {context:collection} not to be empty{reason}, but found {0}.", Subject);
            }

            IEnumerable<object> enumerable = Subject.Cast<object>();

            Execute.Verification
                .ForCondition(enumerable.Any())
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected {context:collection} not to be empty{reason}.");

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the collection does not contain any duplicate items.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<TAssertions> OnlyHaveUniqueItems(string reason = "", params object[] reasonArgs)
        {
            if (ReferenceEquals(Subject, null))
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected {context:collection} to only have unique items{reason}, but found {0}.", Subject);
            }

            IGrouping<object, object> groupWithMultipleItems = Subject.Cast<object>()
                .GroupBy(o => o)
                .FirstOrDefault(g => g.Count() > 1);

            if (groupWithMultipleItems != null)
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected {context:collection} to only have unique items{reason}, but item {0} is not unique.",
                        groupWithMultipleItems.Key);
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the collection does not contain any <c>null</c> items.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<TAssertions> NotContainNulls(string reason = "", params object[] reasonArgs)
        {
            if (ReferenceEquals(Subject, null))
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected {context:collection} not to contain nulls{reason}, but collection is <null>.");
            }

            object[] values = Subject.Cast<object>().ToArray();
            for (int index = 0; index < values.Length; index++)
            {
                if (ReferenceEquals(values[index], null))
                {
                    Execute.Verification
                        .BecauseOf(reason, reasonArgs)
                        .FailWith("Expected {context:collection} not to contain nulls{reason}, but found one at index {0}.", index);
                }
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Expects the current collection to contain all the same elements in the same order as the collection identified by 
        /// <paramref name="elements" />. Elements are compared using their <see cref="object.Equals(object)" />.
        /// </summary>
        /// <param name="elements">A params array with the expected elements.</param>
        public AndConstraint<TAssertions> Equal(params object[] elements)
        {
            return Equal(elements, String.Empty);
        }

        /// <summary>
        /// Expects the current collection to contain all the same elements in the same order as the collection identified by 
        /// <paramref name="expected" />. Elements are compared using their <see cref="object.Equals(object)" />.
        /// </summary>
        /// <param name="expected">An <see cref="IEnumerable"/> with the expected elements.</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<TAssertions> Equal(IEnumerable expected, string reason = "", params object[] reasonArgs)
        {
            AssertSubjectEquality<object>(expected, (s, e) => s.IsSameOrEqualTo(e), reason, reasonArgs);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        protected void AssertSubjectEquality<T>(IEnumerable expectation, Func<T, T, bool> predicate,
            string reason = "", params object[] reasonArgs)
        {
            Verification verification = Execute.Verification.BecauseOf(reason, reasonArgs);

            if (ReferenceEquals(Subject, null))
            {
                verification.FailWith("Expected {context:collection} to be equal{reason}, but found <null>.");
            }

            if (expectation == null)
            {
                throw new ArgumentNullException("expectation", "Cannot compare collection with <null>.");
            }

            T[] expectedItems = expectation.Cast<T>().ToArray();
            T[] actualItems = Subject.Cast<T>().ToArray();

            AssertCollectionsHaveSameCount(expectedItems, actualItems, verification);

            for (int index = 0; index < expectedItems.Length; index++)
            {
                verification
                    .ForCondition((index < actualItems.Length) && predicate(actualItems[index], expectedItems[index]))
                    .FailWith("Expected {context:collection} to be equal to {0}{reason}, but {1} differs at index {2}.", 
                    expectation, Subject, index);
            }
        }

        private void AssertCollectionsHaveSameCount<T>(T[] expectedItems, T[] actualItems, Verification verification)
        {
            int delta = Math.Abs(expectedItems.Length - actualItems.Length);
            if (delta != 0)
            {
                var expected = (IEnumerable)expectedItems;

                if (actualItems.Length == 0)
                {
                    verification.FailWith("Expected {context:collection} to be equal to {0}{reason}, but found empty collection.", 
                        expected);
                }
                else if (actualItems.Length < expectedItems.Length)
                {
                    verification.FailWith("Expected {context:collection} to be equal to {0}{reason}, but {1} contains {2} item(s) less.", 
                        expected, Subject, delta);
                }
                else if (actualItems.Length > expectedItems.Length)
                {
                    verification.FailWith("Expected {context:collection} to be equal to {0}{reason}, but {1} contains {2} item(s) too many.",
                        expected, Subject, delta);
                }
            }
        }

        /// <summary>
        /// Expects the current collection not to contain all the same elements in the same order as the collection identified by 
        /// <paramref name="unexpected" />. Elements are compared using their <see cref="object.Equals(object)" />.
        /// </summary>
        /// <param name="unexpected">An <see cref="IEnumerable"/> with the elements that are not expected.</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<TAssertions> NotEqual(IEnumerable unexpected, string reason = "", params object[] reasonArgs)
        {
            if (ReferenceEquals(Subject, null))
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected collections not to be equal{reason}, but found <null>.");
            }

            if (unexpected == null)
            {
                throw new ArgumentNullException("unexpected", "Cannot compare collection with <null>.");
            }

            List<object> actualitems = Subject.Cast<object>().ToList();

            if (actualitems.SequenceEqual(unexpected.Cast<object>()))
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Did not expect collections {0} and {1} to be equal{reason}.", unexpected, Subject);
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Expects the current collection to contain all elements of the collection identified by <paramref name="elements" />,
        /// regardless of the order. Elements are compared using their <see cref="object.Equals(object)" />.
        /// </summary>
        /// <param name="elements">A params array with the expected elements.</param>
        public AndConstraint<TAssertions> BeEquivalentTo(params object[] elements)
        {
            return BeEquivalentTo(elements, String.Empty);
        }

        /// <summary>
        /// Expects the current collection to contain all elements of the collection identified by <paramref name="expected" />,
        /// regardless of the order. Elements are compared using their <see cref="object.Equals(object)" />.
        /// </summary>
        /// <param name="expected">An <see cref="IEnumerable"/> with the expected elements.</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<TAssertions> BeEquivalentTo(IEnumerable expected, string reason = "", params object[] reasonArgs)
        {
            if (expected == null)
            {
                throw new NullReferenceException("Cannot verify equivalence against a <null> collection.");
            }

            Execute.Verification
                .ForCondition(!ReferenceEquals(Subject, null))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected {context:collection} to be equivalent to {0}{reason}, but found <null>.", expected);

            List<object> expectedItems = expected.Cast<object>().ToList();
            List<object> actualItems = Subject.Cast<object>().ToList();

            Execute.Verification
                .ForCondition(AreEquivalent(expectedItems, actualItems))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected {context:collection} {0} to be equivalent to {1}{reason}.", actualItems, expectedItems);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Expects the current collection not to contain all elements of the collection identified by <paramref name="unexpected" />,
        /// regardless of the order. Elements are compared using their <see cref="object.Equals(object)" />.
        /// </summary>
        /// <param name="unexpected">An <see cref="IEnumerable"/> with the unexpected elements.</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<TAssertions> NotBeEquivalentTo(IEnumerable unexpected, string reason = "",
            params object[] reasonArgs)
        {
            if (unexpected == null)
            {
                throw new NullReferenceException("Cannot verify inequivalence against a <null> collection.");
            }

            if (ReferenceEquals(Subject, null))
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected {context:collection} not to be equivalent{reason}, but found <null>.");
            }

            if (AreEquivalent(unexpected.Cast<object>(), Subject.Cast<object>()))
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected {context:collection} {0} not be equivalent with collection {1}{reason}.", Subject,
                        unexpected);
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the current collection only contains items that are assignable to the type <typeparamref name="T" />.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<TAssertions> ContainItemsAssignableTo<T>(string reason = "", params object[] reasonArgs)
        {
            if (ReferenceEquals(Subject, null))
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected {context:collection} to contain element assignable to type {0}{reason}, but found <null>.",
                        typeof(T));
            }

            int index = 0;
            foreach (object item in Subject)
            {
#if !WINRT
                if (!typeof(T).IsAssignableFrom(item.GetType()))
#else
                if(!typeof(T).GetTypeInfo().IsAssignableFrom(item.GetType().GetTypeInfo()))
#endif
                {
                    Execute.Verification
                        .BecauseOf(reason, reasonArgs)
                        .FailWith(
                            "Expected {context:collection} to contain only items of type {0}{reason}, but item {1} at index {2} is of type {3}.",
                            typeof(T), item, index, item.GetType());
                }

                ++index;
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        private static bool AreEquivalent(IEnumerable<object> expectedItems, IEnumerable<object> actualItems)
        {
            expectedItems = expectedItems.Distinct().ToArray();
            actualItems = actualItems.Distinct().ToArray();

            int expectedCount = expectedItems.Count();

            return (expectedItems.Intersect(actualItems).Count() == expectedCount) &&
                (expectedCount == actualItems.Count());
        }

        /// <summary>
        /// Expects the current collection to contain the specified elements in any order. Elements are compared
        /// using their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="expected">An <see cref="IEnumerable"/> with the expected elements.</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<TAssertions> Contain(IEnumerable expected, string reason = "", params object[] reasonArgs)
        {
            if (expected == null)
            {
                throw new NullReferenceException("Cannot verify containment against a <null> collection");
            }

            IEnumerable<object> expectedObjects = expected.Cast<object>().ToArray();
            if (!expectedObjects.Any())
            {
                throw new ArgumentException("Cannot verify containment against an empty collection");
            }

            if (ReferenceEquals(Subject, null))
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected {context:collection} to contain {0}{reason}, but found <null>.", expected);
            }

            if (expected is string)
            {
                if (!Subject.Cast<object>().Contains(expected))
                {
                    Execute.Verification
                        .BecauseOf(reason, reasonArgs)
                        .FailWith("Expected {context:collection} {0} to contain {1}{reason}.", Subject, expected);
                }
            }
            else
            {
                IEnumerable<object> missingItems = expectedObjects.Except(Subject.Cast<object>());
                if (missingItems.Any())
                {
                    if (expectedObjects.Count() > 1)
                    {
                        Execute.Verification
                            .BecauseOf(reason, reasonArgs)
                            .FailWith("Expected {context:collection} {0} to contain {1}{reason}, but could not find {2}.", Subject,
                                expected, missingItems);
                    }
                    else
                    {
                        Execute.Verification
                            .BecauseOf(reason, reasonArgs)
                            .FailWith("Expected {context:collection} {0} to contain {1}{reason}.", Subject,
                                expected.Cast<object>().First());
                    }
                }
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Expects the current collection to contain the specified elements in the exact same order. Elements are compared
        /// using their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="expected">An <see cref="IEnumerable"/> with the expected elements.</param>
        public AndConstraint<TAssertions> ContainInOrder(params object[] expected)
        {
            return ContainInOrder(expected, "");
        }

        /// <summary>
        /// Expects the current collection to contain the specified elements in the exact same order. Elements are compared
        /// using their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="expected">An <see cref="IEnumerable"/> with the expected elements.</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<TAssertions> ContainInOrder(IEnumerable expected, string reason = "",
            params object[] reasonArgs)
        {
            if (expected == null)
            {
                throw new NullReferenceException("Cannot verify ordered containment against a <null> collection.");
            }

            if (ReferenceEquals(Subject, null))
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected {context:collection} to contain {0} in order{reason}, but found <null>.", expected);
            }

            object[] expectedItems = expected.Cast<object>().ToArray();
            object[] actualItems = Subject.Cast<object>().ToArray();

            for (int index = 0; index < expectedItems.Length; index++)
            {
                object expectedItem = expectedItems[index];
                actualItems = actualItems.SkipWhile(actualItem => !actualItem.IsSameOrEqualTo(expectedItem)).ToArray();
                if (actualItems.Any())
                {
                    actualItems = actualItems.Skip(1).ToArray();
                }
                else
                {
                    Execute.Verification
                        .BecauseOf(reason, reasonArgs)
                        .FailWith(
                            "Expected {context:collection} {0} to contain items {1} in order{reason}, but {2} (index {3}) did not appear (in the right order).",
                            Subject, expected, expectedItem, index);
                }
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Expects the current collection to have all elements in ascending order. Elements are compared
        /// using their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<TAssertions> BeInAscendingOrder(string reason = "", params object[] reasonArgs)
        {
            return BeInOrder(SortOrder.Ascending, reason, reasonArgs);
        }

        /// <summary>
        /// Expects the current collection to have all elements in descending order. Elements are compared
        /// using their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<TAssertions> BeInDescendingOrder(string reason = "", params object[] reasonArgs)
        {
            return BeInOrder(SortOrder.Descending, reason, reasonArgs);
        }

        /// <summary>
        /// Expects the current collection to have all elements in the specified <paramref name="expectedOrder"/>.
        /// Elements are compared using their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        private AndConstraint<TAssertions> BeInOrder(SortOrder expectedOrder, string reason = "", params object[] reasonArgs)
        {
            string sortOrder = (expectedOrder == SortOrder.Ascending) ? "ascending" : "descending";

            if (ReferenceEquals(Subject, null))
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected {context:collection} to have all items in " + sortOrder + " order{reason}, but found {1}.", Subject);
            }

            object[] orderedItems = (expectedOrder == SortOrder.Ascending)
                ? Subject.Cast<object>().OrderBy(item => item).ToArray()
                : Subject.Cast<object>().OrderByDescending(item => item).ToArray();

            object[] actualItems = Subject.Cast<object>().ToArray();

            for (int index = 0; index < actualItems.Length; index++)
            {
                object orderedItem = orderedItems[index];
                object actualItem = actualItems[index];
                int indexOfActualItem = Array.IndexOf(actualItems, orderedItem);

                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .ForCondition(actualItem.IsSameOrEqualTo(orderedItem))
                    .FailWith("Expected {context:collection} to have all items in " + sortOrder +
                            " order{reason}, but found {0} where item at index {1} is in wrong order.",
                        Subject, indexOfActualItem);
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts the current collection does not have all elements in ascending order. Elements are compared
        /// using their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<TAssertions> NotBeAscendingInOrder(string reason = "", params object[] reasonArgs)
        {
            return NotBeInOrder(SortOrder.Ascending, reason, reasonArgs);
        }

        /// <summary>
        /// Asserts the current collection does not have all elements in descending order. Elements are compared
        /// using their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<TAssertions> NotBeDescendingInOrder(string reason = "", params object[] reasonArgs)
        {
            return NotBeInOrder(SortOrder.Descending, reason, reasonArgs);
        }

        /// <summary>
        /// Asserts the current collection does not have all elements in ascending order. Elements are compared
        /// using their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        private AndConstraint<TAssertions> NotBeInOrder(SortOrder order, string reason = "", params object[] reasonArgs)
        {
            string sortOrder = (order == SortOrder.Ascending) ? "ascending" : "descending";

            if (ReferenceEquals(Subject, null))
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Did not expect {context:collection} to have all items in " + sortOrder + " order{reason}, but found {1}.", Subject);
            }

            object[] orderedItems = (order == SortOrder.Ascending)
                ? Subject.Cast<object>().OrderBy(item => item).ToArray()
                : Subject.Cast<object>().OrderByDescending(item => item).ToArray();

            object[] actualItems = Subject.Cast<object>().ToArray();

            bool itemsAreUnordered = actualItems
                .Where((actualItem, index) => !actualItem.IsSameOrEqualTo(orderedItems[index]))
                .Any();

            if (!itemsAreUnordered)
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Did not expect {context:collection} to have all items in " + sortOrder + " order{reason}, but found {0}.", Subject);
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the collection is a subset of the <paramref name="expectedSuperset" />.
        /// </summary>
        /// <param name="expectedSuperset">An <see cref="IEnumerable"/> with the expected superset.</param>
        /// <param name="reason">        
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<TAssertions> BeSubsetOf(IEnumerable expectedSuperset, string reason = "",
            params object[] reasonArgs)
        {
            if (expectedSuperset == null)
            {
                throw new NullReferenceException("Cannot verify a subset against a <null> collection.");
            }

            if (ReferenceEquals(Subject, null))
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected {context:collection} to be a subset of {0}{reason}, but found {1}.", expectedSuperset,
                        Subject);
            }

            IEnumerable<object> expectedItems = expectedSuperset.Cast<object>();
            IEnumerable<object> actualItems = Subject.Cast<object>();

            IEnumerable<object> excessItems = actualItems.Except(expectedItems);

            if (excessItems.Any())
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith(
                        "Expected {context:collection} to be a subset of {0}{reason}, but items {1} are not part of the superset.",
                        expectedSuperset, excessItems);
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the collection is not a subset of the <paramref name="unexpectedSuperset" />.
        /// </summary>
        /// <param name="unexpectedSuperset">An <see cref="IEnumerable"/> with the unexpected superset.</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<TAssertions> NotBeSubsetOf(IEnumerable unexpectedSuperset, string reason = "",
            params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(!ReferenceEquals(Subject, null))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Cannot assert a <null> collection against a subset.");

            IEnumerable<object> expectedItems = unexpectedSuperset.Cast<object>();
            object[] actualItems = Subject.Cast<object>().ToArray();

            if (actualItems.Intersect(expectedItems).Count() == actualItems.Count())
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Did not expect {context:collection} {0} to be a subset of {1}{reason}.", actualItems, expectedItems);
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Assert that the current collection has the same number of elements as <paramref name="otherCollection" />.
        /// </summary>
        /// <param name="otherCollection">The other collection with the same expected number of elements</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<TAssertions> HaveSameCount(IEnumerable otherCollection, string reason = "",
            params object[] reasonArgs)
        {
            if (otherCollection == null)
            {
                throw new NullReferenceException("Cannot verify count against a <null> collection.");
            }

            if (ReferenceEquals(Subject, null))
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected {context:collection} to have the same count as {0}{reason}, but found {1}.",
                        otherCollection,
                        Subject);
            }

            IEnumerable<object> enumerable = Subject.Cast<object>();

            int actualCount = enumerable.Count();
            int expectedCount = otherCollection.Cast<object>().Count();

            Execute.Verification
                .ForCondition(actualCount == expectedCount)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected {context:collection} to have {0} item(s){reason}, but found {1}.", expectedCount, actualCount);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the current collection has not been initialized yet with an actual collection.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<TAssertions> BeNull(string reason = "", params object[] reasonArgs)
        {
            if (!ReferenceEquals(Subject, null))
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected {context:collection} to be <null>{reason}, but found {0}.", Subject);
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the current collection has been initialized with an actual collection.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<TAssertions> NotBeNull(string reason = "", params object[] reasonArgs)
        {
            if (ReferenceEquals(Subject, null))
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected {context:collection} not to be <null>{reason}.");
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the current collection has the supplied <paramref name="element" /> at the 
        /// supplied <paramref name="index" />.
        /// </summary>
        /// <param name="index">The index where the element is expected</param>
        /// <param name="element">The expected element</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<TAssertions> HaveElementAt(int index, object element, string reason = "",
            params object[] reasonArgs)
        {
            if (ReferenceEquals(Subject, null))
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected {context:collection} to have element at index {0}{reason}, but found {1}.", index, Subject);
            }

            IEnumerable<object> enumerable = Subject.Cast<object>();

            if (index < enumerable.Count())
            {
                object actual = Subject.Cast<object>().ElementAt(index);

                Execute.Verification
                    .ForCondition(actual.IsSameOrEqualTo(element))
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected {0} at index {1}{reason}, but found {2}.", element, index, actual);
            }
            else
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected {0} at index {1}{reason}, but found no element.", element, index);
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the current collection does not contain the supplied <paramref name="unexpected" /> item.
        /// </summary>
        /// <param name="unexpected">The element that is not expected to be in the collection</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<TAssertions> NotContain(object unexpected, string reason = "", params object[] reasonArgs)
        {
            if (ReferenceEquals(Subject, null))
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected {context:collection} not to contain element {0}{reason}, but found {1}.", unexpected,
                        Subject);
            }

            if (Subject.Cast<object>().Contains(unexpected))
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("{context:Collection} {0} should not contain {1}{reason}, but found it anyhow.", Subject, unexpected);
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the collection shares one or more items with the specified <paramref name="otherCollection"/>.
        /// </summary>
        /// <param name="otherCollection">The <see cref="IEnumerable"/> with the expected shared items.</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<TAssertions> IntersectWith(IEnumerable otherCollection, string reason = "",
            params object[] reasonArgs)
        {
            if (otherCollection == null)
            {
                throw new NullReferenceException("Cannot verify intersection against a <null> collection.");
            }

            if (ReferenceEquals(Subject, null))
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected {context:collection} to intersect with {0}{reason}, but found {1}.", otherCollection, Subject);
            }

            IEnumerable<object> otherItems = otherCollection.Cast<object>();
            IEnumerable<object> sharedItems = Subject.Cast<object>().Intersect(otherItems);

            if (!sharedItems.Any())
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith(
                        "Expected {context:collection} to intersect with {0}{reason}, but {1} does not contain any shared items.",
                        otherCollection, Subject);
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the collection does not share any items with the specified <paramref name="otherCollection"/>.
        /// </summary>
        /// <param name="otherCollection">The <see cref="IEnumerable"/> to compare to.</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<TAssertions> NotIntersectWith(IEnumerable otherCollection, string reason = "",
            params object[] reasonArgs)
        {
            if (otherCollection == null)
            {
                throw new NullReferenceException("Cannot verify intersection against a <null> collection.");
            }

            if (ReferenceEquals(Subject, null))
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Did not expect {context:collection} to intersect with {0}{reason}, but found {1}.", otherCollection, Subject);
            }

            IEnumerable<object> otherItems = otherCollection.Cast<object>();
            IEnumerable<object> sharedItems = Subject.Cast<object>().Intersect(otherItems);

            if (sharedItems.Any())
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith(
                        "Did not expect {context:collection} to intersect with {0}{reason}, but found the following shared items {1}.",
                        otherCollection, sharedItems);
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }
    }

    internal enum SortOrder
    {
        Ascending,
        Descending
    }
}