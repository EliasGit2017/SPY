using UnityEngine;
using FYFY;
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GenerateLevelEditor : FSystem {

	public static GenerateLevelEditor instance;
	private EditorData editorData;

	// On r�cup�re tout les �l�ments pouvant etre pr�sent dans un niveau 
	private Family f_player = FamilyManager.getFamily(new AllOfComponents(typeof(ScriptRef), typeof(Position)), new AnyOfTags("Player"));
	private Family f_drone = FamilyManager.getFamily(new AllOfComponents(typeof(ScriptRef)), new AnyOfTags("Drone")); 
	private Family f_exit = FamilyManager.getFamily(new AllOfComponents(typeof(Position), typeof(AudioSource)), new AnyOfTags("Exit"));
	private Family f_spawn = FamilyManager.getFamily(new AllOfComponents(typeof(Position), typeof(AudioSource)), new AnyOfTags("Spawn"));

	private Family f_wall = FamilyManager.getFamily(new AllOfComponents(typeof(Position)), new AnyOfTags("Wall"));
	private Family f_door = FamilyManager.getFamily(new AnyOfTags("Door"));
	private Family f_activableConsole = FamilyManager.getFamily(new AllOfComponents(typeof(Direction),typeof(Activable)),new AnyOfTags("ActivableConsole"));
	private Family f_cell  = FamilyManager.getFamily(new AnyOfTags("Cell"));
	private Family f_decoration = FamilyManager.getFamily(new AnyOfTags("Decoration"));

	// On r�cup�re les dialogues
	private Family f_dialogs = FamilyManager.getFamily(new AllOfComponents(typeof(Dialogs)));


	public GenerateLevelEditor()
	{
		instance = this;
	}
	
	// Use to init system before the first onProcess call
	protected override void onStart()
	{
		editorData = GameObject.Find("EditorData").GetComponent<EditorData>();
	}

	public void LevelToXml()
    {
		XmlDocument xmlDoc = new XmlDocument();

		XmlDocument campaignMain = new XmlDocument();
		campaignMain.Load("Assets/StreamingAssets/Scenario/GeneratedCampaign.xml");

		XmlNode docNode = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
		xmlDoc.AppendChild(docNode);
		XmlNode levelNode = xmlDoc.CreateElement("level");
		xmlDoc.AppendChild(levelNode);

		//On d�finit les diff�rents noeuds du xml
		XmlNode mapNode = xmlDoc.CreateElement("map");
		levelNode.AppendChild(mapNode);
		XmlNode dialogsNode = xmlDoc.CreateElement("dialogs");
		levelNode.AppendChild(dialogsNode);
		XmlNode blockLimitsNode = xmlDoc.CreateElement("blockLimits");
		levelNode.AppendChild(blockLimitsNode);


		// Parcours des objets plac�s avec l'�diteur de niveau et on cr�e la map
		List<float> line = new List<float>();
		List<float> column = new List<float>();
		Dictionary<(float,float), int> gridvalue = new Dictionary<(float,float), int>();

		// on pose d'abord les cell
		foreach (GameObject goCell in f_cell)
        {
			// on v�rifie que ce n'est pas le mover
			if(goCell.transform.parent == null)
            {
				Vector3 posCell = goCell.transform.position;
				float gridX = posCell.x;
				float gridY = posCell.z;

				if (line.Contains(gridX) == false)
				{
					line.Add(gridX);
				}
				if (column.Contains(gridY) == false)
				{
					column.Add(gridY);
				}
				gridvalue.Add((gridX, gridY), 0); // value = 0 for a cell path
			}

		}

		// On pose ensuite les murs
		foreach (GameObject goWall in f_wall)
		{
			if (goWall.transform.parent == null)
            {
				Vector3 posWall = goWall.transform.position;
				float gridX = posWall.x;
				float gridY = posWall.z;
				if (line.Contains(gridX) == false)
				{
					line.Add(gridX);
				}
				if (column.Contains(gridY) == false)
				{
					column.Add(gridY);
				}
				if (gridvalue.TryGetValue((gridX, gridY), out int output))
				{
					//si il y a d�j� une cell � cette position on change la valeur
					gridvalue[(gridX, gridY)] = 1;
				}
				else
				{
					gridvalue.Add((gridY, gridX), 1); // value = 1 for a wall 
				}
			}

		}

		// On pose ensuite le spawn
		foreach (GameObject goSpawn in f_spawn)
		{
			if (goSpawn.transform.parent == null)
			{
				Vector3 posSpawn = goSpawn.transform.position;
				float gridX = posSpawn.x;
				float gridY = posSpawn.z;
				if (line.Contains(gridX) == false)
				{
					line.Add(gridX);
				}
				if (column.Contains(gridY) == false)
				{
					column.Add(gridY);
				}
				if (gridvalue.TryGetValue((gridX, gridY), out int output))
				{
					gridvalue[(gridX, gridY)] = 2;
				}
				else
				{
					gridvalue.Add((gridY, gridX), 2); // value = 2 for spawn 
				}
			}

		}

		// On pose ensuite l'exit
		foreach (GameObject goExit in f_exit)
		{
			if (goExit.transform.parent == null)
			{
				Vector3 posExit = goExit.transform.position;
				float gridX = posExit.x;
				float gridY = posExit.z;
				if (line.Contains(gridX) == false)
				{
					line.Add(gridX);
				}
				if (column.Contains(gridY) == false)
				{
					column.Add(gridY);
				}
				if (gridvalue.TryGetValue((gridX, gridY), out int output))
				{
					gridvalue[(gridX, gridY)] = 3;
				}
				else
				{
					gridvalue.Add((gridY, gridX), 3); // value = 3 for exit
				}
			}

		}

		writeXMLMap(xmlDoc, mapNode, line ,column, gridvalue);
		writeXMLDialogs(xmlDoc, dialogsNode);
		writeXMLBlockLimits(xmlDoc, blockLimitsNode);
		writeXMLInteractives(xmlDoc, levelNode, line, column);
		writeXMLRobots(xmlDoc, levelNode, line, column);
		writeXMLDecorations(xmlDoc, levelNode, line, column);

		var custom_level_id = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds(); // short uuid
		string custom_level_id_str = custom_level_id.ToString();

		XmlNode scenario = campaignMain.SelectSingleNode("//scenario");
		XmlNode last_level = campaignMain.SelectSingleNode("//level");
		XmlNode link_node = campaignMain.CreateElement("level");
		XmlAttribute link_lvl_name = campaignMain.CreateAttribute("name");

		link_lvl_name.Value = "Levels/GeneratedCampaign/CustomLevel" + custom_level_id_str.Substring(custom_level_id_str.Length - 3) + ".xml";
		link_node.Attributes.Append(link_lvl_name);
		scenario.InsertAfter(link_node, last_level);

		// xmlDoc.Save("./Assets/XmlTest/CustomLevel" + custom_level_id_str.Substring(custom_level_id_str.Length - 3) + ".xml");
		xmlDoc.Save("./Assets/StreamingAssets/Levels/GeneratedCampaign/CustomLevel" + custom_level_id_str.Substring(custom_level_id_str.Length - 3) + ".xml");
		campaignMain.Save("Assets/StreamingAssets/Scenario/GeneratedCampaign.xml");
		GameObjectManager.loadScene("TitleScreen");
    }

	private void writeXMLMap(XmlDocument xmlDoc, XmlNode mapNode, List<float> line, List<float> column, Dictionary<(float, float), int> gridvalue)
	{
		line.Sort();
		column.Sort();

		int nbline = line.Count();
		int nbcolumn = column.Count();

		Debug.Log("nbline :" + nbline);
		Debug.Log("nbcolumn :" + nbcolumn);
		foreach (float l in line)
		{
			Debug.Log("line :" + l);
		}
		foreach (float c in column)
		{
			Debug.Log("column:" + c);
		}

		for (int i = 0; i < nbline; i++)
		{
			XmlNode lineNode = xmlDoc.CreateElement("line");
			for(int j = 0; j < nbcolumn; j++)
            {
				XmlNode cellNode = xmlDoc.CreateElement("cell");
				XmlAttribute cellAttribute = xmlDoc.CreateAttribute("value");
				int value;

				// On regarde si on a un bloc � cette position dans la grille sinon c'est du vide
				if (gridvalue.TryGetValue((line[i],column[j]), out int output))
                {
					value = output;
                }
                else
                {
					value = -1; 
                }

				cellAttribute.Value = value.ToString();
				cellNode.Attributes.Append(cellAttribute);
				lineNode.AppendChild(cellNode);
			}
			mapNode.AppendChild(lineNode);

		}
		
	}

	private void writeXMLDialogs(XmlDocument xmlDoc, XmlNode dialogsNode)
    {
		foreach (GameObject goDialogs in f_dialogs)
        {
			List<String> AllDialogs = goDialogs.GetComponent<Dialogs>().dialog;
			foreach (String dialog in AllDialogs)
            {
				XmlNode dialogNode = xmlDoc.CreateElement("dialog");
				XmlAttribute textAttribute = xmlDoc.CreateAttribute("text");
				textAttribute.Value = dialog;
				dialogNode.Attributes.Append(textAttribute);
				dialogsNode.AppendChild(dialogNode);
			}
		}


	}

	private void writeXMLBlockLimits(XmlDocument xmlDoc, XmlNode blockLimitsNode)
    {
		foreach((string blockType, int limit) in editorData.actionBlockLimit)
        {
			XmlNode blockLimitNode = xmlDoc.CreateElement("blockLimit");
			XmlAttribute blockTypeAttribute = xmlDoc.CreateAttribute("blockType");
			XmlAttribute limitAttribute = xmlDoc.CreateAttribute("limit");

			blockTypeAttribute.Value = blockType;
			limitAttribute.Value = limit.ToString();

			blockLimitNode.Attributes.Append(blockTypeAttribute);
			blockLimitNode.Attributes.Append(limitAttribute);
			blockLimitsNode.AppendChild(blockLimitNode);
		}

	}

	private void writeXMLInteractives(XmlDocument xmlDoc, XmlNode levelNode, List<float> line, List<float> column)
    {
		line.Sort();
		column.Sort();

		foreach (GameObject goDoor in f_door)
		{
			if (goDoor.transform.parent == null)
			{
				Vector3 posDoor = goDoor.transform.position;
				float gridX = posDoor.x;
				float gridY = posDoor.z;
				if (line.Contains(gridX) && column.Contains(gridY))
				{
					XmlNode doorNode = xmlDoc.CreateElement("door");
					XmlAttribute posYAttribute = xmlDoc.CreateAttribute("posY");
					XmlAttribute posXAttribute = xmlDoc.CreateAttribute("posX");
					XmlAttribute slotIDAttribute = xmlDoc.CreateAttribute("slotId");
					XmlAttribute directionAttribute = xmlDoc.CreateAttribute("direction");

					posYAttribute.Value = line.IndexOf(gridX).ToString();
					posXAttribute.Value = column.IndexOf(gridY).ToString();
					slotIDAttribute.Value = goDoor.GetComponentInChildren<ActivationSlot>().slotID.ToString();
					directionAttribute.Value = ((int)(goDoor.GetComponentInChildren<Direction>().direction)).ToString();

					doorNode.Attributes.Append(posYAttribute);
					doorNode.Attributes.Append(posXAttribute);
					doorNode.Attributes.Append(slotIDAttribute);
					doorNode.Attributes.Append(directionAttribute);
					levelNode.AppendChild(doorNode);
				}
			}
		}

		foreach (GameObject goConsole in f_activableConsole)
		{
			if (goConsole.transform.parent == null)
			{
				Vector3 posConsole = goConsole.transform.position;
				float gridX = posConsole.x;
				float gridY = posConsole.z;
				if (line.Contains(gridX) && column.Contains(gridY))
				{
					XmlNode consoleNode = xmlDoc.CreateElement("console");
					XmlAttribute stateAttribute = xmlDoc.CreateAttribute("state");
					XmlAttribute posYAttribute = xmlDoc.CreateAttribute("posY");
					XmlAttribute posXAttribute = xmlDoc.CreateAttribute("posX");
					XmlAttribute directionAttribute = xmlDoc.CreateAttribute("direction");

					stateAttribute.Value = 1.ToString();
					posYAttribute.Value = line.IndexOf(gridX).ToString();
					posXAttribute.Value = column.IndexOf(gridY).ToString();
					directionAttribute.Value = ((int)(goConsole.GetComponent<Direction>().direction)).ToString();

					consoleNode.Attributes.Append(posYAttribute);
					consoleNode.Attributes.Append(posXAttribute);
					consoleNode.Attributes.Append(directionAttribute);

					List<int> slotsID = goConsole.GetComponent<Activable>().slotID;
					foreach(int slotID in slotsID)
                    {
						XmlNode slotNode = xmlDoc.CreateElement("slot");
						XmlAttribute slotIDAttribute = xmlDoc.CreateAttribute("slotId");
						slotIDAttribute.Value = slotID.ToString();
						slotNode.Attributes.Append(slotIDAttribute);
						consoleNode.AppendChild(slotNode);
					}

					levelNode.AppendChild(consoleNode);
				}
			}
		}

	}

	private void writeXMLRobots(XmlDocument xmlDoc, XmlNode levelNode, List<float> line, List<float> column)
	{
		line.Sort();
		column.Sort();

		foreach (GameObject goPlayer in f_player)
		{
			if (goPlayer.transform.parent == null)
			{
				Vector3 posPlayer = goPlayer.transform.position;
				float gridX = posPlayer.x;
				float gridY = posPlayer.z;
				if(line.Contains(gridX) && column.Contains(gridY))
                {
					XmlNode playerNode = xmlDoc.CreateElement("player");
					XmlAttribute scriptNameAttribute = xmlDoc.CreateAttribute("associatedScriptName");
					XmlAttribute posYAttribute = xmlDoc.CreateAttribute("posY");
					XmlAttribute posXAttribute = xmlDoc.CreateAttribute("posX");
					XmlAttribute directionAttribute = xmlDoc.CreateAttribute("direction");

					scriptNameAttribute.Value = goPlayer.name;
					posYAttribute.Value = line.IndexOf(gridX).ToString();
					posXAttribute.Value = column.IndexOf(gridY).ToString();
					directionAttribute.Value = ((int)(goPlayer.GetComponent<Direction>().direction)).ToString();

					playerNode.Attributes.Append(scriptNameAttribute);
					playerNode.Attributes.Append(posYAttribute);
					playerNode.Attributes.Append(posXAttribute);
					playerNode.Attributes.Append(directionAttribute);
					levelNode.AppendChild(playerNode);

					XmlNode scriptNode = xmlDoc.CreateElement("script");
					XmlAttribute nameAttribute = xmlDoc.CreateAttribute("name");
					XmlAttribute editModeAttribute = xmlDoc.CreateAttribute("editMode");
					nameAttribute.Value = goPlayer.name;
					editModeAttribute.Value = 0.ToString();
					scriptNode.Attributes.Append(nameAttribute);
					scriptNode.Attributes.Append(editModeAttribute);
					levelNode.AppendChild(scriptNode);



				}
			}
			
		}	

		
		foreach (GameObject goDrone in f_drone)
		{
			if (goDrone.transform.parent == null)
			{
				Vector3 posDrone = goDrone.transform.position;
				float gridX = posDrone.x;
				float gridY = posDrone.z;
				if (line.Contains(gridX) && column.Contains(gridY))
				{
					XmlNode enemyNode = xmlDoc.CreateElement("enemy");
					XmlAttribute posYAttribute = xmlDoc.CreateAttribute("posY");
					XmlAttribute posXAttribute = xmlDoc.CreateAttribute("posX");
					XmlAttribute directionAttribute = xmlDoc.CreateAttribute("direction");
					XmlAttribute rangeAttribute = xmlDoc.CreateAttribute("range");
					XmlAttribute selfRangeAttribute = xmlDoc.CreateAttribute("selfRange");
					XmlAttribute typeRangeAttribute = xmlDoc.CreateAttribute("typeRange");

					posYAttribute.Value = line.IndexOf(gridX).ToString();
					posXAttribute.Value = column.IndexOf(gridY).ToString();
					directionAttribute.Value = ((int)(goDrone.GetComponent<Direction>().direction)).ToString();
					rangeAttribute.Value = 1.ToString();
					selfRangeAttribute.Value = "False";
					typeRangeAttribute.Value = 0.ToString();

					enemyNode.Attributes.Append(posYAttribute);
					enemyNode.Attributes.Append(posXAttribute);
					enemyNode.Attributes.Append(directionAttribute);
					enemyNode.Attributes.Append(rangeAttribute);
					enemyNode.Attributes.Append(selfRangeAttribute);
					enemyNode.Attributes.Append(typeRangeAttribute);
					levelNode.AppendChild(enemyNode);
				}
			}
		}

	}

	private void writeXMLDecorations(XmlDocument xmlDoc, XmlNode levelNode, List<float> line, List<float> column)
	{
		line.Sort();
		column.Sort();

		foreach (GameObject goDecoration in f_decoration)
		{
			if (goDecoration.transform.parent == null)
			{
				Vector3 posDecoration = goDecoration.transform.position;
				float gridX = posDecoration.x;
				float gridY = posDecoration.z;
				if (line.Contains(gridX) && column.Contains(gridY))
				{
					XmlNode decorationNode = xmlDoc.CreateElement("decoration");
					XmlAttribute nameAttribute = xmlDoc.CreateAttribute("name");
					XmlAttribute posYAttribute = xmlDoc.CreateAttribute("posY");
					XmlAttribute posXAttribute = xmlDoc.CreateAttribute("posX");
					XmlAttribute directionAttribute = xmlDoc.CreateAttribute("direction");

					int position = goDecoration.name.IndexOf("(");
					nameAttribute.Value = "Modern Furniture/Prefabs/" + goDecoration.name.Substring(0, position);
					posYAttribute.Value = line.IndexOf(gridX).ToString();
					posXAttribute.Value = column.IndexOf(gridY).ToString();
					directionAttribute.Value = ((int)(goDecoration.GetComponent<Direction>().direction)).ToString();

					decorationNode.Attributes.Append(nameAttribute);
					decorationNode.Attributes.Append(posYAttribute);
					decorationNode.Attributes.Append(posXAttribute);
					decorationNode.Attributes.Append(directionAttribute);
					levelNode.AppendChild(decorationNode);
				}
			}
		}


	}

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) 
	{

	}
}