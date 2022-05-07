using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface GridData
{
	int Width { get; }
	int Height { get; }
	float GetValue(int x, int y);
}

public class GridDisplay : MonoBehaviour
{
	MeshRenderer meshRenderer;
	MeshFilter meshFilter;
	Mesh mesh;

	GridData data;

	[SerializeField] Material material;
	[SerializeField] Color neutralColor = Color.white;

	[SerializeField] Color positiveColor = Color.red;

	[SerializeField] Color negativeColor = Color.blue;

	Color[] colors;

	public void SetGridData(GridData m)
	{
		data = m;
	}

	public void CreateMesh(float gridSize)
	{
		mesh = new Mesh();
		mesh.name = name;
		meshFilter = gameObject.AddComponent(typeof(MeshFilter)) as MeshFilter;
		meshRenderer = gameObject.AddComponent(typeof(MeshRenderer)) as MeshRenderer;

		meshFilter.mesh = mesh;
		meshRenderer.material = material;

		float height = transform.position.y;
		float startX = 0;
		float startZ = 0;

		// create squares starting at bottomLeftPos
		List<Vector3> verts = new List<Vector3>();
		for (int y = 0; y < data.Height; ++y)
		{
			for (int x = 0; x < data.Width; ++x)
			{
				Vector3 bottomLeft  = new Vector3(startX + (x * gridSize), height, startZ + (y * gridSize));
				Vector3 bottomRight = new Vector3(startX + ((x + 1) * gridSize), height, startZ + (y * gridSize));
				Vector3 topLeft = new Vector3(startX + (x * gridSize), height, startZ + ((y + 1) * gridSize));
				Vector3 topRight = new Vector3(startX + ((x + 1) * gridSize), height, startZ + ((y + 1) * gridSize));

				verts.Add(bottomLeft);
				verts.Add(bottomRight);
				verts.Add(topLeft);
				verts.Add(topRight);
			}
		}

		List<Color> newColors = new List<Color>();
		for (int y = 0; y < data.Height; ++y)
		{
			for (int x = 0; x < data.Width; ++x)
			{
				newColors.Add(Color.white);
				newColors.Add(Color.white);
				newColors.Add(Color.white);
				newColors.Add(Color.white);
			}
		}
		colors = newColors.ToArray();

		List<Vector3> norms = new List<Vector3>();
		for (int y = 0; y < data.Height; ++y)
		{
			for (int x = 0; x < data.Width; ++x)
			{
				norms.Add(Vector3.up);
				norms.Add(Vector3.up);
				norms.Add(Vector3.up);
				norms.Add(Vector3.up);
			}
		}

		List<Vector2> uvs = new List<Vector2>();
		for (int y = 0; y < data.Height; ++y)
		{
			for (int x = 0; x < data.Width; ++x)
			{
				uvs.Add(new Vector2(0, 0));
				uvs.Add(new Vector2(1, 0));
				uvs.Add(new Vector2(0, 1));
				uvs.Add(new Vector2(1, 1));
			}
		}

		List<int> tris = new List<int>();
		for (int i = 0; i < verts.Count; i += 4)
		{
			int bottomLeft = i;
			int bottomRight = i + 1;
			int topLeft = i + 2;
			int topRight = i + 3;

			tris.Add(bottomLeft);
			tris.Add(topLeft);
			tris.Add(bottomRight);

			tris.Add(topLeft);
			tris.Add(topRight);
			tris.Add(bottomRight);

		}

		mesh.vertices = verts.ToArray();
		mesh.normals = norms.ToArray();
		mesh.uv = uvs.ToArray();
		mesh.colors = colors;
		mesh.triangles = tris.ToArray();
	}

	void SetColor(int x, int y, Color c)
	{
		int i = ((y * data.Width) + x) * 4;
		colors[i] = c;
		colors[i + 1] = c;
		colors[i + 2] = c;
		colors[i + 3] = c;
	}

	void Update()
	{
		for (int y = 0; y < data.Height; ++y)
		{
			for (int x = 0; x < data.Width; ++x)
			{
				float value = data.GetValue(x, y);
				Color c = neutralColor;
				if (value > 0)
				{
					c = Color.Lerp(neutralColor, positiveColor, value / 0.5f);
				}

				else
				{
					c = Color.Lerp(neutralColor, negativeColor, -value / 0.5f);
				}
				SetColor(x, y, c);
			}
		}

		mesh.colors = colors;
	}
}
