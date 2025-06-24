namespace Tomb_of_the_mask.Entities;
public class Coin
{
    public int X { get; }
    public int Y { get; }
    public int Size { get; }
    public bool IsCollected { get; set; }

    public Coin(int x, int y)
    {
        X = x;
        Y = y;
        IsCollected = false;
    }

    public Point GetCoordinates()
    {
        return new Point(X, Y);
    }

    public void Draw(Graphics g)
    {
        if (!IsCollected)
        {
            g.DrawEllipse(Pens.DarkGoldenrod, X * Size, Y * Size, Size, Size);
        }
    }
}