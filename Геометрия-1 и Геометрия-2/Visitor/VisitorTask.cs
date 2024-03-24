using System;
using System.Collections.Generic;
using System.Security.Policy;

namespace Inheritance.Geometry.Visitor
{
    public interface IVisitor
    {
        Body Visit(Ball ball);
        Body Visit(RectangularCuboid rectangularCuboid);
        Body Visit(Cylinder cylinder);
        Body Visit(CompoundBody compoundBody);

    }
    public abstract class Body
    {
        public Vector3 Position { get; }

        protected Body(Vector3 position)
        {
            Position = position;
        }
        public abstract Body Accept(IVisitor visitor);
    }

    public class Ball : Body
    {
        public double Radius { get; }

        public Ball(Vector3 position, double radius) : base(position)
        {
            Radius = radius;
        }

        public override Body Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }

    public class RectangularCuboid : Body
    {
        public double SizeX { get; }
        public double SizeY { get; }
        public double SizeZ { get; }
        public Vector3 Max {  get; }
        public Vector3 Min { get; }

        public RectangularCuboid(Vector3 position, double sizeX, double sizeY, double sizeZ) : base(position)
        {
            SizeX = sizeX;
            SizeY = sizeY;
            SizeZ = sizeZ;
            Min = MinCalculate(Position, SizeX, SizeY, SizeZ);
            Max = MaxCalculate(Position, SizeX, SizeY, SizeZ);
        }
        private static Vector3 MinCalculate(Vector3 position, double sizeX, double sizeY, double sizeZ)
        {
            return new Vector3(position.X - sizeX / 2, position.Y - sizeY / 2, position.Z - sizeZ / 2);
        }

        private static Vector3 MaxCalculate(Vector3 position, double sizeX, double sizeY, double sizeZ)
        {
            return new Vector3(position.X + sizeX / 2, position.Y + sizeY / 2, position.Z + sizeZ / 2);
        }

        public override Body Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
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

        public override Body Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
        }

    }

    public class CompoundBody : Body
    {
        public IReadOnlyList<Body> Parts { get; }

        public CompoundBody(IReadOnlyList<Body> parts) : base(parts[0].Position)
        {
            Parts = parts;
        }

        public override Body Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }

    public class BoundingBoxVisitor : IVisitor
    {
        public Body Visit(Ball ball)
        {
            var length = ball.Radius * 2;
            var box = new RectangularCuboid(ball.Position, length, length, length);
            return box;
        }

        public Body Visit(RectangularCuboid rectangularCuboid)
        {
            var box = new RectangularCuboid(rectangularCuboid.Position, rectangularCuboid.SizeX, rectangularCuboid.SizeY, rectangularCuboid.SizeZ);
            return box;
        }

        public Body Visit(Cylinder cylinder)
        {
            var box = new RectangularCuboid(cylinder.Position, cylinder.Radius * 2, cylinder.Radius * 2, cylinder.SizeZ);
            return box;
        }

        public Body Visit(CompoundBody compoundBody)
        {
            var x = new MinMax();
            var y = new MinMax();
            var z = new MinMax();

            foreach (var elem in compoundBody.Parts)
            {
                RectangularCuboid box = (RectangularCuboid)elem.Accept(new BoundingBoxVisitor());

                Vector3 minPoint = box.Min;
                Vector3 maxPoint = box.Max;

                x.MinMaxUpdate(minPoint.X, maxPoint.X);
                y.MinMaxUpdate(minPoint.Y, maxPoint.Y);
                z.MinMaxUpdate(minPoint.Z, maxPoint.Z);
            }
            Vector3 position = new Vector3(x.Sum / 2, y.Sum / 2, z.Sum / 2);

            return new RectangularCuboid(position, x.Difference, y.Difference, z.Difference);
        }
    }

    public class BoxifyVisitor : IVisitor
    {
        public Body Visit(Ball ball)
        {
            return ball.Accept(new BoundingBoxVisitor());
        }

        public Body Visit(RectangularCuboid rectangularCuboid)
        {
            return rectangularCuboid.Accept(new BoundingBoxVisitor());
        }

        public Body Visit(Cylinder cylinder)
        {
            return cylinder.Accept(new BoundingBoxVisitor());
        }

        public Body Visit(CompoundBody compoundBody)
        {
            List<Body> parts = new List<Body>();

            foreach (Body elem in compoundBody.Parts)
            {
                parts.Add(elem.Accept(new BoxifyVisitor()));
            }

            return new CompoundBody(parts);

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