using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VoronoiMapEditor : MonoBehaviour {

	public Color[] Colors;
	public VoronoiGrid VoronoiGrid;

	private bool _applyColor;
	private Color _activeColor;
	
	private bool _applyElevation;
	private int _actiiveElevation;

	private int _brushSize;
	
	void Awake () {
		SelectColor (-1);
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
			EditCells (VoronoiGrid.GetCell (hit.point));
		}
	}
	
	private void EditCells (VoronoiCell center) {
		Queue<VoronoiCell> cellsQueue = new Queue<VoronoiCell> ();
		List<VoronoiCell> cellsList = new List<VoronoiCell> ();
		cellsQueue.Enqueue (center);
		EditCell (center);
        
		for (int i = 0; i < _brushSize; ++i) {
			List<VoronoiCell> neighborsToAdd = new List<VoronoiCell> ();
			while (cellsQueue.Count > 0) {
				VoronoiCell cell = cellsQueue.Peek ();
				for (int n = 0; n < cell.Neighbors.Count; ++n) {
					if (!cellsList.Contains (cell.GetNeighbor (n)) && !neighborsToAdd.Contains (cell.GetNeighbor (n))) {
						neighborsToAdd.Add (cell.GetNeighbor (n));
					}
				}
				cellsQueue.Dequeue ();
			}
			foreach (VoronoiCell cell in neighborsToAdd) {
				cellsQueue.Enqueue (cell);
				EditCell (cell);
			}
		}
	}

	private void EditCell (VoronoiCell cell) {
		if (_applyColor) {
			cell.Color = _activeColor;
		}

		if (_applyElevation) {
			cell.Elevation = _actiiveElevation;
		}
	}

	public void SelectColor (int index) {
		_applyColor = index >= 0;
		if (_applyColor) {
			_activeColor = Colors[index];
		}
	}

	public void SetApplyElevation (bool toggle) {
		_applyElevation = toggle;
	}
	
	public void SetElevation (float elevation) {
		_actiiveElevation = (int)elevation;
	}
	
	public void SetBrushSize (float size) {
		_brushSize = (int) size;
	}
}
