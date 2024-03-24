using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Delegates.TreeTraversal
{
    public static class Traversal
    {
        public static IEnumerable<Product> GetProducts(ProductCategory root)
        {
            //if (root == null)
            //{
            //    yield break;
            //}

            //var stackProduct = new Stack<ProductCategory>();
            //var current = root;

            //while (current != null || stackProduct.Count > 0)
            //{
            //    while (current != null)
            //    {
            //        stackProduct.Push(current);
            //        current = current.Categories?.FirstOrDefault();
            //    }

            //    current = stackProduct.Pop();

            //    if (current.Products != null)
            //    {
            //        foreach (var product in current.Products)
            //        {
            //            yield return product;
            //        }
            //    }
            //    current = current.Categories?.LastOrDefault();
            //}

            return GeneralMethod(root,
                (current) => current.Categories?.FirstOrDefault(),
                (current) => current.Categories?.LastOrDefault(),
                (current) => current.Products != null,
                (current) => current.Products);

        }

        public static IEnumerable<Job> GetEndJobs(Job root)
        {
            //if (root == null)
            //{
            //    yield break;
            //}

            //var stackJobs = new Stack<Job>();
            //var current = root;

            //while (current != null || stackJobs.Count > 0)
            //{
            //    while (current != null)
            //    {
            //        stackJobs.Push(current);
            //        current = current.Subjobs?.FirstOrDefault();
            //    }

            //    current = stackJobs.Pop();
            //    if (current.Subjobs == null || current.Subjobs.Count == 0)
            //    {
            //        yield return current;
            //    }
            //    current = current.Subjobs?.LastOrDefault();
            //}

            return GeneralMethod(root,
                (current) => current.Subjobs?.FirstOrDefault(),
                (current) => current.Subjobs?.LastOrDefault(),
                (current) => current.Subjobs == null || current.Subjobs.Count == 0,
                (current) => new List<Job> { current });

        }

        public static IEnumerable<T> GetBinaryTreeValues<T>(BinaryTree<T> root)
        {
            //if (root == null)
            //{
            //    yield break;
            //}

            //var stackBinaryTree = new Stack<BinaryTree<T>>();
            //var current = root;

            //while (current != null || stackBinaryTree.Count > 0)
            //{
            //    while (current != null)
            //    {
            //        stackBinaryTree.Push(current);
            //        current = current.Left;
            //    }

            //    current = stackBinaryTree.Pop();
            //    if (current.Left == null && current.Right == null)
            //    {       
            //           yield return current.Value;               
            //    }
            //    current = current.Right;
            //}
            return GeneralMethod(root, 
                (current) => current.Left, 
                (current) => current.Right, 
                (current) => current.Left == null && current.Right == null, 
                (current) => new List<T> { current.Value });
        }

        public static IEnumerable<TResult> GeneralMethod<T, TResult>(T root, Func<T, T> OneSet, Func<T, T> TwoSet, Func<T, bool> Ñondition, Func<T, IEnumerable<TResult>> GetValue)
        {

            if (root == null)
            {
                yield break;
            }

            var stackBinaryTree = new Stack<T>();
            var current = root;

            while (current != null || stackBinaryTree.Count > 0)
            {
                while (current != null)
                {
                    stackBinaryTree.Push(current);
                    current = OneSet(current);
                }

                current = stackBinaryTree.Pop();
                if (Ñondition(current))
                {                 
                    foreach (var value in GetValue(current))
                    {
                        yield return value;
                    }
                }
                current = TwoSet(current);
            }
        }
    }
}
