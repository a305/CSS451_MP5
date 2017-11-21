using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeshUIController : MonoBehaviour
{
	private List<string> dropDownOptions = 
		new List<string> { "Mesh", "Cylinder"};
	
	private GameObject visibleMesh;
	public QuadMesh quadMesh;
	public CylinderMesh cylinderMesh;

	public TheWorld myWorld;
	public Dropdown meshSelector;
	public SliderWithEcho cylinderRes;
	public SliderWithEcho cylinderRot;
	public SliderWithEcho quadRes;

	void Start ()
	{
		Debug.Assert(myWorld != null);
		Debug.Assert(quadMesh != null);
		Debug.Assert(meshSelector != null);
		Debug.Assert(cylinderRes != null);
		Debug.Assert(cylinderRot != null);
		Debug.Assert(quadRes != null);
		visibleMesh = quadMesh.gameObject;

		cylinderRes.onValueChange = OnUICylinderResolutionUpdate;
		cylinderRot.onValueChange = OnUICylinderRotationUpdate;
		quadRes.onValueChange = OnUIQuadResolutionUpdate;

		meshSelector.ClearOptions();
		meshSelector.AddOptions(dropDownOptions);
		meshSelector.onValueChanged.AddListener(DropDownValueChanged);

		meshSelector.value = 1;
	}

	public void HideNormals()
	{
		quadMesh.HideNormals();
		cylinderMesh.HideNormals();
	}

	public void ShowNormals()
	{
		quadMesh.ShowNormals();
		cylinderMesh.ShowNormals();
	}

	private void DropDownValueChanged(int indexOfNewVal)
	{
		visibleMesh.SetActive(false);
		if (indexOfNewVal == 0 && visibleMesh != quadMesh.gameObject)
		{
			visibleMesh = quadMesh.gameObject;
			myWorld.ForceDeselect();
		}
		else if (visibleMesh != cylinderMesh.gameObject)
		{
			visibleMesh = cylinderMesh.gameObject;
			myWorld.ForceDeselect();
		}
		visibleMesh.SetActive(true);
	}

	private void OnUICylinderResolutionUpdate(char id, float newVal)
	{
		myWorld.ForceDeselect();
		cylinderMesh.GetComponent<AllMesh>().SetResolution((int)newVal);
	}

	private void OnUICylinderRotationUpdate(char id, float newVal)
	{
		//myWorld.ForceDeselect();
		cylinderMesh.GetComponent<CylinderMesh>().SetCylinderRotation((int)newVal);
	}

	private void OnUIQuadResolutionUpdate(char id, float newVal)
	{
		quadMesh.GetComponent<AllMesh>().SetResolution((int)newVal);
	}
}
