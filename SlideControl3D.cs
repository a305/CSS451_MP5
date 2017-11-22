using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideControl3D : MonoBehaviour {

    public GameObject selected;
    public Camera mainCamera;
    public GameObject XAxis;
    public GameObject YAxis;
    public GameObject ZAxis;

    GameObject current;
    Vector3 onScreen;
    Color oldColor;

    float x;
    float y;
    Vector3 distance;
    float change;

    // Use this for initialization
    void Start () {
        selected = null;
        XAxis.SetActive(false);
        YAxis.SetActive(false);
        ZAxis.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        if (!Input.GetKey(KeyCode.LeftControl))
        {
            if (current)
            {
                current.GetComponent<Renderer>().material.SetColor("_Color", oldColor);
                current = null;
            }

            selected = null;
            XAxis.SetActive(false);
            YAxis.SetActive(false);
            ZAxis.SetActive(false);

            return;
        }
		if (Input.GetMouseButton(0) && current != null)
        {
            float dX = x - Input.mousePosition.x;
            float dY = y - Input.mousePosition.y;
            float delta = Mathf.Sqrt(x * x + y * y);
            onScreen = mainCamera.WorldToScreenPoint(current.transform.localPosition);
            Vector3 newDist = Input.mousePosition - onScreen;

            if (delta > change)
                current.transform.parent.localPosition += current.transform.up * (newDist - distance).magnitude / 10;
            else if (delta < change)
                current.transform.parent.localPosition -= current.transform.up * (newDist - distance).magnitude / 10;

            distance = newDist;
            change = delta;
            x = Input.mousePosition.x;
            y = Input.mousePosition.y;
        }
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray r = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(r, out hit))
            {
                if (hit.transform.gameObject.name == "XAxis" || hit.transform.gameObject.name == "ZAxis" || hit.transform.gameObject.name == "YAxis")
                {
                    current = hit.transform.gameObject;
                    oldColor = current.GetComponent<Renderer>().material.color;
                    current.GetComponent<Renderer>().material.SetColor("_Color", new Color(200, 200, 0, .2f));
                    onScreen = mainCamera.WorldToScreenPoint(hit.transform.localPosition);

                    x = Input.mousePosition.x;
                    y = Input.mousePosition.y;

                    distance = Input.mousePosition - onScreen;
                    change = 0;
                }
                else
                {
                    selected = hit.transform.gameObject;
                    XAxis.transform.SetParent(selected.transform, false);
                    YAxis.transform.SetParent(selected.transform, false);
                    ZAxis.transform.SetParent(selected.transform, false);
                    XAxis.SetActive(true);
                    YAxis.SetActive(true);
                    ZAxis.SetActive(true);
                }
            }
            else
            {
                selected = null;
                XAxis.SetActive(false);
                YAxis.SetActive(false);
                ZAxis.SetActive(false);
            }
        }
        if (Input.GetMouseButtonUp(0) && current != null)
        {
            current.GetComponent<Renderer>().material.SetColor("_Color", oldColor);
            current = null;
        }
	}
}
