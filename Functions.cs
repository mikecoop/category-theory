namespace CategoryTheory
{
    using System;
    using System.Collections.Generic;

    public static class Functions
    {
        public static Func<T1In, T2Out> ComposeWith<T1In, T1Out, T2Out>(
                this Func<T1In, T1Out> func1,
                Func<T1Out, T2Out> func2) =>
            value => func2(func1(value));

        public static T Id<T>(T value) => value;

        public static Func<TIn, TOut> Memoize<TIn, TOut>(Func<TIn, TOut> function)
        {
            var cache = new Dictionary<TIn, TOut>();

            return x =>
            {
                if (cache.ContainsKey(x))
                {
                    return cache[x];
                }
                else
                {
                    TOut result = function(x);
                    cache.Add(x, result);
                    return result;
                }
            };
        }
    }
}