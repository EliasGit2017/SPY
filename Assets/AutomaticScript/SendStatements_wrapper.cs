using UnityEngine;
using FYFY;

public class SendStatements_wrapper : BaseWrapper
{
	private void Start()
	{
		this.hideFlags = HideFlags.NotEditable;
	}

	public void initGBLXAPI()
	{
		MainLoop.callAppropriateSystemMethod (system, "initGBLXAPI", null);
	}

	public void exampleStatement()
	{
		MainLoop.callAppropriateSystemMethod (system, "exampleStatement", null);
	}

	public void LevelCompletedStatement()
	{
		MainLoop.callAppropriateSystemMethod (system, "LevelCompletedStatement", null);
	}

	public void AtTelePorteStatement(System.String time)
	{
		MainLoop.callAppropriateSystemMethod (system, "AtTelePorteStatement", time);
	}

}
