using System.Linq.Expressions;

namespace RuiSantos.AutoFill.Infrastructure.Repositories.LiteDb.Extensions;

internal static class ExpressionExtensions
{
    public static Expression<Func<TDestination, bool>>? Convert<TSource, TDestination>(this Expression<Func<TSource, bool>>? source)
    {
        if (source == null) return null;

        var parameter = Expression.Parameter(typeof(TDestination), source.Parameters[0].Name);
        var body = new ExpressionConverter(parameter).Visit(source.Body);
        return Expression.Lambda<Func<TDestination, bool>>(body!, parameter);
    }

    private class ExpressionConverter : ExpressionVisitor
    {
        private readonly ParameterExpression _parameter;

        public ExpressionConverter(ParameterExpression parameter)
        {
            _parameter = parameter;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return _parameter;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            return node.Expression is { NodeType: ExpressionType.Parameter } 
                ? Expression.PropertyOrField(_parameter, node.Member.Name) 
                : base.VisitMember(node);
        }
    }
}