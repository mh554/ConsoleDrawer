namespace ConsoleDrawer;
using System;

class Program
{
    static int cursorX = 0;
    static int cursorY = 0;

    static ConsoleColor currentColor = ConsoleColor.White;

    static void Main()
    {
        Console.Clear();
        Console.CursorVisible = true;

        while (true)
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        MoveCursor(0, -1);
                        break;
                    case ConsoleKey.DownArrow:
                        MoveCursor(0, 1);
                        break;
                    case ConsoleKey.LeftArrow:
                        MoveCursor(-1, 0);
                        break;
                    case ConsoleKey.RightArrow:
                        MoveCursor(1, 0);
                        break;
                    case ConsoleKey.Spacebar:
                        PlaceMarker();
                        break;
                    case ConsoleKey.Backspace:
                        DeleteMarker();
                        break;
                    default:
                        ChangeColor(key);
                        break;
                }
            }
        }
    }

    static void MoveCursor(int deltaX, int deltaY)
    {
        cursorX = Math.Max(0, Math.Min(Console.WindowWidth - 1, cursorX + deltaX));
        cursorY = Math.Max(0, Math.Min(Console.WindowHeight - 1, cursorY + deltaY));

        Console.SetCursorPosition(cursorX, cursorY);

    }

    static void PlaceMarker()
    {
        Console.Write("█");
    }

    static void DeleteMarker()
    {
        Console.Write(" ");
    }


    static void ChangeColor(ConsoleKey key)
    {
        switch (key)
        {
            case ConsoleKey.D1:
                currentColor = ConsoleColor.Red;
                break;
            case ConsoleKey.D2:
                currentColor = ConsoleColor.Green;
                break;
            case ConsoleKey.D3:
                currentColor = ConsoleColor.Blue;
                break;
            case ConsoleKey.D4:
                currentColor = ConsoleColor.Yellow;
                break;
            case ConsoleKey.D5:
                currentColor = ConsoleColor.Cyan;
                break;
            case ConsoleKey.D6:
                currentColor = ConsoleColor.Magenta;
                break;
            case ConsoleKey.D7:
                currentColor = ConsoleColor.Gray;
                break;
            case ConsoleKey.D8:
                currentColor = ConsoleColor.DarkRed;
                break;
            case ConsoleKey.D9:
                currentColor = ConsoleColor.DarkGreen;
                break;
        }
        Console.ForegroundColor = currentColor;
    }
}
