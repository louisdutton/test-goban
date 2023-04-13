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

    public void set(int x, int y)
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
    string[] _goban;
    Dictionary<char, Status> _statusDictionary = new Dictionary<char, Status>()
    {
        {'.', Status.Empty},
        {'o', Status.White},
        {'#', Status.Black},
    };


    public Goban(string[] goban)
    {
        this._goban = goban;
    }

    bool InBounds(int x, int y)
    {
        return x >= 0 && y >= 0 && y < _goban.GetLength(0) && x < _goban[0].Length;
    }

    Status GetStatus(int x, int y)
    {
        return InBounds(x, y)
            ? _statusDictionary[_goban[y][x]]
            : Status.Out;
    }

    public bool IsTaken(int x, int y)
    {
        int cellCount = _goban.GetLength(0) * _goban[0].Length;
        HashSet<Vec2> _visited = new HashSet<Vec2>(cellCount);
        Status originStatus;
        Status neighborStatus;
        Neighbors neighbors = new Neighbors();
        Vec2 nextPosition = new Vec2();
        bool foundNext = false;

        bool HasLiberty(Vec2 origin)
        {
            originStatus = GetStatus(origin.x, origin.y);
            foundNext = false;

            neighbors.up.set(origin.x, origin.y - 1);
            neighbors.down.set(origin.x, origin.y + 1);
            neighbors.left.set(origin.x - 1, origin.y);
            neighbors.right.set(origin.x + 1, origin.y);

            foreach (Vec2 position in neighbors)
            {
                if (_visited.Contains(position)) continue;

                neighborStatus = GetStatus(position.x, position.y);

                if (neighborStatus == Status.Empty) return true;
                if (neighborStatus == originStatus)
                {
                    _visited.Add(position);
                    nextPosition = position;
                    foundNext = true;
                }
            }

            if (!foundNext) return false;

            return HasLiberty(nextPosition);

        };

        return !HasLiberty(new Vec2(x, y));
    }

}
