using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

public static class EnumerableExtensions {
    public static T RandomElement<T>(this Random rand, IEnumerable<T> enumerable)
        => enumerable.ElementAt(rand.Next(enumerable.Count()));

    public static T RandomElement<T>(this IEnumerable<T> enumerable)
        => Random.Shared.RandomElement(enumerable);

    public static IEnumerable<T> CumSum<T>(this IEnumerable<T> enumerable) where T : struct, IAdditionOperators<T, T, T> {
        T val = default;
        foreach (var item in enumerable) {
            val += item;
            yield return val;
        }
    }

    public static IEnumerable<(T Item, TSum CumSum)> CumSumBy<T, TSum>(this IEnumerable<T> enumerable, Func<T, TSum> by)
        where TSum : struct, IAdditionOperators<TSum, TSum, TSum>
        => enumerable.CumSumBy(by, (item, val) => (item, val));

    public static IEnumerable<TRes> CumSumBy<T, TSum, TRes>(this IEnumerable<T> enumerable, Func<T, TSum> by, Func<T, TSum, TRes> selector)
        where TSum : struct, IAdditionOperators<TSum, TSum, TSum> {
        TSum val = default;
        foreach (var item in enumerable) {
            val += by(item);
            yield return selector(item, val);
        }
    }
}
