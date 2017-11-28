using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XfromControl : MonoBehaviour {
    public Toggle T, R, S;
    public SliderWithEchoSecondVersion X, Y, Z;
    public Text ObjectName;
    public TexturePlacement mSelected;

    private float rotation = 0;
    private Matrix3x3 position = Matrix3x3.identity;
    private Matrix3x3 scale = Matrix3x3Helpers.CreateScale(Vector2.one);
    private Matrix3x3 rotationMatrix = Matrix3x3Helpers.CreateRotation(0);

	// use this for initialization
	void Start () {
        T.onValueChanged.AddListener(SetToTranslation);
        R.onValueChanged.AddListener(SetToRotation);
        S.onValueChanged.AddListener(SetToScaling);
        X.SetSliderListener(XValueChanged);
        Y.SetSliderListener(YValueChanged);
        Z.SetSliderListener(ZValueChanged);

        T.isOn = true;
        R.isOn = false;
        S.isOn = false;
        SetToTranslation(true);
	}
	
    void SetToTranslation(bool v)
    {
        if (T.isOn == false)
            return;
        Vector3 p = GetSelectedXformParameter();
        X.InitSliderRange(-4, 4, p.x);
        Y.InitSliderRange(-4, 4, p.y);
        Z.InitSliderRange(0, 0, p.z);
    }

    void SetToScaling(bool v)
    {
        if (S.isOn == false)
            return;
        Vector3 s = GetSelectedXformParameter();
        X.InitSliderRange(0.1f, 10, s.x);
        Y.InitSliderRange(0.1f, 10, s.y);
        Z.InitSliderRange(0, 0, s.z);
    }

    void SetToRotation(bool v)
    {
        if (R.isOn == false)
            return;
        Vector3 r = GetSelectedXformParameter();
        X.InitSliderRange(0, 0, r.x);
        Y.InitSliderRange(0, 0, r.y);
        Z.InitSliderRange(-180, 180, r.z);
    }

    void XValueChanged(float v)
    {
        Vector3 p = GetSelectedXformParameter();
        p.x = v;
        SetSelectedXform(ref p, ref v);
    }
    
    void YValueChanged(float v)
    {
        Vector3 p = GetSelectedXformParameter();
        p.y = v;        
        SetSelectedXform(ref p, ref v);
    }

    void ZValueChanged(float v)
    {
        Vector3 p = GetSelectedXformParameter();
        p.z = v;
        SetSelectedXform(ref p, ref v);
    }

    public void SetSelectedObject(TexturePlacement g)
    {
        mSelected = g;
        if (g != null)
            ObjectName.text = "Selected:" + g.name;
        else
            ObjectName.text = "Selected: none";
        ObjectSetUI();
    }

    public void ObjectSetUI()
    {
        Vector3 p = GetNewXformParameter();
        X.SetSliderValue(p.x);  // do not need to call back for this comes from the object
        Y.SetSliderValue(p.y);
        Z.SetSliderValue(p.z);
    }

    private Vector3 GetNewXformParameter()
    {
        Vector3 p;

        rotation = 0;
        position = Matrix3x3.identity;
        scale = Matrix3x3Helpers.CreateScale(Vector2.one);
        rotationMatrix = Matrix3x3Helpers.CreateRotation(0);

        if (T.isOn)
        {
            p = Vector3.zero;
        }
        else if (S.isOn)
        {
            p = Vector3.one;
        }
        else
        {
            p = Vector3.zero;
        }

        return p;
    }

    private Vector3 GetSelectedXformParameter()
    {
        Vector3 p;

        if (T.isOn)
        {
            if (mSelected != null)
                p = new Vector3(position.m02, position.m12, 0);
            else
                p = Vector3.zero;
        }
        else if (S.isOn)
        {
            if (mSelected != null)
                p = new Vector3(scale.m00, scale.m11, 0);
            else
            p = Vector3.one;
        }
        else
        {
            if (mSelected != null)
                p = new Vector3(0, 0, rotation);
            else
                p = Vector3.zero;
        }
        return p;
    }

    private void SetSelectedXform(ref Vector3 p, ref float q)
    {
        if (mSelected == null)
            return;

        Matrix3x3 temp = Matrix3x3.identity;
        if (T.isOn)
        {
            position = Matrix3x3Helpers.CreateTranslation(new Vector2(p.x, p.y));
        }
        else if (S.isOn)
        {
            scale = Matrix3x3Helpers.CreateScale(new Vector2(p.x, p.y));
        }
        else
        {
            rotation = q;
            rotationMatrix = Matrix3x3Helpers.CreateRotation(rotation);
        }

        temp = scale * rotationMatrix * position;
        mSelected.SetTRS(ref temp);
    }
}