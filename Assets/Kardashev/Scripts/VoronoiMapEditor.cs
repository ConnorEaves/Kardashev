using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VoronoiMapEditor : MonoBehaviour {

	private enum OptionalToggle {
		Ignore, Yes, No
	}

	public Color[] Colors;
	public VoronoiGrid VoronoiGrid;

	private bool _isDrag;
	private VoronoiDirection _dragDirection;
	private VoronoiCell _previousCell;

	private bool _applyColor;
	private Color _activeColor;
	
	private bool _applyElevation;
	private int _actiiveElevation;

	private int _brushSize;

	private OptionalToggle _riverMode;
	
	void Awake () {
		SelectColor (-1);
	}

	private void Update () {
		if (Input.GetMouseButton (0) && !EventSystem.current.IsPointerOverGameObject()) {
			HandleInput ();
		} else {
			_previousCell = null;
		}
		
		Ray inputRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast (inputRay, out hit)) {
			HighlightCell (VoronoiGrid.GetCell (hit.point));
		}
	}

	private void HandleInput () {
		Ray inputRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast (inputRay, out hit)) {
			VoronoiCell currentCell = VoronoiGrid.GetCell (hit.point);

			if (_previousCell && _previousCell != currentCell) {
				ValidateDrag (currentCell);
			} else {
				_isDrag = false;
			}
			
			EditCells (currentCell);
			_previousCell = currentCell;
		} else {
			_previousCell = null;
		}
	}

	private void ValidateDrag (VoronoiCell currentCell) {
		for (_dragDirection = 0; _dragDirection < currentCell.Neighbors.Count; ++_dragDirection) {
			if (_previousCell.GetNeighbor (_dragDirection) == currentCell) {
				_isDrag = true;
				return;
			}
		}
		_isDrag = false;
	}

	private void EditCells (VoronoiCell center) {
		Queue<VoronoiCell> cellsQueue = new Queue<VoronoiCell> ();
		List<VoronoiCell> cellsList = new List<VoronoiCell> ();
		cellsQueue.Enqueue (center);
		cellsList.Add (center);
        
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
			foreach (VoronoiCell v in neighborsToAdd) {
				cellsQueue.Enqueue (v);
				cellsList.Add (v);
			}
		}

		foreach (VoronoiCell cell in cellsList) {
			EditCell (cell);
		}
	}

	private void EditCell (VoronoiCell cell) {
		if (_applyColor) {
			cell.Color = _activeColor;
		}

		if (_applyElevation) {
			cell.Elevation = _actiiveElevation;
		}

		if (_riverMode == OptionalToggle.No) {
			cell.RemoveRiver ();
		}

		if (_isDrag) {
			if (_riverMode == OptionalToggle.Yes) {
				_previousCell.SetOutgoingRiver (_dragDirection);
			}
		}
	}
	
	private void HighlightCell (VoronoiCell cell) {
		for (int i = 0; i < cell.Corners.Count - 1; ++i) {
			Debug.DrawLine (cell.transform.position + cell.Corners[i], cell.transform.position + cell.Corners[i + 1], Color.black);
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

	public void SetRiverMode (int mode) {
		_riverMode = (OptionalToggle) mode;
	}
}
