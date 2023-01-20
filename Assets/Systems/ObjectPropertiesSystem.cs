using System.Collections.Generic;
using System.Linq;
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

	public void changeDirection(int dir)
	{
		editorData.propertiesBlock.GetComponent<Direction>().direction = (Direction.Dir)dir;
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

	protected override void onProcess(int familiesUpdateCount)
	{
		if (currentGameObject != editorData.propertiesBlock)
		{
			editionPanel.transform.GetChild(0).GetComponent<TMP_Text>().text = editorData.propertiesBlock.name;
			
			List<Direction> dir = editorData.propertiesBlock.GetComponentsInChildren<Direction>().ToList();
			dir.Add(editorData.propertiesBlock.GetComponent<Direction>());
			if (dir.Count == 0)
			{
				editionPanel.transform.GetChild(1).gameObject.SetActive(false);
			}
			else
			{
				editionPanel.transform.GetChild(1).gameObject.SetActive(true);
				GameObject.Find("DirectionDropDown").GetComponent<TMP_Dropdown>().value = (int)dir[0].direction;
			}

			Activable[] act = editorData.propertiesBlock.GetComponentsInChildren<Activable>();
			if (act.Length == 0)
			{
				editionPanel.transform.GetChild(2).gameObject.SetActive(false);
			}
			else
			{
				editionPanel.transform.GetChild(2).gameObject.SetActive(true);
				GameObject.Find("IdInputField").GetComponent<TMP_InputField>().text = act[0].slotID.ToString();
			}
			currentGameObject = editorData.propertiesBlock;
		}
	}
}