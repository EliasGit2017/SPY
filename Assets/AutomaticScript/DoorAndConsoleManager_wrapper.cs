using UnityEngine;
using FYFY;

public class DoorAndConsoleManager_wrapper : BaseWrapper
{
	public UnityEngine.GameObject doorPathPrefab;
	public UnityEngine.Color pathOn;
	public UnityEngine.Color pathOff;
	private void Start()
	{
		this.hideFlags = HideFlags.NotEditable;
		MainLoop.initAppropriateSystemField (system, "doorPathPrefab", doorPathPrefab);
		MainLoop.initAppropriateSystemField (system, "pathOn", pathOn);
		MainLoop.initAppropriateSystemField (system, "pathOff", pathOff);
	}

	public void DoorOpenedStatement(System.String ObjectName)
	{
		MainLoop.callAppropriateSystemMethod (system, "DoorOpenedStatement", ObjectName);
	}

}
