namespace ConsoleDrawer
{
    internal class Program
    {
        private static char symbol = '█';
        private static List<DrawnSymbol> drawnSymbols = new List<DrawnSymbol>();

        static void DrawSymbol(char? character = null)
        {
            var cursorPosition = Console.GetCursorPosition();
            char symbolToDraw = character ?? symbol;
            Console.Write(symbolToDraw);
            Console.SetCursorPosition(cursorPosition.Left, cursorPosition.Top);
            drawnSymbols.Add(new DrawnSymbol(cursorPosition.Left, cursorPosition.Top, symbolToDraw, Console.ForegroundColor));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
        static void Main(string[] args)
        {
            ConsoleKeyInfo info;
            do
            {
                info = Console.ReadKey(true);
                switch (info.Key)
                {
                    case ConsoleKey.UpArrow: if (0 < Console.CursorTop) Console.CursorTop -= 1; break;
                    case ConsoleKey.DownArrow: if (Console.CursorTop < Console.WindowHeight - 1) Console.CursorTop += 1; break;
                    case ConsoleKey.LeftArrow: if (0 < Console.CursorLeft) Console.CursorLeft -= 1; break;
                    case ConsoleKey.RightArrow: if (Console.CursorLeft < Console.WindowWidth - 1) Console.CursorLeft += 1; break;
                    case ConsoleKey.F1: symbol = '█'; break;
                    case ConsoleKey.F2: symbol = '▓'; break;
                    case ConsoleKey.F3: symbol = '▒'; break;
                    case ConsoleKey.F4: symbol = '░'; break;
                    case ConsoleKey.Spacebar:
                        DrawSymbol();
                        break;
                    case ConsoleKey.D0:
                    case ConsoleKey.NumPad0:
                        Console.ResetColor(); break;
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        Console.ForegroundColor = ConsoleColor.White; break;
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        Console.ForegroundColor = ConsoleColor.Red; break;
                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        Console.ForegroundColor = ConsoleColor.Green; break;
                    case ConsoleKey.D4:
                    case ConsoleKey.NumPad4:
                        Console.ForegroundColor = ConsoleColor.Blue; break;
                    case ConsoleKey.D5:
                    case ConsoleKey.NumPad5:
                        Console.ForegroundColor = ConsoleColor.Yellow; break;
                    case ConsoleKey.D6:
                    case ConsoleKey.NumPad6:
                        Console.ForegroundColor = ConsoleColor.Cyan; break;
                    case ConsoleKey.D7:
                    case ConsoleKey.NumPad7:
                        Console.ForegroundColor = ConsoleColor.Magenta; break;
                    case ConsoleKey.D8:
                    case ConsoleKey.NumPad8:
                        Console.ForegroundColor = ConsoleColor.Gray; break;
                    case ConsoleKey.D9:
                    case ConsoleKey.NumPad9:
                        Console.ForegroundColor = ConsoleColor.Black; break;
                    case ConsoleKey.Add:
                        AdjustColorBrightness(1);
                        break;
                    case ConsoleKey.Subtract:
                        AdjustColorBrightness(-1);
                        break;
                    case ConsoleKey.Delete:
                    case ConsoleKey.Backspace:
                        var color = Console.ForegroundColor;
                        Console.ResetColor();
                        DrawSymbol(' ');
                        Console.ForegroundColor = color;
                        break;
                    case ConsoleKey.S:
                        if (info.Modifiers == ConsoleModifiers.Control)
                        {
                            SaveDrawing("drawing.txt");
                        }
                        break;
                    case ConsoleKey.L:
                        if (info.Modifiers == ConsoleModifiers.Control)
                        {
                            LoadDrawing("drawing.txt");
                        }
                        break;
                    case ConsoleKey.C:
                        if (info.Modifiers == ConsoleModifiers.Control)
                        {
                            ClearDrawing();
                        }
                        break;
                }
                switch (info.Key)
                {
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.RightArrow:
                        if (Console.CapsLock)
                            DrawSymbol();
                        break;
                }
            } while (info.Key != ConsoleKey.Escape);
        }

        private static void AdjustColorBrightness(int direction)
        {
            ConsoleColor newColor = Console.ForegroundColor switch
            {
                ConsoleColor.DarkBlue when direction > 0 => ConsoleColor.Blue,
                ConsoleColor.DarkGreen when direction > 0 => ConsoleColor.Green,
                ConsoleColor.DarkCyan when direction > 0 => ConsoleColor.Cyan,
                ConsoleColor.DarkRed when direction > 0 => ConsoleColor.Red,
                ConsoleColor.DarkMagenta when direction > 0 => ConsoleColor.Magenta,
                ConsoleColor.DarkYellow when direction > 0 => ConsoleColor.Yellow,
                ConsoleColor.DarkGray when direction > 0 => ConsoleColor.Gray,
                ConsoleColor.Black when direction > 0 => ConsoleColor.DarkGray,
                ConsoleColor.Gray when direction > 0 => ConsoleColor.White,
                ConsoleColor.Blue when direction < 0 => ConsoleColor.DarkBlue,
                ConsoleColor.Green when direction < 0 => ConsoleColor.DarkGreen,
                ConsoleColor.Cyan when direction < 0 => ConsoleColor.DarkCyan,
                ConsoleColor.Red when direction < 0 => ConsoleColor.DarkRed,
                ConsoleColor.Magenta when direction < 0 => ConsoleColor.DarkMagenta,
                ConsoleColor.Yellow when direction < 0 => ConsoleColor.DarkYellow,
                ConsoleColor.Gray when direction < 0 => ConsoleColor.DarkGray,
                ConsoleColor.White when direction < 0 => ConsoleColor.Gray,
                ConsoleColor.DarkGray when direction < 0 => ConsoleColor.Black,
                _ => Console.ForegroundColor
            };

            Console.ForegroundColor = newColor;
        }

        private static void SaveDrawing(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var symbol in drawnSymbols)
                {
                    writer.WriteLine($"{symbol.X},{symbol.Y},{symbol.Character},{(int)symbol.Color}");
                }
            }
        }

        private static void LoadDrawing(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File not found.");
                return;
            }

            drawnSymbols.Clear();
            Console.Clear();

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split(',');
                    if (parts.Length == 4 &&
                        int.TryParse(parts[0], out int x) &&
                        int.TryParse(parts[1], out int y) &&
                        char.TryParse(parts[2], out char character) &&
                        int.TryParse(parts[3], out int color))
                    {
                        var symbol = new DrawnSymbol(x, y, character, (ConsoleColor)color);
                        drawnSymbols.Add(symbol);
                        Console.SetCursorPosition(x, y);
                        Console.ForegroundColor = (ConsoleColor)color;
                        Console.Write(character);
                    }
                }
            }
        }

        private static void ClearDrawing()
        {
            drawnSymbols.Clear();
            Console.Clear();
        }

        private class DrawnSymbol
        {
            public int X { get; }
            public int Y { get; }
            public char Character { get; }
            public ConsoleColor Color { get; }

            public DrawnSymbol(int x, int y, char character, ConsoleColor color)
            {
                X = x;
                Y = y;
                Character = character;
                Color = color;
            }
        }
    }
}
