using Reflection.Randomness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Reflection.Randomness
{
    public class Generator<T>
    {
        private Dictionary<PropertyInfo, FromDistribution> attributsDictionary;

        public Type Type { get; }
        public Generator()
        {
            Type = typeof(T);

            var resultDict = new Dictionary<PropertyInfo, FromDistribution>();
            foreach (var property in Type.GetProperties())
            {
                var attribute = property
                    .GetCustomAttributes(false)
                    .OfType<FromDistribution>()
                    .FirstOrDefault();

                if (attribute != null)
                {
                    resultDict[property] = attribute;
                }
            }
            attributsDictionary = resultDict;
        }

        public T Generate(Random rnd)
        {
            ConstructorInfo ctor = Type.GetConstructor(new Type[] { });
            var result = ctor.Invoke(new object[] { });

            foreach (var atr in attributsDictionary)
            {
                if (atr.Value.typeValue == typeof(NormalDistribution))
                {
                    atr.Key.SetValue(result, new NormalDistribution(atr.Value.ValueOne, atr.Value.ValueTwo).Generate(rnd));
                }
                else if (atr.Value.typeValue == typeof(ExponentialDistribution))
                {
                    atr.Key.SetValue(result, new ExponentialDistribution(atr.Value.ValueOne).Generate(rnd));
                }
            }
            return (T)result;
        }
    }

    public class FromDistribution : Attribute
    {
        public Type typeValue { get; }
        public double ValueOne { get; }
        public double ValueTwo { get; }

        public FromDistribution(Type typeValue, params int[] param)
        {
            if (typeValue != typeof(NormalDistribution) && typeValue != typeof(ExponentialDistribution))
                throw new ArgumentException($"Type {typeValue} not suitable for distributor");
            if (param.Length > 2)
                throw new ArgumentException($"For NormalDistribution 2 parameters required");

            this.typeValue = typeValue;
            if (param.Length == 0)
            {
                ValueOne = 0;
                ValueTwo = 1;
            }
            if (typeValue == typeof(ExponentialDistribution))
                ValueOne = param[0];
            if (typeValue == typeof(NormalDistribution) && param.Length != 0)
            {
                ValueOne = param[0];
                ValueTwo = param[1];
            }
        }
    }
}
