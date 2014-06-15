// -----------------------------------------------------------------------
// <copyright file="EmptyEnumerable.cs" company="HelGames Company Identifier">
// Copyright 2014 HelGames Company Identifier. All rights reserved.
// </copyright>
// <author>Paul Schulze</author>
// -----------------------------------------------------------------------
namespace HelGames.Teaching.ComponentSystem
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines the Enumerable helper class. This is a pure utility class,
    /// that helps with the use of enumerables.
    /// </summary>
    public class Enumerable
    {
        /// <summary>
        /// Generate an empty enumerable. This method can be used, to generate
        /// a default return value of an empty enumerable, where the enumerated
        /// type is known at compile time, but the instanciation of the object
        /// graph to generate the enumerable is deemed too complex but null is
        /// not a desired return value.
        /// </summary>
        /// <typeparam name="TYPE">
        /// The type of objects, the enumerator is expected to contain. This type
        /// is only used to type the returned IEnumerable, as there will not be
        /// any objects, contained in it.
        /// </typeparam>
        /// <returns>
        /// The empty <see cref="IEnumerable"/> enumerable for the given type.
        /// </returns>
        public static IEnumerable<TYPE> Empty<TYPE>()
        {
            yield break;
        }
    }
}