using System;

namespace DataLoaderConsoleTest.Table
{
    internal static class DirectionExtension
    {
        public static Direction Inverse(this Direction direction) => direction switch
        {
            Direction.Left => Direction.Right,
            Direction.Right => Direction.Left,
            Direction.Top => Direction.Bottom,
            Direction.Bottom => Direction.Top,
            _ => throw new ArgumentException(),
        };

        public static (int x, int y) NextPointBy(this (int x, int y) point, Direction direction) => direction switch
        {
            Direction.Left => (point.x - 1, point.y),
            Direction.Right => (point.x + 1, point.y),
            Direction.Top => (point.x, point.y + 1),
            Direction.Bottom => (point.x, point.y - 1),
            _ => throw new ArgumentException(),
        };
    }

}
