using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class VoronoiCell {

	private Color _color;
	public Color Color {
		get { return _color; }
		set {
			if (_color == value) {
				return;
			}
			_color = value;
			Refresh ();
		}
	}
}
