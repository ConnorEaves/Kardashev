using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class VoronoiDirection {

	private int _direction;

	public static implicit operator VoronoiDirection (int i) {
		return new VoronoiDirection {_direction = i};
	}

	public static implicit operator int (VoronoiDirection d) {
		return d._direction;
	}

	// Required to avoid reference comparisons in lists
	public override bool Equals (object obj) {
		VoronoiDirection item = (VoronoiDirection) obj;
		return item != null && _direction.Equals (item._direction);
	}

	public override int GetHashCode () {
		return 0;
	}
	
	public static bool operator == (VoronoiDirection a, VoronoiDirection b) {
		if (ReferenceEquals (a, null)) {
			return ReferenceEquals (b, null);
		}
		return a.Equals (b);
	}

	public static bool operator != (VoronoiDirection a, VoronoiDirection b) {
		return !(a == b);
	}
}

public static class VoronoiDirectionExtension {
	/// <summary>
	/// Get the direction to this cell from neighbor at direction.
	/// </summary>
	/// <param name="direction"></param>
	/// <param name="cell"></param>
	/// <returns></returns>
	public static VoronoiDirection Opposite (this VoronoiDirection direction, VoronoiCell cell) {
		return cell.GetNeighbor (direction).Neighbors.IndexOf (cell);
	}
	
	public static VoronoiDirection Previous (this VoronoiDirection direction, VoronoiCell cell) {
		return (VoronoiDirection) Mathf.Repeat (direction - 1, cell.Neighbors.Count);
	}
	
	public static VoronoiDirection Previous2 (this VoronoiDirection direction, VoronoiCell cell) {
		return (VoronoiDirection) Mathf.Repeat (direction - 2, cell.Neighbors.Count);
	}
	
	public static VoronoiDirection Previous3 (this VoronoiDirection direction, VoronoiCell cell) {
		return (VoronoiDirection) Mathf.Repeat (direction - 3, cell.Neighbors.Count);
	}
	
	public static VoronoiDirection Previous4 (this VoronoiDirection direction, VoronoiCell cell) {
		return (VoronoiDirection) Mathf.Repeat (direction - 4, cell.Neighbors.Count);
	}
	
	public static VoronoiDirection Previous5 (this VoronoiDirection direction, VoronoiCell cell) {
		return (VoronoiDirection) Mathf.Repeat (direction - 5, cell.Neighbors.Count);
	}

	public static VoronoiDirection Next (this VoronoiDirection direction, VoronoiCell cell) {
		return (VoronoiDirection) Mathf.Repeat (direction + 1, cell.Neighbors.Count);
	}
	
	public static VoronoiDirection Next2 (this VoronoiDirection direction, VoronoiCell cell) {
		return (VoronoiDirection) Mathf.Repeat (direction + 2, cell.Neighbors.Count);
	}
	
	public static VoronoiDirection Next3 (this VoronoiDirection direction, VoronoiCell cell) {
		return (VoronoiDirection) Mathf.Repeat (direction + 3, cell.Neighbors.Count);
	}
	
	public static VoronoiDirection Next4 (this VoronoiDirection direction, VoronoiCell cell) {
		return (VoronoiDirection) Mathf.Repeat (direction + 4, cell.Neighbors.Count);
	}
	
	public static VoronoiDirection Next5 (this VoronoiDirection direction, VoronoiCell cell) {
		return (VoronoiDirection) Mathf.Repeat (direction + 5, cell.Neighbors.Count);
	}
}
