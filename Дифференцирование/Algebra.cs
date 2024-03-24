using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Reflection.Differentiation
{
    public class Algebra
    {
        public static Expression<Func<double, double>> Differentiate(Expression<Func<double, double>> function)
        {
            var nodeType = function.Body.NodeType;
            var param = function.Parameters;
            var body = function.Body;

            return Expression.Lambda<Func<double, double>>(
                        GetSwitch(nodeType, body), param);
        }
        public static Expression GetSwitch(ExpressionType nodeType, Expression body)
        {
            switch (nodeType)
            {
                case ExpressionType.Constant:
                    return Expression.Constant(0.0);

                case ExpressionType.Parameter:
                    return Expression.Constant(1.0);

                case ExpressionType.Multiply:
                    var binaryBody = (BinaryExpression)body;
                    return Expression.Add(
                            Expression.Multiply(binaryBody.Left, GetSwitch(binaryBody.Right.NodeType, binaryBody.Right)),
                            Expression.Multiply(binaryBody.Right, GetSwitch(binaryBody.Left.NodeType, binaryBody.Left)));

                case ExpressionType.Add:
                    binaryBody = (BinaryExpression)body;
                    return Expression.Add(
                        GetSwitch(binaryBody.Left.NodeType, binaryBody.Left),
                        GetSwitch(binaryBody.Right.NodeType, binaryBody.Right));

                case ExpressionType.Call:

                    var methodCall = (MethodCallExpression)body;
                    var fds = methodCall.Arguments[0];

                    if(methodCall.Method.Name == "Sin")
                    {
                        return Expression.Multiply(
                            Expression.Call(
                                null,
                                typeof(Math).GetMethod("Cos", new[] { typeof(double) }),
                                methodCall.Arguments[0]),
                            GetSwitch(methodCall.Arguments[0].NodeType, methodCall.Arguments[0]));
                    }
                    else if (methodCall.Method.Name == "Cos")
                    {                  
                        return Expression.Multiply(
                            Expression.Multiply(Expression.Call(
                                null,
                                typeof(Math).GetMethod("Sin", new[] { typeof(double) }),
                                methodCall.Arguments[0]), Expression.Constant(-1.0)),
                            GetSwitch(methodCall.Arguments[0].NodeType, methodCall.Arguments[0]));
                    }
                    else
                        throw new ArgumentException(($"function {body} not supported"));              
                default:
                    throw new ArgumentException(($"function {body} not supported"));
            }
        }
    }
}
