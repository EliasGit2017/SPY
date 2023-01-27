using UnityEngine;
using FYFY;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;

public class CreateDialogsSystem : FSystem {

	public static CreateDialogsSystem instance;

	private Family f_dialogs = FamilyManager.getFamily(new AllOfComponents(typeof(Dialogs)));

	public CreateDialogsSystem()
	{
		instance = this;
	}
	
	// Use to init system before the first onProcess call
	protected override void onStart()
	{

	}

	public void AddDialog(TMP_InputField inputDialog)
    {
		foreach (GameObject goDialogs in f_dialogs)
        {
			goDialogs.GetComponent<Dialogs>().dialog.Add(inputDialog.text);
		}
		inputDialog.text = "";
    }

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) 
	{
	}
}