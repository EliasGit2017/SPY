using UnityEngine;
using FYFY;

public class CreateDialogsSystem_wrapper : BaseWrapper
{
	private void Start()
	{
		this.hideFlags = HideFlags.NotEditable;
	}

	public void AddDialog(TMPro.TMP_InputField inputDialog)
	{
		MainLoop.callAppropriateSystemMethod (system, "AddDialog", inputDialog);
	}

}
