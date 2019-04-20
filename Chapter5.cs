namespace CategoryTheory.Chapter5
{
    using System;
    using FluentAssertions;
    using NUnit.Framework;

    public class Either<T1, T2>
    {
        private Either(T1 left, T2 right)
        {
            Left = left;
            Right = right;
        }

        public static Either<T1, T2> FromLeft(T1 value)
        {
            var either = new Either<T1, T2>(value, default);
            either.IsLeft = true;
            return either;
        }

        public static Either<T1, T2> FromRight(T2 value)
        {
            var either = new Either<T1, T2>(default, value);
            either.IsLeft = false;
            return either;
        }

        public bool IsLeft { get; private set; }

        public bool IsRight => !IsLeft;

        public T1 Left { get; }
        
        public T2 Right { get; }
    }

    public class Tests
    {
        [Test]
        public void Left_WhenCalled_CorrectValueIsReturned()
        {
            var either = Either<bool, string>.FromLeft(true);

            either.IsLeft.Should().BeTrue();
            either.IsRight.Should().BeFalse();

            either.Left.Should().BeTrue();
        }

        [Test]
        public void Right_WhenCalled_CorrectValueIsReturned()
        {
            var either = Either<bool, string>.FromRight("Hello 1234!");

            either.IsRight.Should().BeTrue();
            either.IsLeft.Should().BeFalse();

            either.Right.Should().Be("Hello 1234!");
        }
    }
}
