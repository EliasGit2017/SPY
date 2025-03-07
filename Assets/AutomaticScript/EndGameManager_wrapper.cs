using UnityEngine;
using FYFY;

public class EndGameManager_wrapper : BaseWrapper
{
	public UnityEngine.GameObject playButtonAmount;
	public UnityEngine.GameObject endPanel;
	private void Start()
	{
		this.hideFlags = HideFlags.NotEditable;
		MainLoop.initAppropriateSystemField (system, "playButtonAmount", playButtonAmount);
		MainLoop.initAppropriateSystemField (system, "endPanel", endPanel);
	}

	public void cancelEnd()
	{
		MainLoop.callAppropriateSystemMethod (system, "cancelEnd", null);
	}

	public void LevelCompleteStatement()
	{
		MainLoop.callAppropriateSystemMethod (system, "LevelCompleteStatement", null);
	}

	public void AtTelePorteStatementt(System.String time)
	{
		MainLoop.callAppropriateSystemMethod (system, "AtTelePorteStatementt", time);
	}

}
