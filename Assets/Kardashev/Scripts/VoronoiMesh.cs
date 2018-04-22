using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent (typeof (MeshFilter), typeof (MeshRenderer))]
public class VoronoiMesh : MonoBehaviour {

	private Mesh _voronoiMesh;
	private MeshCollider _meshCollider;
	
	private List<Vector3> _vertices = new List<Vector3> ();
	private List<int> _triangles = new List<int> ();
	private List<Vector2> _uvs = new List<Vector2> ();
	private List<Color> _colors = new List<Color> ();

	private void Awake () {
		GetComponent<MeshFilter> ().mesh = _voronoiMesh = new Mesh ();
		_voronoiMesh.name = "Voronoi Mesh";
		_meshCollider = gameObject.AddComponent<MeshCollider> ();
		
		_vertices = new List<Vector3> ();
		_triangles = new List<int> ();
		_colors = new List<Color> ();
	}

	public void Triangulate (VoronoiCell[] cells) {
		_voronoiMesh.Clear ();
		_vertices.Clear ();
		_triangles.Clear ();
		_colors.Clear ();
		
		for (int i = 0; i < cells.Length; ++i) {
			Triangulate (cells[i]);
		}
		
		_voronoiMesh.SetVertices (_vertices);
		_voronoiMesh.SetTriangles (_triangles, 0);
		_voronoiMesh.SetColors (_colors);
		_voronoiMesh.RecalculateNormals ();

		_meshCollider.sharedMesh = _voronoiMesh;
	}

	private void Triangulate (VoronoiCell cell) {
		Vector3 center = cell.transform.localPosition;

		for (int i = 0; i < cell.Corners.Count - 1; ++i) {
			AddTriangle (
				center,
				center + cell.Corners[i],
				center + cell.Corners[i + 1]);
			AddTriangleColor (cell.Color);
		}
	}

	private void AddTriangle (Vector3 v1, Vector3 v2, Vector3 v3) {
		int vertexIndex = _vertices.Count;
		_vertices.Add (v1);
		_vertices.Add (v2);
		_vertices.Add (v3);
		
		_triangles.Add (vertexIndex);
		_triangles.Add (vertexIndex + 1);
		_triangles.Add (vertexIndex + 2);
	}

	private void AddTriangleColor (Color color) {
		_colors.Add (color);
		_colors.Add (color);
		_colors.Add (color);
	}
}
