using System;
using System.Globalization;

namespace FluentAssertions.Equivalency
{
    internal class TryConversionEquivalencyStep : IEquivalencyStep
    {
        /// <summary>
        /// Gets a value indicating whether this step can handle the current subject and/or expectation.
        /// </summary>
        public bool CanHandle(EquivalencyValidationContext context)
        {
            return true;
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
        public bool Handle(EquivalencyValidationContext context, IEquivalencyValidator structuralEqualityValidator)
        {
            if (!ReferenceEquals(context.Expectation, null) && !ReferenceEquals(context.Subject, null)
                && !context.Subject.GetType().IsSameOrInherits(context.Expectation.GetType()))
            {
                try
                {
                    context.Subject = Convert.ChangeType(context.Subject, context.Expectation.GetType(), CultureInfo.CurrentCulture);
                }
                catch (FormatException)
                {
                }
                catch (InvalidCastException)
                {
                }
            }

            return false;
        }
    }
}