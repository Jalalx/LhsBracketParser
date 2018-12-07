using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LhsParser
{
    public class DefaultParser
    {
        public static readonly IReadOnlyDictionary<TokenType, int> TokenOrder = new Dictionary<TokenType, int>
        {
            {  TokenType.ParenthesesOpen, 0 },
            {  TokenType.ParenthesesClose, 0 },

            {  TokenType.Equal, 10 },
            {  TokenType.NotEqual, 10 },
            {  TokenType.GreaterThan, 10 },
            {  TokenType.GreaterThanOrEqual, 10 },
            {  TokenType.LessThan, 10 },
            {  TokenType.LessThanOrEqual, 10 },

            {  TokenType.And, 9 },
            {  TokenType.Or, 8 },

            {  TokenType.Boolean, 0 },
            {  TokenType.Date, 0 },
            {  TokenType.EndOfLine, 0 },
            {  TokenType.Identifier, 0 },
            {  TokenType.Number, 0 },
            {  TokenType.String, 0 },
        };
        
        /// <summary>
        /// Converts a string statement to the Postfix form. Read more about the algorithm in https://www.geeksforgeeks.org/stack-set-2-infix-to-postfix/
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IEnumerable<Token> Postfix(string query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            if (string.IsNullOrWhiteSpace(query))
            {
                return Enumerable.Empty<Token>();
            }

            var operatorStack = new Stack<Token>();
            var operandQueue = new Queue<Token>();

            using (var reader = new StringReader(query))
            using (var tokenizer = new Tokenizer(reader))
            {
                while (tokenizer.CanRead())
                {
                    var currentToken = tokenizer.NextToken();
                    if (currentToken == null)
                    {
                        // The rest of string is white space, tabs or new lines.
                        break;
                    }
                    else if (currentToken.IsOperand())
                    {
                        operandQueue.Enqueue(currentToken);
                    }
                    else if (currentToken.Type == TokenType.ParenthesesOpen)
                    {
                        operatorStack.Push(currentToken);
                    }
                    else if (currentToken.Type == TokenType.ParenthesesClose)
                    {
                        while (operatorStack.Count > 0 && operatorStack.Peek().Type != TokenType.ParenthesesOpen)
                        {
                            var popedToken = operatorStack.Pop();
                            operandQueue.Enqueue(popedToken);
                        }

                        if (operatorStack.Count > 0 && operatorStack.Peek().Type != TokenType.ParenthesesOpen)
                        {
                            throw new InvalidSytaxException("There is a right parenthesis ')' that doesn't have any corresponding left parenthesis '('.");
                        }
                        else if (operatorStack.Count > 0)
                        {
                            operatorStack.Pop();
                        }
                    }
                    else
                    {
                        while (operatorStack.Count > 0 && TokenOrder[currentToken.Type] <= TokenOrder[operatorStack.Peek().Type])
                        {
                            var popedToken = operatorStack.Pop();
                            operandQueue.Enqueue(popedToken);
                        }

                        operatorStack.Push(currentToken);
                    }
                }

                // After the last token is read, pop the remainder of the stack and write any symbol (except left parenthesis) to output. 
                while (operatorStack.Count > 0)
                {
                    var popedToken = operatorStack.Pop();
                    operandQueue.Enqueue(popedToken);
                }
            }

            var result = new List<Token>();
            while (operandQueue.Count > 0)
            {
                result.Add(operandQueue.Dequeue());
            }

            return result;
        }
    }
}
