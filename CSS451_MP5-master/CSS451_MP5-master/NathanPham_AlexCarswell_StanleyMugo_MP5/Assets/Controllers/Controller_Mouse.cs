using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public partial class Controller : MonoBehaviour
{
	/// <summary>
	/// Checks if something has been pressed by polling the current mouse status.
	/// Called once per frame.
	/// </summary>
	public void ProcessMouseEvents()
	{
		if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
			meshUIController.ShowNormals();
		
		if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl))
			if (!myWorld.HasSelected())
				meshUIController.HideNormals();

		if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
			HandCamera();
		else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
			HandleMesh();
	}

	private void HandleMesh()
	{
		if (EventSystem.current.IsPointerOverGameObject() && !myWorld.HasSelected()) return;

		GameObject selectedObj = null;
		Vector3 selectedPos;

		if (Input.GetMouseButtonUp(0))
		{
			// LMB released
			myWorld.ReleaseSelected();
		}
		else if (Input.GetMouseButtonDown(0))
		{
			// LMB pressed for the first time
			if (GetObjectAtMouse(out selectedObj, out selectedPos, 1 << myWorld.selectableLayer))
			{
				myWorld.SetSelected(selectedPos, ref selectedObj);
			}
			else
			{
				myWorld.DeselectSelected();
			}
		}
		else if (Input.GetMouseButton(0) && myWorld.HasSelected())
		{
			// LMB dragged
			GetObjectAtMouse(out selectedObj, out selectedPos, 1 << myWorld.selectableLayer);
			myWorld.DragedSelected(selectedPos, ref selectedObj);
		}
	}

	private void HandCamera()
	{
		if (EventSystem.current.IsPointerOverGameObject() && !myWorld.HasSelected()) return;

		if (Input.GetMouseButtonDown(0))
			mainCameraCtrl.StartTumble(Input.mousePosition);

		else if (Input.GetMouseButton(0))
			mainCameraCtrl.MoveTumble(Input.mousePosition);

		else if (Input.GetMouseButtonDown(1))
			mainCameraCtrl.StartTrack(Input.mousePosition);

		else if (Input.GetMouseButton(1))
			mainCameraCtrl.MoveTrack(Input.mousePosition);

		mainCameraCtrl.OnScroll(Input.mouseScrollDelta);

		//if (Input.GetKey(KeyCode.LeftShift))
		//	mainCameraCtrl.MoveForward();
	}

	/// <summary>
	/// Gets the object pointed to by the cursor.
	/// </summary>
	/// <param name="gObj">The game object selected by the cursor. Null if nothing selected.</param>
	/// <param name="mousePos">The position of the mouse in 3d space. Null if nothing selected.</param>
	/// <param name="layerMask">The layer mask where each bit represents one of 32 layers.</param>
	/// <returns>True if there is an object at the cursor's position else False.</returns>
	private bool GetObjectAtMouse(out GameObject gObj, out Vector3 mousePos, int layerMask)
	{
		RaycastHit hitInfo;

		bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),
										out hitInfo, Mathf.Infinity, layerMask);

		// Find what 3d point was clicked
		if (hit)
		{
			mousePos = hitInfo.point;
			gObj = hitInfo.transform.gameObject;
		}
		else
		{
			mousePos = Vector3.zero;
			gObj = null;
		}

		return hit;
	}
}
