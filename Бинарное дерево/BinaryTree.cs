using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Generics.BinaryTrees
{
    public class BinaryTree
    {
        public static BinaryTree<T> Create<T>(params T[] items) where T : IComparable
        {
            BinaryTree<T> tree = new BinaryTree<T>();
            foreach (var item in items)
                tree.Add(item);
            return tree;
        }
    }

    public class BinaryTree<T> : IEnumerable<T> 
        where T : IComparable
    {
        private bool initialized = false;
        private bool containsDuplicates = false;

        public BinaryTree<T> Left { get; private set; }
        public BinaryTree<T> Right { get; private set; }
        public T Value { get; private set; }

        public void Add(T newValue)
        {
            if (!initialized)
            {
                Value = newValue;
                initialized = true;
            }
            else if (newValue.CompareTo(Value) < 0 || newValue.CompareTo(Value) == 0)
            {
                if (Left == null)
                    Left = new BinaryTree<T>();
                Left.Add(newValue);
            }
            else if (newValue.CompareTo(Value) > 0)
            {
                if (Right == null)
                    Right = new BinaryTree<T>();
                Right.Add(newValue);
            }
            else if (newValue.CompareTo(Value) == 0)
            {
                containsDuplicates = true;
            }
        }

        public bool Contains(T value)
        {
            int comparison = value.CompareTo(Value);
            if (comparison == 0)
            {
                if (containsDuplicates)
                    return true;
                else
                    return false;
            }
            else if (comparison < 0 && Left != null)
            {
                return Left.Contains(value);
            }
            else if (comparison > 0 && Right != null)
            {
                return Right.Contains(value);
            }
            else
            {
                return false;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (Left != null)
            {
                foreach (var n in Left)
                {
                    yield return n;
                }
            }

            if (initialized)
            {
                yield return Value;

                
                
                    int count = 1;
                    while (count > 0)
                    {
                        yield return Value;
                        count--;
                    }
                
            }

            if (Right != null)
            {
                foreach (var n in Right)
                {
                    yield return n;
                }
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
