using System.Collections;
using System.Linq;

namespace FluentAssertions.Equivalency
{
    internal class EnumerableEquivalencyStep : IEquivalencyStep
    {
        /// <summary>
        /// Gets a value indicating whether this step can handle the current subject and/or expectation.
        /// </summary>
        public bool CanHandle(EquivalencyValidationContext context)
        {
            return IsCollection(context.Subject);
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
            AssertExpectationIsCollection(context);

            var subject = ((IEnumerable)context.Subject).Cast<object>().ToArray();
            var expectation = ((IEnumerable)context.Expectation).Cast<object>().ToArray();

            AssertCollectionsHaveEqualLength(context, subject, expectation);

            if (context.IsRoot || context.Config.IsRecursive)
            {
                EnumerateElements(context, subject, expectation, parent);
            }
            else
            {
                subject.Should().Equal(expectation, context.Reason, context.ReasonArgs);
            }

            return true;
        }

        private void EnumerateElements(EquivalencyValidationContext context, object[] subject, object[] expectation,
            IEquivalencyValidator parent)
        {
            if (!subject.SequenceEqual(expectation))
            {
                for (int i = 0; i < subject.Length; i++)
                {
                    parent.AssertEqualityUsing(context.CreateForCollectionItem(i, subject[i], expectation[i]));
                }
            }
        }

        private static void AssertExpectationIsCollection(EquivalencyValidationContext context)
        {
            context.Verification
                .ForCondition(IsCollection(context.Expectation))
                .FailWith((context.IsRoot ? "Subject" : context.PropertyDescription) +
                    " is a collection and cannot be compared with a non-collection type.",
                    context.Subject, context.Subject.GetType().FullName);
        }

        private static void AssertCollectionsHaveEqualLength(EquivalencyValidationContext context, object[] subject, object[] expectation)
        {
            context.Verification
                .ForCondition(subject.Length == expectation.Length)
                .FailWith(
                    "Expected " + (context.IsRoot ? "subject" : context.PropertyDescription) +
                        " to be a collection with {0} item(s){reason}, but found {1}.",
                    expectation.Length, subject.Length);
        }

        private static bool IsCollection(object value)
        {
            return (!(value is string) && (value is IEnumerable));
        }
    }
}