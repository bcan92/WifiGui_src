using System;
using Collections = System.Collections.Generic;
using Text=System.Text;
using IO=System.IO;

namespace WindowsFormsApplication1
{
    /// <summary>
    /// Executes actions based on the token list recieved from the Scanner class.
    /// </summary>
    class Parser
    {
        private Token head;
        private Collections.Dictionary<string, string> _VARS;
        private Collections.Dictionary<string,IO.StreamReader> _PIPES;
        int cline;

        public Parser(Token head)
        {
            this.head = head;
            cline = 0;
            _VARS = new System.Collections.Generic.Dictionary<string, string>();
            _PIPES = new System.Collections.Generic.Dictionary<string, System.IO.StreamReader>();
        }
        /// <summary>
        /// Main function that iterates over the token list.
        /// </summary>
        /// <param name="lambda">Generic lambda function that will be executed whenever an EOL is reached in the script file.</param>
        public void Interpret(Action<string,object> lambda)
        {
            Token current = head;
            Text.StringBuilder accum = new System.Text.StringBuilder();
            while (current != null)
            {
                switch (current.kind)
                {
                    case 1:
                        accum.Append(current.val);
                        break;
                    case 2:
                        //In this case, variable is not an argument, thus just replace it with its value;
                        {
                            string s; _VARS.TryGetValue(current.val, out s);
                            accum.Append(s);
                        }
                        break;
                    case 3:
                        accum.Append(ParseFunction(ref current).val);
                        break;
                }
                if (cline < current.line || current.kind == 7)
                {
                    //Send string
                    if (accum.ToString().Trim() != string.Empty)
                    {
                        try
                        {
                            if (lambda != null)
                                lambda(accum.ToString(),this);
                            accum = new System.Text.StringBuilder();
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                    cline = current.line;
                }
                if (current.next != null) current = current.next;
                else break;
            }
        }

        /// <summary>
        /// Private function for handling subroutines.
        /// Encountered subroutines are handled first! Nested subs may not yield the expected outcome... Refer to sample script file.
        /// </summary>
        /// <param name="t_arg">Token that holds the ParL value after the function name</param>
        /// <returns>Token after the one that contains the ParR value</returns>
        private Token ParseFunction(ref Token t_arg)
        {
            string fname = t_arg.val;
            if (t_arg.next.kind != 4)
                throw new Exception("Expected '(' after function name!");
            t_arg = t_arg.next.next; //eat parL and get next char
            //
            Collections.List<Token> args = new Collections.List<Token>();
            while (t_arg.kind != 8)
            {
                switch (t_arg.kind)
                {
                    case 1:
                        args.Add(new Token() { val = t_arg.val, kind = t_arg.kind });
                        break;
                    case 2:
                        args.Add(new Token() { val = t_arg.val, kind = t_arg.kind });
                        break;
                    case 3:
                        args.Add(ParseFunction(ref t_arg));
                        break;
                    default:
                        throw new Exception("Unsupported character as argument!");
                }
                t_arg = t_arg.next;
            }
            char op = '\0';
            /*
             * List of functions. Some were added later on to be able to run certain test cases.
             * That might explain the messy appearance of this bit.. Highly influenced by foobar2000 scripting.
             */
            switch (fname)
            {
                case "if":
                    if (args.Count < 2)
                        throw new ArgumentException("Expected at least 2 arguments!");
                    return (this.ConvertToBool(GetValue(args[0]))) ? args[1] : new Token() { val = String.Empty, kind = 1, line = 0, next = null };
                case "if2":
                    if (args.Count < 3)
                        throw new ArgumentException("Expected at least 3 arguments!");
                    return new Token() { val = (this.ConvertToBool(GetValue(args[0]))) ? GetValue(args[1]) : GetValue(args[2]), kind = 1 };
                case "puts":
                    if(args.Count < 2)
                        throw new ArgumentException("Expected at least 2 arguments!");
                    if (args[0].kind != 2)
                        throw new Exception("Expected a variable for the first argument");
                    _VARS[args[0].val] = GetValue(args[1]);
                    return new Token(){ kind =1, val = ""};
                case "add":
                        op = '+';
                        goto case "mathop";
                case "sub":
                        op = '-';
                        goto case "mathop";
                case "mul":
                        op = '*';
                        goto case "mathop";
                case "div":
                        op = '/';
                        goto case "mathop";
                case "mathop":
                    {
                        if (args.Count < 2)
                            throw new ArgumentException("Expected at least 2 arguments!");
                        double a, b;
                        if (double.TryParse(GetValue(args[0]), out a) && double.TryParse(GetValue(args[1]), out b))
                            switch (op)
                            {
                                case '+':
                                    return new Token() { kind = 1, val = (a + b).ToString() };
                                case '-':
                                    return new Token() { kind = 1, val = (a - b).ToString() };
                                case '/':
                                    return new Token() { kind = 1, val = (a / b).ToString() };
                                case '*':
                                    return new Token() { kind = 1, val = (a * b).ToString() };
                                default:
                                    throw new ArgumentException("Invalid operator detected.");
                            }
                        else
                            throw new ArgumentException("Non-numeric arguments detected.");
                    }
                case "eq":
                     if(args.Count < 2)
                        throw new ArgumentException("Expected at least 2 arguments!");
                    else if (args.Count == 2)
                         return new Token() { kind = 1, val = GetValue(args[0]).Equals(GetValue(args[1])).ToString() };
                     else //more than 2 arguments
                     {
                         int argnum = 1; bool state = false;
                         while (argnum < args.Count)
                         {
                             state = state | GetValue(args[0]).Equals(GetValue(args[argnum++]));
                         }
                         return new Token() { kind = 1, val = state.ToString() };
                     }
                case "and":
                     if (args.Count < 2)
                         throw new ArgumentException("Expected at least 2 arguments!");
                     else if (args.Count == 2)
                     {
                         return new Token()
                         {
                             kind = 1,
                             val =
                                 (Convert.ToBoolean(GetValue(args[0])) && Convert.ToBoolean(GetValue(args[1]))).ToString()
                         };
                     }
                     else //more than 2 arguments
                     {
                         int argnum = 1; bool state = true;
                         while (argnum < args.Count)
                         {
                             state = state & Boolean.Parse(GetValue(args[argnum++]));
                         }
                         return new Token() { kind = 1, val = state.ToString() };
                     }
                case "cmp":
                     if (args.Count < 3 || args.Count > 3)
                         throw new ArgumentException("Expected 3 arguments!");
                     else 
                     {
                         int scenario; bool result;
                         double arg0, arg1;
                         if (Int32.TryParse(GetValue(args[2]), out scenario) &&
                             Double.TryParse(GetValue(args[0]), out arg0) &&
                             Double.TryParse(GetValue(args[1]), out arg1))
                         {
                             switch (scenario)
                             {
                                 case -1:
                                     result = (arg0 < arg1);
                                     break;
                                 case 1:
                                     result = (arg0 > arg1);
                                     break;
                                 default:
                                     result = (arg0 == arg1);
                                     break;
                             }
                         }
                         else { throw new ArgumentException("Expected numerical arguments! Use eq function for string comparison!"); }
                         return new Token() { kind = 1, val = result.ToString() };
                     }
                //IO Functions that were never implemented..
                case "open":
                    {
                     if (args.Count < 2)
                         throw new ArgumentException("Expected at least 2 arguments!");
                     try
                     {
                         IO.StreamReader temp = new IO.StreamReader(GetValue(args[1]));
                         _PIPES[args[0].val] = temp;
                     }
                     catch (IO.IOException ie) { throw ie; }
                        return new Token(){ kind =1, val = ""};
                    }
                case "close":
                    {
                     if (args.Count < 1)
                         throw new ArgumentException("Expected 1 argument!");
                    IO.StreamReader temp;
                    if (_PIPES.TryGetValue(GetValue(args[0]), out temp)) 
                    {
                        temp.Close();
                        _PIPES.Remove(GetValue(args[0]));
                    }
                    return new Token() { kind = 1, val = "" };
                    }
                default:
                    throw new Exception("Unexistent function name.");
            }
        }
        private string GetValue(Token t)
        {
            if (t.kind == 1) return t.val;
            else if (t.kind == 2)
            {
                string s; _VARS.TryGetValue(t.val, out s);
                return s;
            }
            else throw new ArgumentException("Unexpected Token kind.");
        }
        private bool ConvertToBool(string cstate)
        {
            bool cond = false;
            //
            float a;
            bool b;
            if (float.TryParse(cstate, out a))
            {
                cond = (a != 0);
            }
            else if (bool.TryParse(cstate, out b))
            {
                cond = b;
            }
            else
                cond = (cstate != String.Empty);

            return cond;
        }
        /// <summary>
        /// Function for adding new variables to the scripting environment
        /// </summary>
        /// <param name="key">Name of the new variable</param>
        /// <param name="val">The corresponding value. All values should be added in a string form.</param>
        public void AddData(string key, string val)
        {
            if (_VARS.ContainsKey(key))
                _VARS[key] = val;
            else
            _VARS.Add(key, val);
        }
     /*Currently:
     * 1.Plain Text
     * 2.Variable
     * 3.Function
     * 4.ParL
     * 8.ParR
     * 5.Argument -> obsolete
     * 6.Query -> uncertain
     * 7.EOL
     * 9.PREPROC
        */

    }
}
