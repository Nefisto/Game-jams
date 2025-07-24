using System;
using System.Collections.Generic;
using System.Linq;

public static partial class Extensions
{
    public static T GetRandom<T>(this IEnumerable<T> source)
    {
        return source.GetRandom(1).Single();
    }

    public static IEnumerable<T> GetRandom<T>(this IEnumerable<T> source, int count)
    {
        return source.Shuffle().Take(count);
    }

    private static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
    {
        return source.OrderBy(x => Guid.NewGuid());
    }
}
