using Inheritance.Geometry.Visitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;

namespace Inheritance.Geometry.Virtual
{
    public abstract class Body
    {
        public Vector3 Position { get; }

        protected Body(Vector3 position)
        {
            Position = position;
        }

        public abstract bool ContainsPoint(Vector3 point);
        public abstract RectangularCuboid GetBoundingBox();

    }

    public class Ball : Body
    {
        public double Radius { get; }

        public Ball(Vector3 position, double radius) : base(position)
        {
            Radius = radius;
        }

        public override bool ContainsPoint(Vector3 point)
        {
            var vector = point - Position;
            var length2 = vector.GetLength2();
            return length2 <= Radius * Radius;
        }

        public override RectangularCuboid GetBoundingBox()
        {
            var length = Radius * 2;
            var box = new RectangularCuboid(Position, length, length, length);
            return box;
        }

    }

    public class RectangularCuboid : Body
    {
        public double SizeX { get; }
        public double SizeY { get; }
        public double SizeZ { get; }
        public Vector3 Min { get; }
        public Vector3 Max { get; }

        public RectangularCuboid(Vector3 position, double sizeX, double sizeY, double sizeZ) : base(position)
        {
            SizeX = sizeX;
            SizeY = sizeY;
            SizeZ = sizeZ;
            Min = MinCalculate(Position, SizeX, SizeY, SizeZ);
            Max = MaxCalculate(Position, SizeX, SizeY, SizeZ);

        }

        public override bool ContainsPoint(Vector3 point)
        {
            return point >= Min && point <= Max;
        }

        public override RectangularCuboid GetBoundingBox()
        {
            var box = new RectangularCuboid(Position, SizeX, SizeY, SizeZ);
            return box;
        }

        private static Vector3 MinCalculate(Vector3 position, double sizeX, double sizeY, double sizeZ)
        {
            return new Vector3(position.X - sizeX / 2, position.Y - sizeY / 2, position.Z - sizeZ / 2);
        }

        private static Vector3 MaxCalculate(Vector3 position, double sizeX, double sizeY, double sizeZ)
        {
            return new Vector3( position.X + sizeX / 2, position.Y + sizeY / 2, position.Z + sizeZ / 2);
        }
    }

    public class Cylinder : Body
    {
        public double SizeZ { get; }

        public double Radius { get; }

        public Cylinder(Vector3 position, double sizeZ, double radius) : base(position)
        {
            SizeZ = sizeZ;
            Radius = radius;
        }

        public override bool ContainsPoint(Vector3 point)
        {
            var vectorX = point.X - Position.X;
            var vectorY = point.Y - Position.Y;
            var length2 = vectorX * vectorX + vectorY * vectorY;
            var minZ = Position.Z - SizeZ / 2;
            var maxZ = minZ + SizeZ;

            return length2 <= Radius * Radius && point.Z >= minZ && point.Z <= maxZ;
        }

        public override RectangularCuboid GetBoundingBox()
        {
            var box = new RectangularCuboid(Position, Radius * 2, Radius * 2, SizeZ);
            return box;
        }

    }

    public class CompoundBody : Body
    {
        public IReadOnlyList<Body> Parts { get; }

        public CompoundBody(IReadOnlyList<Body> parts) : base(parts[0].Position)
        {
            Parts = parts;
        }

        public override bool ContainsPoint(Vector3 point)
        {
            return Parts.Any(body => body.ContainsPoint(point));
        }

        public override RectangularCuboid GetBoundingBox()
        {
            var x = new MinMax();
            var y = new MinMax();
            var z = new MinMax();

            foreach (var elem in Parts)
            {
                RectangularCuboid box = elem.GetBoundingBox();

                Vector3 minPoint = box.Min;
                Vector3 maxPoint = box.Max;

                x.MinMaxUpdate(minPoint.X, maxPoint.X);
                y.MinMaxUpdate(minPoint.Y, maxPoint.Y);
                z.MinMaxUpdate(minPoint.Z, maxPoint.Z);
            }
            Vector3 position = new Vector3(x.Sum / 2, y.Sum / 2 ,z.Sum / 2);

            return new RectangularCuboid(position, x.Difference, y.Difference, z.Difference);


        }
    }

    public class MinMax
    {
        public double Max { get; set; }
        public double Min { get; set; }
        public double Sum => Max + Min;
        public double Difference => Max - Min;

        public MinMax()
        {
            Max = double.MinValue;
            Min = double.MaxValue;
        }

        public void MinMaxUpdate(double newMax, double newMin)
        {
            Max = Math.Max(newMax, Max);
            Min = Math.Min(newMax, Min);
            Max = Math.Max(newMin, Max);
            Min = Math.Min(newMin, Min);
        }
    }
}