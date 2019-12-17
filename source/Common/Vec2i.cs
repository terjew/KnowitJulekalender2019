using System;
using System.Diagnostics.CodeAnalysis;

namespace Common
{
    public struct Vec2i : IEquatable<Vec2i>
    {
        public int X { get; }
        public int Y { get; }
        public Vec2i(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            return obj is Vec2i i && Equals(i);
        }

        public bool Equals([AllowNull] Vec2i other)
        {
            return X == other.X &&
                   Y == other.Y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public static Vec2i operator +(Vec2i a, Vec2i b)
        {
            return new Vec2i(a.X + b.X, a.Y + b.Y);
        }

        public static Vec2i operator -(Vec2i a, Vec2i b)
        {
            return new Vec2i(a.X - b.X, a.Y - b.Y);
        }

        public static Vec2i operator *(Vec2i a, int b)
        {
            return new Vec2i(a.X * b, a.Y * b);
        }

        public static bool operator ==(Vec2i a, Vec2i b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Vec2i a, Vec2i b)
        {
            return !a.Equals(b);
        }
    }
}
