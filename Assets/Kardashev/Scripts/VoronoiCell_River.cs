using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class VoronoiCell {

	private bool _hasIncomingRiver, _hasOutgoingRiver;
	private VoronoiDirection _incomingRiver, _outgoingRiver;

	public bool HasIncomingRiver {
		get { return _hasIncomingRiver; }
	}
	
	public bool HasOutgoingRiver {
		get { return _hasOutgoingRiver; }
	}

	public VoronoiDirection IncomingRiver {
		get { return _incomingRiver; }
	}

	public VoronoiDirection OutgoingRiver {
		get { return _outgoingRiver; }
	}

	public bool HasRiver {
		get { return _hasIncomingRiver || _hasOutgoingRiver; }
	}

	public bool HasRiverBeginOrEnd {
		get { return _hasIncomingRiver != _hasOutgoingRiver; }
	}

	public bool HasRiverThroughEdge (VoronoiDirection direction) {
		return _hasIncomingRiver && _incomingRiver == direction ||
		       _hasOutgoingRiver && _outgoingRiver == direction;
	}

	public float StreamBedElevation {
		get { return BaseElevation + (_elevation + VoronoiMetrics.StreamBedElevationOffset) * VoronoiMetrics.ElevationStep; }
	}

	public float RiverSurfaceElevation {
		get { return BaseElevation + (_elevation + VoronoiMetrics.RiverSurfaceElevationOffset) * VoronoiMetrics.ElevationStep; }
	}

	public void SetOutgoingRiver (VoronoiDirection direction) {
		if (_hasOutgoingRiver && _outgoingRiver == direction) {
			return;
		}

		VoronoiCell neighbor = GetNeighbor (direction);
		if (!neighbor || _elevation < neighbor._elevation) {
			return;
		}
		
		RemoveOutgoingRiver ();
		if (_hasIncomingRiver && _incomingRiver == direction) {
			RemoveIncomingRiver ();
		}

		_hasOutgoingRiver = true;
		_outgoingRiver = direction;
		RefreshSelfOnly ();
		
		neighbor.RemoveIncomingRiver ();
		neighbor._hasIncomingRiver = true;
		neighbor._incomingRiver = direction.Opposite (this);
		neighbor.RefreshSelfOnly ();
	}
	
	public void RemoveRiver () {
		RemoveOutgoingRiver ();
		RemoveIncomingRiver ();
	}

	public void RemoveOutgoingRiver () {
		if (!_hasOutgoingRiver) {
			return;
		}
		_hasOutgoingRiver = false;
		RefreshSelfOnly ();

		VoronoiCell neighbor = GetNeighbor (_outgoingRiver);
		neighbor._hasIncomingRiver = false;
		neighbor.RefreshSelfOnly ();
	}

	public void RemoveIncomingRiver () {
		if (!_hasIncomingRiver) {
			return;
		}
		_hasIncomingRiver = false;
		RefreshSelfOnly ();

		VoronoiCell neighbor = GetNeighbor (_incomingRiver);
		neighbor._hasOutgoingRiver = false;
		neighbor.RefreshSelfOnly ();
	}
}
