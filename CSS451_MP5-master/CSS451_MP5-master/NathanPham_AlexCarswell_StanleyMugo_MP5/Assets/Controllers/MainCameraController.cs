using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{
	public GameObject lookAtObj;
	private bool trackEnabled = false;			// Insure Tumble and Track aren't being done at the same time
	private float trackSpeed = 0.01f;
	private float maxRotationDegY = 180;
	private float maxRotationDegX = 180;

	private Vector3 cameraToAxisDirection;		// Used to move the camera to the same position it was in relative to the axis frame
	
	private Vector3 oldAxisFramePosition;		// Used to get the translated camera position relative to how much the mouse has been moved.
	private Vector2 mouseTrackStartPos;         // Same use as {@code oldAxisFramePosition}

	private void Start()
	{
		Debug.Assert(lookAtObj != null);
		UpdateLookat();
	}

	private void UpdateLookat()
	{
		transform.LookAt(lookAtObj.transform);
	}

	public void MoveForward()
	{
		const float speed = 0.1f;
		lookAtObj.transform.localPosition += transform.forward.normalized * speed;
		transform.localPosition += transform.forward.normalized * speed;
	}

	// Called the first time the appropriate mouse button is pressed down
	public void StartTrack(Vector2 mouseCenter)
	{
		trackEnabled = true;
		mouseTrackStartPos = mouseCenter;
		cameraToAxisDirection = (transform.localPosition - lookAtObj.transform.localPosition);
		oldAxisFramePosition = lookAtObj.transform.localPosition;
	}

	public static float distance;

	// Called everytime the appropriate mouse button is pressed down and the mouse is being moved
	public void MoveTrack(Vector2 mousePosition)
	{
		if (trackEnabled)
		{
			Vector3 offset = transform.right * (mousePosition.x - mouseTrackStartPos.x) * trackSpeed +
								transform.up * (mousePosition.y - mouseTrackStartPos.y) * trackSpeed;

			lookAtObj.transform.localPosition = oldAxisFramePosition + offset;
			transform.localPosition = lookAtObj.transform.localPosition + cameraToAxisDirection;
		}
	}

	// Called the first time the appropriate mouse button is pressed down
	public void StartTumble(Vector2 mouseCenter)
	{
		trackEnabled = false;
		mouseTrackStartPos = mouseCenter;
		distance = (transform.localPosition - lookAtObj.transform.localPosition).magnitude;
	}

	// Called everytime the appropriate mouse button is pressed down and the mouse is being moved
	public void MoveTumble(Vector2 mousePosition)
	{
		Vector3 rotatedCamPos = RotatePointAroundSphere(transform.localPosition,
									lookAtObj.transform.localPosition,
									maxRotationDegY * (mousePosition.y - mouseTrackStartPos.y) / Screen.height,
									transform.right
									);
		
		if (Mathf.Abs(Vector3.Dot((rotatedCamPos - lookAtObj.transform.localPosition).normalized, Vector3.up)) > 0.99f)
			rotatedCamPos = transform.localPosition;

		rotatedCamPos = RotatePointAroundSphere(rotatedCamPos,
									lookAtObj.transform.localPosition,
									maxRotationDegX * (mousePosition.x - mouseTrackStartPos.x) / Screen.width,
									transform.up
									);

		transform.localPosition = rotatedCamPos;
		mouseTrackStartPos = mousePosition;
		UpdateLookat();
	}

	public void OnScroll(Vector2 delta)
	{
		Vector3 nPostion = transform.localPosition +
			(lookAtObj.transform.localPosition - transform.localPosition).normalized * delta.y;

		float distToNewPosition = Vector3.Distance(nPostion, transform.localPosition);
		float distToCamera = Vector3.Distance(nPostion, lookAtObj.transform.localPosition);

		if (distToCamera < distToNewPosition)
		{
			if (distToCamera > Camera.main.nearClipPlane * 2)
			{
				transform.localPosition = transform.localPosition + transform.forward.normalized * Mathf.Min(0.05f, distToCamera / 4);
			}
		}
		else
			transform.localPosition = nPostion;
	}

	public static Vector3 RotatePointAroundSphere(Vector3 pointPos, Vector3 sphereCenterPos, float angleDeg, Vector3 rotationAxis)
	{
		Matrix4x4 r = Matrix4x4.TRS(Vector3.zero, Quaternion.AngleAxis(angleDeg, rotationAxis), Vector3.one);
		Matrix4x4 invP = Matrix4x4.TRS(sphereCenterPos, Quaternion.identity, Vector3.one);
		Matrix4x4 p = Matrix4x4.TRS(-sphereCenterPos, Quaternion.identity, Vector3.one);
		return (invP * r * p).MultiplyPoint(pointPos);
	}
}