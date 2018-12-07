using SD.LLBLGen.Pro.ORMSupportClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LhsParser.LLBLGenAdapter
{
    public class PredicateExpressionEvaluator : EvaluatorBase
    {
        public PredicateExpressionEvaluator(IDataConverter dataConverter, params EntityBase2[] entityBases) : base(dataConverter)
        {
            EntityBases = entityBases;
        }

        public PredicateExpressionEvaluator(params EntityBase2[] entityBases) : this(new DefaultDataConverter(), entityBases)
        {
        }

        public EntityBase2[] EntityBases { get; }

        protected override object And(object leftExpression, object rightExpression)
        {
            return new PredicateExpression(leftExpression as Predicate & rightExpression as Predicate);
        }

        protected override object Or(object leftExpression, object rightExpression)
        {
            return new PredicateExpression(leftExpression as Predicate | rightExpression as Predicate);
        }

        protected override object Equal(string fieldName, Token constant)
        {
            var field = GetField(fieldName);
            var value = constant.GetParsedValue(field.DataType);
            return new PredicateExpression(field == value);
        }

        protected override object GreaterThan(string fieldName, Token constant)
        {
            var field = GetField(fieldName);
            var value = constant.GetParsedValue(field.DataType);
            return new PredicateExpression(field > value);
        }

        protected override object GreaterThanOrEqual(string fieldName, Token constant)
        {
            var field = GetField(fieldName);
            var value = constant.GetParsedValue(field.DataType);
            return new PredicateExpression(field >= value);
        }

        protected override object LessThan(string fieldName, Token constant)
        {
            var field = GetField(fieldName);
            var value = constant.GetParsedValue(field.DataType);
            return new PredicateExpression(field < value);
        }

        protected override object LessThanOrEqual(string fieldName, Token constant)
        {
            var field = GetField(fieldName);
            var value = constant.GetParsedValue(field.DataType);
            return new PredicateExpression(field <= value);
        }

        protected override object NotEqual(string fieldName, Token constant)
        {
            var field = GetField(fieldName);
            var value = constant.GetParsedValue(field.DataType);
            return new PredicateExpression(field != value);
        }

        private EntityField2 GetField(string identifier)
        {
            //if (identifier.Contains('.'))
            //{
            //    var splitedParts = identifier.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            //    var entityName = splitedParts[splitedParts.Length - 2];
            //    var propertyName = splitedParts[splitedParts.Length - 1];

            //    EntityBases
            //}

            foreach (var field in EntityBases.SelectMany(x => x.Fields))
            {
                if (string.Equals(field.Name, identifier, StringComparison.OrdinalIgnoreCase))
                {
                    return field as EntityField2;
                }
            }

            throw new ArgumentException($"Field with name '{identifier}' was not found in given entities.");
        }

    }
}
