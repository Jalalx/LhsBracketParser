using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace LhsBracketParser.LinqProvider
{
    public class LinqExpressionEvaluator<T> : EvaluatorBase where T : class
    {
        public LinqExpressionEvaluator(IDataConverter dataConverter)
        {
            ModelType = typeof(T);
            DataConverter = dataConverter;
        }

        public LinqExpressionEvaluator() : this(new DefaultDataConverter())
        {
        }

        public Type ModelType { get; }

        public IDataConverter DataConverter { get; }

        protected virtual PropertyInfo GetProperty(string propertyName)
        {
            var property = ModelType.GetProperty(propertyName, BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
            if (property == null)
            {
                throw new ArgumentException($"Property '{propertyName}' in type '{ModelType}' not found.");
            }

            return property;
        }

        protected Expression GetPropertyExpression(string propertyName)
        {
            var param = Expression.Parameter(ModelType, "x");
            var body = Expression.PropertyOrField(param, propertyName);

            return Expression.Lambda(body, param);
        }

        public new Func<T, bool> Evaluate(string query)
        {
            var predicate = base.Evaluate(query) as Expression<Func<T, bool>>;
            return predicate.Compile();
        }

        protected override object And(object leftExpression, object rightExpression)
        {
            return Expression.And(leftExpression as Expression, rightExpression as Expression);
        }

        protected override object Equal(string fieldName, Token token)
        {
            var propertyInfo = GetProperty(fieldName);
            var value = DataConverter.Convert(propertyInfo.PropertyType, token);

            var param = Expression.Parameter(ModelType, "x");
            var constant = Expression.Constant(value);

            var fieldExpression = Expression.PropertyOrField(param, fieldName);
            var equalExpression = Expression.Equal(fieldExpression, constant);

            return (Expression<Func<T, bool>>)Expression.Lambda(equalExpression, param);
        }


        protected override object GreaterThan(string fieldName, Token token)
        {
            var propertyInfo = GetProperty(fieldName);
            var value = DataConverter.Convert(propertyInfo.PropertyType, token);

            var param = Expression.Parameter(ModelType, "x");
            var constant = Expression.Constant(value);

            var fieldExpression = Expression.PropertyOrField(param, fieldName);
            var equalExpression = Expression.GreaterThan(fieldExpression, constant);

            return (Expression<Func<T, bool>>)Expression.Lambda(equalExpression, param);
        }

        protected override object GreaterThanOrEqual(string fieldName, Token token)
        {
            var propertyInfo = GetProperty(fieldName);
            var value = DataConverter.Convert(propertyInfo.PropertyType, token);

            var param = Expression.Parameter(ModelType, "x");
            var constant = Expression.Constant(value);

            var fieldExpression = Expression.PropertyOrField(param, fieldName);
            var equalExpression = Expression.GreaterThanOrEqual(fieldExpression, constant);

            return (Expression<Func<T, bool>>)Expression.Lambda(equalExpression, param);
        }

        protected override object LessThan(string fieldName, Token token)
        {
            var propertyInfo = GetProperty(fieldName);
            var value = DataConverter.Convert(propertyInfo.PropertyType, token);

            var param = Expression.Parameter(ModelType, "x");
            var constant = Expression.Constant(value);

            var fieldExpression = Expression.PropertyOrField(param, fieldName);
            var equalExpression = Expression.LessThan(fieldExpression, constant);

            return (Expression<Func<T, bool>>)Expression.Lambda(equalExpression, param);
        }

        protected override object LessThanOrEqual(string fieldName, Token token)
        {
            var propertyInfo = GetProperty(fieldName);
            var value = DataConverter.Convert(propertyInfo.PropertyType, token);

            var param = Expression.Parameter(ModelType, "x");
            var constant = Expression.Constant(value);

            var fieldExpression = Expression.PropertyOrField(param, fieldName);
            var equalExpression = Expression.LessThanOrEqual(fieldExpression, constant);

            return (Expression<Func<T, bool>>)Expression.Lambda(equalExpression, param);
        }


        protected override object NotEqual(string fieldName, Token token)
        {
            var propertyInfo = GetProperty(fieldName);
            var value = DataConverter.Convert(propertyInfo.PropertyType, token);

            var param = Expression.Parameter(ModelType, "x");
            var constant = Expression.Constant(value);

            var fieldExpression = Expression.PropertyOrField(param, fieldName);
            var equalExpression = Expression.NotEqual(fieldExpression, constant);

            return (Expression<Func<T, bool>>)Expression.Lambda(equalExpression, param);
        }

        protected override object Or(object leftExpression, object rightExpression)
        {
            return Expression.Or(leftExpression as Expression, rightExpression as Expression);
        }

        protected override object Like(string fieldName, Token token)
        {
            var containsMethodInfo = typeof(string).GetMethod(nameof(string.Contains), BindingFlags.Public | BindingFlags.Instance);

            var propertyInfo = GetProperty(fieldName);
            var value = DataConverter.Convert(propertyInfo.PropertyType, token);

            var param = Expression.Parameter(ModelType, "x");
            var constant = Expression.Constant(value);

            var fieldExpression = Expression.PropertyOrField(param, fieldName);
            var callExpression = Expression.Call(fieldExpression, containsMethodInfo, constant);

            return (Expression<Func<T, bool>>)Expression.Lambda(callExpression, param);
        }

        protected override object Range(string fieldName, Token token)
        {
            throw new NotImplementedException();
            //var propertyInfo = GetProperty(fieldName);
            //var value = DataConverter.Convert(propertyInfo.PropertyType, token);

            //var param = Expression.Parameter(ModelType, "x");
            //var constant = Expression.Constant(value);

            //var fieldExpression = Expression.PropertyOrField(param, fieldName);
            //var equalExpression = Expression.Equal(fieldExpression, constant);

            //return (Expression<Func<T, bool>>)Expression.Lambda(equalExpression, param);
        }
    }
}
