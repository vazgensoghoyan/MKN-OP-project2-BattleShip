using System.Text.RegularExpressions;
using static System.Math;

namespace SoghoyanProduction.Games.BattleShip;

public static class BattleShip
{
    private static Int32[,] _field1;
    private static Int32[,] _field2;

    static BattleShip()
    {
        _field1 = new Int32[12, 12];
        _field2 = new Int32[12, 12];

        ReadShips(EPlayer.First);
        ReadShips(EPlayer.Second);
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
        Console.Write("Введите корабль длины {0}: ", shipLength);
        var s = Console.ReadLine();

        if ( s == null || !new Regex(@"[a-j]([1-9]|10) [a-j]([1-9]|10)").IsMatch(s) )
            throw new Exception("Неправильный формат вводимых данных!");

        String[] split = s.Split().Where(a => a.Length > 0).ToArray();
        var (aX, aY, bX, bY) = ( split[0][0] - 'a' + 1, Int32.Parse(split[0][1..]),
                                 split[1][0] - 'a' + 1, Int32.Parse(split[1][1..]) );

        if ( aX - bX != 0 && aY - bY != 0 )
            throw new Exception("Кораблики не могут идти по диагонали!");

        if ( Abs(aX - bX) != shipLength && Abs(aY - bY) != shipLength )
            throw new Exception("Введен неправильный кораблик!");

        var currField = (player == EPlayer.First) ? _field1 : _field2;

        for (int i = aX - 1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
            }
        }
    }

    public static string GetString()
    {
        var result = string.Empty;

        for (int i = 0; i < 10; ++i)
        {
            for (int j = 0; j < 10; ++j)
            {
                result +=  _field1[i, j] + ' ';
            }
            result += '\n';
        }
        
        return result;
    }
}
