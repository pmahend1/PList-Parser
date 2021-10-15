using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Sample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var text = File.ReadAllText("Plist/PlistFile1.plist", System.Text.Encoding.UTF8);
            var parser = new PListParserLibrary.Parser();
            var outPut = parser.Parse(text);
            var json = JsonConvert.SerializeObject(outPut, Formatting.Indented);
            Console.WriteLine("As Json" + Environment.NewLine + "----------------" + Environment.NewLine + json);
            Console.WriteLine();
            Console.WriteLine("As Dictionary" + Environment.NewLine + "----------------"  );
            foreach (var kv in outPut)
            {
                if (kv.Value is IList li && kv.Value.GetType().IsGenericType)
                {
                    Console.WriteLine(kv.Key);
                    foreach (var liItem in li)
                    {
                        Console.WriteLine("\t" + liItem.ToString());
                    }
                }
                else if (kv.Value is Dictionary<string, object> dic)
                {
                    Console.WriteLine(kv.Key);
                    foreach (var dicItem in dic)
                    {
                        Console.WriteLine("\t" + dicItem.Key + ": " + dicItem.Value.ToString());
                    }
                }
                else
                {
                    Console.WriteLine(kv.Key + ": " + kv.Value.ToString());
                }

            }
        }
    }
}
