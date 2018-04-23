using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoronoiGridChunk : MonoBehaviour {

	private List<VoronoiCell> _cells;
	private Queue<VoronoiCell> _cellQueue;

	private Color _color;

	private VoronoiMesh _voronoiMesh;

	private void Awake () {
		_cells = new List<VoronoiCell> ();
		_cellQueue = new Queue<VoronoiCell> ();
		_voronoiMesh = GetComponentInChildren<VoronoiMesh> ();
		_color = Random.ColorHSV ();
	}
	
	private void OnDrawGizmos () {
		Gizmos.DrawCube (transform.position, Vector3.one * 0.1f);
	}

	public void AddCell (VoronoiCell cell) {
		_cells.Add (cell);
		_cellQueue.Enqueue (cell);
		cell.Chunk = this;
		cell.transform.SetParent (transform, false);
		cell.Color = _color;
	}
	
	public bool ExtendChunk () {
		while (_cellQueue.Count > 0) {
			VoronoiCell cell = _cellQueue.Peek ();
			
			foreach (VoronoiCell neighbor in cell.Neighbors) {
				if (neighbor.Chunk == null) {
					AddCell (neighbor);
					return true;
				}
			}
			_cellQueue.Dequeue ();
		}
		return false;
	}

	public void Refresh () {
		enabled = true;
	}

	private void LateUpdate () {
		_voronoiMesh.Triangulate (_cells.ToArray ());
		enabled = false;
	}

	public void AddToAtmosphere (SgtAtmosphere atmosphere) {
		atmosphere.AddInnerRenderer (GetComponentInChildren<MeshRenderer>());
	}
}
