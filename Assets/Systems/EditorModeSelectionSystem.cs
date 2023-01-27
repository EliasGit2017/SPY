using System;
using UnityEngine;
using FYFY;

public class EditorModeSelectionSystem : FSystem {
	private EditorData editorData;
	public GameData prefabGameData;

	// Use to init system before the first onProcess call
	protected override void onStart(){
		editorData = GameObject.Find("EditorData").GetComponent<EditorData>();
		if (!GameObject.Find("GameData"))
		{
			GameData gameData = UnityEngine.Object.Instantiate(prefabGameData);
			gameData.name = "GameData";
			GameObjectManager.dontDestroyOnLoadAndRebind(gameData.gameObject);
		}
	}

	
	public void selectMode(int mode)
	{
		editorData.editorMode = (EditorData.Mode)mode;
	}
}