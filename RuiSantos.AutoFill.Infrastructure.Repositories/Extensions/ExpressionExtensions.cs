using System.Linq.Expressions;

namespace RuiSantos.AutoFill.Infrastructure.Repositories.Extensions;

/// <summary>
/// Provides extension methods for working with expressions.
/// </summary>
public static class ExpressionExtensions
{
    /// <summary>
    /// Converts an expression of type <typeparamref name="TSource"/> to an expression of type <typeparamref name="TDestination"/>.
    /// </summary>
    /// <typeparam name="TSource">The source type of the expression.</typeparam>
    /// <typeparam name="TDestination">The destination type of the expression.</typeparam>
    /// <param name="source">The source expression to convert.</param>
    /// <returns>An expression of type <typeparamref name="TDestination"/> if the source is not null; otherwise, null.</returns>
    public static Expression<Func<TDestination, bool>>? Convert<TSource, TDestination>(this Expression<Func<TSource, bool>>? source)
    {
        if (source == null) return null;

        var parameter = Expression.Parameter(typeof(TDestination), source.Parameters[0].Name);
        var body = new ExpressionConverter(parameter).Visit(source.Body);
        return Expression.Lambda<Func<TDestination, bool>>(body!, parameter);
    }

    /// <summary>
    /// A visitor that converts expressions by replacing parameters.
    /// </summary>
    private class ExpressionConverter : ExpressionVisitor
    {
        private readonly ParameterExpression _parameter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionConverter"/> class.
        /// </summary>
        /// <param name="parameter">The parameter expression to replace with.</param>
        public ExpressionConverter(ParameterExpression parameter)
        {
            _parameter = parameter;
        }

        /// <summary>
        /// Visits the <see cref="ParameterExpression"/> and replaces it with the provided parameter.
        /// </summary>
        /// <param name="node">The parameter expression to visit.</param>
        /// <returns>The replaced parameter expression.</returns>
        protected override Expression VisitParameter(ParameterExpression node)
        {
            return _parameter;
        }

        /// <summary>
        /// Visits the <see cref="MemberExpression"/> and replaces it if it is a parameter expression.
        /// </summary>
        /// <param name="node">The member expression to visit.</param>
        /// <returns>The replaced member expression if it is a parameter; otherwise, the base visit result.</returns>
        protected override Expression VisitMember(MemberExpression node)
        {
            return node.Expression is { NodeType: ExpressionType.Parameter } 
                ? Expression.PropertyOrField(_parameter, node.Member.Name) 
                : base.VisitMember(node);
        }
    }
}