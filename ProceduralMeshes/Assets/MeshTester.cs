using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshTester : MonoBehaviour {

    const int RESOLUTION_X = 100;
    const int RESOLUTION_Z = 100;
    const float SCALE_Y = 0.5f;

    public Vector3 origin = new Vector3(0, 0, 0);
    public float sizeX = 5.0f;
    public float sizeZ = 5.0f;

    MeshBuilder builder;

    void Start() {
        builder = new MeshBuilder();

        builder.SetFunction(MyFunction, Time.time, RESOLUTION_X, RESOLUTION_Z, origin, sizeX / (float)RESOLUTION_X, SCALE_Y, sizeZ / (float)RESOLUTION_Z);
        Mesh mesh = builder.BakeMesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    void Update()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        builder.UpdateMeshFunction(ref mesh, Time.time);
    }

    float MyFunction(float x, float z, float time)
    {
        // sine waves
        //return (Mathf.Sin(x/20f * Mathf.PI) * Mathf.Sin(z/50f * Mathf.PI)) * Mathf.Cos(time / 0.5f) + 0.2f;

        // discretized wave equation
        // http://www.mtnmath.com/whatrh/node66.html

        return (Mathf.Sin(x / 100f * Mathf.PI) * Mathf.Sin(z / 50f * Mathf.PI) + Mathf.Min(x, z) / 50f) * Mathf.Cos(time / 0.5f) + 0.2f;
    }

    public void UpdateScale(float sizeX, float sizeZ)
    {
        this.sizeX = sizeX;
        this.sizeZ = sizeZ;
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        builder.UpdateScale(sizeX / (float)RESOLUTION_X, SCALE_Y, sizeZ / (float)RESOLUTION_Z);
    }

    public void UpdateOrigin(Vector3 origin)
    {
        this.origin = origin;
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        builder.UpdateOrigin(origin);
    }
}
