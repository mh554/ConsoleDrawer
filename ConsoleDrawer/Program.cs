namespace ConsoleDrawer;
using System;

class Program
{
    static int cursorX = 0;
    static int cursorY = 0;

    static bool running = true;

    static ConsoleColor currentColor = ConsoleColor.White;
    static readonly Dictionary<(int, int), (char, ConsoleColor)> drawing = new();

    static void Main()
    {
        LoadMenu();

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
                    Console.Clear();
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
        if (!File.Exists(filePath))
        {
            Console.SetCursorPosition(0, 10);
            Console.WriteLine($"File '{filePath}' does not exist.");
            Thread.Sleep(2000); 
            return;
        }

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

        Console.ForegroundColor = currentColor;
        Console.CursorVisible = true;
    }
    static void DeleteFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Console.WriteLine($"File '{filePath}' deleted successfully.");
        }
        else
        {
            Console.WriteLine($"File '{filePath}' does not exist.");
        }
    }

    static void LoadMenuText()
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.CursorVisible = false;
        ClearLine(1);
        ClearLine(2);
        ClearLine(3);
        ClearLine(4);
        Console.SetCursorPosition(0, 0);
        Console.WriteLine("Drawer thing");
        Console.WriteLine("Use arrow keys to navigate");
        Console.WriteLine("Press Enter to select an option");
        Console.WriteLine("Press Escape to go back to drawing");
        Console.SetCursorPosition(0, 5);
        for (int i = 0; i < menuOptions.Length; i++)
        {
            if (i == 0)
            {
                Console.Write(">");
            }
            else
            {
                Console.Write(" ");
            }
            Console.WriteLine(menuOptions[i]);
        }
    }

    static void LoadMenu()
    {
        if (running)
        {
            LoadMenuText();
        }
        running = false;
        menuSelection = 0;

        while (!running)
        {
            var key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    MoveMenuSelection(-1);
                    break;
                case ConsoleKey.DownArrow:
                    MoveMenuSelection(1);
                    break;
                case ConsoleKey.Enter:
                    SelectMenuOption();
                    break;
                case ConsoleKey.Escape:
                    SaveDrawing("temp.txt");
                    LoadDrawing("temp.txt");
                    running = true;
                    Console.SetCursorPosition(0, 0);
                    break;
                default:
                    break;
            }
            Thread.Sleep(10);
        }
    }

    static void MoveMenuSelection(int direction)
    {
        menuSelection += direction;
        if (menuSelection < 0)
            menuSelection = menuOptions.Length - 1;
        else if (menuSelection >= menuOptions.Length)
            menuSelection = 0;

        ClearLine(5);
        ClearLine(6);
        ClearLine(7);

        for (int i = 0; i < menuOptions.Length; i++)
        {
            Console.SetCursorPosition(0, 5 + i);
            if (i == menuSelection)
            {
                Console.Write(">");
            }
            else
            {
                Console.Write(" ");
            }
            Console.WriteLine(menuOptions[i]);
        }
    }

    static void SelectMenuOption()
    {
        switch (menuSelection)
        {
            case 0:
                Console.SetCursorPosition(0, 10);
                Console.WriteLine("Enter the file name to load:");
                string loadFileName = Console.ReadLine();
                LoadDrawing(loadFileName + ".txt");
                Console.SetCursorPosition(0, 0);
                running = true;
                break;
            case 1:
                Console.SetCursorPosition(0, 10);
                Console.WriteLine("Enter the file name to save:");
                string saveFileName = Console.ReadLine();
                SaveDrawing(saveFileName + ".txt");
                LoadDrawing(saveFileName + ".txt");
                running = true;
                break;
            case 2:
                Console.SetCursorPosition(0, 10);
                Console.WriteLine("Enter the file name to delete:");
                string deleteFileName = Console.ReadLine();
                DeleteFile(deleteFileName + ".txt");
                break;
            case 3:
                Console.Clear();
                drawing.Clear();
                Console.CursorVisible = true;
                running = true;
                break;
            case 4:
                Environment.Exit(0);
                break;
            default:
                break;
        }
    }



    static int menuSelection = 0;
    static readonly string[] menuOptions = { "Load a file", "Save to a file", "Delete a file","Start drawing", "Quit"};
    static void ClearLine(int line)
    {
        int currentLineCursor = Console.CursorTop;
        Console.SetCursorPosition(0, line);
        Console.Write(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(0, currentLineCursor);
    }
}
