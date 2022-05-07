using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridDirection
{
    public readonly Vector2Int vector;
    
    private GridDirection(int x, int y)
	{
        vector = new Vector2Int(x, y);
	}

    public static implicit operator Vector2Int(GridDirection dir)
	{
        return dir.vector;
	}

	public static GridDirection GetDirectionFromV2I(Vector2Int vector)
	{
        return CardinalAndInterCardinalDirections.DefaultIfEmpty(None).FirstOrDefault(dir => dir == vector);
    }

	//
	public static readonly GridDirection None = new GridDirection(0, 0);
    public static readonly GridDirection North = new GridDirection(0, 1);
    public static readonly GridDirection South = new GridDirection(0, -1);
    public static readonly GridDirection East = new GridDirection(1, 0);
    public static readonly GridDirection West = new GridDirection(-1, 0);

    // Diagonals
    public static readonly GridDirection NorthEast = new GridDirection(1, 1);
    public static readonly GridDirection NorthWest = new GridDirection(-1, 1);
    public static readonly GridDirection SouthEast = new GridDirection(1, -1);
    public static readonly GridDirection SouthWest = new GridDirection(-1, -1);

    public static readonly List<GridDirection> CardinalDirections = new List<GridDirection>
    {
        North,
        East,
        South,
        West
    };

    public static readonly List<GridDirection> CardinalAndInterCardinalDirections = new List<GridDirection>
    {
        North,
        East,
        South,
        West,
        NorthEast,
        NorthWest,
        SouthEast,
        SouthWest
    };

    public static readonly List<GridDirection> AllDirections = new List<GridDirection>
    {
        None,
        North,
        East,
        South,
        West,
        NorthEast,
        NorthWest,
        SouthEast,
        SouthWest
	};
}
