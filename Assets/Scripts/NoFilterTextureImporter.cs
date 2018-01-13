using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// 강제로 텍스쳐를 Point모드로 바꿔주는 프로세서
public class NoFilterTextureImporter : AssetPostprocessor {

	void OnPostprocessTexture(Texture2D texture)
	{
		TextureImporter importer = assetImporter as TextureImporter;
		importer.anisoLevel = 0;
		importer.filterMode = FilterMode.Point;

		Object asset = AssetDatabase.LoadAssetAtPath(importer.assetPath, typeof(Texture2D));
		if (asset)
		{
			EditorUtility.SetDirty(asset);
		}
		else
		{
			texture.anisoLevel = 0;
			texture.filterMode = FilterMode.Trilinear;          
		} 
	}
}
