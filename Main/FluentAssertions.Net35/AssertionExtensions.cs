﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Linq;
using FluentAssertions.Collections;
using FluentAssertions.Equivalency;
using FluentAssertions.Formatting;
using FluentAssertions.Numeric;
using FluentAssertions.Primitives;
using FluentAssertions.Specialized;
using FluentAssertions.Types;
using FluentAssertions.Xml;

#if NET45 || WINRT
using System.Threading.Tasks;
#endif

namespace FluentAssertions
{
    /// <summary>
    /// Contains extension methods for custom assertions in unit tests.
    /// </summary>
    [DebuggerNonUserCode]
    public static class AssertionExtensions
    {
        /// <summary>
        /// Invokes the specified action on an subject so that you can chain it with any of the ShouldThrow or ShouldNotThrow 
        /// overloads.
        /// </summary>
        public static Action Invoking<T>(this T subject, Action<T> action)
        {
            return () => action(subject);
        }

#if !SILVERLIGHT

        /// <summary>
        /// Provides methods for asserting the execution time of a method or property.
        /// </summary>
        /// <param name="subject">The object that exposes the method or property.</param>
        /// <param name="action">A reference to the method or property to measure the execution time of.</param>
        /// <returns>
        /// Returns an object for asserting that the execution time matches certain conditions.
        /// </returns>
        public static MemberExecutionTimeAssertions<T> ExecutionTimeOf<T>(this T subject, Expression<Action<T>> action)
        {
            return new MemberExecutionTimeAssertions<T>(subject, action);
        }

        /// <summary>
        /// Provides methods for asserting the execution time of a method or property.
        /// </summary>
        /// <param name="action">A reference to the method or property to measure the execution time of.</param>
        /// <returns>
        /// Returns an object for asserting that the execution time matches certain conditions.
        /// </returns>
        public static ExecutionTimeAssertions ExecutionTime(this Action action)
        {
            return new ExecutionTimeAssertions(action);
        }

#endif

        /// <summary>
        /// Returns an <see cref="XDocumentAssertions"/> object that can be used to assert the
        /// current <see cref="XElement"/>.
        /// </summary>
        public static XDocumentAssertions Should(this XDocument actualValue)
        {
            return new XDocumentAssertions(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="XElementAssertions"/> object that can be used to assert the
        /// current <see cref="XElement"/>.
        /// </summary>
        public static XElementAssertions Should(this XElement actualValue)
        {
            return new XElementAssertions(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="XAttributeAssertions"/> object that can be used to assert the
        /// current <see cref="XAttribute"/>.
        /// </summary>
        public static XAttributeAssertions Should(this XAttribute actualValue)
        {
            return new XAttributeAssertions(actualValue);
        }

        /// <summary>
        /// Asserts that the <paramref name="action"/> throws an exception.
        /// </summary>
        /// <param name="action">A reference to the method or property.</param>
        /// <typeparam name="TException">
        /// The type of the exception it should throw.
        /// </typeparam>
        /// <param name="reason">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        /// <returns>
        /// Returns an object that allows asserting additional members of the thrown exception.
        /// </returns>
        public static ExceptionAssertions<TException> ShouldThrow<TException>(this Action action, string reason = "",
            params object[] reasonArgs)
            where TException : Exception
        {
            return new ActionAssertions(action).ShouldThrow<TException>(reason, reasonArgs);
        }

#if NET45 || WINRT

    /// <summary>
    /// Asserts that the <paramref name="asyncAction"/> throws an exception.
    /// </summary>
    /// <param name="asyncAction">A reference to the method or property.</param>
    /// <typeparam name="TException">
    /// The type of the exception it should throw.
    /// </typeparam>
    /// <param name="reason">
    /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
    /// start with the word <i>because</i>, it is prepended to the message.
    /// </param>
    /// <param name="reasonArgs">
    /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
    /// </param>
    /// <returns>
    /// Returns an object that allows asserting additional members of the thrown exception.
    /// </returns>
        public static ExceptionAssertions<TException> ShouldThrow<TException>(this Func<Task> asyncAction, string reason = "", params object[] reasonArgs) 
            where TException : Exception
        {
            return new AsyncFunctionAssertions(asyncAction).ShouldThrow<TException>(reason, reasonArgs);
        }

#endif

        /// <summary>
        /// Asserts that the <paramref name="action"/> does not throw a particular exception.
        /// </summary>
        /// <param name="action">The current method or property.</param>
        /// <typeparam name="TException">
        /// The type of the exception it should not throw. Any other exceptions are ignored and will satisfy the assertion.
        /// </typeparam>
        /// <param name="reason">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        public static void ShouldNotThrow<TException>(this Action action, string reason = "", params object[] reasonArgs)
        {
            new ActionAssertions(action).ShouldNotThrow<TException>(reason, reasonArgs);
        }

        /// <summary>
        /// Asserts that the <paramref name="action"/> does not throw any exception at all.
        /// </summary>
        /// <param name="action">The current method or property.</param>
        /// <param name="reason">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        public static void ShouldNotThrow(this Action action, string reason = "", params object[] reasonArgs)
        {
            new ActionAssertions(action).ShouldNotThrow(reason, reasonArgs);
        }

        /// <summary>
        /// Forces enumerating a collection. Should be used to assert that a method that uses the 
        /// <c>yield</c> keyword throws a particular exception.
        /// </summary>
        public static Action Enumerating(this Func<IEnumerable> enumerable)
        {
            return () => ForceEnumeration(enumerable);
        }

        /// <summary>
        /// Forces enumerating a collection. Should be used to assert that a method that uses the 
        /// <c>yield</c> keyword throws a particular exception.
        /// </summary>
        public static Action Enumerating<T>(this Func<IEnumerable<T>> enumerable)
        {
            return () => ForceEnumeration(() => (IEnumerable)enumerable());
        }

        private static void ForceEnumeration(Func<IEnumerable> enumerable)
        {
            foreach (object item in enumerable())
            {
                // Do nothing
            }
        }

        /// <summary>
        /// Returns an <see cref="ObjectAssertions"/> object that can be used to assert the
        /// current <see cref="object"/>.
        /// </summary>
        public static ObjectAssertions Should(this object actualValue)
        {
            return new ObjectAssertions(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="BooleanAssertions"/> object that can be used to assert the
        /// current <see cref="bool"/>.
        /// </summary>
        public static BooleanAssertions Should(this bool actualValue)
        {
            return new BooleanAssertions(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NullableBooleanAssertions"/> object that can be used to assert the
        /// current nullable <see cref="bool"/>.
        /// </summary>
        public static NullableBooleanAssertions Should(this bool? actualValue)
        {
            return new NullableBooleanAssertions(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="GuidAssertions"/> object that can be used to assert the
        /// current <see cref="Guid"/>.
        /// </summary>
        public static GuidAssertions Should(this Guid actualValue)
        {
            return new GuidAssertions(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NullableGuidAssertions"/> object that can be used to assert the
        /// current nullable <see cref="Guid"/>.
        /// </summary>
        public static NullableGuidAssertions Should(this Guid? actualValue)
        {
            return new NullableGuidAssertions(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NonGenericCollectionAssertions"/> object that can be used to assert the
        /// current <see cref="IEnumerable"/>.
        /// </summary>
        public static NonGenericCollectionAssertions Should(this IEnumerable actualValue)
        {
            return new NonGenericCollectionAssertions(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="GenericCollectionAssertions{T}"/> object that can be used to assert the
        /// current <see cref="IEnumerable{T}"/>.
        /// </summary>
        public static GenericCollectionAssertions<T> Should<T>(this IEnumerable<T> actualValue)
        {
            return new GenericCollectionAssertions<T>(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="GenericDictionaryAssertions{TKey, TValue}"/> object that can be used to assert the
        /// current <see cref="IDictionary{TKey, TValue}"/>.
        /// </summary>
        public static GenericDictionaryAssertions<TKey, TValue> Should<TKey, TValue>(this IDictionary<TKey, TValue> actualValue)
        {
            return new GenericDictionaryAssertions<TKey, TValue>(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="DateTimeAssertions"/> object that can be used to assert the
        /// current <see cref="DateTime"/>.
        /// </summary>
        public static DateTimeAssertions Should(this DateTime actualValue)
        {
            return new DateTimeAssertions(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NullableDateTimeAssertions"/> object that can be used to assert the
        /// current nullable <see cref="DateTime"/>.
        /// </summary>
        public static NullableDateTimeAssertions Should(this DateTime? actualValue)
        {
            return new NullableDateTimeAssertions(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="ComparableTypeAssertions{T}"/> object that can be used to assert the
        /// current <see cref="IComparable{T}"/>.
        /// </summary>
        public static ComparableTypeAssertions<T> Should<T>(this IComparable<T> comparableValue)
        {
            return new ComparableTypeAssertions<T>(comparableValue);
        }

        /// <summary>
        /// Returns an <see cref="NumericAssertions{T}"/> object that can be used to assert the
        /// current <see cref="int"/>.
        /// </summary>
        public static NumericAssertions<int> Should(this int actualValue)
        {
            return new NumericAssertions<int>(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NullableNumericAssertions{T}"/> object that can be used to assert the
        /// current nullable <see cref="int"/>.
        /// </summary>
        public static NullableNumericAssertions<int> Should(this int? actualValue)
        {
            return new NullableNumericAssertions<int>(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NumericAssertions{T}"/> object that can be used to assert the
        /// current <see cref="decimal"/>.
        /// </summary>
        public static NumericAssertions<decimal> Should(this decimal actualValue)
        {
            return new NumericAssertions<decimal>(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NullableNumericAssertions{T}"/> object that can be used to assert the
        /// current nullable <see cref="decimal"/>.
        /// </summary>
        public static NullableNumericAssertions<decimal> Should(this decimal? actualValue)
        {
            return new NullableNumericAssertions<decimal>(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NumericAssertions{T}"/> object that can be used to assert the
        /// current <see cref="byte"/>.
        /// </summary>
        public static NumericAssertions<byte> Should(this byte actualValue)
        {
            return new NumericAssertions<byte>(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NullableNumericAssertions{T}"/> object that can be used to assert the
        /// current nullable <see cref="byte"/>.
        /// </summary>
        public static NullableNumericAssertions<byte> Should(this byte? actualValue)
        {
            return new NullableNumericAssertions<byte>(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NumericAssertions{T}"/> object that can be used to assert the
        /// current <see cref="short"/>.
        /// </summary>
        public static NumericAssertions<short> Should(this short actualValue)
        {
            return new NumericAssertions<short>(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NullableNumericAssertions{T}"/> object that can be used to assert the
        /// current nullable <see cref="short"/>.
        /// </summary>
        public static NullableNumericAssertions<short> Should(this short? actualValue)
        {
            return new NullableNumericAssertions<short>(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NumericAssertions{T}"/> object that can be used to assert the
        /// current <see cref="long"/>.
        /// </summary>
        public static NumericAssertions<long> Should(this long actualValue)
        {
            return new NumericAssertions<long>(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NullableNumericAssertions{T}"/> object that can be used to assert the
        /// current nullable <see cref="long"/>.
        /// </summary>
        public static NullableNumericAssertions<long> Should(this long? actualValue)
        {
            return new NullableNumericAssertions<long>(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NumericAssertions{T}"/> object that can be used to assert the
        /// current <see cref="float"/>.
        /// </summary>
        public static NumericAssertions<float> Should(this float actualValue)
        {
            return new NumericAssertions<float>(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NullableNumericAssertions{T}"/> object that can be used to assert the
        /// current nullable <see cref="float"/>.
        /// </summary>
        public static NullableNumericAssertions<float> Should(this float? actualValue)
        {
            return new NullableNumericAssertions<float>(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NumericAssertions{T}"/> object that can be used to assert the
        /// current <see cref="double"/>.
        /// </summary>
        public static NumericAssertions<double> Should(this double actualValue)
        {
            return new NumericAssertions<double>(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NullableNumericAssertions{T}"/> object that can be used to assert the
        /// current nullable <see cref="double"/>.
        /// </summary>
        public static NullableNumericAssertions<double> Should(this double? actualValue)
        {
            return new NullableNumericAssertions<double>(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="StringAssertions"/> object that can be used to assert the
        /// current <see cref="string"/>.
        /// </summary>
        public static StringAssertions Should(this string actualValue)
        {
            return new StringAssertions(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="SimpleTimeSpanAssertions"/> object that can be used to assert the
        /// current <see cref="TimeSpan"/>.
        /// </summary>
        public static SimpleTimeSpanAssertions Should(this TimeSpan actualValue)
        {
            return new SimpleTimeSpanAssertions(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NullableSimpleTimeSpanAssertions"/> object that can be used to assert the
        /// current nullable <see cref="TimeSpan"/>.
        /// </summary>
        public static NullableSimpleTimeSpanAssertions Should(this TimeSpan? actualValue)
        {
            return new NullableSimpleTimeSpanAssertions(actualValue);
        }

        /// <summary>
        /// Returns a <see cref="TypeAssertions"/> object that can be used to assert the
        /// current <see cref="Type"/>.
        /// </summary>
        public static TypeAssertions Should(this Type subject)
        {
            return new TypeAssertions(subject);
        }

        /// <summary>
        /// Returns a <see cref="TypeAssertions"/> object that can be used to assert the
        /// current <see cref="Type"/>.
        /// </summary>
        public static TypeSelectorAssertions Should(this TypeSelector typeSelector)
        {
            return new TypeSelectorAssertions(typeSelector.ToArray());
        }

        /// <summary>
        /// Returns a <see cref="MethodInfoAssertions"/> object that can be used to assert the current <see cref="MethodInfo"/>.
        /// </summary>
        /// <seealso cref="TypeAssertions"/>
        public static MethodInfoAssertions Should(this MethodInfo methodInfo)
        {
            return new MethodInfoAssertions(methodInfo);
        }

        /// <summary>
        /// Returns a <see cref="MethodInfoAssertions"/> object that can be used to assert the methods returned by the
        /// current <see cref="MethodInfoSelector"/>.
        /// </summary>
        /// <seealso cref="TypeAssertions"/>
        public static MethodInfoAssertions Should(this MethodInfoSelector methodSelector)
        {
            return new MethodInfoAssertions(methodSelector.ToArray());
        }

        /// <summary>
        /// Returns a <see cref="PropertyInfoAssertions"/> object that can be used to assert the
        /// current <see cref="PropertyInfoSelector"/>.
        /// </summary>
        /// <seealso cref="TypeAssertions"/>
        public static PropertyInfoAssertions Should(this PropertyInfo propertyInfo)
        {
            return new PropertyInfoAssertions(propertyInfo);
        }

        /// <summary>
        /// Returns a <see cref="PropertyInfoAssertions"/> object that can be used to assert the properties returned by the
        /// current <see cref="PropertyInfoSelector"/>.
        /// </summary>
        /// <seealso cref="TypeAssertions"/>
        public static PropertyInfoAssertions Should(this PropertyInfoSelector propertyInfoSelector)
        {
            return new PropertyInfoAssertions(propertyInfoSelector.ToArray());
        }

#if !WINDOWS_PHONE && !WINRT

        /// <summary>
        /// Asserts that the properties of an object matches those of another object.
        /// </summary>
        public static PropertyAssertions<T> ShouldHave<T>(this T subject)
        {
            return new PropertyAssertions<T>(subject);
        }

#endif
        
        /// <summary>
        /// Asserts that an object is equivalent to another object. 
        /// </summary>
        /// <remarks>
        /// Objects are equivalent when both object graphs have equally named properties with the same value, 
        /// irrespective of the type of those objects. Two properties are also equal if one type can be converted to another and the result is equal.
        /// The type of a collection property is ignored as long as the collection implements <see cref="IEnumerable"/> and all
        /// items in the collection are structurally equal. 
        /// Notice that actual behavior is determined by the <see cref="EquivalencyAssertionOptions{TSubject}.Default"/> instance of the 
        /// <see cref="EquivalencyAssertionOptions{TSubject}"/> class.
        /// </remarks>
        /// <param name="reason">
        /// An optional formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the 
        /// assertion is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public static void ShouldBeEquivalentTo<T>(this T subject, object expectation, string reason = "",
            params object[] reasonArgs)
        {
            ShouldBeEquivalentTo(subject, expectation, config => config, reason, reasonArgs);
        }

        /// <summary>
        /// Asserts that an object is equivalent to another object. 
        /// </summary>
        /// <remarks>
        /// Objects are equivalent when both object graphs have equally named properties with the same value, 
        /// irrespective of the type of those objects. Two properties are also equal if one type can be converted to another and the result is equal.
        /// The type of a collection property is ignored as long as the collection implements <see cref="IEnumerable"/> and all
        /// items in the collection are structurally equal. 
        /// </remarks>
        /// <param name="config">
        /// A reference to the <see cref="EquivalencyAssertionOptions{TSubject}.Default"/> configuration object that can be used 
        /// to influence the way the object graphs are compared. You can also provide an alternative instance of the 
        /// <see cref="EquivalencyAssertionOptions{TSubject}"/> class.
        /// </param>
        /// <param name="reason">
        /// An optional formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the 
        /// assertion is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public static void ShouldBeEquivalentTo<T>(this T subject, object expectation,
            Func<EquivalencyAssertionOptions<T>, EquivalencyAssertionOptions<T>> config, string reason = "", params object[] reasonArgs)
        {
            var context = new EquivalencyValidationContext(config(EquivalencyAssertionOptions<T>.Default()))
            {
                Subject = subject,
                Expectation = expectation,
                CompileTimeType = typeof (T),
                Reason = reason,
                ReasonArgs = reasonArgs
            };

            new EquivalencyValidator().AssertEquality(context);
        }

        public static void ShouldAllBeEquivalentTo<T>(this IEnumerable<T> subject, IEnumerable expectation,
            string reason = "", params object[] reasonArgs)
        {
            ShouldAllBeEquivalentTo(subject, expectation, config => config, reason, reasonArgs);
        }

        public static void ShouldAllBeEquivalentTo<T>(this IEnumerable<T> subject, IEnumerable expectation,
            Func<EquivalencyAssertionOptions<T>, EquivalencyAssertionOptions<T>> config, string reason = "", params object[] reasonArgs)
        {
            var context = new EquivalencyValidationContext(config(EquivalencyAssertionOptions<T>.Default()))
            {
                Subject = subject,
                Expectation = expectation,
                CompileTimeType = typeof (T),
                Reason = reason,
                ReasonArgs = reasonArgs
            };

            new EquivalencyValidator().AssertEquality(context);
        }


        /// <summary>
        /// Safely casts the specified object to the type specified through <typeparamref name="TTo"/>.
        /// </summary>
        /// <remarks>
        /// Has been introduced to allow casting objects without breaking the fluent API.
        /// </remarks>
        /// <typeparam name="TTo"></typeparam>
        public static TTo As<TTo>(this object subject)
        {
            return subject is TTo ? (TTo)subject : default(TTo);
        }
    }
}