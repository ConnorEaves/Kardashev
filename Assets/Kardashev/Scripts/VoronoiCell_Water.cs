using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class VoronoiCell {

	private int _waterLevel;
	public int WaterLevel {
		get { return _waterLevel; }
		set {
			if (_waterLevel == value) {
				return;
			}
			_waterLevel = value;
			Refresh ();
		}
	}

	public bool IsUnderwater {
		get { return _waterLevel > _elevation; }
	}

	public float WaterSurfaceElevation {
		get {
			return BaseElevation + (_waterLevel + VoronoiMetrics.WaterSurfaceElevationOffset) * VoronoiMetrics.ElevationStep;
		}
	}
}
