using System;
using System.Collections.Generic;

namespace SharpFireWall
{
    public static class ArgParse
    {
        public static Dictionary<string,string> Parse(string[] argsArray)
        {
            var result = new Dictionary<string, string>();
            try
            {
                foreach (string arg in argsArray)
                {
                    var idx = arg.IndexOf(":");
                    if (idx > 0)
                        result[arg.Substring(0, idx)] = arg.Substring(idx + 1);
                    else
                        result[arg] = string.Empty;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return result;
        }
    }
}
