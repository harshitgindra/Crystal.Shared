#region USING

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace Crystal.Shared.Decorator
{
    public static class ListDecorator
    {
        public static bool IsNullOrEmpty<TItem>(this IEnumerable<TItem> dataset)
        {
            var returnValue = dataset == null || !dataset.Any();
            return returnValue;
        }

        public static T PickRandom<T>(this IEnumerable<T> source)
        {
            return source.PickRandom(1).Single();
        }

        public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
        {
            return source.Shuffle().Take(count);
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(x => Guid.NewGuid());
        }
    }
}