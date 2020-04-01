using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq;

namespace Crystal.Core.Shared.Extension
{
    public static class ListExtensions
    {
        public static bool IsNullOrEmpty<TItem>(this IEnumerable<TItem> dataset)
        {
            bool returnValue = dataset == null || !dataset.Any();
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
