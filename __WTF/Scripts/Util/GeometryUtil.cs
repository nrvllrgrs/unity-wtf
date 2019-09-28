using UnityEngine;

public static class GeometryUtil
{
	public static Mesh CreateCone(int size, float height, float radius)
	{
		Mesh mesh = new Mesh();

		Vector3[] vertices = new Vector3[size + 2];
		Vector2[] uv = new Vector2[vertices.Length];
		int[] triangles = new int[(size * 2) * 3];

		vertices[0] = Vector3.zero;
		uv[0] = new Vector2(0.5f, 0f);

		for (int i = 0, n = size - 1; i < size; i++)
		{
			float ratio = (float)i / n;
			float r = ratio * (Mathf.PI * 2f);
			float x = Mathf.Cos(r) * radius;
			float z = Mathf.Sin(r) * radius;

			vertices[i + 1] = new Vector3(x, 0f, z);
			uv[i + 1] = new Vector2(ratio, 0f);
		}

		vertices[size + 1] = new Vector3(0f, height, 0f);
		uv[size + 1] = new Vector2(0.5f, 1f);

		// Construct bottom
		for (int i = 0, n = size - 1; i < n; i++)
		{
			int offset = i * 3;
			triangles[offset] = 0;
			triangles[offset + 1] = i + 1;
			triangles[offset + 2] = i + 2;
		}

		// Construct sides
		int bottomOffset = size * 3;
		for (int i = 0, n = size - 1; i < n; i++)
		{
			int offset = i * 3 + bottomOffset;
			triangles[offset] = i + 1;
			triangles[offset + 1] = size + 1;
			triangles[offset + 2] = i + 2;
		}

		mesh.vertices = vertices;
		mesh.uv = uv;
		mesh.triangles = triangles;
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();

		return mesh;
	}
}