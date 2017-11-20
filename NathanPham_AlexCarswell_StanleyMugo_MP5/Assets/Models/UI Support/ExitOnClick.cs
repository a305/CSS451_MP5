using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitOnClick : MonoBehaviour
{
	public Button btnExit;

	// Make the window close when the exit button is pressed
	void Start()
	{
		GetComponent<Button>().onClick.AddListener(Application.Quit);
	}
}
