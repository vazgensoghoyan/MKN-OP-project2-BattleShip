namespace SoghoyanProduction.Games.BattleShip;

public struct Pole
{
    public Pole(Int32 length)
    {
        Length = length;
        IsHit = false;
    }

    public Int32 Length { get; }
    public Boolean IsHit { get; private set; }

    public static Boolean operator ==(Pole left, Pole right) => left.Length == right.Length;

    public static Boolean operator !=(Pole left, Pole right) => !(left == right);

    public override Boolean Equals(object? obj)
    {
        if (obj is null || obj is not Pole)
            return false;

        return this == (Pole)obj;
    }

    public override int GetHashCode() => HashCode.Combine(Length, IsHit);

    public static implicit operator Pole(Int32 number) => new Pole(number);

    public void Hit() => IsHit = true;

    public override String ToString() => Length.ToString();
}