using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace HinterLib
{
    public class Hinter
    {
        public static string ReadHintedLine<T, TResult>(IEnumerable<T> hintSource, Func<T, TResult> hintField, string inputRegex = ".*", ConsoleColor hintColor = ConsoleColor.DarkGray)
        {
            ConsoleKeyInfo input;

            var suggestion = string.Empty;
            var userInput = string.Empty;

            while (ConsoleKey.Enter != (input = Console.ReadKey()).Key)
            {
                if (input.Key == ConsoleKey.Backspace && userInput.Any())
                    userInput = userInput.Remove(userInput.Length - 1, 1);

                else if (input != null && Regex.IsMatch(input.KeyChar.ToString(), inputRegex))
                    userInput += input.KeyChar;

                suggestion = hintSource.Select(item => hintField(item).ToString())
                    .FirstOrDefault(item => item.Length > userInput.Length && item.Substring(0, userInput.Length) == userInput);

                var inputText = suggestion == null ? userInput : suggestion;

                ClearCurrentConsoleLine();

                Console.Write(userInput);

                var originalColor = Console.ForegroundColor;

                Console.ForegroundColor = hintColor;

                if (userInput.Any()) Console.Write(inputText.Substring(userInput.Length, inputText.Length - userInput.Length));

                Console.ForegroundColor = originalColor;
            }

            var result = suggestion == null ? userInput : suggestion;

            Console.WriteLine(result);

            return result;
        }
        
        private static void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }
    }
}