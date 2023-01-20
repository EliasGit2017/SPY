using System;
using UnityEngine;
using FYFY;
using FYFY_plugins.PointerManager;
using Debug = UnityEngine.Debug;

public class FollowMouseSystem : FSystem
{
	private EditorData editorData;

	public static EndGameManager instance;

	public Camera camera;
	public GameObject leftMenu;	
	
	private Family f_followMouse = FamilyManager.getFamily(new AllOfComponents(typeof(FollowMouse)));
	private Family f_pointerOver = FamilyManager.getFamily(new AllOfComponents(typeof(PointerOver)));
	private RaycastHit _hit;
	private Ray _ray;
	private RectTransform _rectTransform;

	protected override void onStart()
	{
		base.onStart();
		Application.targetFrameRate = 60;
		_rectTransform = leftMenu.GetComponent<RectTransform>();
		editorData = GameObject.Find("EditorData").GetComponent<EditorData>();
	}

	private void placeMode()
	{
		_ray = camera.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(_ray, out _hit, 100.0f);
        if (_hit.distance > 0)
        {
            Vector3 pos = _hit.point;
            if (_hit.transform.gameObject.layer == 0)
            {
                pos = new Vector3((float)Math.Floor(pos.x / 3 + 0.5f) * 3,
                    (float)Math.Floor(pos.y / 3 + 0.5f) * 3,
                    (float)Math.Floor(pos.z / 3 + 0.5f) * 3);
            }
            else
            {
                pos = _hit.transform.gameObject.transform.position + 3 * _hit.normal;
            }

            Debug.DrawRay(_hit.point, _hit.normal, Color.green);

            foreach (GameObject go in f_followMouse)
            {
                go.transform.position = pos * go.transform.localScale.x;
                if (Input.GetMouseButtonDown(0))
                {
                    GameObject newGo = UnityEngine.Object.Instantiate(editorData.editorBlock, pos, Quaternion.identity);
                    GameObjectManager.bind(newGo);
                    // newGo.transform.localScale /= 3;
                    foreach (var col in newGo.transform.GetComponentsInChildren<Collider>())
                    {
	                    UnityEngine.Object.Destroy(col);
                    }

                    newGo.AddComponent<BoxCollider>();
                    BoxCollider collider = newGo.GetComponent<BoxCollider>();
                    Vector3 scale = newGo.transform.localScale;
                    collider.size = new Vector3(3f / scale.x, 3f / scale.y, 3f / scale.z);

                    if (newGo.name.Contains("Teleporter"))
                    {
	                    newGo.transform.Rotate(Vector3.left, 90f);
	                    newGo.transform.localPosition -= new Vector3(0, 1.5f, 0);
	                    collider.center = new Vector3(0f, 0f, 3f);
                    }
                    else if (newGo.name.Contains("Robot"))
                    {
	                    newGo.transform.localPosition -= new Vector3(0, 1.3f, 0);
	                    collider.center = new Vector3(0, 1, 0);
                    }

                    if (newGo.layer == 0)
                    {
	                    newGo.layer = 3;
                    }
                }

                if (Input.GetMouseButtonDown(1))
                {
                    if (_hit.transform.gameObject.layer != 0)
                    {
	                    GameObject toDelete = _hit.transform.gameObject;
	                    GameObjectManager.unbind(toDelete);
	                    UnityEngine.Object.Destroy(toDelete);
                    }
                }
            }
        }
	}

	private void propertiesMode()
	{
		_ray = camera.ScreenPointToRay(Input.mousePosition);
		Physics.Raycast(_ray, out _hit, 100.0f);
		if (_hit.transform.gameObject != null && Input.GetMouseButtonDown(0))
		{
			editorData.propertiesBlock = _hit.transform.gameObject;
		}
	}

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount)
	{
		bool overUI = false;
		foreach (var go in f_pointerOver)
		{
			if (go.layer == 5)
			{
				overUI = true;
				break;
			}
		}
		if (editorData.editorMode == EditorData.Mode.Place && !overUI)
		{
			placeMode();
		}
		else if (editorData.editorMode == EditorData.Mode.Properties && !overUI)
		{
			propertiesMode();
		}
	}
}