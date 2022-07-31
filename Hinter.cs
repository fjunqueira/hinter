using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace HinterLib
{
	public static class Hinter
	{
		public static string ReadHintedLine<T, TResult>(IEnumerable<T> hintSource, Func<T, TResult> hintField, string inputRegex = ".*", ConsoleColor hintColor = ConsoleColor.DarkGray)
		{
			ConsoleKeyInfo input;

			var hint = new Hint();
			var suggestion = string.Empty;
			var userInput = string.Empty;
			var readLine = string.Empty;

			hint.makeListHints(hintSource, hintField, userInput);

			while (ConsoleKey.Enter != (input = Console.ReadKey()).Key)
			{
				if (input.Key == ConsoleKey.Backspace)
					userInput = _removeFinalChar(userInput);

				else if (input.Key == ConsoleKey.Tab || input.Key == ConsoleKey.RightArrow)
					userInput = suggestion ?? userInput;

				else if (input != null && Regex.IsMatch(input.KeyChar.ToString(), inputRegex))
					userInput += input.KeyChar;

				if (input.Key == ConsoleKey.UpArrow)
				{
					userInput = _removeFinalChar(userInput);
					suggestion = hint.nextHint;
				}
				else if (input.Key == ConsoleKey.DownArrow)
				{
					userInput = _removeFinalChar(userInput);
					suggestion = hint.previousHint;
				}
				else if (input.Key == ConsoleKey.LeftArrow || input.Key == ConsoleKey.Escape)
				{
					userInput = _removeFinalChar(userInput);
					suggestion = userInput;
				}
				else
				{
					if (userInput != "")
					{
						hint.makeListHints(hintSource, hintField, userInput);
						suggestion = hint.currentHint;
					}
					else
						suggestion = userInput;
				}

				readLine = suggestion == null ? userInput : suggestion;

				ClearCurrentConsoleLine();

				Console.Write(userInput);

				var originalColor = Console.ForegroundColor;

				Console.ForegroundColor = hintColor;

				Console.Write(readLine.Substring(userInput.Length, readLine.Length - userInput.Length));

				Console.ForegroundColor = originalColor;
			}

			Console.WriteLine(readLine);

			return userInput.Any() ? readLine : string.Empty;

		}

		private static void ClearCurrentConsoleLine()
		{
			int cursorTop = Console.CursorTop;
			Console.SetCursorPosition(0, Console.CursorTop);
			Console.Write(new string(' ', Console.WindowWidth));
			Console.SetCursorPosition(0, cursorTop);
		}

		private static string _removeFinalChar(string userInput)
		{
			return (userInput.Length > 0) ? userInput.Substring(0, userInput.Length - 1) : userInput;
		}
	}

	public class Hint
	{
		private List<string> _hintList;
		private int _hintIndex;

		public Hint()
		{
			_hintList = new List<string>();
			_hintIndex = 0;
		}
		public bool makeListHints<T, TResult>(IEnumerable<T> hintSource, Func<T, TResult> hintField, string userInput)
		{
			try
			{
				_hintIndex = 0;
				_hintList = Enumerable.Where(Enumerable.Select(hintSource, (T item) => hintField(item).ToString()), (string item) => item.Length > userInput.Length && item.Substring(0, userInput.Length) == userInput).ToList();
				if (_hintList.Count() == 0)
					_hintList.Add(null);
				return true;
			}
			catch
			{
				return false;
			}
		}

		public void resetCurrentHint()
		{
			_hintIndex = 0;
		}

		public string nextHint
		{
			get
			{
				_hintIndex = _calcNextHintIndex();
				return _hintList[_hintIndex];
			}
		}

		public string previousHint
		{
			get
			{
				_hintIndex = _calcPreviousHintIndex();
				return _hintList[_hintIndex];
			}
		}

		public string currentHint
		{
			get
			{
				return _hintList[_hintIndex];
			}
		}

		private int _calcNextHintIndex()
		{
			if (_hintList.Count() > 1)
				return (_hintIndex + 1) % _hintList.Count();
			else
				return 0;
		}

		private int _calcPreviousHintIndex()
		{
			if (_hintList.Count() > 1)
				return (_hintIndex - 1) < 0 ? _hintList.Count() - 1 : _hintIndex - 1;
			else
				return 0;
		}
	}
}