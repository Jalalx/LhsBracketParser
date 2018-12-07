using System.Collections.Generic;
using System.IO;

namespace LhsParser
{
    public class DefaultParser
    {
        public static readonly IReadOnlyDictionary<TokenType, int> TokenOrder = new Dictionary<TokenType, int>
        {
            {  TokenType.ParenthesesOpen, 20 },
            {  TokenType.ParenthesesClose, 0 },

            {  TokenType.Equal, 10 },
            {  TokenType.NotEqual, 10 },
            {  TokenType.GreaterThan, 10 },
            {  TokenType.GreaterThanOrEqual, 10 },
            {  TokenType.LessThan, 10 },
            {  TokenType.LessThanOrEqual, 10 },

            {  TokenType.And, 5 },
            {  TokenType.Or, 1 },

            {  TokenType.Boolean, 0 },
            {  TokenType.Date, 0 },
            {  TokenType.EndOfLine, 0 },
            {  TokenType.Identifier, 0 },
            {  TokenType.Number, 0 },
            {  TokenType.String, 0 },
        };

        // Read more: http://condor.depaul.edu/ichu/csc415/notes/notes9/Infix.htm

        public IEnumerable<Token> Postfix(string query)
        {
            var operatorStack = new Stack<Token>();
            var operandQueue = new Queue<Token>();

            using (var reader = new StringReader(query))
            using (var tokenizer = new Tokenizer(reader))
            {
                while (tokenizer.CanRead())
                {
                    var currentToken = tokenizer.NextToken();
                    if (currentToken.IsOperand())
                    {
                        operandQueue.Enqueue(currentToken);
                    }
                    else if (currentToken.IsOperator() || currentToken.IsParenthesis())
                    {
                        //  Pop the stack until you find a symbol of lower priority number than the current one.
                        while (operatorStack.Count > 0)
                        {
                            var popedOperator = operatorStack.Pop();
                            if (TokenOrder[popedOperator.Type] < TokenOrder[currentToken.Type])
                            {
                                // The popped stack elements will be written to output. 
                                operandQueue.Enqueue(popedOperator);
                            }
                        }

                        // An incoming left parenthesis will be considered to have higher priority than any 
                        // other symbol. A left parenthesis on the stack will not be removed unless an incoming right parenthesis is found.
                        // Stack the current symbol.
                        operatorStack.Push(currentToken);

                        // If a right parenthesis is the current symbol, pop the stack down to (and including) 
                        // the first left parenthesis. Write all the symbols except the left parenthesis to the output (i.e. write the operators to the output). 
                        if (currentToken.Type == TokenType.ParenthesesClose)
                        {
                            while (operatorStack.Count > 0)
                            {
                                var popedOperator = operatorStack.Pop();
                                if (popedOperator.Type == TokenType.ParenthesesOpen)
                                {
                                    break;
                                }

                                operandQueue.Enqueue(popedOperator);
                            }
                        }

                    }
                }

                // After the last token is read, pop the remainder of the stack and write any symbol (except left parenthesis) to output. 
                while (operatorStack.Count > 0)
                {
                    var popedToken = operatorStack.Pop();
                    operandQueue.Enqueue(popedToken);
                }
            }

            return operandQueue;
        }
    }
}
