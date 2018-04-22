using UnityEngine;
using UnityEngine.EventSystems;

public class VoronoiMapEditor : MonoBehaviour {

	public Color[] Colors;
	public VoronoiGrid VoronoiGrid;

	private Color _activeColor;

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
			VoronoiGrid.ColorCell (hit.point, _activeColor);
		}
	}

	public void SelectColor (int index) {
		_activeColor = Colors[index];
	}

}
