namespace ConsoleDrawer;
using System;

class Program
{
    static int cursorX = 0;
    static int cursorY = 0;

    static bool running = false;

    static ConsoleColor currentColor = ConsoleColor.White;
    static readonly Dictionary<(int, int), (char, ConsoleColor)> drawing = new();

    static void Main()
    {
        Console.Clear();
        Console.CursorVisible = true;

        while (!running)
        {
            LoadMenu();
        }

        while (running)
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
                case ConsoleKey.Escape:
                    running = false;
                    LoadMenu();
                    break;
                default:
                    ChangeColor(key);
                    break;
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
        Console.ForegroundColor = currentColor;
        Console.Write("█");
        drawing[(cursorX, cursorY)] = ('█', currentColor);
        MoveCursor(1, 0); 
    }

    static void DeleteMarker()
    {
        Console.Write(" ");
        drawing.Remove((cursorX, cursorY));
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

    static void SaveDrawing(string filePath)
    {
        using (var writer = new StreamWriter(filePath))
        {
            foreach (var entry in drawing)
            {
                var (position, (character, color)) = entry;
                writer.WriteLine($"{position.Item1},{position.Item2},{character},{(int)color}");
            }
        }
    }

    static void LoadDrawing(string filePath)
    {
        if (!File.Exists(filePath)) return;

        drawing.Clear();
        Console.Clear();

        using (var reader = new StreamReader(filePath))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                var parts = line.Split(',');
                var x = int.Parse(parts[0]);
                var y = int.Parse(parts[1]);
                var character = parts[2][0];
                var color = (ConsoleColor)int.Parse(parts[3]);

                drawing[(x, y)] = (character, color);
                Console.SetCursorPosition(x, y);
                Console.ForegroundColor = color;
                Console.Write(character);
            }
        }
        Console.SetCursorPosition(cursorX, cursorY);
        Console.ForegroundColor = currentColor;
    }

    static void LoadMenu()
    {
        if (running == false)
        {
            Console.Clear();
            Console.WriteLine("Drawer thing");
            Console.WriteLine("Press L to load a file");
            Console.WriteLine("Press S to save to a file");
            Console.WriteLine("Press C to start drawing");
        }

        var key = Console.ReadKey(true).Key;

        switch (key)
        {
            case ConsoleKey.L:
                LoadDrawing("drawing.txt");
                running = true;
                break;
            case ConsoleKey.S:
                SaveDrawing("drawing.txt");
                LoadDrawing("drawing.txt");
                running = true;
                break;
            case ConsoleKey.C:
                Console.Clear();
                drawing.Clear();
                running = true;
                break;
            case ConsoleKey.Escape:
                SaveDrawing("temp.txt");
                LoadDrawing("temp.txt");
                running = true;
                break;
            default:
                break;
        }
        Thread.Sleep(100);

    }
}
