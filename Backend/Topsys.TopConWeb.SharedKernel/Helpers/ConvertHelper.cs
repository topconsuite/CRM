using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Topsys.TopConWeb.SharedKernel.Helpers
{
    public static class ConvertHelper
    {
        public static Expression<Func<T2, TResult>> ConvertExpression<T1, T2, TResult>(Expression<Func<T1, TResult>> expression)
        {
            var beforeParameter = expression.Parameters.Single();
            var afterParameter = Expression.Parameter(typeof(T2), beforeParameter.Name);
            var visitor = new SubstitutionExpressionVisitor(beforeParameter, afterParameter);
            return Expression.Lambda<Func<T2, TResult>>(visitor.Visit(expression.Body), afterParameter);
        }

        private class SubstitutionExpressionVisitor : ExpressionVisitor
        {
            private Expression before, after;
            public SubstitutionExpressionVisitor(Expression before, Expression after)
            {
                this.before = before;
                this.after = after;
            }
            public override Expression Visit(Expression node)
            {
                return node == before ? after : base.Visit(node);
            }
        }

    }
}
