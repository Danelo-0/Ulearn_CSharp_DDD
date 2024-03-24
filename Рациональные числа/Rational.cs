using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Incapsulation.RationalNumbers
{
    public class Rational
    {
        private int numerator;
        public int Numerator
        {
            get
            {
                return numerator;
            }
            set
            {
                this.numerator = value;
            }
        }
        private int denominator;
        public int Denominator
        {
            get
            {
                return denominator;
            }
            set
            {
                this.denominator = value;
            }
        }

        private bool isNan;
        public bool IsNan
        {
            get
            {
                return isNan;
            }
            set
            {
                this.isNan = value;
            }
        }

        public Rational(int numerator, int denominator = 1)
        {
            if (denominator == 0)
            {
                this.isNan = true;
            }
            if (numerator == 0)
            {
                this.denominator = 1;
            }
            else 
            {
                int greatestCommonDivisor = EuclidsAlgorithm(numerator, denominator);
                this.denominator = denominator / greatestCommonDivisor;
                this.numerator = numerator / greatestCommonDivisor;
            }

        }

        private int EuclidsAlgorithm(int numerator, int denominator)
        {
            var constNumerator = numerator;
            var constDenominator = denominator;
            while (denominator != 0)
            {
                var temp = denominator;
                denominator = numerator % denominator;
                numerator = temp;
            }
            if (constNumerator < 0 && !(constDenominator < 0))
            {
                return Math.Abs(numerator);
            }
            else if ((constNumerator > 0 && constDenominator < 0) || (constNumerator < 0 && constDenominator < 0))
            {
                return Math.Abs(numerator) * -1;
            }
            else 
            { 
                return Math.Abs(numerator); 
            }
        }

        public static Rational operator+ (Rational rationalOne, Rational rationalTwo)
        {
            if (rationalOne.denominator == 0 || rationalTwo.denominator == 0)
            {
                return new Rational(0, 0);
            }
            else if (rationalOne.denominator != rationalTwo.denominator)
            {
                return new Rational((rationalOne.numerator * rationalTwo.denominator) + (rationalTwo.numerator * rationalOne.denominator), rationalOne.denominator * rationalTwo.denominator);
            }
            else
            {
                return new Rational(rationalOne.numerator + rationalTwo.numerator, rationalOne.denominator);
            }
        }

        public static Rational operator- (Rational rationalOne, Rational rationalTwo)
        {
            if (rationalOne.denominator == 0 || rationalTwo.denominator == 0)
            {
                return new Rational(0, 0);
            }
            else if (rationalOne.denominator != rationalTwo.denominator)
            {
                return new Rational((rationalOne.numerator * rationalTwo.denominator) - (rationalTwo.numerator * rationalOne.denominator), rationalOne.denominator * rationalTwo.denominator);
            }
            else
            {
                return new Rational(rationalOne.numerator - rationalTwo.numerator, rationalOne.denominator);
            }
        }

        public static Rational operator* (Rational rationalOne, Rational rationalTwo)
        {
            if (rationalOne.denominator == 0 || rationalTwo.denominator == 0)
            {
                return new Rational(0, 0);
            }
            else
            {
                return new Rational(rationalOne.numerator * rationalTwo.numerator, rationalOne.denominator * rationalTwo.denominator);
            }
           
        }

        public static Rational operator/ (Rational rationalOne, Rational rationalTwo)
        {
            if (rationalOne.denominator == 0 || rationalTwo.denominator == 0 || rationalTwo.numerator == 0)
            {
                return new Rational(0, 0);
            }
            else
            {
                return new Rational(rationalOne.numerator / rationalTwo.numerator, rationalOne.denominator / rationalTwo.denominator);
            }
        }

        public static implicit operator double(Rational rational)
        {
            if(rational.denominator == 0 || rational.numerator == 0)
            {
                return double.NaN;
            }
            else
            {
                double result = (double)rational.numerator / (double)rational.denominator;
                return result;
            }         
        }

        public static implicit operator Rational(int intt)
        {
            return new Rational(intt);
        }

        public static implicit operator int(Rational rational)
        {
            if (rational.denominator == 0 || rational.numerator == 0)
            {
                return 0;
            }
            else if (rational.numerator % rational.denominator != 0)
            {
                throw new Exception();
            }
            else
            {
                int result = rational.numerator / rational.denominator;
                return result;
            }
        }
    }
}
 