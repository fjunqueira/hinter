# hinter
A (very) simple lib to add hinting capabilities to C# console applications

I had to add this feature to a project I was working on, and thought it might be useful to someone else.

You can find it on [nuget](https://www.nuget.org/packages/hinter/)

Example program:

```c#
using System;
using System.Collections.Generic;
using HinterLib;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;

            var countries = new List<string>()
            {
                "Brazil",
                "Argentina",
                "Uruguay",
                "Chile",
                "Peru"
            };

            Console.WriteLine("Available countries:");
            Console.WriteLine();

            foreach (var country in countries)
                Console.WriteLine(country);

            Console.WriteLine();
            Console.WriteLine("Enter a country:");

            var input = Hinter.ReadHintedLine(countries, country => country);

            Console.WriteLine();
            Console.WriteLine($"You entered: {input}");
            Console.ReadLine();
        }
    }
}
```

![alt tag](https://github.com/fjunqueira/hinter/blob/master/sample.gif)
