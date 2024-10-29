using System.Text.RegularExpressions;
using static System.Math;

namespace SoghoyanProduction.Games.BattleShip;

public static class BattleShip
{
    private static Pole[,] _field1;
    private static Pole[,] _field2;

    private static String _lastException;

    static BattleShip()
    {
        _field1 = new Pole[12, 12];
        _field2 = new Pole[12, 12];

        for (int i = 12; i < 12; i++)
        {
            for (int j = 12; j < 12; j++)
            {
                _field1[i, j] = 0;
                _field2[i, j] = 0;
            }
        }

        _lastException = "";
    }

    public static void PlayGame()
    {
        Console.Clear();
        Console.WriteLine("Добрый день!");
        Console.WriteLine("Кораблики надо вводить в формате a1 a2 a3 (вводить все поля, на которых будет находиться кораблик).");
        Console.WriteLine("Сначала данные вводит игрок 1, потом игрок 2.");
        Console.WriteLine("Нажмите Enter, чтобы продолжить...");

        while (Console.ReadKey().Key != ConsoleKey.Enter) { }

        ReadShips(EPlayer.First);
        ReadShips(EPlayer.Second);

        var currPlayer = EPlayer.First;

        while (true)
        {
            if ( AreAllHit(EPlayer.First) || AreAllHit(EPlayer.Second) )
                break;

            Console.WriteLine("Вот ваше поле: ");
            Print(currPlayer);
            PrintForEnemy(currPlayer);

            var coords = ReadMove(currPlayer);

            if (currPlayer == EPlayer.First)
            {
                if (_field2[coords.X, coords.Y].Length != 0)
                {
                    _field2[coords.X, coords.Y].Hit();
                    Console.WriteLine("Есть попадание! Давайте еще");
                    currPlayer = Enemy(currPlayer);
                }
            }
            else
            {
                if (_field1[coords.X, coords.Y].Length != 0)
                {
                    _field1[coords.X, coords.Y].Hit();
                    Console.WriteLine("Есть попадание! Давайте еще");
                    currPlayer = Enemy(currPlayer);
                }
            }

            currPlayer = Enemy(currPlayer);
        }

        Console.Clear();
        Console.WriteLine("Спасибо за игру!");

        if ( AreAllHit(EPlayer.First) )
            Console.WriteLine("Игрок 2 выиграл!");
        else
            Console.WriteLine("Игрок 1 выиграл!");
    }

    public static Coords ReadMove(EPlayer player)
    {
        Coords coords = new Coords("a31");

        while (coords.Y == 31)
        {
            try
            {
                if (player == EPlayer.First)
                    Console.Write("Игрок 1, ");
                else
                    Console.Write("Игрок 2, ");

                Console.Write("введите поле, которое хотите бить: ");

                var s = Console.ReadLine();

                if ( s == null || !new Regex(@"([a-j]([1-9]|10))").IsMatch(s) )
                    throw new Exception("Неправильный формат вводимых данных!");

                var split = s.Split().Where(a => a.Length > 0).ToArray();

                if (split.Length != 1)
                    throw new Exception("Неправильный формат вводимых данных!");

                coords = new Coords(split[0]);
            }
            catch (Exception e) { Console.WriteLine(e); }
        }

        return coords;
    }

    private static EPlayer Enemy(EPlayer curr) => (curr == EPlayer.First) ? EPlayer.Second : EPlayer.First;

    private static bool AreAllHit(EPlayer player)
    {
        var currField = (player == EPlayer.First) ? _field1 : _field2;

        for (int i = 0; i < 12; i++)
            for (int j = 0; j < 12; j++)
                if ( currField[i, j] != 0 && !currField[i, j].IsHit )
                    return false;

        return true;
    }

    private static void ReadShips(EPlayer player)
    {
        for (int shipLength = 1; shipLength <= 4; shipLength++)
        {
            int shipsCount = 5 - shipLength;

            for (int j = 0; j < shipsCount; j++)
            {
                try 
                {
                    Print(player);
                    _lastException = string.Empty;
                    var coords = ReadPoles(shipLength);

                    var currField = (player == EPlayer.First) ? _field1 : _field2;

                    foreach (var coord in coords)
                    {
                        for (int ii = -1; ii <= 1; ii++)
                        {
                            for (int jj = -1; jj <= 1; jj++)
                            {
                                if (currField[coord.X + ii, coord.Y + jj] != 0)
                                    throw new Exception("Не там поставлен кораблик!");
                            }
                        }
                    }

                    foreach (var coord in coords)
                    {
                        if (player == EPlayer.First)
                            _field1[coord.X, coord.Y] = shipLength;
                        else
                            _field2[coord.X, coord.Y] = shipLength;
                    }
                }
                catch (Exception e)
                {
                    _lastException = e.Message;
                    j--;
                }
            }
        }
    }

    private static Coords[] ReadPoles(int shipLength)
    {
        Console.Write("Введите корабль длины {0}: ", shipLength);
        var s = Console.ReadLine();

        if ( s == null || !new Regex(@"([a-j]([1-9]|10))").IsMatch(s) )
            throw new Exception("Неправильный формат вводимых данных!");

        var split = s.Split().Where(a => a.Length > 0).ToArray();

        if ( split.Length != shipLength )
            throw new Exception("Введен неправильный кораблик!");

        var coords = new Coords[shipLength];
        for (int i = 0; i < shipLength; i++) 
            coords[i] = new Coords(split[i]);

        var isVertical = true;
        var isHorizontal = true;

        for (int i = 1; i < shipLength; i++)
        {
            if ( coords[i].X != coords[i-1].X ) isVertical = false;
            if ( coords[i].Y != coords[i-1].Y ) isHorizontal = false;
        }

        if ( !isVertical && !isHorizontal )
            throw new Exception("Кораблики не могут ходить по диагонали!");

        if ( shipLength > 1 )
        {
            if ( isHorizontal )
            {
                var a = ( coords[0].X < coords[1].X ) ? 1 : -1;

                for (int i = 1; i < shipLength; i++)
                    if ( coords[i].X - coords[i-1].X != a )
                        throw new Exception("Кораблики должны быть непрерывны!");
            }
            else
            {
                var a = (coords[0].Y < coords[1].Y) ? 1 : -1;

                for (int i = 1; i < shipLength; i++)
                    if ( coords[i].Y - coords[i-1].Y != a )
                        throw new Exception("Кораблики должны быть непрерывны!");
            }
        }

        return coords;
    }

    private static Int32[] GetIndexes(Int32 aX, Int32 aY, Int32 bX, Int32 bY)
    {
        var horDir = Math.Sign(bX - aX);
        var verDir = Math.Sign(bY - aY);

        Int32[] indexes = new Int32[(aX - bX + aY - bY) * 2];
        Int32 i = 0;

        while (aX != bX && aY != bY)
        {
            indexes[i++] = aX;
            indexes[i++] = aY;

            aX += horDir;
            aY += verDir;
        }

        return indexes;
    }

    public static void Print(EPlayer player)
    {
        var currField = (player == EPlayer.First) ? _field1 : _field2;

        Console.Clear();    
        Console.WriteLine("  1 2 3 4 5 6 7 8 9 10");
        var s = "abcdefghij";

        for (int i = 1; i <= 10; ++i)
        {
            Console.Write(s[i-1] + " ");
            for (int j = 1; j <= 10; ++j)
            {
                if (currField[i, j] != 0)
                {
                    Console.ForegroundColor = currField[i, j].IsHit ? ConsoleColor.Red : ConsoleColor.Blue;
                    Console.Write(currField[i, j] + " ");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else Console.Write(currField[i, j] + " ");
            }
            Console.WriteLine();
        }

        Console.WriteLine(_lastException);
    }


    public static void PrintForEnemy(EPlayer player)
    {
        var currField = (player == EPlayer.First) ? _field2 : _field1;

        Console.WriteLine("  1 2 3 4 5 6 7 8 9 10");
        var s = "abcdefghij";

        for (int i = 1; i <= 10; ++i)
        {
            Console.Write(s[i - 1] + " ");
            for (int j = 1; j <= 10; ++j)
            {
                if ( currField[i, j] != 0 && currField[i, j].IsHit )
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("1 ");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else Console.Write("0 ");
            }
            Console.WriteLine();
        }

        Console.WriteLine(_lastException);
    }
}
