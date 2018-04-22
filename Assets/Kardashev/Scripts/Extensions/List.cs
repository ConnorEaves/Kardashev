using System.Collections.Generic;

public static class ListExtensions {

	public static void Rotate<T> (this IList<T> list, int places) {
		// Complete circle...
		if (places % list.Count == 0) {
			return;
		}

		T[] copy = new T[list.Count];
		list.CopyTo (copy, 0);

		for (int i = 0; i < list.Count; ++i) {
			int index = (i + places) % list.Count;
			list[i] = copy[index];
		}
	}
}
