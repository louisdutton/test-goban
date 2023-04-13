using System.Collections.Generic;

namespace Go;

public enum Status
{
    White = 1,
    Black = 2,
    Empty = 3,
    Out = 4,
}

public struct Vec2
{
    public int x;
    public int y;

    public Vec2(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}

public struct Neighbors
{
    public Vec2 up;
    public Vec2 down;
    public Vec2 left;
    public Vec2 right;

    public Neighbors(int x, int y)
    {
        up = new Vec2(x, y - 1);
        down = new Vec2(x, y + 1);
        left = new Vec2(x - 1, y);
        right = new Vec2(x + 1, y);
    }

    public IEnumerator<Vec2> GetEnumerator()
    {
        yield return up;
        yield return down;
        yield return left;
        yield return right;
    }
}


public class Goban
{
    private string[] _goban;
    private Dictionary<char, Status> _statusDictionary = new Dictionary<char, Status>()
    {
        {'.', Status.Empty},
        {'o', Status.White},
        {'#', Status.Black},
    };


    public Goban(string[] goban)
    {
        this._goban = goban;
    }

    /// <summary>
    /// Get the status of a given position
    /// </summary>
    private Status GetStatus(int x, int y)
    {
        if (x >= 0 && y >= 0 && y < _goban.GetLength(0) && x < _goban[0].Length)
        {
            return _statusDictionary[_goban[y][x]];
        }
        else return Status.Out;
    }

    /// <summary>
    /// Returns true if the stone at the given position is taken.
    /// </summary>
    public bool IsTaken(int x, int y)
    {
        var _visited = new HashSet<Vec2>(_goban.GetLength(0) * _goban[0].Length);
        Status stone;
        Neighbors neighbors = new Neighbors();
        Vec2 nextPosition = new Vec2();
        bool foundNext = false;

        bool HasLiberty(int _x, int _y)
        {
            stone = GetStatus(_x, _y);
            foundNext = false;
            neighbors = new Neighbors(_x, _y);

            foreach (Vec2 position in neighbors)
            {
                if (_visited.Contains(position)) continue;

                Status neighborStatus = GetStatus(position.x, position.y);

                if (neighborStatus == Status.Empty) return true;
                if (neighborStatus == stone)
                {
                    _visited.Add(position);
                    nextPosition = position;
                    foundNext = true;
                }
            }

            if (!foundNext) return false;

            return HasLiberty(nextPosition.x, nextPosition.y);

        };

        return !HasLiberty(x, y);
    }

}
