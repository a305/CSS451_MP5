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

	public SliderWithEcho cylinderResWidth;
	public SliderWithEcho cylinderResHeight;

	public SliderWithEcho quadResWidth;
	public SliderWithEcho quadResHeight;

	public SliderWithEcho cylinderRot;

	void Start ()
	{
		Debug.Assert(myWorld != null);
		Debug.Assert(quadMesh != null);
		Debug.Assert(meshSelector != null);
		Debug.Assert(cylinderResWidth != null);
		Debug.Assert(cylinderResHeight != null);
		Debug.Assert(cylinderRot != null);
		Debug.Assert(quadResWidth != null);
		Debug.Assert(quadResHeight != null);
		visibleMesh = quadMesh.gameObject;

		quadMesh.enabled = true;
		cylinderResWidth.enabled = true;

		cylinderResWidth.onValueChange = OnUICylinderResolutionUpdate;
		cylinderResHeight.onValueChange = OnUICylinderResolutionUpdate;
		cylinderRot.onValueChange = OnUICylinderRotationUpdate;
		quadResWidth.onValueChange = OnUIQuadResolutionUpdate;
		quadResHeight.onValueChange = OnUIQuadResolutionUpdate;

		meshSelector.ClearOptions();
		meshSelector.AddOptions(dropDownOptions);
		meshSelector.onValueChanged.AddListener(DropDownValueChanged);

		meshSelector.value = 0; // Start of showing the quadmesh
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
		cylinderMesh.GetComponent<AllMesh>().SetResolution((int)cylinderResWidth.GetValue(),
															(int) cylinderResHeight.GetValue());
	}

	private void OnUICylinderRotationUpdate(char id, float newVal)
	{
		//myWorld.ForceDeselect();
		cylinderMesh.GetComponent<CylinderMesh>().SetCylinderRotation((int)newVal);
	}

	private void OnUIQuadResolutionUpdate(char id, float newVal)
	{
		quadMesh.GetComponent<AllMesh>().SetResolution((int)quadResWidth.GetValue(),
														(int)quadResHeight.GetValue());
	}
}
