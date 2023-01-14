using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Zenvin.EditorUtil {
	public static class Utility {

		private static readonly char[] Separators = new char[] { '/' };

		public static SerializedProperty FindSiblingProperty (this SerializedProperty prop, string name) {
			if (string.IsNullOrEmpty (name)) {
				return null;
			}

			string[] pathParts = prop.propertyPath.Split (Separators, System.StringSplitOptions.RemoveEmptyEntries);
			string path = "";
			for (int i = 0; i < pathParts.Length - 1; i++) {
				path = path + pathParts[i];
			}

			pathParts = name.Split (Separators, System.StringSplitOptions.RemoveEmptyEntries);

			path = path + "/" + pathParts[0];
			path = path.Trim ('/');

			SerializedProperty current = prop.serializedObject.FindProperty (path);
			if (current == null) {
				return null;
			}

			for (int i = 1; i < pathParts.Length; i++) {
				string _name = pathParts[i];

				if (current.propertyType == SerializedPropertyType.ObjectReference) {
					if (current.objectReferenceValue == null) {
						return null;
					}
					SerializedObject obj = new SerializedObject (current.objectReferenceValue);
					current = obj.FindProperty (pathParts[i]);
				} else {
					current = current.FindPropertyRelative (_name);
				}

				if (current == null) {
					return null;
				}
			}

			return current;
		}

		public static T[] FindAll<T> () where T : ScriptableObject {
			string[] guids = AssetDatabase.FindAssets ($"t:{typeof (T).FullName}");
			T[] objects = new T[guids.Length];
			for (int i = 0; i < guids.Length; i++) {
				string path = AssetDatabase.GUIDToAssetPath (guids[i]);
				objects[i] = AssetDatabase.LoadAssetAtPath<T> (path);
			}
			return objects;
		}

		public static Object[] FindAll (Type type) {
			string[] guids = AssetDatabase.FindAssets ($"t:{type.FullName}");
			Object[] objects = new Object[guids.Length];
			for (int i = 0; i < guids.Length; i++) {
				string path = AssetDatabase.GUIDToAssetPath (guids[i]);
				objects[i] = AssetDatabase.LoadAssetAtPath (path, type);
			}
			return objects;
		}

		public static bool CompareValue (this SerializedProperty prop, object value) {
			if (prop != null) {
				switch (prop.propertyType) {
					case SerializedPropertyType.Boolean:
						return prop.boolValue.Equals (value);
					case SerializedPropertyType.Float:
						return prop.floatValue.Equals (value);
					case SerializedPropertyType.String:
						return prop.stringValue.Equals (value);
					case SerializedPropertyType.Integer:
						return prop.intValue.Equals (value);
					case SerializedPropertyType.Enum:
						return prop.enumValueIndex.Equals (value);
					case SerializedPropertyType.Character:
						return prop.stringValue.Length > 0 && prop.stringValue[0].Equals (value);
				}
			}
			return false;
		}

		public static bool CompareValue (this SerializedProperty a, SerializedProperty b) {
			if (a == b) {
				return true;
			}
			if (a == null || b == null) {
				return false;
			}
			if (a.propertyType != b.propertyType) {
				return false;
			}
			switch (a.propertyType) {
				case SerializedPropertyType.ArraySize:
				case SerializedPropertyType.LayerMask:
				case SerializedPropertyType.Integer:
					return a.intValue == b.intValue;
				case SerializedPropertyType.Boolean:
					return a.boolValue == b.boolValue;
				case SerializedPropertyType.Float:
					return a.floatValue == b.floatValue;
				case SerializedPropertyType.String:
					return a.stringValue == b.stringValue;
				case SerializedPropertyType.Color:
					return a.colorValue == b.colorValue;
				case SerializedPropertyType.ObjectReference:
					return a.objectReferenceValue == b.objectReferenceValue;
				case SerializedPropertyType.Enum:
					return a.enumValueIndex == b.enumValueIndex;
				case SerializedPropertyType.Vector2:
					return a.vector2Value == b.vector2Value;
				case SerializedPropertyType.Vector3:
					return a.vector3Value == b.vector3Value;
				case SerializedPropertyType.Vector4:
					return a.vector4Value == b.vector4Value;
				case SerializedPropertyType.Rect:
					return a.rectValue == b.rectValue;
				case SerializedPropertyType.Character:
					return a.stringValue == b.stringValue;
				case SerializedPropertyType.Bounds:
					return a.boundsValue == b.boundsValue;
				case SerializedPropertyType.Quaternion:
					return a.quaternionValue == b.quaternionValue;
				case SerializedPropertyType.ExposedReference:
					return a.exposedReferenceValue == b.exposedReferenceValue;
				case SerializedPropertyType.FixedBufferSize:
					return a.fixedBufferSize == b.fixedBufferSize;
				case SerializedPropertyType.Vector2Int:
					return a.vector2IntValue == b.vector2IntValue;
				case SerializedPropertyType.Vector3Int:
					return a.vector3IntValue == b.vector3IntValue;
				case SerializedPropertyType.BoundsInt:
					return a.boundsIntValue == b.boundsIntValue;
			}
			return false;
		}

	}
}