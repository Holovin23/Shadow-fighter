using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityToolbarExtender;

[Serializable]
internal class ToolbarClearPrefs : BaseToolbarElement
{
	private static GUIContent clearPlayerPrefsBtn;

	private string _saveFolderPath;
	public override string NameInList => "[Button] Clear saves";

	public override void Init()
	{
		clearPlayerPrefsBtn = EditorGUIUtility.IconContent("SaveFromPlay");
		clearPlayerPrefsBtn.tooltip = "Clear saves";

		_saveFolderPath = Application.persistentDataPath;
	}

	protected override void OnDrawInList(Rect position)
	{

	}

	protected override void OnDrawInToolbar()
	{
		if (GUILayout.Button(clearPlayerPrefsBtn, ToolbarStyles.commandButtonStyle))
		{
			var saveDir = new DirectoryInfo(_saveFolderPath);
			
			foreach (var file in saveDir.GetFiles()) 
				file.Delete();
			
			PlayerPrefs.DeleteAll();
			Debug.Log("Saves cleared");
		}
	}
}