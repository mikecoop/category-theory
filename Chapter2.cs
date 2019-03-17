using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using static CategoryTheory.Functions;
using Moq;
using System.Collections.Generic;

namespace CategoryTheory.Chapter2
{
    public class Tests
    {
        [Test]
        public void Memoize_CalledWithFunction_FunctionIsCalledOnce()
        {
            int callCount = 0;
            Func<int, int> test = i =>
            {
                callCount++;
                return i;
            };

            Func<int, int> memoizeTest = Memoize(test);

            int result = memoizeTest(5);
            result.Should().Be(5);

            memoizeTest(5);
            callCount.Should().Be(1);
        }

        [Test]
        public void Memoize_CalledWithRandom_ReturnsCorrectResult()
        {
            Random random = new Random();
            Func<int, int> rand = i => random.Next(i);

            Func<int, int> memoizeRand = Memoize(rand);

            var results = new List<int>();
            for (int i = 0; i < 10; i++)
            {
                results.Add(memoizeRand(11));
            }

            results.Distinct().Count().Should().Be(1);
        }

        [Test]
        public void Memoize_CalledWithRandomSeed_ReturnsCorrectResult()
        {
            Func<int, int> rand = i =>
            {
                Random random = new Random(i);
                return random.Next();
            };

            Func<int, int> memoizeRand = Memoize(rand);

            var results = new List<int>();
            for (int i = 0; i < 10; i++)
            {
                results.Add(memoizeRand(10));
            }

            results.Distinct().Count().Should().Be(1);
        }
    }
}