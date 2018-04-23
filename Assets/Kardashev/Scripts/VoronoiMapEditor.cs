using UnityEngine;
using UnityEngine.EventSystems;

public class VoronoiMapEditor : MonoBehaviour {

	public Color[] Colors;
	public VoronoiGrid VoronoiGrid;

	private Color _activeColor;
	private int _actiiveElevation;

	void Awake () {
		SelectColor (0);
	}

	private void Update () {
		if (Input.GetMouseButton (0) && !EventSystem.current.IsPointerOverGameObject()) {
			HandleInput ();
		}
	}

	private void HandleInput () {
		Ray inputRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast (inputRay, out hit)) {
			EditCell (VoronoiGrid.GetCell (hit.point));
		}
	}

	private void EditCell (VoronoiCell cell) {
		cell.Color = _activeColor;
		cell.Elevation = _actiiveElevation;
		VoronoiGrid.Refresh ();
	}

	public void SelectColor (int index) {
		_activeColor = Colors[index];
	}

	public void SetElevation (float elevation) {
		_actiiveElevation = (int)elevation;
	}

}
