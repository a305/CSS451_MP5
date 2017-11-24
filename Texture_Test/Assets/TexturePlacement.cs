using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TexturePlacement : MonoBehaviour {

    public Vector2 Offset = Vector2.zero;
    public Vector2 Scale = Vector2.one;
    public Matrix3x3 trs = Matrix3x3.identity;
    public float rotation = 0;
    Vector2[] mInitUV = null; // initial values

    public void SaveInitUV(Vector2[] uv)
    {
        mInitUV = new Vector2[uv.Length];
        for (int i = 0; i < uv.Length; i++)
            mInitUV[i] = uv[i];
    }
	
	// Update is called once per frame
	void Update () {
        //Mesh theMesh = GetComponent<MeshFilter>().mesh;
        //Vector2[] uv = theMesh.uv;
        //for (int i = 0; i < uv.Length; i++)
        //{
        //    //uv[i].x = mInitUV[i].x * Scale.x;
        //    //uv[i].y = mInitUV[i].y * Scale.y;
        //    //uv[i] = Offset + uv[i];
        //    uv[i] = Matrix3x3.MultiplyVector2(trs, uv[i]);
        //}
        //theMesh.uv = uv;
    }

    public void SetTRS(ref Matrix3x3 newTRS)
    {
        trs = newTRS;

        Mesh theMesh = GetComponent<MeshFilter>().mesh;
        Vector2[] uv = theMesh.uv;
        for (int i = 0; i < uv.Length; i++)
        {
            //uv[i].x = mInitUV[i].x * Scale.x;
            //uv[i].y = mInitUV[i].y * Scale.y;
            //uv[i] = Offset + uv[i];
            uv[i] = Matrix3x3.MultiplyVector2(newTRS, uv[i]);
        }
        theMesh.uv = uv;
    }

    public Matrix3x3 GetTRS()
    {
        return trs;
    }
}
