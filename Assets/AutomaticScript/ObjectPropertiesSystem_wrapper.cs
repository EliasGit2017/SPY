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

	public void saveActivable(System.String str)
	{
		MainLoop.callAppropriateSystemMethod (system, "saveActivable", str);
	}

	public void saveActivationSlot(System.String str)
	{
		MainLoop.callAppropriateSystemMethod (system, "saveActivationSlot", str);
	}

	public void saveAgentName(System.String str)
	{
		MainLoop.callAppropriateSystemMethod (system, "saveAgentName", str);
	}

}
