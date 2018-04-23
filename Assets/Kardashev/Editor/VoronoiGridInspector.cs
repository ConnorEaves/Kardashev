using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (VoronoiGrid))]
public class VoronoiGridInspector : Editor {

	public override void OnInspectorGUI () {
		VoronoiGrid grid = (VoronoiGrid)target;

		#region Seed
		
		EditorGUI.BeginChangeCheck ();
			grid.UseRandomSeed = EditorGUILayout.Toggle ("Random Seed", grid.UseRandomSeed);
		
			EditorGUI.BeginDisabledGroup (grid.UseRandomSeed);
				grid.Seed = EditorGUILayout.IntField ("Seed", grid.Seed);
			EditorGUI.EndDisabledGroup ();

		if (EditorGUI.EndChangeCheck ()) {
			if (grid.Seed < 0) {
				grid.Seed = 0;
			}
		}

		#endregion

		#region Grid Properties

		EditorGUI.BeginChangeCheck ();
			grid.Radius = EditorGUILayout.FloatField ("Sphere Radius", grid.Radius);
			
			EditorGUI.BeginDisabledGroup (grid.OverrideCellCount);
				grid.CellSize = EditorGUILayout.FloatField ("Cell Size", grid.CellSize);
			EditorGUI.EndDisabledGroup ();
	
			grid.OverrideCellCount = EditorGUILayout.Toggle ("Override Cell Count", grid.OverrideCellCount);
		
			EditorGUI.BeginDisabledGroup (!grid.OverrideCellCount);
				grid.CellCount = EditorGUILayout.IntField ("Cell Count", grid.CellCount);
			EditorGUI.EndDisabledGroup ();
		
		if (EditorGUI.EndChangeCheck ()) {
			if (grid.Radius < Mathf.Epsilon) {
				grid.Radius = Mathf.Epsilon;
			}
			if (grid.CellSize < Mathf.Epsilon) {
				grid.CellSize = Mathf.Epsilon;
			}
			if (!grid.OverrideCellCount) {
				grid.CellCount = Mathf.RoundToInt (4 * Mathf.PI * grid.Radius * grid.Radius / grid.CellSize);
			} else if (grid.CellCount < 4) {
				grid.CellCount = 4;
			}
		}

		#endregion

		#region Relaxation

		EditorGUI.BeginChangeCheck ();
		grid.RelaxationShader =
			(ComputeShader) EditorGUILayout.ObjectField ("Relaxation Shader", grid.RelaxationShader, typeof (ComputeShader), true);
		
			grid.RelaxationSteps = EditorGUILayout.IntField ("Relaxation Steps", grid.RelaxationSteps);

		if (EditorGUI.EndChangeCheck ()) {
			if (grid.RelaxationSteps < 0) {
				grid.RelaxationSteps = 0;
			}
		}

		#endregion

		#region Color

		grid.DefaultColor = EditorGUILayout.ColorField ("Default Color", grid.DefaultColor);

		#endregion

		#region Noise

		grid.NoiseSource = (Texture2D)EditorGUILayout.ObjectField("Noise Source", grid.NoiseSource, typeof(Texture2D), true);

		#endregion
		
		#region Cell Prefab

		grid.CellPrefab =
			(VoronoiCell) EditorGUILayout.ObjectField ("Cell Prefab", grid.CellPrefab, typeof (VoronoiCell), true);
		
		#endregion

		#region Generate

		if (GUILayout.Button ("Generate Grid")) {
			if (EditorApplication.isPlaying) {
				if (grid.UseRandomSeed) {
					grid.Seed = Random.Range (0, int.MaxValue);
				}
				grid.StartCoroutine (grid.Generate ());
			} else {
				Debug.LogError ("Cannot generate grid outside of play mode.");
			}
		}

		#endregion
		
	}
	
}
