using UnityEngine;
using FYFY;

public class SelectBlockSystem : FSystem
{
	private EditorData editorData;
	
	public static SelectBlockSystem instance;
	public GameObject mover;

	public SelectBlockSystem()
	{
		instance = this;
	}
	
	protected override void onStart()
	{
		base.onStart();
		editorData = GameObject.Find("EditorData").GetComponent<EditorData>();
	}

	public void selectBlock(GameObject obj)
	{
		GameObjectManager.unbind(mover.transform.GetChild(0).gameObject);
		GameObject.Destroy(mover.transform.GetChild(0).gameObject);
		GameObject newGO = GameObject.Instantiate(obj, mover.transform);
		if (newGO.GetComponent<BoxCollider>() != null)
			newGO.GetComponent<BoxCollider>().enabled = false;
		foreach (var collider in newGO.transform.GetComponentsInChildren<Collider>())
		{
			collider.enabled = false;
		}

		GameObjectManager.bind(newGO);
		editorData.editorBlock = obj;
	}
}


