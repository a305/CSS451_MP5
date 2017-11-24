using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XfromControl : MonoBehaviour {
    public Toggle T, R, S;
    public SliderWithEcho X, Y, Z;
    public Text ObjectName;
    public TexturePlacement mSelected;

    private Vector3 mPreviousSliderValues = Vector3.zero;
    private Matrix3x3 current = Matrix3x3.identity;
    private float rotation = 0;

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
        SetSelectedObject(mSelected);
	}
	
    void SetToTranslation(bool v)
    {
        Vector3 p = GetSelectedXformParameter();
        mPreviousSliderValues = p;
        X.InitSliderRange(-4, 4, p.x);
        Y.InitSliderRange(-4, 4, p.y);
        Z.InitSliderRange(0, 0, p.z);
    }

    void SetToScaling(bool v)
    {
        Vector3 s = GetSelectedXformParameter();
        mPreviousSliderValues = s;
        X.InitSliderRange(0.1f, 10, s.x);
        Y.InitSliderRange(0.1f, 10, s.y);
        Z.InitSliderRange(0, 0, s.z);
    }

    void SetToRotation(bool v)
    {
        Vector3 r = GetSelectedXformParameter();
        mPreviousSliderValues = r;
        X.InitSliderRange(0, 0, r.x);
        Y.InitSliderRange(0, 0, r.y);
        Z.InitSliderRange(-180, 180, r.z);
        mPreviousSliderValues = r;
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
        rotation = v;
        SetSelectedXform(ref p, ref v);
    }

    public void SetSelectedObject(TexturePlacement g)
    {
        mSelected = g;
        mPreviousSliderValues = Vector3.zero;
        if (g != null)
            ObjectName.text = "Selected:" + g.name;
        else
            ObjectName.text = "Selected: none";
        ObjectSetUI();
    }

    public void ObjectSetUI()
    {
        Vector3 p = GetSelectedXformParameter();
        X.SetSliderValue(p.x);  // do not need to call back for this comes from the object
        Y.SetSliderValue(p.y);
        Z.SetSliderValue(p.z);
    }

    private Vector3 GetSelectedXformParameter()
    {
        Matrix3x3 trs = mSelected.GetTRS();
        current = trs;
        Vector3 p;
        
        if (T.isOn)
        {
            if (mSelected != null)
                p = new Vector3(trs.m02, trs.m12, trs.m22);
            else
                p = Vector3.zero;
        }
        else if (S.isOn)
        {
            if (mSelected != null)
                p = new Vector3(trs.m00, trs.m11, trs.m22);
            else
                p = Vector3.one;
        }
        else
        {
            p = Vector3.zero;
        }
        return p;
    }

    private void SetSelectedXform(ref Vector3 p, ref float q)
    {
        if (mSelected == null)
            return;

        Matrix3x3 temp;
        if (T.isOn)
        {
            //temp = Matrix3x3Helpers.CreateTranslation(new Vector2(p.x, p.y));
            temp = Matrix3x3Helpers.CreateTRS(new Vector2(p.x, p.y), rotation, new Vector2(current.m00, current.m11));
        }
        else if (S.isOn)
        {
            //temp = Matrix3x3Helpers.CreateScale(new Vector2(p.x, p.y));
            temp = Matrix3x3Helpers.CreateTRS(new Vector2(current.m02, current.m12), rotation, new Vector2(p.x, p.y));
        } else
        {
            //temp = Matrix3x3Helpers.CreateRotation(q);
            temp = Matrix3x3Helpers.CreateTRS(new Vector2(current.m02, current.m12), q, new Vector2(current.m00, current.m11));
        }

        mSelected.SetTRS(ref temp);
    }
}