using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
namespace ONP_lvl_hard
{
    class Program
    {
        public static bool nopwaswritten = false;
        public static string resultofONP = string.Empty;
        public static int n;
        public static float x_inputed, x_min, x_max, step;
        const string InvalidMessage = "Invalid input";
        static void Main(string[] args)
        {
            string sentence;                //"(1+sin(pi/2)-(7+sqrt(9)))"
            sentence = Console.ReadLine();
            space_counter(sentence);
            
            {
                int index = sentence.IndexOf(' ') + 1;
                
                string holder = null;
                for (; ; index++)
                {
                    if (sentence[index] == ' ')
                    {
                        break;
                    }
                    holder = string.Concat(holder, sentence[index]);
                    if (sentence[index] != '0' && sentence[index] != '1' && sentence[index] != '2' && sentence[index] != '3' &&
                        sentence[index] != '4' && sentence[index] != '5' && sentence[index] != '6' && sentence[index] != '7' &&
                        sentence[index] != '8' && sentence[index] != '9' && sentence[index] != '.')
                    {
                        throw new ArgumentException(InvalidMessage);
                    }
                }
                if (holder == null)
                {
                    throw new ArgumentException(InvalidMessage);
                }
                index++;
                holder = holder.Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                x_inputed = float.Parse(holder);
                holder = null;
                for (; ; index++)
                {
                    if (sentence[index] == ' ')
                    {
                        break;
                    }
                    holder = string.Concat(holder, sentence[index]);
                    if (sentence[index] != '0' && sentence[index] != '1' && sentence[index] != '2' && sentence[index] != '3' &&
                        sentence[index] != '4' && sentence[index] != '5' && sentence[index] != '6' && sentence[index] != '7' &&
                        sentence[index] != '8' && sentence[index] != '9' && sentence[index] != '.')
                    {
                        throw new ArgumentException(InvalidMessage);
                    }
                }
                if (holder == null)
                {
                    throw new ArgumentException(InvalidMessage);
                }
                index++;
                holder = holder.Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                x_min = float.Parse(holder);
                holder = null;
                for (; ; index++)
                {
                    if (sentence[index] == ' ')
                    {
                        break;
                    }
                    holder = string.Concat(holder, sentence[index]);
                    if (sentence[index] != '0' && sentence[index] != '1' && sentence[index] != '2' && sentence[index] != '3' &&
                        sentence[index] != '4' && sentence[index] != '5' && sentence[index] != '6' && sentence[index] != '7' &&
                        sentence[index] != '8' && sentence[index] != '9' && sentence[index] != '.')
                    {
                        throw new ArgumentException(InvalidMessage);
                    }
                }
                if (holder == null)
                {
                    throw new ArgumentException(InvalidMessage);
                }
                index++;
                holder = holder.Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                x_max = float.Parse(holder);
                holder = null;
                for (; ; index++)
                {
                    if (index == sentence.Length)
                    {
                        break;
                    }
                    holder = string.Concat(holder, sentence[index]);
                    if (sentence[index] != '0' && sentence[index] != '1' && sentence[index] != '2' && sentence[index] != '3' &&
                        sentence[index] != '4' && sentence[index] != '5' && sentence[index] != '6' && sentence[index] != '7' &&
                        sentence[index] != '8' && sentence[index] != '9')
                    {
                        throw new ArgumentException(InvalidMessage);
                    }
                }
                if (holder == null)
                {
                    throw new ArgumentException(InvalidMessage);
                }
                n = int.Parse(holder);
            }

            step = (x_max - x_min) / (n-1);
            sentence = sentence.Remove(sentence.IndexOf(" "));
            Console.WriteLine(sentence);
            for(int loop=0;loop<=n ;loop++)
            {
                if (loop >1)
                {
                    x_inputed +=step;
                }
                Interpreter Inp = new Interpreter();
                double result = Inp.Calculate(sentence);
                Console.WriteLine(x_inputed + " => " + result);
                if (loop == 0)
                {
                    x_inputed = x_min;
                }
                
                
            }
            
            void space_counter(string sentence)
            {
                const char V = ' ';
                try
                {
                    if (sentence.Count(x => x == V) != 4)
                    {
                        throw new ArgumentException(InvalidMessage);
                    }
                }
                catch (ArgumentException)
                {

                }
                


            }
        }


        public class Interpreter
        {

            enum Side
            {
                Left, Right
            }
            public double Calculate(string input)
            {
                if (input == null)
                    throw new ArgumentNullException("input");
                if (input == string.Empty)
                    throw new ArgumentException(InvalidMessage);
                string expression = FormatExpression(input);
                TokenFactory tf = new TokenFactory();
                Queue<Token> postfix = GetPostFix(expression, tf);
                if (nopwaswritten == false)
                {
                    nopwaswritten = true;
                    Queue<Token> helper = GetPostFix(expression, tf);
                    while (helper.Count > 0)
                    {
                        Token tokenholder = helper.Dequeue();
                        if(tokenholder is NumberBase)
                        {
                            if(Convert.ToString(tokenholder)== "ONP_lvl_hard.Program+Interpreter+variable")
                            {
                                resultofONP = resultofONP + tokenholder + ' ';
                            }
                            else
                            {
                                NumberBase valueholder = (NumberBase)tokenholder;
                                resultofONP = resultofONP + valueholder.Value + ' ';
                            }
                            
                        }
                        else
                        {
                            if (helper.Count != 0)
                            {
                                resultofONP = resultofONP + tokenholder + ' ';
                            }
                            else
                            {
                                resultofONP = resultofONP + tokenholder;
                            }
                        }
                    }
                    ONPwriter(resultofONP);
                }
                
                return ProcessPostfix(postfix);
            }

            Queue<Token> GetPostFix(string input, TokenFactory tokenFactory)
            {
                Queue<Token> output = new Queue<Token>();
                Stack<Token> stack = new Stack<Token>();
                int position = 0;
                while (position < input.Length)
                {
                    Token token = GetNextToken(ref position, input, tokenFactory);
                    if (token == null)
                        break;
                    if (token is NumberBase)
                        output.Enqueue(token);
                    else if (token is FunctionBase)
                        stack.Push(token);
                    else if (token is LeftBracket)
                        stack.Push(token);
                    else if (token is RightBracket)
                    {
                        while (true)
                        {
                            Token taken = stack.Pop();
                            if (!(taken is LeftBracket))
                                output.Enqueue(taken);
                            else
                            {
                                break;
                            }
                        }
                    }
                    else if (token is OperatorBase)
                    {
                        if (stack.Count > 0)
                        {
                            Token top = stack.Peek();
                            bool nested = true;
                            while (nested)
                            {
                                if (top == null || !(top is OperatorBase))
                                    break;
                                OperatorBase o1 = (OperatorBase)token;
                                OperatorBase o2 = (OperatorBase)top;
                                if (o1.Side == Side.Left && (o2.Precedence >= o1.Precedence))
                                    output.Enqueue(stack.Pop());
                                else if (o2.Side == Side.Right && (o2.Precedence > o1.Precedence))
                                    output.Enqueue(stack.Pop());
                                else
                                    nested = false;
                                top = (stack.Count > 0) ? stack.Peek() : null;
                            }
                        }
                        stack.Push(token);
                    }
                }
                while (stack.Count > 0)
                {
                    Token next = stack.Pop();
                    if (next is LeftBracket || next is RightBracket)
                    {
                        throw new ArgumentException(InvalidMessage);
                    }
                    output.Enqueue(next);
                }
                return output;
            }
            public void ONPwriter(string resultofONP)
            {
                nopwaswritten = true;
                resultofONP = resultofONP.Replace("ONP_lvl_hard.Program+Interpreter+Addition", "+");
                resultofONP = resultofONP.Replace("ONP_lvl_hard.Program+Interpreter+Subtraction", "-");
                resultofONP = resultofONP.Replace("ONP_lvl_hard.Program+Interpreter+Multiplication", "*");
                resultofONP = resultofONP.Replace("ONP_lvl_hard.Program+Interpreter+Division", "/");
                resultofONP = resultofONP.Replace("ONP_lvl_hard.Program+Interpreter+Power", "^");

                resultofONP = resultofONP.Replace("ONP_lvl_hard.Program+Interpreter+Sinus", "sin");
                resultofONP = resultofONP.Replace("ONP_lvl_hard.Program+Interpreter+Cosinus", "cos");
                resultofONP = resultofONP.Replace("ONP_lvl_hard.Program+Interpreter+Tangens", "tan");
                resultofONP = resultofONP.Replace("ONP_lvl_hard.Program+Interpreter+Cotangens", "cot");
                resultofONP = resultofONP.Replace("ONP_lvl_hard.Program+Interpreter+Sqrt", "sqrt");
                resultofONP = resultofONP.Replace("ONP_lvl_hard.Program+Interpreter+Exp", "exp");

                resultofONP = resultofONP.Replace("ONP_lvl_hard.Program+Interpreter+E", "e");
                resultofONP = resultofONP.Replace("3,141592653589793", "pi");
                resultofONP = resultofONP.Replace("ONP_lvl_hard.Program+Interpreter+variable", "x");
                Console.WriteLine(resultofONP);
            }

            double ProcessPostfix(Queue<Token> postfix)
            {
                Stack<Token> stack = new Stack<Token>();
                Token token = null;
                while (postfix.Count > 0)
                {
                    token = postfix.Dequeue();
                    if (token is NumberBase)
                        stack.Push(token);
                    else if (token is OperatorBase)
                    {
                        NumberBase right = (NumberBase)stack.Pop();
                        NumberBase left = (NumberBase)stack.Pop();
                        double value = ((OperatorBase)token).Calculate(left.Value, right.Value);
                        stack.Push(new Number(value));
                    }
                    else if (token is FunctionBase)
                    {
                        NumberBase arg = (NumberBase)stack.Pop();
                        double value = ((FunctionBase)token).Calculate(arg.Value);
                        stack.Push(new Number(value));
                    }
                }
                double toret = ((NumberBase)stack.Pop()).Value;
                if (stack.Count != 0)
                    throw new ArgumentException(InvalidMessage);
                return toret;
            }

            Token GetNextToken(ref int position, string input, TokenFactory tokenFactory)
            {
                Token toret = null;
                Type found;
                string rest = input.Substring(position);
                int count = 0;
                int pos = 0;
                while (count++ < rest.Length)
                {
                    string cand = rest.Substring(0, count);
                    Token latest = tokenFactory.GetToken(cand);
                    if (latest != null)
                    {
                        //if(found!=null && latest.GetType()!=found)
                        //break;
                        found = latest.GetType();
                        toret = latest;
                        pos = count;
                    }
                    else
                    {
                        //break;
                    }
                }
                if (toret != null)
                    position += pos;
                return toret;
            }

            string FormatExpression(string input)
            {
                string toreturn = input;
                string separator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                toreturn = toreturn.Replace(".", separator);
                toreturn = toreturn.Replace(",", separator);
                toreturn = Regex.Replace(toreturn, @"^\-", @"0-");
                toreturn = Regex.Replace(toreturn, @"\(\-", @"(0-");
                return toreturn;
            }
            public abstract class Token
            {

                public readonly string Entry;

                public Token(string entry)
                {
                    Entry = entry;
                }
            }
            abstract class NumberBase : Token
            {

                public abstract double Value
                {
                    get;
                }

                public NumberBase(string value): base(value)
                {
                }

            }
            class Number : NumberBase
            {

                public override double Value
                {
                    get
                    {
                        return double.Parse(Entry);
                    }
                }

                public Number(double value)
                    : this(value.ToString())
                {
                }

                public Number(string value)
                    : base(value)
                {
                }

            }
            class Pi : NumberBase
            {

                public override double Value
                {
                    get
                    {
                        return Math.PI;
                    }
                }

                public Pi(string value): base(value)
                {
                }
            }
            class E : NumberBase
            {

                public override double Value
                {
                    get
                    {
                        return Math.E;
                    }
                }

                public E(string value) : base(value)
                {
                }
            }

            abstract class FunctionBase : Token
            {

                public virtual int OperandsCount
                {
                    get
                    {
                        return 1;
                    }
                }

                public FunctionBase(string value): base(value)
                {
                }

                public abstract double Calculate(params double[] args);

            }
            class Sinus : FunctionBase
            {

                public Sinus(string value)
                    : base(value)
                {
                }

                public override double Calculate(params double[] args)
                {
                    return Math.Sin(args[0]);
                }

            }
            class Cosinus : FunctionBase
            {

                public Cosinus(string value)
                    : base(value)
                {
                }

                public override double Calculate(params double[] args)
                {
                    return Math.Cos(args[0]);
                }

            }
            class Tangens : FunctionBase
            {

                public Tangens(string value)
                    : base(value)
                {
                }

                public override double Calculate(params double[] args)
                {
                    return Math.Tan(args[0]);
                }

            }
            class Cotangens : FunctionBase
            {

                public Cotangens(string value) : base(value)
                {
                }

                public override double Calculate(params double[] args)
                {
                    return 1 / Math.Tan(args[0]);
                }

            }
            class Sqrt : FunctionBase
            {

                public Sqrt(string value) : base(value)
                {
                }

                public override double Calculate(params double[] args)
                {
                    return Math.Sqrt(args[0]);
                }

            }
            class Exp : FunctionBase
            {

                public Exp(string value) : base(value)
                {
                }

                public override double Calculate(params double[] args)
                {
                    return Math.Exp(args[0]);
                }

            }

            abstract class OperatorBase : Token
            {

                public virtual int Precedence
                {
                    get
                    {
                        return 1;
                    }
                }

                public virtual Side Side
                {
                    get
                    {
                        return Side.Left;
                    }
                }

                public OperatorBase(string value)
                    : base(value)
                {
                }

                public abstract double Calculate(double left, double right);

            }
            class Addition : OperatorBase
            {

                public Addition(string value) : base(value)
                {
                }

                public override double Calculate(double left, double right)
                {
                    return left + right;
                }
            }
            class Subtraction : OperatorBase
            {

                public Subtraction(string value) : base(value)
                {
                }

                public override double Calculate(double left, double right)
                {
                    return left - right;
                }
            }
            class Multiplication : OperatorBase
            {

                public override int Precedence
                {
                    get
                    {
                        return 2;
                    }
                }

                public Multiplication(string value) : base(value)
                {
                }

                public override double Calculate(double left, double right)
                {
                    return left * right;
                }
            }
            class Division : OperatorBase
            {

                public override int Precedence
                {
                    get
                    {
                        return 2;
                    }
                }

                public Division(string value) : base(value)
                {
                }

                public override double Calculate(double left, double right)
                {
                    return left / right;
                }
            }
            class Power : OperatorBase
            {

                public override int Precedence
                {
                    get
                    {
                        return 3;
                    }
                }

                public override Side Side
                {
                    get
                    {
                        return Side.Right;
                    }
                }

                public Power(string value) : base(value)
                {
                }

                public override double Calculate(double left, double right)
                {
                    return Math.Pow(left, right);
                }
            }
            class LeftBracket : Token
            {

                public LeftBracket(string value) : base(value)
                {
                }

            }
            class RightBracket : Token
            {

                public RightBracket(string value) : base(value)
                {
                }

            }

            class variable : NumberBase
            {
                public override double Value
                {
                    get
                    {
                        return x_inputed;
                    }
                }

                public variable(string value) : base(value)
                {
                }
            }
            class TokenFactory
            {
                Dictionary<Func<string, bool>, Type> RegisteredTokens;

                public TokenFactory()
                {
                    RegisteredTokens = new Dictionary<Func<string, bool>, Type>();
                    string separator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                    string reqexSeparator = Regex.Escape(separator);

                    
                    RegisterToken<Addition>(x => x == "+");
                    RegisterToken<Subtraction>(x => x == "-");
                    RegisterToken<Multiplication>(x => x == "*");
                    RegisterToken<Division>(x => x == "/" || x == @"\");
                    RegisterToken<Power>(x => x == "^");

                    
                    RegisterToken<Pi>(x => Match(x, "pi", "π"));
                    RegisterToken<E>(x => x == "e");
                    RegisterToken<variable>(x => x == "x");

                    RegisterToken<Number>(x =>
                    {
                        string[] patterns = new string[] {string.Format(@"^(\d+({0}\d*)?)$",reqexSeparator)};
                        return patterns.Any(p => Regex.Match(x, p).Success);
                    });

                    
                    RegisterToken<LeftBracket>(x => x == "(" || x == "[" || x == "{");
                    RegisterToken<RightBracket>(x => x == ")" || x == "]" || x == "}");

                    
                    RegisterToken<Sinus>(x => Match(x, "sin"));
                    RegisterToken<Cosinus>(x => Match(x, "cos"));
                    RegisterToken<Tangens>(x => Match(x, "tan"));
                    RegisterToken<Cotangens>(x => Match(x, "ctg"));
                    RegisterToken<Sqrt>(x => Match(x, "sqrt"));
                    RegisterToken<Sqrt>(x => Match(x, "exp"));
                }

                public Token GetToken(string exact)
                {
                    Token toret = null;
                    foreach (var kvp in RegisteredTokens)
                    {
                        if (kvp.Key(exact))
                        {
                            toret = (Token)Activator.CreateInstance(kvp.Value, exact);
                            break;
                        }
                    }
                    return toret;
                }

                bool Match(string cand, params string[] names)
                {
                    foreach (string name in names)
                    {
                        if (name.Equals(cand, StringComparison.InvariantCultureIgnoreCase))
                            return true;
                    }
                    return false;
                }

                void RegisterToken<T>(Func<string, bool> match) where T : Token
                {
                    if (RegisteredTokens.ContainsKey(match))
                        throw new NotSupportedException("Token already added");
                    RegisteredTokens[match] = typeof(T);
                }

            }

        }
        public class ArgumentException : Exception
        {
            public ArgumentException(string yes)
            {
                Console.WriteLine(yes);
            }
        }
    }
}
