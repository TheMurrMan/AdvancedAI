using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Vector2I
{
	public int x;
	public int y;
	public float d;

	public Vector2I(int nx, int ny)
	{
		x = nx;
		y = ny;
		d = 1;
	}

	public Vector2I(int nx, int ny, float nd)
	{
		x = nx;
		y = ny;
		d = nd;
	}
}

public class InfluenceMap : GridData
{
	List<ITower> towers = new List<ITower>();

	float[,] influences;
	public float InfluenceSize { get; set; }
	public float Momentum { get; set; }
	public int Width { get { return influences.GetLength(0); } }
	public int Height { get { return influences.GetLength(1); } }
	public float GetValue(int x, int y)
	{
		return influences[x, y];
	}

	public InfluenceMap(int width, int height, float influenceSize, float momentum)
	{
		influences = new float[width, height];
		InfluenceSize = influenceSize;
		Momentum = momentum;
	}

	public void SetInfluence(Vector2I pos, float value)
	{
		if (pos.x < Width && pos.y < Height)
		{
			influences[pos.x, pos.y] = value;
		}
	}

	public void RegisterTower(ITower t)
	{
		towers.Add(t);
	}

	public void DeregisterTower(ITower t)
	{
		towers.Remove(t);
	}

	public void UpdateTower()
	{
		UpdateTowersInfluence();
		UpdateInfluences();
	}

	void UpdateTowersInfluence()
	{
		foreach (ITower p in towers)
		{
			SetInfluence(p.GridPosition, p.Value);
		}
	}

	void UpdateInfluences()
	{
		for (int x = 0; x < influences.GetLength(0); ++x)
		{
			for (int y = 0; y < influences.GetLength(1); ++y)
			{
				//Debug.Log("at " + x + ", " + y);
				float maxInf = 0.0f;
				float minInf = 0.0f;
				Vector2I[] neighbors = GetNeighbors(x, y);
				foreach (Vector2I n in neighbors)
				{
					//Debug.Log(n.x + " " + n.y);
					float inf = influences[n.x, n.y] * Mathf.Exp(-InfluenceSize * n.d); //* Decay;
					maxInf = Mathf.Max(inf, maxInf);
					minInf = Mathf.Min(inf, minInf);
				}

				if (Mathf.Abs(minInf) > maxInf)
				{
					influences[x, y] = Mathf.Lerp(influences[x, y], minInf, Momentum);
				}
				else
				{
					influences[x, y] = Mathf.Lerp(influences[x, y], maxInf, Momentum);
				}
			}
		}
	}

	Vector2I[] GetNeighbors(int x, int y)
	{
		List<Vector2I> retVal = new List<Vector2I>();

		// as long as not in left edge
		if (x > 0)
		{
			retVal.Add(new Vector2I(x - 1, y));
		}

		// as long as not in right edge
		if (x < influences.GetLength(0) - 1)
		{
			retVal.Add(new Vector2I(x + 1, y));
		}

		// as long as not in bottom edge
		if (y > 0)
		{
			retVal.Add(new Vector2I(x, y - 1));
		}

		// as long as not in upper edge
		if (y < influences.GetLength(1) - 1)
		{
			retVal.Add(new Vector2I(x, y + 1));
		}


		// diagonals

		// as long as not in bottom-left
		if (x > 0 && y > 0)
		{
			retVal.Add(new Vector2I(x - 1, y - 1, 1.4142f));
		}

		// as long as not in upper-right
		if (x < influences.GetLength(0) - 1 && y < influences.GetLength(1) - 1)
		{
			retVal.Add(new Vector2I(x + 1, y + 1, 1.4142f));
		}

		// as long as not in upper-left
		if (x > 0 && y < influences.GetLength(1) - 1)
		{
			retVal.Add(new Vector2I(x - 1, y + 1, 1.4142f));
		}

		// as long as not in bottom-right
		if (x < influences.GetLength(0) - 1 && y > 0)
		{
			retVal.Add(new Vector2I(x + 1, y - 1, 1.4142f));
		}

		return retVal.ToArray();
	}
}
