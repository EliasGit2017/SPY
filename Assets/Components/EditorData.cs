using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class EditorData : MonoBehaviour {
	public enum Mode {Place, Properties, Scenario};
	public Mode editorMode;
	public GameObject editorBlock;
	public GameObject propertiesBlock;
	public Dictionary<string, int> actionBlockLimit;
}