using UnityEngine;
using FYFY;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System;


public class SetBlocLimitation : FSystem {

	public static SetBlocLimitation instance;

	private EditorData editorData;
	private Family f_draggableBlocElement = FamilyManager.getFamily(new AllOfComponents(typeof(ElementToDrag)),new AnyOfTags("Bloc"));

	public SetBlocLimitation()
	{
		instance = this;
	}
	
	// Use to init system before the first onProcess call
	protected override void onStart()
	{
		editorData = GameObject.Find("EditorData").GetComponent<EditorData>();
		editorData.actionBlockLimit = new Dictionary<string, int>();

		// init the actionBlockLimit
		foreach (GameObject draggableGO in f_draggableBlocElement)
		{
			editorData.actionBlockLimit.Add(draggableGO.name, 0);
		}
	}

	public void updateBlocLimit(TMP_InputField inputLimit)
    {
		GameObject draggableGO = inputLimit.transform.parent.gameObject;
		Debug.Log("Name :" + draggableGO.name);
		if (editorData.actionBlockLimit.ContainsKey(draggableGO.name))
        {

			editorData.actionBlockLimit[draggableGO.name] = Int32.Parse(inputLimit.text);

		}

	}

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) 
	{

	}
}