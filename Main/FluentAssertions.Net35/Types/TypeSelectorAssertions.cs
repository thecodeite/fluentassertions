using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

#if WINRT
using System.Reflection;
#endif

using FluentAssertions.Execution;
using FluentAssertions.Common;

namespace FluentAssertions.Types
{
    /// <summary>
    /// Contains a number of methods to assert that all <see cref="Type"/>s in a <see cref="TypeSelector"/>
    /// meet certain expectations.
    /// </summary>
    [DebuggerNonUserCode]
    public class TypeSelectorAssertions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        protected internal TypeSelectorAssertions(IEnumerable<Type> types)
        {
            Subject = types;
        }

        /// <summary>
        /// Gets the object which value is being asserted.
        /// </summary>
        public IEnumerable<Type> Subject { get; private set; }

        /// <summary>
        /// Asserts that the current <see cref="Type"/> is decorated with the specified <typeparamref name="TAttribute"/>.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<TypeSelectorAssertions> BeDecoratedWith<TAttribute>(string reason = "", params object[] reasonArgs)
        {
            IEnumerable<Type> typesWithoutAttribute = Subject
                .Where(type => !type.IsDecoratedWith<TAttribute>())
                .ToArray();

            Execute.Verification
                .ForCondition(!typesWithoutAttribute.Any())
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected all types to be decorated with {0}{reason}," +
                    " but the attribute was not found on the following types:\r\n" + GetDescriptionsFor(typesWithoutAttribute),
                    typeof(TAttribute));

            return new AndConstraint<TypeSelectorAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="Type"/> is decorated with an attribute of type <typeparamref name="TAttribute"/>
        /// that matches the specified <paramref name="isMatchingAttributePredicate"/>.
        /// </summary>
        /// <param name="isMatchingAttributePredicate">
        /// The predicate that the attribute must match.
        /// </param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<TypeSelectorAssertions> BeDecoratedWith<TAttribute>(
            Expression<Func<TAttribute, bool>> isMatchingAttributePredicate, string reason = "", params object[] reasonArgs)
        {
            IEnumerable<Type> typesWithoutMatchingAttribute = Subject
                .Where(type => !type.HasMatchingAttribute(isMatchingAttributePredicate))
                .ToArray();

            Execute.Verification
                .ForCondition(!typesWithoutMatchingAttribute.Any())
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected all types to be decorated with {0} that matches {1}{reason}," +
                    " but no matching attribute was found on the following types:\r\n" + GetDescriptionsFor(typesWithoutMatchingAttribute),
                    typeof(TAttribute), isMatchingAttributePredicate.Body);

            return new AndConstraint<TypeSelectorAssertions>(this);
        }

        private static string GetDescriptionsFor(IEnumerable<Type> types)
        {
            return string.Join(Environment.NewLine, types.Select(GetDescriptionFor).ToArray());
        }

        private static string GetDescriptionFor(Type type)
        {
#if !WINRT
            return type.ToString();
#else
            return type.GetTypeInfo().ToString();
#endif
        }
    }
}