namespace SoghoyanProduction.Games.BattleShip;

public struct Coords
{
    public Coords(string s)
    {
        X = s[0] - 'a' + 1;
        Y = Int32.Parse(s[1..]);
    }

    public Int32 X { get; }
    public Int32 Y { get; }


}