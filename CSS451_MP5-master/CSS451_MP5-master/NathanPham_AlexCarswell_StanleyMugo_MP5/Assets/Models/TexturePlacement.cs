using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TexturePlacement : MonoBehaviour {

    private Matrix3x3 trs = Matrix3x3.identity;
    Vector2[] mInitUV = null; // initial values

    public void SaveInitUV(Vector2[] uv)
    {
        mInitUV = new Vector2[uv.Length];
        for (int i = 0; i < uv.Length; i++)
            mInitUV[i] = uv[i];
    }

    public void SetTRS(ref Matrix3x3 newTRS)
    {
        if (mInitUV == null)
            return;

        trs = newTRS;

        Mesh theMesh = GetComponent<MeshFilter>().mesh;
        Vector2[] uv = theMesh.uv;
        for (int i = 0; i < uv.Length; i++)
        {
            uv[i] = Matrix3x3.MultiplyVector2(newTRS, mInitUV[i]);
        }
        theMesh.uv = uv;
    }

    public Matrix3x3 GetTRS()
    {
        return trs;
    }
}
