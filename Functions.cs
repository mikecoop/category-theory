namespace CategoryTheory
{
    using System;

    public static class Functions
    {
        public static Func<T1In, T2Out> ComposeWith<T1In, T1Out, T2Out>(
                this Func<T1In, T1Out> func1,
                Func<T1Out, T2Out> func2) =>
            value => func2(func1(value));

        public static T Id<T>(T value) => value;
    }
}