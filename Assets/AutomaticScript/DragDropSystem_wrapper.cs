using UnityEngine;
using FYFY;

public class DragDropSystem_wrapper : BaseWrapper
{
	public UnityEngine.GameObject mainCanvas;
	public UnityEngine.GameObject lastDropZoneUsed;
	public UnityEngine.AudioSource audioSource;
	public System.Single catchTime;
	private void Start()
	{
		this.hideFlags = HideFlags.NotEditable;
		MainLoop.initAppropriateSystemField (system, "mainCanvas", mainCanvas);
		MainLoop.initAppropriateSystemField (system, "lastDropZoneUsed", lastDropZoneUsed);
		MainLoop.initAppropriateSystemField (system, "audioSource", audioSource);
		MainLoop.initAppropriateSystemField (system, "catchTime", catchTime);
	}

	public void checkHighlightDropArea(UnityEngine.GameObject dropArea)
	{
		MainLoop.callAppropriateSystemMethod (system, "checkHighlightDropArea", dropArea);
	}

	public void DraggredActionStatement(System.String actionName)
	{
		MainLoop.callAppropriateSystemMethod (system, "DraggredActionStatement", actionName);
	}

	public void unhighlightDropArea(UnityEngine.GameObject dropArea)
	{
		MainLoop.callAppropriateSystemMethod (system, "unhighlightDropArea", dropArea);
	}

	public void beginDragElementFromLibrary(UnityEngine.EventSystems.BaseEventData element)
	{
		MainLoop.callAppropriateSystemMethod (system, "beginDragElementFromLibrary", element);
	}

	public void DroppedActionStatement()
	{
		MainLoop.callAppropriateSystemMethod (system, "DroppedActionStatement", null);
	}

	public void beginDragElementFromEditableScript(UnityEngine.EventSystems.BaseEventData element)
	{
		MainLoop.callAppropriateSystemMethod (system, "beginDragElementFromEditableScript", element);
	}

	public void dragElement()
	{
		MainLoop.callAppropriateSystemMethod (system, "dragElement", null);
	}

	public void endDragElement()
	{
		MainLoop.callAppropriateSystemMethod (system, "endDragElement", null);
	}

	public void deleteElement(UnityEngine.GameObject elementToDelete)
	{
		MainLoop.callAppropriateSystemMethod (system, "deleteElement", elementToDelete);
	}

	public void refreshHierarchyContainers(UnityEngine.GameObject elementToRefresh)
	{
		MainLoop.callAppropriateSystemMethod (system, "refreshHierarchyContainers", elementToRefresh);
	}

	public void checkDoubleClick(UnityEngine.EventSystems.BaseEventData element)
	{
		MainLoop.callAppropriateSystemMethod (system, "checkDoubleClick", element);
	}

}
