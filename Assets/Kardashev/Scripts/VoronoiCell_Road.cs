using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class VoronoiCell {

	[SerializeField]
	public List<bool> Roads;

	public bool HasRoadThroughEdge (VoronoiDirection direction) {
		return Roads[direction];
	}

	public bool HasRoads {
		get { return Roads.Any (road => road); }
	}

	private void SetRoad (VoronoiDirection direction, bool state) {
		Roads[direction] = state;
		Neighbors[direction].Roads[direction.Opposite (this)] = state;
		Neighbors[direction].RefreshSelfOnly ();
		RefreshSelfOnly ();
	}

	public void RemoveRoads () {
		for (VoronoiDirection direction = 0; direction < Roads.Count; ++direction) {
			if (Roads[direction]) {
				SetRoad (direction, false);
			}
		}
	}
	
	public void AddRoad (VoronoiDirection direction) {
		if (!Roads[direction] && !HasRiverThroughEdge (direction) && GetElevationDifference (direction) <= 1) {
			SetRoad (direction, true);
		}
	}
	
}
