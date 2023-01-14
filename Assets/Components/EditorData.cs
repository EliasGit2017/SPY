using UnityEngine;

public class EditorData : MonoBehaviour {
	public enum Mode {Place, Properties, Scenario};
	public Mode editorMode;
	public GameObject editorBlock;
}