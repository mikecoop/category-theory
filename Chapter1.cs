using NUnit.Framework;
using FluentAssertions;
using System;
using static CategoryTheory.Functions;

namespace CategoryTheory.Chapter1
{
    public class Tests
    {
        [Test]
        public void ComposeWith_GivenTwoFucntionsSameTypes_FunctionsAreCompsed()
        {
            Func<int, int> addOne = n => n + 1;
            Func<int, int> timesTwo = n => n * 2;

            Func<int, int> addOneTimesTwo = addOne.ComposeWith(timesTwo);

            int result = addOneTimesTwo(3);
            result.Should().Be(8);
        }

        [Test]
        public void ComposeWith_GivenTwoFucntionsDifferentTypes_FunctionsAreCompsed()
        {
            Func<int, int> addOne = n => n + 1;
            Func<int, string> toString = n => n.ToString();

            Func<int, string> addOneToString = addOne.ComposeWith(toString);

            string result = addOneToString(3);
            result.Should().Be("4");
        }

        [Test]
        public void ComposeWith_FunctionAndId_FunctionReturnsSameValue()
        {
            Func<int, int> addOne = n => n + 1;

            Func<int, int> addOneWithIdFunction = addOne.ComposeWith(Id);

            int result = addOneWithIdFunction(10);
            result.Should().Be(11);
        }
    }
}