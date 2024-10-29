using System.Text;
using System.Text.RegularExpressions;
using static System.Math;

namespace SoghoyanProduction.Games.BattleShip;

public static class BattleShip
{
    private static int[,] _field1;
    private static int[,] _field2;

    static BattleShip()
    {
        _field1 = new int[12, 12];
        _field2 = new int[12, 12];

        for (int i = 12; i < 12; i++)
        {
            for (int j = 12; j < 12; j++)
            {
                _field1[i, j] = 0;
                _field2[i, j] = 0;
            }
        }
    }

    public static void PlayGame()
    {
        Console.Clear();

        ReadShips(EPlayer.First);
        ReadShips(EPlayer.Second);

        var currPlayer = EPlayer.First;

        while (true)
        {


            currPlayer = Enemy(currPlayer);
        }
    }

    private static EPlayer Enemy(EPlayer curr) => (curr == EPlayer.First) ? EPlayer.Second : EPlayer.First;

    private static bool AreAllHit(EPlayer player)
    {
        return false;
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
                    ReadShip(shipLength, player); 
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    j--;
                }
            }
        }
    }

    private static void ReadShip(int shipLength, EPlayer player)
    {
        Print();
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

        var areEq1 = true;
        var areEq2 = true;
        for (int i = 1; i < shipLength; i++)
            if ( coords[i].X != coords[i-1].X )
                areEq1 = false;
        for (int i = 1; i < shipLength; i++)
            if ( coords[i].Y != coords[i-1].Y )
                areEq2 = false;

        if ( !areEq1 && !areEq2 )
            throw new Exception("Кораблики не могут ходить по диагонали!");

        var currField = (player == EPlayer.First) ? _field1 : _field2;
        
        foreach (var coord in coords)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (currField[coord.X + i, coord.Y + j] != 0)
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

    public static void Print()
    {
        Console.Clear();
        Console.WriteLine("  1 2 3 4 5 6 7 8 9 10");
        var s = "abcdefghij";

        for (int i = 1; i <= 10; ++i)
        {
            Console.Write(s[i-1] + " ");
            for (int j = 1; j <= 10; ++j)
            {
                if (_field1[i, j] != 0)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write(_field1[i, j] + " ");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else Console.Write(_field1[i, j] + " ");
            }
            Console.WriteLine();
        }
    }
}
