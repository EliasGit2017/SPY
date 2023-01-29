using UnityEngine;
using FYFY;

public class GenerateLevelEditor_wrapper : BaseWrapper
{
	private void Start()
	{
		this.hideFlags = HideFlags.NotEditable;
	}

	public void LevelToXml()
	{
		MainLoop.callAppropriateSystemMethod (system, "LevelToXml", null);
	}

}
