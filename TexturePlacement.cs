using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TexturePlacement : MonoBehaviour {

    Vector2[] mInitUV;

    public void SaveInitUV(Vector2[] uv)
    {
        mInitUV = new Vector2[uv.Length];
        for (int i = 0; i < uv.Length; i++)
            mInitUV[i] = uv[i];
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update ()
    {

        Mesh theMesh = GetComponent<MeshFilter>().mesh;
        Vector2[] uv = theMesh.uv;
        UpdateManualUVTRS(ref theMesh, uv);
    }

    public Vector2 Offset = Vector2.zero;
    public Vector2 Scale = Vector2.one;
    public Matrix3x3 trs = Matrix3x3.identity;
    public float rotation = 0;
    private Matrix3x3 inverseTRS = Matrix3x3.identity; // to undo current uv
    private Vector2 inverseT = Vector2.zero;
    public bool activateUpdate = false;

    // Preconditions: Pass in a float for rotation, and 2 vector 2's for scaling and translation (Offset)
    // In addition, pass in the CURRENT uv. not the old one.
    private void UpdateManualUVTRS(ref Mesh theMesh, Vector2[] uv)
    {
        float cosZ = Mathf.Cos(Mathf.Rad2Deg * rotation); // rotation
        float sinZ = Mathf.Sin(Mathf.Rad2Deg * rotation);
        Matrix3x3 rotationMatrix = new Matrix3x3(cosZ, -sinZ, 0, sinZ, cosZ, 0, 0, 0, 1); // counter clockwise rotation
        Matrix3x3 scalingMatrix = new Matrix3x3(Scale.x, 0, 0, 0, Scale.y, 0, 0, 0, 1); // scaling
        Matrix3x3 translateMatrix = new Matrix3x3(1, 0, 0, 0, 1, 0, Offset.x, Offset.y, 1); // translation
        Matrix3x3 newtrs = rotationMatrix * scalingMatrix;

        if (activateUpdate)
        {
            for (int i = 0; i < uv.Length; i++)
            {
                uv[i] += inverseT;
                uv[i] = Matrix3x3.MultiplyVector2(inverseTRS,uv[i]);
                uv[i] = Matrix3x3.MultiplyVector2(newtrs, uv[i]);
                uv[i] += Offset;
            }

            theMesh.uv = uv;
            inverseT = -Offset;
            activateUpdate = false;
            inverseTRS = newtrs.Invert(); // save for later use
        }
    }

    public void SetTRS(ref Matrix3x3 newTRS)
    {
        trs = newTRS;

        Mesh theMesh = GetComponent<MeshFilter>().mesh;
        Vector2[] uv = mInitUV;
        for (int i = 0; i < uv.Length; i++)
        {
            uv[i].x = mInitUV[i].x * Scale.x;
            uv[i].y = mInitUV[i].y * Scale.y;
            uv[i].x = mInitUV[i].x * Mathf.Cos(rotation);
            uv[i].y = mInitUV[i].y * Mathf.Sin(rotation);
            uv[i] = Offset + uv[i];
            //uv[i] = Matrix3x3.MultiplyVector2(newTRS, uv[i]);
        }
        theMesh.uv = uv;
    }

    public Matrix3x3 GetTRS()
    {
        return trs;
    }
}
