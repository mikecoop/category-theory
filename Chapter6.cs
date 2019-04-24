namespace CategoryTheory.Chapter6
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IShape
    {
        double Area();

        double Perimeter();
    }

    public class Circle : IShape
    {
        public Circle(double radius) => Radius = radius;

        public double Radius { get; }

        public double Area() => Math.PI * Math.Pow(Radius, 2);

        public double Perimeter() => 2.0 * Math.PI * Radius;
    }

    public class Rectangle : IShape
    {
        public Rectangle(double length, double width)
        {
            Length = length;
            Width = width;
        }

        public double Length { get; }

        public double Width { get; }

        public double Area() => Length * Width;

        public double Perimeter() => 2 * (Length + Width);
    }

    public class Square : IShape
    {
        public Square(double sideLength) => SideLength = sideLength;

        public double SideLength { get; }

        public double Area() => SideLength * SideLength;

        public double Perimeter() => SideLength * 4;
    }
}
