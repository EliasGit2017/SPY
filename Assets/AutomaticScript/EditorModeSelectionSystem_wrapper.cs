using UnityEngine;
using FYFY;

public class EditorModeSelectionSystem_wrapper : BaseWrapper
{
	private void Start()
	{
		this.hideFlags = HideFlags.NotEditable;
	}

	public void selectMode(System.Int32 mode)
	{
		MainLoop.callAppropriateSystemMethod (system, "selectMode", mode);
	}

}
