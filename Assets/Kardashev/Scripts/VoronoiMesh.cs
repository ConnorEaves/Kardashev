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
		for (VoronoiDirection d = 0; d < cell.Neighbors.Count; ++d) {
			Triangulate (cell, d);
		}
	}

	private void Triangulate (VoronoiCell cell, VoronoiDirection direction) {
		Vector3 center = cell.transform.localPosition;
		Vector3 v1 = center + VoronoiMetrics.GetFirstSolidCorner (cell, direction);
		Vector3 v2 = center + VoronoiMetrics.GetSecondSolidCorner (cell, direction);
		
		AddTriangle (center, v1, v2);
		AddTriangleColor (cell.Color);

		if (cell.EdgeConnections.Contains (direction)) {
			TriangulateConnection (cell, direction, v1, v2);
		} 
	}

	private void TriangulateConnection (VoronoiCell cell, VoronoiDirection direction, Vector3 v1, Vector3 v2) {
		VoronoiCell neighbor = cell.GetNeighbor (direction);
		
		Vector3 bridge = VoronoiMetrics.GetBridge (cell, direction);
		Vector3 v3 = v1 + bridge;
		Vector3 v4 = v2 + bridge;
		
		AddQuad (v1, v2, v3, v4);
		AddQuadColor (cell.Color, neighbor.Color);

		if (cell.CornerConnections.Contains (direction)) {
			VoronoiCell nextNeighbor = cell.GetNeighbor (direction.Next (cell));
			AddTriangle (v2, v4, v2 + VoronoiMetrics.GetBridge (cell, direction.Next (cell)));
			AddTriangleColor (cell.Color, neighbor.Color, nextNeighbor.Color);
		}
		
		
	}

	// Triangle creation
	
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
	
	private void AddTriangleColor (Color c1, Color c2, Color c3) {
		_colors.Add (c1);
		_colors.Add (c2);
		_colors.Add (c3);
	}

	// Quad creation
	
	private void AddQuad (Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4) {
		int vertexIndex = _vertices.Count;
		_vertices.Add (v1);
		_vertices.Add (v2);
		_vertices.Add (v3);
		_vertices.Add (v4);
		
		_triangles.Add (vertexIndex);
		_triangles.Add (vertexIndex + 2);
		_triangles.Add (vertexIndex + 1);
		_triangles.Add (vertexIndex + 1);
		_triangles.Add (vertexIndex + 2);
		_triangles.Add (vertexIndex + 3);
	}
	
	private void AddQuadColor (Color c1, Color c2) {
		_colors.Add (c1);
		_colors.Add (c1);
		_colors.Add (c2);
		_colors.Add (c2);
	}
	
	private void AddQuadColor (Color c1, Color c2, Color c3, Color c4) {
		_colors.Add (c1);
		_colors.Add (c2);
		_colors.Add (c3);
		_colors.Add (c4);
	}
}
