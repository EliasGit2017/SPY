using UnityEngine;
using FYFY;

public class EditorModeSelectionSystem_wrapper : BaseWrapper
{
	public GameData prefabGameData;
	private void Start()
	{
		this.hideFlags = HideFlags.NotEditable;
		MainLoop.initAppropriateSystemField (system, "prefabGameData", prefabGameData);
	}

	public void selectMode(System.Int32 mode)
	{
		MainLoop.callAppropriateSystemMethod (system, "selectMode", mode);
	}

}
