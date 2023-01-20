using System;
using UnityEngine;
using FYFY;

public class EditorModeSelectionSystem : FSystem {
	private EditorData editorData;

	// Use to init system before the first onProcess call
	protected override void onStart(){
		editorData = GameObject.Find("EditorData").GetComponent<EditorData>();
	}

	
	public void selectMode(int mode)
	{
		editorData.editorMode = (EditorData.Mode)mode;
	}
}