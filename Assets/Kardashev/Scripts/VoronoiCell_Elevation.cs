using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class VoronoiCell {

	public float BaseElevation;
	
	private int _elevation = int.MinValue;
	public int Elevation {
		get { return _elevation; }
		set {
			if (_elevation == value) {
				return;
			}
			_elevation = value;
			Vector3 position = transform.localPosition;
			position = position.normalized * (BaseElevation + value * VoronoiMetrics.ElevationStep +
			                                  (VoronoiMetrics.SampleNoise (position).y * 2f - 1f) *
			                                  VoronoiMetrics.ElevationPerturbSrength);
			transform.localPosition = position;
			
			// Prevent impossible uphill rivers
			if (_hasOutgoingRiver && _elevation < GetNeighbor (_outgoingRiver)._elevation) {
				RemoveOutgoingRiver ();
			}
			if (_hasIncomingRiver && _elevation > GetNeighbor (_incomingRiver)._elevation) {
				RemoveIncomingRiver ();
			}

			for (int i = 0; i < Roads.Count; ++i) {
				if (Roads[i] && GetElevationDifference (i) > 1) {
					SetRoad (i, false);
				}
			}
			
			Refresh ();
		}
	}
	
	public VoronoiEdgeType GetEdgeType (VoronoiDirection direction) {
		return VoronoiMetrics.GetEdgeType (_elevation, Neighbors[direction]._elevation);
	}

	public VoronoiEdgeType GetEdgeType (VoronoiCell otherCell) {
		return VoronoiMetrics.GetEdgeType (_elevation, otherCell._elevation);
	}

	public int GetElevationDifference (VoronoiDirection direction) {
		int difference = _elevation - GetNeighbor (direction)._elevation;
		return difference >= 0 ? difference : -difference;
	}
}
