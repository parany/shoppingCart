using System;
using System.Linq.Expressions;
using Simpro.Expr;

namespace SharedFunctions.LambdaGenerator
{
    class GenericLambda
    {
        private ExprParser _Expression;
        private LambdaExpression _lambda;
        public GenericLambda()
        {
            _Expression = new ExprParser();
        }

        public void GenLambda(string s)
        {

            _lambda = _Expression.Parse(s);
        }

        public Object RunExpression(Object obj, string name, string category, decimal price)
        {
            return _Expression.Run(_lambda, obj, name, category, price);
        }

        public LambdaExpression GetExpression()
        {
            return _lambda;
        }
    }
}
