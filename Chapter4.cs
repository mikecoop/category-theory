using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using static CategoryTheory.Functions;
using System.Collections.Generic;
using Moq;

namespace CategoryTheory.Chapter4
{
    public class Writer<TOut>
    {
        public TOut Value { get; set; }

        public string Message { get; set; }
    }

    public static class StringTransforms
    {
        public static Writer<string> ToUpper(string input)
        {
            return new Writer<string> { Value = input.ToUpper(), Message = "ToUpper " };
        }

        public static Writer<IEnumerable<string>> ToWords(string input)
        {
            return new Writer<IEnumerable<string>> { Value = input.Split(' '), Message = "ToWords " };
        }

        public static Writer<IEnumerable<string>> Process(string input)
        {
            var upper = ToUpper(input);
            var words = ToWords(upper.Value);
            return new Writer<IEnumerable<string>>
            {
                Value = words.Value,
                Message = (upper.Message + words.Message).Trim()
            };
        }

        public static Func<T1In, Writer<T2Out>> ComposeWith<T1In, T1Out, T2Out>(
            this Func<T1In, Writer<T1Out>> func1,
            Func<T1Out, Writer<T2Out>> func2)
        {
            return value =>
            {
                var writer1 = func1(value);
                var writer2 = func2(writer1.Value);
                return new Writer<T2Out> { Value = writer2.Value, Message = (writer1.Message + writer2.Message).Trim() };
            };
        }
    }

    public class Option<T>
    {
        private Option (T value, bool isValid)
        {
            Value = value;
            IsValid = isValid;
        }

        public T Value { get; }

        public bool IsValid { get; }

        public static Option<T> Some(T value)
        {
            return new Option<T>(value, true);
        }

        public static Option<T> None()
        {
            return new Option<T>(default(T), false);
        }
    }

    public static class OptionExtensions
    {
        public static Func<T1In, Option<T2Out>> ComposeWith<T1In, T1Out, T2Out>(
            this Func<T1In, Option<T1Out>> func1,
            Func<T1Out, Option<T2Out>> func2)
        {
            return value =>
            {
                var option1 = func1(value);
                if (option1.IsValid)
                {
                    return func2(option1.Value);
                }
                else
                {
                    return Option<T2Out>.None();
                }
            };
        }
    }

    public static class SafeMath
    {
        public static Option<double> SafeReciprocal(double value)
        {
            if (value != 0.0)
            {
                return Option<double>.Some(1 / value);
            }
            else
            {
                return Option<double>.None();
            }
        }

        public static Option<double> SafeSquareRoot(double value)
        {
            if (value > 0.0)
            {
                return Option<double>.Some(Math.Sqrt(value));
            }
            else
            {
                return Option<double>.None();
            }
        }
    }

    public class Tests
    {
        [Test]
        public void Test1()
        {
            var processed = StringTransforms.Process("The dog barked at the cat.");

            processed.Value.Should().ContainInOrder("THE", "DOG", "BARKED", "AT", "THE", "CAT.");
            processed.Message.Should().Be("ToUpper ToWords");
        }

        [Test]
        public void ComposeWith_UsedOnStringTransforms_CorrectValueIsReturned()
        {
            Func<string, Writer<string>> toUpper = StringTransforms.ToUpper;
            Func<string, Writer<IEnumerable<string>>> toWords = StringTransforms.ToWords;
            var process = toUpper.ComposeWith(toWords);

            var processed = process("The dog barked at the cat.");

            processed.Value.Should().ContainInOrder("THE", "DOG", "BARKED", "AT", "THE", "CAT.");
            processed.Message.Should().Be("ToUpper ToWords");
        }

        [Test]
        public void SafeReciprocal_WhenCalled_ReturnsCorrectValues()
        {
            Func<double, Option<double>> safeReciprocal = SafeMath.SafeReciprocal;

            var result = safeReciprocal(0);
            result.IsValid.Should().BeFalse();

            result = safeReciprocal(0.2);
            result.IsValid.Should().BeTrue();
            result.Value.Should().BeApproximately(5.0, 1E-02);
        }

        [Test]
        public void SafeSquareRoot_WhenCalled_ReturnsCorrectValues()
        {
            Func<double, Option<double>> safeSquareRoot = SafeMath.SafeSquareRoot;

            var result = safeSquareRoot(-25);
            result.IsValid.Should().BeFalse();

            result = safeSquareRoot(25);
            result.IsValid.Should().BeTrue();
            result.Value.Should().BeApproximately(5.0, 1E-02);
        }

        [Test]
        public void ComposeWith_UsedWithSafeMath_ReturnsCorrectValues()
        {
            Func<double, Option<double>> safeReciprocal = SafeMath.SafeReciprocal;
            Func<double, Option<double>> safeSquareRoot = SafeMath.SafeSquareRoot;
            var safeReciprocalSquareRoot = safeReciprocal.ComposeWith(safeSquareRoot);

            var result = safeReciprocalSquareRoot(0.04);
            result.IsValid.Should().BeTrue();
            result.Value.Should().BeApproximately(5.0, 1E-02);

            result = safeReciprocalSquareRoot(-5);
            result.IsValid.Should().BeFalse();

            result = safeReciprocalSquareRoot(0);
            result.IsValid.Should().BeFalse();
        }
    }
}