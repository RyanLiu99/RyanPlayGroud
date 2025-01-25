using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ConsoleApp1
{
    internal static class TestRegex
    {
        public static void RunTests()
        {
            var ms = Regex.Matches("XYZAbcAbcAbcXYZAbcAb", "(Abc)+", RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase); //default to RegexOptions.None
            Console.WriteLine("Find (Abc)+, 1 or more, in XYZAbcAbcAbcXYZAbcAb");

            int matchIndex = 0;
            foreach (Match m in ms)
            {
                matchIndex++;
                Console.WriteLine($"---Found Match {matchIndex}, it has {m.Groups.Count}  groups. "); //No other meaningful properties.

                for (int i = 0; i < m.Groups.Count; i++)
                {
                    var g = m.Groups[i];
                    Console.WriteLine($"\tGroup {i}'s Captures count = {g.Captures.Count}, g.Name:{g.Name}, g.Success: {g.Success}"); //g.Success true means captures > 0

                    for (int ii = 0; ii < g.Captures.Count; ii++)
                    {
                        var capture = g.Captures[ii];
                        Console.WriteLine($"\t\tCaptured {capture.Value}, Starts at character {capture.Index}, length={capture.Length}. ");  //Success: n/a, it only apply at group level
                    }
                }
            }
        }
    }
}
