using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LhsBracketParser
{
    public abstract class EvaluatorBase : IEvaluator
    {
        private readonly IParser _parser;
        private readonly IDataConverter _dataConverter;

        protected EvaluatorBase(IParser parser, IDataConverter dataConverter)
        {
            if (parser == null)
                throw new ArgumentNullException(nameof(parser));

            if (dataConverter == null)
                throw new ArgumentNullException(nameof(dataConverter));

            _parser = parser;
            _dataConverter = dataConverter;
        }

        public EvaluatorBase() : this(new PostfixParser(), new DefaultDataConverter())
        {
        }

        public EvaluatorBase(IDataConverter dataConverter) : this(new PostfixParser(), dataConverter)
        {
        }

        public object Evaluate(string query)
        {
            var stack = new Stack<object>();

            var tokens = _parser.Parse(query);

            foreach (var token in tokens)
            {
                if (token.IsOperand())
                {
                    stack.Push(token);
                }
                else if (token.IsOperator())
                {
                    var operand2 = stack.Pop();
                    var operand1 = stack.Pop();

                    if (token.IsLogicalOperator())
                    {
                        if (token.Type == TokenType.And)
                        {
                            stack.Push(And(operand1, operand2));
                        }
                        else if (token.Type == TokenType.Or)
                        {
                            stack.Push(Or(operand1, operand2));
                        }
                        else
                        {
                            throw new InvalidOperationException("Logical operator type is not supported.");
                        }
                    }
                    else if (token.IsComparerOperator())
                    {
                        if (!(operand1 is Token) || (operand1 as Token).Type != TokenType.Identifier)
                        {
                            throw new InvalidSytaxException("Only a FieldName can be in left side of a compare operator.");
                        }

                        var fieldName = (operand1 as Token).Value;

                        if (!(operand2 is Token) || !(operand2 as Token).IsOperand())
                        {
                            throw new InvalidSytaxException("Right side of a compare operator can only be a field or constant.");
                        }

                        switch (token.Type)
                        {
                            case TokenType.Equal:
                                stack.Push(Equal(fieldName, operand2 as Token));
                                break;

                            case TokenType.NotEqual:
                                stack.Push(NotEqual(fieldName, operand2 as Token));
                                break;

                            case TokenType.LessThan:
                                stack.Push(LessThan(fieldName, operand2 as Token));
                                break;

                            case TokenType.LessThanOrEqual:
                                stack.Push(LessThanOrEqual(fieldName, operand2 as Token));
                                break;

                            case TokenType.GreaterThan:
                                stack.Push(GreaterThan(fieldName, operand2 as Token));
                                break;

                            case TokenType.GreaterThanOrEqual:
                                stack.Push(GreaterThanOrEqual(fieldName, operand2 as Token));
                                break;

                            case TokenType.Range:
                                stack.Push(Range(fieldName, operand2 as Token));
                                break;
                        }
                    }
                }
            }

            if (stack.Count != 1)
            {
                throw new ArgumentException("Query is not a valid expression.", nameof(query));
            }

            return stack.Pop();
        }

        protected abstract object Range(string fieldName, Token token);

        protected abstract object Equal(string fieldName, Token token);

        protected abstract object NotEqual(string fieldName, Token token);

        protected abstract object LessThan(string fieldName, Token token);

        protected abstract object LessThanOrEqual(string fieldName, Token token);

        protected abstract object GreaterThan(string fieldName, Token token);

        protected abstract object GreaterThanOrEqual(string fieldName, Token token);

        protected abstract object And(object leftExpression, object rightExpression);

        protected abstract object Or(object leftExpression, object rightExpression);
    }

}
