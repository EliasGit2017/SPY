using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using FYFY;
using TMPro;

public class ObjectPropertiesSystem : FSystem {

	private EditorData editorData;
	public GameObject editionPanel;
	
	private GameObject currentGameObject;

	// Use to init system before the first onProcess call
	protected override void onStart(){
		editorData = GameObject.Find("EditorData").GetComponent<EditorData>();
		currentGameObject = editorData.propertiesBlock;
	}

	private string list2string(List<int> l)
	{
		string str = "";
		
		if (l.Count == 0)
			return "";
		
		for (int i = 0; i < l.Count - 1; i++)
		{
			str += i + ", ";
		}

		str += l[l.Count - 1].ToString();
		return str;
	}

	private List<int> string2list(string str)
	{
		List<string> str_l = str.Split(',').ToList();
		return str_l.Select(int.Parse).ToList();
	}

	public void changeDirection(int dir)
	{
		List<Direction> go = editorData.propertiesBlock.GetComponentsInChildren<Direction>().ToList();
		Direction goDir = editorData.propertiesBlock.GetComponent<Direction>();
		if (goDir != null)
			go.Add(goDir);
		go[0].direction = (Direction.Dir)dir;
		if (dir == 0)
			editorData.propertiesBlock.transform.rotation = Quaternion.Euler(0, -90, 0);
		else if (dir == 1)
		{
			editorData.propertiesBlock.transform.rotation = Quaternion.Euler(0, 90, 0);
		}
		else if (dir == 2)
		{
			editorData.propertiesBlock.transform.rotation = Quaternion.Euler(0, 0, 0);
		}
		else
		{
			editorData.propertiesBlock.transform.rotation = Quaternion.Euler(0, 180, 0);
		}
	}

	public void saveActivable(string str)
	{
		List<Activable> act = editorData.propertiesBlock.GetComponentsInChildren<Activable>().ToList();
		Activable goAct = editorData.propertiesBlock.GetComponent<Activable>();
		if (goAct != null)
			act.Add(goAct);
		act[0].slotID = string2list(str);
	}

	public void saveActivationSlot(string str)
	{
		List<ActivationSlot> actSlot = editorData.propertiesBlock.GetComponentsInChildren<ActivationSlot>().ToList();
		ActivationSlot goActSlot = editorData.propertiesBlock.GetComponent<ActivationSlot>();
		if (goActSlot != null)
			actSlot.Add(goActSlot);
		actSlot[0].slotID = Int32.Parse(str);
	}
	
	
	protected override void onProcess(int familiesUpdateCount)
	{
		if (currentGameObject != editorData.propertiesBlock)
		{
			string name = editorData.propertiesBlock.name;
			name = name.Replace("(Clone)", "");
			
			editionPanel.transform.GetChild(0).GetComponent<TMP_Text>().text = name;
			
			List<Direction> dir = editorData.propertiesBlock.GetComponentsInChildren<Direction>().ToList();
			Direction goDir = editorData.propertiesBlock.GetComponent<Direction>();
			if (goDir != null)
				dir.Add(goDir);
			if (dir.Count == 0)
			{
				editionPanel.transform.GetChild(1).gameObject.SetActive(false);
			}
			else
			{
				editionPanel.transform.GetChild(1).gameObject.SetActive(true);
				GameObject.Find("DirectionDropDown").GetComponent<TMP_Dropdown>().value = (int)dir[0].direction;
			}

			List<Activable> act = editorData.propertiesBlock.GetComponentsInChildren<Activable>().ToList();
			Activable goAct = editorData.propertiesBlock.GetComponent<Activable>();
			if (goAct != null)
				act.Add(goAct);
			if (act.Count == 0)
			{
				editionPanel.transform.GetChild(2).gameObject.SetActive(false);
			}
			else
			{
				editionPanel.transform.GetChild(2).gameObject.SetActive(true);
				GameObject.Find("IdInputField").GetComponent<TMP_InputField>().text = list2string(act[0].slotID);
			}
			
			List<ActivationSlot> actSlot = editorData.propertiesBlock.GetComponentsInChildren<ActivationSlot>().ToList();
			ActivationSlot goActSlot = editorData.propertiesBlock.GetComponent<ActivationSlot>();
			if (goActSlot != null)
				actSlot.Add(goActSlot);
			foreach (var d in actSlot)
			{
				Debug.Log(d);
			}
			if (actSlot.Count == 0)
			{
				editionPanel.transform.GetChild(3).gameObject.SetActive(false);
			}
			else
			{
				editionPanel.transform.GetChild(3).gameObject.SetActive(true);
				GameObject.Find("SlotInputField").GetComponent<TMP_InputField>().text = actSlot[0].slotID.ToString();
			}
			
			currentGameObject = editorData.propertiesBlock;
		}
	}
}