using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ShoppingCart.Models.Models.Entities;
using Simpro.Expr;

namespace SharedFunctions.LambdaGenerator
{
    public class ProductExpression
    {
        
        private ExprParser _Expression;
        public ProductExpression()
        {
            _Expression = new ExprParser();
        }

        public LambdaExpression Compare(string variable, string name, string category, decimal price)
        {
            return _Expression.Parse(
                variable + "=>(" + 
                "(" + name + " != null && !" + name + ".Equals('')) ? "+ variable + ".Name.Equals(" + name +") : true)" +
                "&& ((" + category + " != null && !" + category + ".Equals('')) ? " + variable + ".Category.Name.Equals(" + category + ") : true)" +
                "&& ((" + price + " > 0) ? " + variable + ".Price == " + price + " : true)" + 
                ")"
                );
        }
    }
}
