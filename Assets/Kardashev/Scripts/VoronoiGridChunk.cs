using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class VoronoiGridChunk : MonoBehaviour {

	private List<VoronoiCell> _cells;
	private Queue<VoronoiCell> _cellQueue;

	public VoronoiMesh Terrain;
	public VoronoiMesh Rivers;

	private void Awake () {
		_cells = new List<VoronoiCell> ();
		_cellQueue = new Queue<VoronoiCell> ();
	}

	public void AddCell (VoronoiCell cell) {
		_cells.Add (cell);
		_cellQueue.Enqueue (cell);
		cell.Chunk = this;
		cell.transform.SetParent (transform, false);
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
		Triangulate ();
		enabled = false;
	}

	public void AddToAtmosphere (SgtAtmosphere atmosphere) {
		atmosphere.AddInnerRenderer (Terrain.GetComponent<MeshRenderer> ());
		atmosphere.AddInnerRenderer (Rivers.GetComponent<MeshRenderer> ());
	}
	
	public void Triangulate () {
		Terrain.Clear ();
		Rivers.Clear ();
		
		for (int i = 0; i < _cells.Count; ++i) {
			Triangulate (_cells[i]);
		}
		
		Terrain.Apply ();
		Rivers.Apply ();
	}

	private void Triangulate (VoronoiCell cell) {
		for (VoronoiDirection d = 0; d < cell.Neighbors.Count; ++d) {
			Triangulate (cell, d);
		}
	}

	private void Triangulate (VoronoiCell cell, VoronoiDirection direction) {
		Vector3 center = cell.transform.localPosition;
		
		EdgeVertices e = new EdgeVertices(
			center + VoronoiMetrics.GetFirstSolidCorner (cell, direction),
			center + VoronoiMetrics.GetSecondSolidCorner (cell, direction)
		);

		if (cell.HasRiver) {
			if (cell.HasRiverThroughEdge (direction)) {
				e.V3 = Vector3.ClampMagnitude (e.V3, cell.StreamBedElevation);
				if (cell.HasRiverBeginOrEnd) {
					TriangulateWithRiverBeginOrEnd (cell, direction, center, e);
				} else {
					TriangulateWithRiver (cell, direction, center, e);
				}
			} else {
				TriangulateAdjacentToRiver (cell, direction, center, e);
			}
		} else {
			TriangulateEdgeFan (center, e, cell.Color);
		}

		if (cell.EdgeConnections.Contains (direction)) {
			TriangulateConnection (cell, direction, e);
		} 
	}

	private void TriangulateAdjacentToRiver (VoronoiCell cell, VoronoiDirection direction, Vector3 center, EdgeVertices e) {

		// Inside two step turn
		if (cell.HasRiverThroughEdge (direction.Next (cell)) &&
		    cell.HasRiverThroughEdge (direction.Previous (cell))) {
			if (cell.Neighbors.Count == 4) {
				center += VoronoiMetrics.GetSolidEdgeMiddle (cell, direction) * 
				          (0.25f * VoronoiMetrics.InnerToOuter (cell, direction));
			} else {
				center += VoronoiMetrics.GetSolidEdgeMiddle (cell, direction) *
				          (0.5f * VoronoiMetrics.InnerToOuter (cell, direction));
			}
			
			// 6 sided adjacencies
		} else if (cell.Neighbors.Count == 6) {
			if (cell.HasRiverThroughEdge (direction.Next (cell)) &&
			    cell.HasRiverThroughEdge (direction.Previous2 (cell))) {
				center += VoronoiMetrics.GetFirstSolidCorner (cell, direction) * 0.25f;
				
			} else if (cell.HasRiverThroughEdge (direction.Previous (cell)) &&
			           cell.HasRiverThroughEdge (direction.Next2 (cell))) {
				center += VoronoiMetrics.GetSecondSolidCorner (cell, direction) * 0.25f;
			}
			
			// 7 sided adjacencies
		} else if (cell.Neighbors.Count == 7) {
			if (cell.HasRiverThroughEdge (direction.Next (cell)) &&
			    cell.HasRiverThroughEdge (direction.Previous2 (cell))) {
				center += VoronoiMetrics.GetFirstSolidCorner (cell, direction) * 0.25f;
				
			} else if (cell.HasRiverThroughEdge (direction.Previous (cell)) &&
			           cell.HasRiverThroughEdge (direction.Next2 (cell))) {
				center += VoronoiMetrics.GetSecondSolidCorner (cell, direction) * 0.25f;
				
			} else if (cell.HasRiverThroughEdge (direction.Next2 (cell)) &&
			           cell.HasRiverThroughEdge (direction.Previous2 (cell))) {
				center += VoronoiMetrics.GetSolidEdgeMiddle (cell, direction) * 0.25f;
				
			} else if (cell.HasRiverThroughEdge (direction.Next (cell)) &&
			          cell.HasRiverThroughEdge (direction.Previous3 (cell))) {
				center += VoronoiMetrics.GetSolidEdgeMiddle (cell, direction.Previous (cell)) * 0.25f;
				
			} else if (cell.HasRiverThroughEdge (direction.Next3 (cell)) &&
			          cell.HasRiverThroughEdge (direction.Previous (cell))) {
				center += VoronoiMetrics.GetSolidEdgeMiddle (cell, direction.Next (cell)) * 0.25f;
				
			}
		} else if (cell.Neighbors.Count == 8) {
			if (cell.HasRiverThroughEdge (direction.Next (cell)) &&
			    cell.HasRiverThroughEdge (direction.Previous3 (cell))) {
				center += VoronoiMetrics.GetSolidEdgeMiddle (cell, direction.Previous (cell)) * 0.25f;
				
			} else if (cell.HasRiverThroughEdge (direction.Next2 (cell)) &&
			           cell.HasRiverThroughEdge (direction.Previous2 (cell))) {
				center += VoronoiMetrics.GetSolidEdgeMiddle (cell, direction) * 0.25f;
				
			} else if (cell.HasRiverThroughEdge (direction.Next3 (cell)) &&
			           cell.HasRiverThroughEdge (direction.Previous (cell))) {
				center += VoronoiMetrics.GetSolidEdgeMiddle (cell, direction.Next (cell)) * 0.25f;
				
			} else if (cell.HasRiverThroughEdge (direction.Previous (cell)) &&
			           cell.HasRiverThroughEdge (direction.Next4 (cell))) {
				center += VoronoiMetrics.GetSecondSolidCorner (cell, direction.Next (cell)) * 0.25f;
				
			} else if (cell.HasRiverThroughEdge (direction.Previous2 (cell)) &&
				           cell.HasRiverThroughEdge (direction.Next3 (cell))) {
				center += VoronoiMetrics.GetSecondSolidCorner (cell, direction) * 0.25f;
				
			} else if (cell.HasRiverThroughEdge (direction.Previous3 (cell)) &&
			           cell.HasRiverThroughEdge (direction.Next2 (cell))) {
				center += VoronoiMetrics.GetFirstSolidCorner (cell, direction) * 0.25f;
				
			} else if (cell.HasRiverThroughEdge (direction.Previous4 (cell)) &&
			          cell.HasRiverThroughEdge (direction.Next (cell))) {
				center += VoronoiMetrics.GetFirstSolidCorner (cell, direction.Previous (cell)) * 0.25f;
				
			} else if (cell.HasRiverThroughEdge (direction.Previous (cell)) &&
			           cell.HasRiverThroughEdge (direction.Next2 (cell))) {
				center += VoronoiMetrics.GetSecondSolidCorner (cell, direction) * 0.25f;
				
			} else if (cell.HasRiverThroughEdge (direction.Previous2 (cell)) &&
			          cell.HasRiverThroughEdge (direction.Next (cell))) {
				center += VoronoiMetrics.GetFirstSolidCorner (cell, direction) * 0.25f;
				
			}
		}
		
		EdgeVertices m = new EdgeVertices (
			Vector3.Lerp (center, e.V1, 0.5f),
			Vector3.Lerp (center, e.V5, 0.5f)
		);
		
		TriangulateEdgeStrip (m, cell.Color, e, cell.Color);
		TriangulateEdgeFan (center, m, cell.Color);
	}

	private void TriangulateWithRiverBeginOrEnd (VoronoiCell cell, VoronoiDirection direction, Vector3 center, EdgeVertices e) {
		EdgeVertices m = new EdgeVertices (
			Vector3.Lerp (center, e.V1, 0.5f),
			Vector3.Lerp (center, e.V5, 0.5f)
		);
		
		m.V3 = Vector3.ClampMagnitude (m.V3, cell.StreamBedElevation);

		TriangulateEdgeStrip (m, cell.Color, e, cell.Color);
		TriangulateEdgeFan (center, m, cell.Color);

		bool reversed = cell.HasIncomingRiver;
		TriangulateRiverQuad (m.V2, m.V4, e.V2, e.V4, cell.RiverSurfaceElevation, 0.6f, reversed);

		center = Vector3.ClampMagnitude (center, cell.RiverSurfaceElevation);
		m.V2 = Vector3.ClampMagnitude (m.V2, cell.RiverSurfaceElevation);
		m.V4 = Vector3.ClampMagnitude (m.V4, cell.RiverSurfaceElevation);
		Rivers.AddTriangle (center, m.V2, m.V4);
		
		if (reversed) {
			Rivers.AddTriangleUV (new Vector2 (0.5f, 0.4f), new Vector2 (1f, 0.2f), new Vector2 (0f, 0.2f));
		} else {
			Rivers.AddTriangleUV (new Vector2 (0.5f, 0.4f), new Vector2 (0f, 0.6f), new Vector2 (1f, 0.6f));
		}
	}

	private void TriangulateWithRiver (VoronoiCell cell, VoronoiDirection direction, Vector3 center, EdgeVertices e) {
		Vector3 centerL, centerR;
		centerL = centerR = center;

		// 4 sided straight
		if (cell.HasRiverThroughEdge (direction.Next2 (cell)) && cell.HasRiverThroughEdge (direction.Previous2 (cell))) {
			centerL = center + VoronoiMetrics.GetSolidEdgeMiddle (cell, direction.Previous (cell)) * 
			          (0.25f * VoronoiMetrics.InnerToOuter (cell, direction.Previous (cell)));
			centerR = center + VoronoiMetrics.GetSolidEdgeMiddle (cell, direction.Next (cell)) * 
			          (0.25f * VoronoiMetrics.InnerToOuter (cell, direction.Next (cell)));
			
			// 1 step turn
		} else if (cell.HasRiverThroughEdge (direction.Next (cell))) {
			centerL = center;
			centerR = Vector3.Lerp (center, e.V5, 2 / 3f);
			
			// 1 step turn
		} else if (cell.HasRiverThroughEdge (direction.Previous (cell))) {
			centerL = Vector3.Lerp (center, e.V1, 2 / 3f);
			centerR = center;
			
			// 2 step turn
		} else if (cell.HasRiverThroughEdge (direction.Next2 (cell))) {
			centerL = center;
			centerR = center + VoronoiMetrics.GetSolidEdgeMiddle (cell, direction.Next (cell)) * 
			          (0.5f * VoronoiMetrics.InnerToOuter (cell, direction.Next (cell)));
			
			// 2 step turn
		} else if (cell.HasRiverThroughEdge (direction.Previous2 (cell))) {
			centerL = center + VoronoiMetrics.GetSolidEdgeMiddle (cell, direction.Previous (cell)) * 
			          (0.5f * VoronoiMetrics.InnerToOuter (cell, direction.Previous (cell)));
			centerR = center;

			// 6 sided straight
		} else if (cell.HasRiverThroughEdge (direction.Next3 (cell)) &&
		           cell.HasRiverThroughEdge (direction.Previous3 (cell))) {
			centerL = center + VoronoiMetrics.GetFirstSolidCorner (cell, direction.Previous (cell)) * 0.25f;
			centerR = center + VoronoiMetrics.GetSecondSolidCorner (cell, direction.Next (cell)) * 0.25f;

			// 7 sided straight
		} else if (cell.HasRiverThroughEdge (direction.Previous3 (cell)) &&
		           cell.HasRiverThroughEdge (direction.Next4 (cell)) &&
		           cell.Neighbors.Count > 4) {
			centerL = center + VoronoiMetrics.GetFirstSolidCorner (cell, direction.Previous (cell)) * 0.25f;
			centerR = center + VoronoiMetrics.GetSolidEdgeMiddle (cell, direction.Next2 (cell)) * 0.25f;
			
			// 7 sided straight
		} else if (cell.HasRiverThroughEdge (direction.Previous4 (cell)) &&
		           cell.HasRiverThroughEdge (direction.Next3 (cell)) &&
		           cell.Neighbors.Count > 4) {
			centerL = center + VoronoiMetrics.GetSolidEdgeMiddle (cell, direction.Previous2 (cell)) * 0.25f;
			centerR = center + VoronoiMetrics.GetSecondSolidCorner (cell, direction.Next (cell)) * 0.25f;
			
			// 8 sided straight
		} else if (cell.HasRiverThroughEdge (direction.Previous4 (cell)) &&
		           cell.HasRiverThroughEdge (direction.Next4 (cell)) &&
		           cell.Neighbors.Count > 4) {
			centerL = center + VoronoiMetrics.GetSolidEdgeMiddle (cell, direction.Previous2 (cell)) * 0.25f;
			centerR = center + VoronoiMetrics.GetSolidEdgeMiddle (cell, direction.Next2 (cell)) * 0.25f;

			// 3 step turn
		} else if (cell.HasRiverThroughEdge (direction.Previous3 (cell)) && 
		           cell.HasRiverThroughEdge (direction.Next5 (cell)) &&
		           cell.Neighbors.Count > 5) {
			centerR = center + VoronoiMetrics.GetSecondSolidCorner (cell, direction.Next2 (cell)) * 0.25f;
			centerL = center + VoronoiMetrics.GetFirstSolidCorner (cell, direction.Previous (cell)) * 0.25f;

			// 3 step turn
		} else if (cell.HasRiverThroughEdge (direction.Previous5 (cell)) && 
		           cell.HasRiverThroughEdge (direction.Next3 (cell)) &&
		           cell.Neighbors.Count > 5) {
			centerL = center + VoronoiMetrics.GetFirstSolidCorner (cell, direction.Previous2 (cell)) * 0.25f;
			centerR = center + VoronoiMetrics.GetSecondSolidCorner (cell, direction.Next (cell)) * 0.25f;
		}

		center = Vector3.Lerp (centerL, centerR, 0.5f);
		

		EdgeVertices m = new EdgeVertices (
			Vector3.Lerp (centerL, e.V1, 0.5f),
			Vector3.Lerp (centerR, e.V5, 0.5f),
			1 / 6f
		);

		m.V3 = Vector3.ClampMagnitude (m.V3, cell.StreamBedElevation);
		center = Vector3.ClampMagnitude (center, cell.StreamBedElevation);
		
		TriangulateEdgeStrip (m, cell.Color, e, cell.Color);
		Terrain.AddTriangle (centerL, m.V1, m.V2);
		Terrain.AddTriangleColor (cell.Color);
		Terrain.AddQuad (centerL, center, m.V2, m.V3);
		Terrain.AddQuadColor (cell.Color);
		Terrain.AddQuad (center, centerR, m.V3, m.V4);
		Terrain.AddQuadColor (cell.Color);
		Terrain.AddTriangle (centerR, m.V4, m.V5);
		Terrain.AddTriangleColor (cell.Color);

		bool reversed = cell.IncomingRiver == direction;
		TriangulateRiverQuad (centerL, centerR, m.V2, m.V4, cell.RiverSurfaceElevation, 0.4f, reversed);
		TriangulateRiverQuad (m.V2, m.V4, e.V2, e.V4, cell.RiverSurfaceElevation, 0.6f, reversed);
	}

	private void TriangulateEdgeFan (Vector3 center, EdgeVertices edge, Color color) {
		Terrain.AddTriangle (center, edge.V1, edge.V2);
		Terrain.AddTriangleColor (color);
		Terrain.AddTriangle (center, edge.V2, edge.V3);
		Terrain.AddTriangleColor (color);
		Terrain.AddTriangle (center, edge.V3, edge.V4);
		Terrain.AddTriangleColor (color);
		Terrain.AddTriangle (center, edge.V4, edge.V5);
		Terrain.AddTriangleColor (color);
	}

	private void TriangulateEdgeStrip (EdgeVertices e1, Color c1, EdgeVertices e2, Color c2) {
		Terrain.AddQuad (e1.V1, e1.V2, e2.V1, e2.V2);
		Terrain.AddQuadColor (c1, c2);
		Terrain.AddQuad (e1.V2, e1.V3, e2.V2, e2.V3);
		Terrain.AddQuadColor (c1, c2);
		Terrain.AddQuad (e1.V3, e1.V4, e2.V3, e2.V4);
		Terrain.AddQuadColor (c1, c2);
		Terrain.AddQuad (e1.V4, e1.V5, e2.V4, e2.V5);
		Terrain.AddQuadColor (c1, c2);
	}

	private void TriangulateConnection (VoronoiCell cell, VoronoiDirection direction, EdgeVertices e1) {
		VoronoiCell neighbor = cell.GetNeighbor (direction);
		
		Vector3 bridge = VoronoiMetrics.GetBridge (cell, direction);
		
		EdgeVertices e2 = new EdgeVertices(
			e1.V1 + bridge,
			e1.V5 + bridge
		);

		if (cell.HasRiverThroughEdge (direction)) {
			e2.V3 = Vector3.ClampMagnitude (e2.V3, neighbor.StreamBedElevation);
			TriangulateRiverQuad (e1.V2, e1.V4, e2.V2, e2.V4, 
				cell.RiverSurfaceElevation, neighbor.RiverSurfaceElevation, 0.8f,
				cell.HasIncomingRiver && cell.IncomingRiver == direction
			);
		}

		if (cell.GetEdgeType (direction) == VoronoiEdgeType.Slope) {
			TriangulateEdgeTerraces (e1, cell, e2, neighbor);
		} else {
			TriangulateEdgeStrip (e1, cell.Color, e2, neighbor.Color);
		}
		
		VoronoiCell nextNeighbor = cell.GetNeighbor (direction.Next (cell));
		if (cell.CornerConnections.Contains (direction)) {
			Vector3 v5 = e1.V5 + VoronoiMetrics.GetBridge (cell, direction.Next (cell));

			if (cell.Elevation <= neighbor.Elevation) {
				if (cell.Elevation <= nextNeighbor.Elevation) {
					TriangulateCorner (e1.V5, cell, e2.V5, neighbor, v5, nextNeighbor);
				} else {
					TriangulateCorner (v5, nextNeighbor, e1.V5, cell, e2.V5, neighbor);
				}
			} else if (neighbor.Elevation <= nextNeighbor.Elevation) {
				TriangulateCorner (e2.V5, neighbor, v5, nextNeighbor, e1.V5, cell);
			} else {
				TriangulateCorner (v5, nextNeighbor, e1.V5, cell, e2.V5, neighbor);
			}
		}
	}

	private void TriangulateEdgeTerraces (
		EdgeVertices begin, VoronoiCell beginCell, 
		EdgeVertices end, VoronoiCell endCell) {

		EdgeVertices e2 = EdgeVertices.TerraceLerp (begin, end, 1);
		Color c2 = VoronoiMetrics.TerraceLerp (beginCell.Color, endCell.Color, 1);

		TriangulateEdgeStrip (begin, beginCell.Color, e2, c2);

		for (int i = 2; i < VoronoiMetrics.TerraceSteps; ++i) {
			EdgeVertices e1 = e2;
			Color c1 = c2;
			e2 = EdgeVertices.TerraceLerp (begin, end, i);
			c2 = VoronoiMetrics.TerraceLerp (beginCell.Color, endCell.Color, i);
			TriangulateEdgeStrip (e1, c1, e2, c2);
		}

		TriangulateEdgeStrip (e2, c2, end, endCell.Color);
	} 

	private void TriangulateCorner (
		Vector3 bottom, VoronoiCell bottomCell,
		Vector3 left, VoronoiCell leftCell,
		Vector3 right, VoronoiCell rightCell) {

		VoronoiEdgeType leftEdgeType = bottomCell.GetEdgeType (leftCell);
		VoronoiEdgeType rightEdgeType = bottomCell.GetEdgeType (rightCell);

		if (leftEdgeType == VoronoiEdgeType.Slope) {
			if (rightEdgeType == VoronoiEdgeType.Slope) {
				TriangulateCornerTerraces (bottom, bottomCell, left, leftCell, right, rightCell);
			} else if (rightEdgeType == VoronoiEdgeType.Flat) {
				TriangulateCornerTerraces (left, leftCell, right, rightCell, bottom, bottomCell);
			} else {
				TriangulateCornerTerracesCliff (bottom, bottomCell, left, leftCell, right, rightCell);
			}
		} else if (rightEdgeType == VoronoiEdgeType.Slope) {
			if (leftEdgeType == VoronoiEdgeType.Flat) {
				TriangulateCornerTerraces (right, rightCell, bottom, bottomCell, left, leftCell);
			} else {
				TriangulateCornerCliffTerraces (bottom, bottomCell, left, leftCell, right, rightCell);
			}
		} else if (leftCell.GetEdgeType (rightCell) == VoronoiEdgeType.Slope) {
			if (leftCell.Elevation < rightCell.Elevation) {
				TriangulateCornerCliffTerraces (right, rightCell, bottom, bottomCell, left, leftCell);
			} else {
				TriangulateCornerTerracesCliff (left, leftCell, right, rightCell, bottom, bottomCell);
			}
		} else {
			Terrain.AddTriangle (bottom, left, right);
			Terrain.AddTriangleColor (bottomCell.Color, leftCell.Color, rightCell.Color);
		}
	}

	private void TriangulateCornerTerraces (
		Vector3 begin, VoronoiCell beginCell, 
		Vector3 left, VoronoiCell leftCell, 
		Vector3 right, VoronoiCell rightCell) {

		Vector3 v3 = VoronoiMetrics.TerraceLerp (begin, left, 1);
		Vector3 v4 = VoronoiMetrics.TerraceLerp (begin, right, 1);
		Color c3 = VoronoiMetrics.TerraceLerp (beginCell.Color, leftCell.Color, 1);
		Color c4 = VoronoiMetrics.TerraceLerp (beginCell.Color, rightCell.Color, 1);

		Terrain.AddTriangle (begin, v3, v4);
		Terrain.AddTriangleColor (beginCell.Color, c3, c4);

		for (int i = 2; i < VoronoiMetrics.TerraceSteps; ++i) {
			Vector3 v1 = v3;
			Vector3 v2 = v4;
			Color c1 = c3;
			Color c2 = c4;
			v3 = VoronoiMetrics.TerraceLerp (begin, left, i);
			v4 = VoronoiMetrics.TerraceLerp (begin, right, i);
			c3 = VoronoiMetrics.TerraceLerp (beginCell.Color, leftCell.Color, i);
			c4 = VoronoiMetrics.TerraceLerp (beginCell.Color, rightCell.Color, i);
			Terrain.AddQuad(v1, v2, v3, v4);
			Terrain.AddQuadColor(c1, c2, c3, c4);
		}
		
		Terrain.AddQuad (v3, v4, left, right);
		Terrain.AddQuadColor (c3, c4, leftCell.Color, rightCell.Color);
	}

	private void TriangulateCornerTerracesCliff (
		Vector3 begin, VoronoiCell beginCell,
		Vector3 left, VoronoiCell leftCell,
		Vector3 right, VoronoiCell rightCell) {

		float b = 1f / (rightCell.Elevation - beginCell.Elevation);
		b = b < 0 ? -b : b;
		Vector3 boundary = Vector3.Lerp (VoronoiMetrics.Perturb (begin), VoronoiMetrics.Perturb (right), b);
		Color boundaryColor = Color.Lerp (beginCell.Color, rightCell.Color, b);

		TriangulateBoundaryTriangle (begin, beginCell, left, leftCell, boundary, boundaryColor);

		if (leftCell.GetEdgeType (rightCell) == VoronoiEdgeType.Slope) {
			TriangulateBoundaryTriangle (left, leftCell, right, rightCell, boundary, boundaryColor);
		} else {
			Terrain.AddTriangleUnperturbed (VoronoiMetrics.Perturb (left), VoronoiMetrics.Perturb (right), boundary);
			Terrain.AddTriangleColor (leftCell.Color, rightCell.Color, boundaryColor);
		}
	}
	
	private void TriangulateCornerCliffTerraces (
		Vector3 begin, VoronoiCell beginCell,
		Vector3 left, VoronoiCell leftCell,
		Vector3 right, VoronoiCell rightCell) {

		float b = 1f / (leftCell.Elevation - beginCell.Elevation);
		b = b < 0 ? -b : b;
		Vector3 boundary = Vector3.Lerp (VoronoiMetrics.Perturb (begin), VoronoiMetrics.Perturb (left), b);
		Color boundaryColor = Color.Lerp (beginCell.Color, leftCell.Color, b);

		TriangulateBoundaryTriangle (right, rightCell, begin, beginCell, boundary, boundaryColor);

		if (leftCell.GetEdgeType (rightCell) == VoronoiEdgeType.Slope) {
			TriangulateBoundaryTriangle (left, leftCell, right, rightCell, boundary, boundaryColor);
		} else {
			Terrain.AddTriangleUnperturbed (VoronoiMetrics.Perturb (left), VoronoiMetrics.Perturb (right), boundary);
			Terrain.AddTriangleColor (leftCell.Color, rightCell.Color, boundaryColor);
		}
	}

	private void TriangulateBoundaryTriangle (
		Vector3 begin, VoronoiCell beginCell,
		Vector3 left, VoronoiCell leftCell,
		Vector3 boundary, Color boundaryColor) {
		
		Vector3 v2 = VoronoiMetrics.Perturb(VoronoiMetrics.TerraceLerp (begin, left, 1));
		Color c2 = VoronoiMetrics.TerraceLerp (beginCell.Color, leftCell.Color, 1);
		
		Terrain.AddTriangleUnperturbed (VoronoiMetrics.Perturb (begin), v2, boundary);
		Terrain.AddTriangleColor (beginCell.Color, c2, boundaryColor);
		
		for (int i = 2; i < VoronoiMetrics.TerraceSteps; ++i) {
			Vector3 v1 = v2;
			Color c1 = c2;
			v2 = VoronoiMetrics.Perturb(VoronoiMetrics.TerraceLerp (begin, left, i));
			c2 = VoronoiMetrics.TerraceLerp (beginCell.Color, leftCell.Color, i);
			Terrain.AddTriangleUnperturbed (v1, v2, boundary);
			Terrain.AddTriangleColor (c1, c2, boundaryColor);
		}

		Terrain.AddTriangleUnperturbed (v2, VoronoiMetrics.Perturb (left), boundary);
		Terrain.AddTriangleColor (c2, leftCell.Color, boundaryColor);
	}

	private void TriangulateRiverQuad (Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, float elevation, float v, bool reversed) {
		TriangulateRiverQuad (v1, v2, v3, v4, elevation, elevation, v, reversed);
	}
	
	private void TriangulateRiverQuad (Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, float elevation1, float elevation2, float v, bool reversed) {
		v1 = Vector3.ClampMagnitude (v1, elevation1);
		v2 = Vector3.ClampMagnitude (v2, elevation1);
		v3 = Vector3.ClampMagnitude (v3, elevation2);
		v4 = Vector3.ClampMagnitude (v4, elevation2);
		Rivers.AddQuad (v1, v2, v3, v4);

		if (reversed) {
			Rivers.AddQuadUV (1f, 0f, 0.8f - v, 0.6f - v);
		} else {
			Rivers.AddQuadUV (0f, 1f, v, v + 0.2f);
		}
	}
}
