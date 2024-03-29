using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentAssertions.Equivalency
{
    internal class ComplexTypeEquivalencyStep : IEquivalencyStep
    {
        /// <summary>
        /// Gets a value indicating whether this step can handle the current subject and/or expectation.
        /// </summary>
        public bool CanHandle(EquivalencyValidationContext context)
        {
            return (context.Subject != null) &&
                context.Subject.GetType().IsComplexType() && (context.IsRoot || context.Config.IsRecursive);
        }

        /// <summary>
        /// Applies a step as part of the task to compare two objects for structural equality.
        /// </summary>
        /// <value>
        /// Should return <c>true</c> if the subject matches the expectation or if no additional assertions
        /// have to be executed. Should return <c>false</c> otherwise.
        /// </value>
        /// <remarks>
        /// May throw when preconditions are not met or if it detects mismatching data.
        /// </remarks>
        public bool Handle(EquivalencyValidationContext context, IEquivalencyValidator parent)
        {
            IEnumerable<PropertyInfo> selectedProperties = context.SelectedProperties.ToArray();
            if (context.IsRoot && !selectedProperties.Any())
            {
                throw new InvalidOperationException("Please specify some properties to include in the comparison.");
            }

            foreach (PropertyInfo propertyInfo in selectedProperties)
            {
                AssertPropertyEquality(context, parent, propertyInfo);
            }

            return true;
        }

        private void AssertPropertyEquality(EquivalencyValidationContext context, IEquivalencyValidator parent,
            PropertyInfo propertyInfo)
        {
            var nestedContext = context.CreateForNestedProperty(propertyInfo);
            if (nestedContext != null)
            {
                parent.AssertEqualityUsing(nestedContext);
            }
        }
    }
}