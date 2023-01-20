using UnityEngine;
using FYFY;

public class ObjectPropertiesSystem_wrapper : BaseWrapper
{
	public UnityEngine.GameObject editionPanel;
	private void Start()
	{
		this.hideFlags = HideFlags.NotEditable;
		MainLoop.initAppropriateSystemField (system, "editionPanel", editionPanel);
	}

	public void changeDirection(System.Int32 dir)
	{
		MainLoop.callAppropriateSystemMethod (system, "changeDirection", dir);
	}

}
