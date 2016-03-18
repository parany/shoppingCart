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
        private LambdaExpression _lambda;
        public ProductExpression()
        {
            _Expression = new ExprParser();
        }

        private void genLambda()
        {

            _lambda =  _Expression.Parse(
                "(Produit p,  string name, string category, decimal price) => " +
                "(" + 
                "(name != null && !name.Equals('')) ? p.Name.Equals(name) : true)" +
                "&& ((category != null && !category.Equals('')) ? p.Category.Name.Equals(category) : true)" +
                "&& (( price> 0) ? p.Price == price : true)" + 
                ")"
                );
        }

        public bool RunExpression(Product produit, string name, string category, decimal price)
        {
            genLambda();
            return (bool)_Expression.Run(_lambda, produit, name, category, price);
        }

    }
}
