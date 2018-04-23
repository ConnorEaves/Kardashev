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
		return direction == 0 ? cell.Neighbors.Count - 1 : direction - 1;
	}

	public static VoronoiDirection Next (this VoronoiDirection direction, VoronoiCell cell) {
		return direction == cell.Neighbors.Count - 1 ? 0 : direction + 1;
	}
}
