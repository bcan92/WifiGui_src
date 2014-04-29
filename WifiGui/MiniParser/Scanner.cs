using System;
using System.Collections.Generic;
using IO = System.IO;
using Text = System.Text;
using Collections = System.Collections;

namespace WindowsFormsApplication1
{
    /// <summary>
    /// Text scanner for tokenizing script files.
    /// </summary>
    class Scanner
    {
        Token current, head;
        int line = 0;
        public Scanner()
        {
            head = current = new Token();
        }

        /// <summary>
        /// 
        /// </summary>
        public Token FirstToken
        {
            get { return this.head; }
        }
        /// <summary>
        /// Method for automatically tokenizing script files.
        /// Tokens are stored in linked-list format. FirstToken field will contain the head of the tokenized list
        /// after this function is executed.
        /// </summary>
        /// <param name="filename">Location of the script file</param>
        public void ScanScript(string filename)
        {
            IO.TextReader tr = IO.File.OpenText(filename);
            Scan(tr);
        }
        void Scan(IO.TextReader input)
        {
            while (input.Peek() != -1)
            {
                char ch = (char)input.Peek();
                if (ch == '%')
                {
                    //eat '%'
                    input.Read();
                    Text.StringBuilder accum = new Text.StringBuilder();
                    ch = (char)input.Peek();
                    while (ch != '%')
                    {
                        accum.Append((char)input.Read());
                        if (input.Peek() == -1)
                        {
                            break;
                        }
                        ch = (char)input.Peek();
                    }
                    input.Read(); //eat '%'
                    AppendToken(accum.ToString(), line, 2);
                }
                else if (ch == ',')
                {
                    input.Read();
                }
                else if (ch == '$')
                {
                    //omg a function
                    Text.StringBuilder accum = new Text.StringBuilder();
                    input.Read();
                    ch = (char)input.Peek();
                    while (char.IsLetterOrDigit(ch) || ch == '_')
                    {
                        accum.Append(ch);
                        input.Read();
                        if (input.Peek() == -1)
                        {
                            break;
                        }
                        ch = (char)input.Peek();
                    }
                    if (ch != '(')
                        throw new Exception("Invalid character in function name");
                    AppendToken(accum.ToString(), line, 3);
                }
                else if (ch == '(')
                {
                    input.Read();
                    AppendToken("", line, 4);
                }
                else if (ch == ')')
                {
                    input.Read();
                    AppendToken("", line, 8);
                }
                else if (ch == '\r')
                {
                    input.Read(); //eat CR
                    char c = (char)input.Peek();
                    if (c == '\n')
                    {
                        input.Read(); //eat LF
                        AppendToken("", line, 7);
                    }
                    else
                        line++;
                }
                else if (ch == '#')
                {
                    input.Read();
                    AppendToken("", line, 9);
                }
                else
                {
                    Text.StringBuilder accum = new System.Text.StringBuilder();
                    while (ch != '$' & ch != '%' & ch != '\r' & ch != ')')
                    {
                        if (ch == '\\')
                        {
                            //skip escape char and insert next one immediately
                            input.Read();
                        }
                        accum.Append((char)input.Read());
                        if (input.Peek() == -1)
                        {
                            break;
                        }
                        else if (input.Peek() == ',')
                        {
                            input.Read();
                            break;
                        }
                        ch = (char)input.Peek();
                    }
                    AppendToken(accum.ToString(), line, 1);
                }
            }
        }
        void AppendToken(string val, int line, int kind)
        {
            Token t = new Token();
            t.val = val;
            t.line = line;
            t.kind = kind;
            current.next = t;
            current = current.next;
        }
        /// <summary>
        /// Test function that outputs token list in a human-readable format.
        /// </summary>
        public void DebugScanner()
        {
            Token t = this.head;
            int i = 1;
            while (t != null)
            {
                Console.WriteLine("Token {0}\r\n=====================\r\nKind:{1}\r\nLine:{2}\r\nValue:{3}",
                    i++, t.kind, t.line, t.val);
                t = t.next;
            }
            Console.WriteLine("=====================");
        }
    }
    /// <summary>
    /// Basic linked-list structure for Tokenized lists. Refer to the list below for token types.
    /// </summary>
    internal class Token
    {
        public int kind;    // token kind
        public int line;    // token line (starting at 1)
        public string val;  // token value
        public Token next;  // ML 2005-03-11 Tokens are kept in linked list
    }
    /*Currently:
     * 1.Plain Text
     * 2.Variable
     * 3.Function
     * 4.ParL
     * 8.ParR
     * 5.Argument -> obsolete
     * 6.Query -> uncertain
     * 7.EOF
     * 9.PREPROC -> currently not implemented, maybe after a more robust scanner implementation...
    */
}
