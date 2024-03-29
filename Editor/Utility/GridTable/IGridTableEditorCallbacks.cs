using UnityEngine;

namespace Zenvin.EditorUtil.GridTable {
	public interface IGridTableEditorCallbacks {
		int ColumnCount { get; }
		int RowCount { get; }

		bool HasCellError (Vector2Int cell);

		void OnDrawColumnHeader (int columnIndex, Rect position);
		void OnDrawRowHeader (int rowIndex, Rect position);

		void OnDrawCell (Vector2Int cell, Rect position);
	}

	public interface IGridTableEditorCallbacksExtended : IGridTableEditorCallbacks {
		void OnDrawCorner (Rect position);
	}
}