/// Created by Stanley Mugo Nov 19, 2017

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Controller : MonoBehaviour
{
	public MeshUIController meshUIController;
	public MainCameraController mainCameraCtrl;
	public PositionController posCtrl;
	public TheWorld myWorld;

	/// <summary>
	/// Initialize stuff
	/// </summary>
	private void Start()
	{
		Debug.Assert(myWorld != null);
		Debug.Assert(posCtrl != null);
		Debug.Assert(mainCameraCtrl != null);
		Debug.Assert(meshUIController != null);

		Repositionable.posCtrl = posCtrl;
	}
	
	/// <summary>
	/// Handle mouse events
	/// </summary>
	private void Update ()
	{
		ProcessMouseEvents();
	}
}
