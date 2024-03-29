using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentAssertions.Types
{
    /// <summary>
    /// Allows for fluent selection of methods of a type through reflection.
    /// </summary>
    public class MethodInfoSelector : IEnumerable<MethodInfo>
    {
        private IEnumerable<MethodInfo> selectedMethods = new List<MethodInfo>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodInfoSelector"/> class.
        /// </summary>
        /// <param name="type">The type from which to select methods.</param>
        public MethodInfoSelector(Type type)
            : this(new[]{type})
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodInfoSelector"/> class.
        /// </summary>
        /// <param name="types">The types from which to select methods.</param>
        public MethodInfoSelector(IEnumerable<Type> types)
        {
#if !WINRT
            selectedMethods = types.SelectMany(t => t
                .GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(method => !HasSpecialName(method)));
#else
            selectedMethods = from type in types
                              from method in type.GetTypeInfo().DeclaredMethods
                              where !method.IsStatic && !HasSpecialName(method)
                              select method;
#endif
        }

        /// <summary>
        /// Only select the methods that are public or internal.
        /// </summary>
        public MethodInfoSelector ThatArePublicOrInternal
        {
            get
            {
                selectedMethods = selectedMethods.Where(method => method.IsPublic || method.IsAssembly);
                return this;
            }
        }

        /// <summary>
        /// Only select the methods without a return value
        /// </summary>
        public MethodInfoSelector ThatReturnVoid
        {
            get
            {
                selectedMethods = selectedMethods.Where(method => method.ReturnType == typeof (void));
                return this;
            }
        }

        /// <summary>
        /// Only select the methods that return the specified type 
        /// </summary>
        public MethodInfoSelector ThatReturn<TReturn>()
        {
            selectedMethods = selectedMethods.Where(method => method.ReturnType == typeof(TReturn));
            return this;
        }

        /// <summary>
        /// Only select the methods that are decorated with an attribute of the specified type.
        /// </summary>
        public MethodInfoSelector ThatAreDecoratedWith<TAttribute>()
        {
            selectedMethods = selectedMethods.Where(method => method.GetCustomAttributes(false).OfType<TAttribute>().Any());
            return this;
        }

        /// <summary>
        /// The resulting <see cref="MethodInfo"/> objects.
        /// </summary>
        public MethodInfo[] ToArray()
        {
            return selectedMethods.ToArray();
        }

        /// <summary>
        /// Determines whether the specified method has a special name (like properties and events).
        /// </summary>
        private bool HasSpecialName(MethodInfo method)
        {
#if WINRT
            return method.IsSpecialName;
#else
            return (method.Attributes & MethodAttributes.SpecialName) == MethodAttributes.SpecialName;
#endif
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<MethodInfo> GetEnumerator()
        {
            return selectedMethods.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}