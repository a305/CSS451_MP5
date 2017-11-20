/// Created by Stanley Mugo Nov 18, 2017
/// 
/// Automatically finds the echo Text which should be named "lblEcho"
/// as well as the slider which must be children of the object this
/// script is attatched to.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Wrapper function for the slider.
/// 
/// Insures that the change of value listener is not called when an attempt is
/// made to change value to what it already is. Provides the listener identifing
/// information about the calling slider.
/// 
/// Automatically updates the echo with the correct formating when the value of
/// the slider is changed.
/// </summary>
public class SliderWithEcho : MonoBehaviour
{
	public char id;
	protected Text echo;
	protected Slider slider;
	public delegate void OnValueChanged(char id, float val);
	public OnValueChanged onValueChange;

	/// <summary>
	/// Initialize the handler to update the echo when the value is changed.
	/// </summary>
	private void Start()
	{
		foreach (Text child in GetComponentsInChildren<Text>())
			if (child.name == "lblEcho")
			{
				echo = child;
				break;
			}

		slider = GetComponentInChildren<Slider>();

		Debug.Assert(echo != null);
		Debug.Assert(slider != null);
		slider.onValueChanged.AddListener(OnValueChange);
		UpdateEcho(slider.value);
	}
	
	/// <summary>
	/// Updates the echo to the inputed value with the corrent formating.
	/// </summary>
	/// <param name="val">The value to display in the echo label.</param>
	private void UpdateEcho(float val)
	{
		echo.text = string.Format("{0:0.0000}", val);
	}
	
	/// <summary>
	/// Sets the value of the currently selected slider.
	/// Insures that the change listener only called when the inputed value
	/// is different from the last value.If the value is greater or less
	/// than that allowed by the slider, then the slider will be set to the
	/// </summary>
	/// <param name="val">The value to set the slider to.</param>
	/// <returns>True if the value is between the max and min slider range.</returns>
	public bool SetValue(float val)
	{
		if (slider.value != val)
			slider.value = val;

		return slider.value == val;
	}
	
	/// <summary>
	/// Returns the current value of the slider.
	/// </summary>
	/// <returns>A value between the max and min slider values (inclusive).</returns>
	public float GetValue()
	{
		return slider.value;
	}

	/// <summary>
	/// Sets the event listener which will be called when the value of the dropdown
	/// menu is changed. The listener will be passed the value of the drop down.
	/// </summary>
	/// <param name="vHandler">The call function with the newly changed argument.</param>
	private void OnValueChange(float val)
	{
		UpdateEcho(val);

		if (onValueChange != null)
			onValueChange.Invoke(id, val);
	}

	/// <summary>
	/// Sets the minimum value that the slider supports.
	/// </summary>
	/// <param name="val">Minimum value the slider can be set to</param>
	public void SetMin(float val)
	{
		slider.minValue = val;
	}

	/// <summary>
	/// Sets the maximum value that the slider supports.
	/// </summary>
	/// <param name="val">Maximum value the slider can be set to</param>
	public void SetMax(float val)
	{
		slider.maxValue = val;
	}
}
