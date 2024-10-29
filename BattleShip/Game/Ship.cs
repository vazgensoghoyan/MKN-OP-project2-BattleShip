namespace SoghoyanProduction.Games.BattleShip;

public struct Pole
{
    public Pole(Int32 length)
    {
        Length = length;
        IsHit = false;
    }

    public Int32 Length { get; }
    public bool IsHit { get; private set; }

    public static bool operator ==(Pole left, Pole right) => left.Length == right.Length;

    public static bool operator !=(Pole left, Pole right) => !(left == right);

    public static implicit operator Pole(Int32 number) => new Pole(number);

    public void Hit() => IsHit = true;
}