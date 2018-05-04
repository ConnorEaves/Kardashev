using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MIConvexHull;
using UnityEngine;

public class VoronoiGrid : MonoBehaviour {

	// Seed Variables
	public int Seed;
	public bool UseRandomSeed;
	
	// Grid Variables
	public float Radius = 1f;
	public float CellSize = 1f;
	public int CellCount;
	public bool OverrideCellCount;
	
	// Relaxation Variables
	public ComputeShader RelaxationShader;
	public int RelaxationSteps = 100;
	
	// Color
	public Color DefaultColor;
	
	// Noise Soruce
	public Texture2D NoiseSource;

	// Prefabs
	public VoronoiCell CellPrefab;
	public VoronoiGridChunk ChunkPrefab;

	// Cells lists
	private VoronoiCell[] _cells;
	private VoronoiGridChunk[] _chunks;
	
	// Atmosphere
	private SgtAtmosphere _atmosphere;
	
	// For play mode recompiles
	private void OnEnable () {
		VoronoiMetrics.NoiseSource = NoiseSource;
	}

	private void Awake () {
		VoronoiMetrics.NoiseSource = NoiseSource;
		
		_atmosphere = GetComponentInChildren<SgtAtmosphere> ();
	}

	private void Start () {
		StartCoroutine (Generate ());
	}

	public VoronoiCell GetCell (Vector3 position) {
		position -= transform.position;
		return _cells.OrderByDescending (x => Vector3.Dot (x.transform.localPosition.normalized, position.normalized))
					 .First ();
	}

	/// <summary>
	/// Generate a new Voronoi Grid.
	/// </summary>
	/// <returns></returns>
	public IEnumerator Generate () {
		// Ensure everything has been initialized
		yield return new WaitForEndOfFrame ();
		
		float startTime = Time.time;
		
		Random.InitState (Seed);

		yield return DestroyChildChunks ();
		yield return CreateCells ();
		yield return ConnectCells ();
		yield return FinalizeCells ();
		yield return CreateChunks ();
		
		_atmosphere.InnerMeshRadius = Radius;
		_atmosphere.Height = VoronoiMetrics.ElevationStep * (10f + 1f);
		_atmosphere.UpdateOuters ();
		
		Debug.Log ("Finished generating in " + (Time.time - startTime) + " seconds.");
	}

	/// <summary>
	/// Destroy any existing child cells so that new cells can be generated.
	/// </summary>
	private IEnumerator DestroyChildCells () {
		for (int i = transform.childCount - 1; i >= 0; --i) {
			Transform child = transform.GetChild (i);
			if (child != null && child.GetComponent<VoronoiCell> () != null) {
				Destroy (child.gameObject);
			}
		}
		yield return null;
	}
	
	/// <summary>
	/// Destroy any existing child chuks so that new chunks can be generated.
	/// </summary>
	private IEnumerator DestroyChildChunks () {
		for (int i = transform.childCount - 1; i >= 0; --i) {
			Transform child = transform.GetChild (i);
			if (child != null && child.GetComponent<VoronoiGridChunk> () != null) {
				Destroy (child.gameObject);
			}
		}
		yield return null;
	}

	/// <summary>
	/// Create new cells.
	/// </summary>
	/// <returns></returns>
	private IEnumerator CreateCells () {
		_cells = new VoronoiCell[CellCount];
		
		Vector3[] points = new Vector3[CellCount];
		yield return GeneratePoints (points);
		
		for (int i = 0; i < CellCount; ++i) {
			CreateCell (i, points[i]);
		}
		yield return null;
	}

	/// <summary>
	/// Use compute shader to apply iterations of Lloyd's relaxation to points.
	/// </summary>
	/// <param name="points"></param>
	/// <returns></returns>
	private IEnumerator GeneratePoints (Vector3[] points) {
		// Init with white noise
		for (int i = 0; i < points.Length; ++i) {
			points[i] = Random.onUnitSphere * Radius;
		}

		// Create structures for passing and dispatching
		int kernel = RelaxationShader.FindKernel ("Relaxation");
		ComputeBuffer pointsBuffer = new ComputeBuffer (points.Length, 12);
		ComputeBuffer relaxedPointsBuffer = new ComputeBuffer (points.Length, 12);
		
		// Assign shader variables
		RelaxationShader.SetInt ("cellCount", points.Length);
		RelaxationShader.SetFloat ("radius", Radius);
		RelaxationShader.SetBuffer (kernel, "points", pointsBuffer);
		RelaxationShader.SetBuffer (kernel, "relaxedPoints", relaxedPointsBuffer);
		
		// Iterate shader
		for (int i = 0; i < RelaxationSteps; ++i) {
			pointsBuffer.SetData (points);
			relaxedPointsBuffer.SetData (points);

			RelaxationShader.Dispatch (kernel, points.Length, 1, 1);

			relaxedPointsBuffer.GetData (points);
			yield return null;
		}
		
		// Cleanup
		pointsBuffer.Dispose ();
		relaxedPointsBuffer.Dispose ();
		yield return null;
	}
	
	/// <summary>
	/// Initialize cell parameters.
	/// </summary>
	/// <param name="i"></param>
	/// <param name="position"></param>
	private void CreateCell (int i, Vector3 position) {
		VoronoiCell cell = _cells[i] = Instantiate (CellPrefab);
		cell.name = "Voronoi Cell " + i;
		cell.transform.SetParent (transform, false);
		cell.transform.localPosition = position;
		cell.transform.rotation = Quaternion.FromToRotation (Vector3.up, cell.transform.localPosition);
		cell.Color = DefaultColor;
	}

	/// <summary>
	/// Assign cell corners, neighbors, edge connections, and corner connections
	/// </summary>
	/// <returns></returns>
	private IEnumerator ConnectCells () {
		ConvexHull<VoronoiCell, DefaultConvexFace<VoronoiCell>> hull =
			ConvexHull.Create<VoronoiCell, DefaultConvexFace<VoronoiCell>> (_cells);
		
		foreach (DefaultConvexFace<VoronoiCell> face in hull.Faces) {
			VoronoiCell c0 = face.Vertices[0];
			VoronoiCell c1 = face.Vertices[1];
			VoronoiCell c2 = face.Vertices[2];

			Vector3 centroid = ((c0.transform.position + 
			                     c1.transform.position + 
			                     c2.transform.position) / 3f).normalized * Radius;
			
			c0.Corners.Add (centroid - c0.transform.position);
			c1.Corners.Add (centroid - c1.transform.position);
			c2.Corners.Add (centroid - c2.transform.position);

			c0.SetNeighbor (c1);
			c0.SetNeighbor (c2);
			c1.SetNeighbor (c2);
		}
		yield return null;
	}


	/// <summary>
	/// Sorts and aligns neighbors and corners.
	/// Flattens cell centers and adjusts corner vectors accordingly.
	/// </summary>
	/// <returns></returns>
	private IEnumerator FinalizeCells () {
		List<VoronoiCell> processed = new List<VoronoiCell> ();
		foreach (VoronoiCell cell in _cells) {
			// Sort Corners
			cell.Corners = cell.Corners
				.OrderByDescending (x => (cell.transform.localPosition + x).ClockwiseAngle (cell.transform.localPosition))
				.ToList ();
			
			// Sort Neighbors
			cell.Neighbors = cell.Neighbors
				.OrderByDescending (x => x.transform.position.ClockwiseAngle (cell.transform.position))
				.ToList ();
			
			// Allign Corners and Neighbors
			if ((cell.transform.localPosition + cell.Corners[0]).ClockwiseAngle (cell.transform.localPosition) <
				 cell.Neighbors[0].transform.localPosition.ClockwiseAngle (cell.transform.localPosition)) {
				cell.Neighbors.Rotate (1);
			}

			for (VoronoiDirection d = 0; d < cell.Neighbors.Count; ++d) {
				if (!processed.Contains (cell.GetNeighbor (d))) {
					cell.EdgeConnections.Add (d);
					
					if (!processed.Contains (cell.GetNeighbor (d.Next (cell)))) {
						cell.CornerConnections.Add (d);
					}
				}
			}

			processed.Add (cell);

			// Flatten cell
			Vector3 originalPosition = cell.transform.localPosition;
			Vector3 center = Vector3.zero;
			foreach (Vector3 c in cell.Corners) {
				center += cell.transform.localPosition + c;
			}
			cell.transform.localPosition = center / cell.Corners.Count;
		
			// Correct corners and corner connections
			for (int i = 0; i < cell.Corners.Count; ++i) {
				cell.Corners[i] = cell.Corners[i] + (originalPosition - cell.transform.localPosition);
			}
			cell.Corners.Add (cell.Corners[0]);

			cell.BaseElevation = Radius;
			//cell.BaseElevation = cell.transform.localPosition.magnitude;
			cell.Elevation = 0;

			cell.Roads = new bool[cell.Neighbors.Count].ToList ();

			switch (cell.Neighbors.Count) {
				case 3:
					cell.Color = Color.yellow;
					break;
				case 4:
					cell.Color = Color.blue;
					break;
				case 5:
					cell.Color = Color.green;
					break;
				case 7:
					cell.Color = Color.red;
					break;
				case 8:
					cell.Color = Color.magenta;
					break;
				case 9:
					cell.Color = Color.black;
					break;
				default:
					cell.Color = Color.white;
					break;
			}
		}

		yield return null;
	}

	
	private IEnumerator CreateChunks () {
		int chunkCount = Mathf.CeilToInt(_cells.Length / VoronoiMetrics.ChunkSize);
		_chunks = new VoronoiGridChunk[chunkCount];

		Vector3[] points = new Vector3[chunkCount];
		yield return GeneratePoints (points);

		for (int i = 0; i < points.Length; ++i) {
			CreateChunk (i, points[i]);
		}

		bool chunksFinished = false;
		while (!chunksFinished) {
			chunksFinished = true;
			for (int i = 0; i < chunkCount; ++i) {
				if (_chunks[i].ExtendChunk ()) {
					chunksFinished = false;
				}
			}
		}

		yield return null;
	}

	private void CreateChunk (int i, Vector3 position) {
		VoronoiGridChunk chunk = _chunks[i] = Instantiate (ChunkPrefab);
		chunk.name = "Voronoi Grid Chunk " + i;
		chunk.transform.SetParent (transform, false);
		chunk.AddCell (GetCell (position));
		chunk.AddToAtmosphere (_atmosphere);
	}
}
