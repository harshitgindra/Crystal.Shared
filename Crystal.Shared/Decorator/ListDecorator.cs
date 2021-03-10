#region USING

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace Crystal.Shared
{
    /// <summary>
    /// List extensions
    /// </summary>
    public static class ListDecorator
    {
        /// <summary>
        /// Return true if records is null oe empty
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="records"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<TItem>(this IEnumerable<TItem> records)
        {
            return records == null || !records.Any();
        }

        /// <summary>
        /// Pick a random record from the records
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="records"></param>
        /// <returns></returns>
        public static T PickRandom<T>(this IEnumerable<T> records)
        {
            //***
            //*** Shuffles and return the first record
            //***
            return records.PickRandom(1).Single();
        }
        /// <summary>
        /// Pick a random list of record from the records
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="records"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> records, int count)
        {
            //***
            //*** Shuffles and return the selected count of records
            //***
            return records.Shuffle().Take(count);
        }

        /// <summary>
        /// Shuffle the records randomly
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="records"></param>
        /// <returns></returns>
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> records)
        {
            return records.OrderBy(x => Guid.NewGuid());
        }
    }
}