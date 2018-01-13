using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities {

	public static T FindObjectOfType<T>() where T : Object
	{
		var objects = Resources.FindObjectsOfTypeAll<T>();
		Debug.Log("test " + objects.Length.ToString());
		foreach (var obj in objects)
		{
			if (obj.hideFlags == HideFlags.NotEditable || obj.hideFlags == HideFlags.HideAndDontSave)
			{
				Debug.Log("noteditable");
				continue;
			}

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

			ret.Add(obj);
		}

		return ret.ToArray();
	}
}
