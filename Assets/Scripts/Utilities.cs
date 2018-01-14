using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public static class Utilities {

	public static T FindObjectOfType<T>() where T : Object
	{
		var objects = Resources.FindObjectsOfTypeAll<T>();
		foreach (var obj in objects)
		{
			if (obj.hideFlags == HideFlags.NotEditable || obj.hideFlags == HideFlags.HideAndDontSave)
			{
				Debug.Log("noteditable");
				continue;
			}

#if UNITY_EDITOR
            if (obj is GameObject && PrefabUtility.GetPrefabType(obj as GameObject) == PrefabType.Prefab)
                continue;

            if (obj is MonoBehaviour && PrefabUtility.GetPrefabType((obj as MonoBehaviour).gameObject) == PrefabType.Prefab)
                continue;
#endif

			return obj;
		}

		return null;
	}

	public static T[] FindObjectsOfType<T>() where T : Object
	{
		var ret = new List<T>();
		var objects = Resources.FindObjectsOfTypeAll<T>();
		foreach (var obj in objects)
		{
			if (obj.hideFlags == HideFlags.NotEditable || obj.hideFlags == HideFlags.HideAndDontSave)
				continue;

#if UNITY_EDITOR
            if (obj is GameObject && PrefabUtility.GetPrefabType(obj as GameObject) == PrefabType.Prefab)
                continue;

            if (obj is MonoBehaviour && PrefabUtility.GetPrefabType((obj as MonoBehaviour).gameObject) == PrefabType.Prefab)
                continue;
#endif
			ret.Add(obj);
		}

		return ret.ToArray();
	}
}
