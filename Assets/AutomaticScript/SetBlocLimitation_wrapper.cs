using UnityEngine;
using FYFY;

public class SetBlocLimitation_wrapper : BaseWrapper
{
	private void Start()
	{
		this.hideFlags = HideFlags.NotEditable;
	}

	public void updateBlocLimit(TMPro.TMP_InputField inputLimit)
	{
		MainLoop.callAppropriateSystemMethod (system, "updateBlocLimit", inputLimit);
	}

}
