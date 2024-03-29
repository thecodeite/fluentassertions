using System;
using System.Diagnostics;
using FluentAssertions.Execution;

namespace FluentAssertions.Numeric
{
    /// <summary>
    /// Contains a number of methods to assert that an <see cref="IComparable{T}"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class ComparableTypeAssertions<T>
    {
        private const int Equal = 0;

        protected internal ComparableTypeAssertions(IComparable<T> value)
        {
            Subject = value;
        }

        /// <summary>
        /// Gets the object which value is being asserted.
        /// </summary>
        public IComparable<T> Subject { get; private set; }

        /// <summary>
        /// Asserts that the subject is considered equal to another object according to the implementation of <see cref="IComparable{T}"/>.
        /// </summary>
        /// <param name="expected">
        /// The object to pass to the subject's <see cref="IComparable{T}.CompareTo"/> method.
        /// </param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason"/>.
        /// </param>
        public AndConstraint<ComparableTypeAssertions<T>> Be(T expected, string reason = "", params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(ReferenceEquals(Subject, expected) || (Subject.CompareTo(expected) == Equal))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected {0}{reason}, but found {1}.", expected, Subject);

            return new AndConstraint<ComparableTypeAssertions<T>>(this);
        }

        /// <summary>
        /// Asserts that the subject is not equal to another object according to its implementation of <see cref="IComparable{T}"/>.
        /// </summary>
        /// <param name="expected">
        /// The object to pass to the subject's <see cref="IComparable{T}.CompareTo"/> method.
        /// </param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason"/>.
        /// </param>
        public AndConstraint<ComparableTypeAssertions<T>> NotBe(T expected, string reason = "", params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.CompareTo(expected) != Equal)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Did not expect object to be equal to {0}{reason}.", expected);

            return new AndConstraint<ComparableTypeAssertions<T>>(this);
        }

        /// <summary>
        /// Asserts that the subject is less than another object according to its implementation of <see cref="IComparable{T}"/>.
        /// </summary>
        /// <param name="expected">
        /// The object to pass to the subject's <see cref="IComparable{T}.CompareTo"/> method.
        /// </param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason"/>.
        /// </param>
        public AndConstraint<ComparableTypeAssertions<T>> BeLessThan(T expected, string reason = "", params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.CompareTo(expected) < Equal)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected object {0} to be less than {1}{reason}.", Subject, expected);

            return new AndConstraint<ComparableTypeAssertions<T>>(this);
        }

        /// <summary>
        /// Asserts that the subject is less than or equal to another object according to its implementation of <see cref="IComparable{T}"/>.
        /// </summary>
        /// <param name="expected">
        /// The object to pass to the subject's <see cref="IComparable{T}.CompareTo"/> method.
        /// </param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason"/>.
        /// </param>
        public AndConstraint<ComparableTypeAssertions<T>> BeLessOrEqualTo(T expected, string reason = "", params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.CompareTo(expected) <= Equal)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected object {0} to be less or equal to {1}{reason}.", Subject, expected);

            return new AndConstraint<ComparableTypeAssertions<T>>(this);
        }

        /// <summary>
        /// Asserts that the subject is greater than another object according to its implementation of <see cref="IComparable{T}"/>.
        /// </summary>
        /// <param name="expected">
        /// The object to pass to the subject's <see cref="IComparable{T}.CompareTo"/> method.
        /// </param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason"/>.
        /// </param>
        public AndConstraint<ComparableTypeAssertions<T>> BeGreaterThan(T expected, string reason = "", params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.CompareTo(expected) > Equal)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected object {0} to be greater than {1}{reason}.", Subject, expected);

            return new AndConstraint<ComparableTypeAssertions<T>>(this);
        }

        /// <summary>
        /// Asserts that the subject is greater than or equal to another object according to its implementation of <see cref="IComparable{T}"/>.
        /// </summary>
        /// <param name="expected">
        /// The object to pass to the subject's <see cref="IComparable{T}.CompareTo"/> method.
        /// </param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason"/>.
        /// </param>
        public AndConstraint<ComparableTypeAssertions<T>> BeGreaterOrEqualTo(T expected, string reason = "", params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.CompareTo(expected) >= Equal)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected object {0} to be greater or equal to {1}{reason}.", Subject, expected);

            return new AndConstraint<ComparableTypeAssertions<T>>(this);
        }

        /// <summary>
        /// Asserts that a value is within a range.
        /// </summary>
        /// <remarks>
        /// Where the range is continuous or incremental depends on the actual type of the value. 
        /// </remarks>
        /// <param name="minimumValue">
        /// The minimum valid value of the range.
        /// </param>
        /// <param name="maximumValue">
        /// The maximum valid value of the range.
        /// </param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason"/>.
        /// </param>
        public AndConstraint<ComparableTypeAssertions<T>> BeInRange(T minimumValue, T maximumValue, string reason = "",
            params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition((Subject.CompareTo(minimumValue) >= Equal) && (Subject.CompareTo(maximumValue) <= Equal))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected object to be between {0} and {1}{reason}, but found {2}.",
                    minimumValue, maximumValue, Subject);

            return new AndConstraint<ComparableTypeAssertions<T>>(this);
        }

        /// <summary>
        /// Asserts that a nullable numeric value is not <c>null</c>.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason"/>.
        /// </param>      
        public AndConstraint<ComparableTypeAssertions<T>> NotBeNull(string reason = "", params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(!ReferenceEquals(Subject, null))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected a value{reason}, but found {0}.", Subject);

            return new AndConstraint<ComparableTypeAssertions<T>>(this);
        }

        /// <summary>
        /// Asserts that a nullable numeric value is <c>null</c>.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason"/>.
        /// </param>  
        public AndConstraint<ComparableTypeAssertions<T>> BeNull(string reason = "", params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(ReferenceEquals(Subject, null))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Did not expect a value{reason}, but found {0}.", Subject);

            return new AndConstraint<ComparableTypeAssertions<T>>(this);
        }
    }
}