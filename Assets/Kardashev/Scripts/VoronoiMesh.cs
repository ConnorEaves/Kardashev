using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (MeshFilter), typeof (MeshRenderer))]
public class VoronoiMesh : MonoBehaviour {

	public bool UseCollider, UseUV, UseColors;
	
	private Mesh _voronoiMesh;
	private MeshCollider _meshCollider;
	
	[NonSerialized] private List<Vector3> _vertices;
	[NonSerialized] private List<int> _triangles;
	[NonSerialized] private List<Vector2> _uvs;
	[NonSerialized] private List<Color> _colors;

	private void Awake () {
		GetComponent<MeshFilter> ().mesh = _voronoiMesh = new Mesh ();
		
		if (UseCollider) {
			_meshCollider = gameObject.AddComponent<MeshCollider> ();
		}
		
		_voronoiMesh.name = "Voronoi Mesh";
	}

	public void Clear () {
		_voronoiMesh.Clear();
		_vertices = ListPool<Vector3>.Get ();
		_triangles = ListPool<int>.Get ();

		if (UseUV) {
			_uvs = ListPool<Vector2>.Get ();
		}

		if (UseColors) {
			_colors = ListPool<Color>.Get ();
		}
	}

	public void Apply () {
		_voronoiMesh.SetVertices (_vertices);
		ListPool<Vector3>.Add (_vertices);
		_voronoiMesh.SetTriangles (_triangles, 0);
		ListPool<int>.Add (_triangles);

		if (UseUV) {
			_voronoiMesh.SetUVs (0, _uvs);
			ListPool<Vector2>.Add (_uvs);
		}
		
		if (UseColors) {
			_voronoiMesh.SetColors (_colors);
			ListPool<Color>.Add (_colors);
		}
		
		_voronoiMesh.RecalculateNormals ();

		if (UseCollider) {
			_meshCollider.sharedMesh = _voronoiMesh;
		}
	}

	// Triangle creation
	
	public void AddTriangle (Vector3 v1, Vector3 v2, Vector3 v3) {
		int vertexIndex = _vertices.Count;
		_vertices.Add (VoronoiMetrics.Perturb (v1));
		_vertices.Add (VoronoiMetrics.Perturb (v2));
		_vertices.Add (VoronoiMetrics.Perturb (v3));
		
		_triangles.Add (vertexIndex);
		_triangles.Add (vertexIndex + 1);
		_triangles.Add (vertexIndex + 2);
	}

	public void AddTriangleUnperturbed (Vector3 v1, Vector3 v2, Vector3 v3) {
		int vertexIndex = _vertices.Count;
		_vertices.Add (v1);
		_vertices.Add (v2);
		_vertices.Add (v3);
		
		_triangles.Add (vertexIndex);
		_triangles.Add (vertexIndex + 1);
		_triangles.Add (vertexIndex + 2);
	}

	public void AddTriangleUV (Vector2 uv1, Vector2 uv2, Vector2 uv3) {
		_uvs.Add (uv1);
		_uvs.Add (uv2);
		_uvs.Add (uv3);
	}

	public void AddTriangleColor (Color color) {
		_colors.Add (color);
		_colors.Add (color);
		_colors.Add (color);
	}
	
	public void AddTriangleColor (Color c1, Color c2, Color c3) {
		_colors.Add (c1);
		_colors.Add (c2);
		_colors.Add (c3);
	}

	// Quad creation
	
	public void AddQuad (Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4) {
		int vertexIndex = _vertices.Count;
		_vertices.Add (VoronoiMetrics.Perturb (v1));
		_vertices.Add (VoronoiMetrics.Perturb (v2));
		_vertices.Add (VoronoiMetrics.Perturb (v3));
		_vertices.Add (VoronoiMetrics.Perturb (v4));
		
		_triangles.Add (vertexIndex);
		_triangles.Add (vertexIndex + 2);
		_triangles.Add (vertexIndex + 1);
		_triangles.Add (vertexIndex + 1);
		_triangles.Add (vertexIndex + 2);
		_triangles.Add (vertexIndex + 3);
	}
	
	public void AddQuadUV (Vector2 uv1, Vector2 uv2, Vector2 uv3, Vector2 uv4) {
		_uvs.Add (uv1);
		_uvs.Add (uv2);
		_uvs.Add (uv3);
		_uvs.Add (uv4);
	}

	public void AddQuadUV (float uMin, float uMax, float vMin, float vMax) {
		_uvs.Add (new Vector2 (uMin, vMin));
		_uvs.Add (new Vector2 (uMax, vMin));
		_uvs.Add (new Vector2 (uMin, vMax));
		_uvs.Add (new Vector2 (uMax, vMax));
	}
	
	public void AddQuadColor (Color color) {
		_colors.Add (color);
		_colors.Add (color);
		_colors.Add (color);
		_colors.Add (color);
	}
	
	public void AddQuadColor (Color c1, Color c2) {
		_colors.Add (c1);
		_colors.Add (c1);
		_colors.Add (c2);
		_colors.Add (c2);
	}
	
	public void AddQuadColor (Color c1, Color c2, Color c3, Color c4) {
		_colors.Add (c1);
		_colors.Add (c2);
		_colors.Add (c3);
		_colors.Add (c4);
	}
}
