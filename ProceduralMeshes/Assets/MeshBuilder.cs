using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshBuilder {
    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();

    public delegate float Function(float x, float z, float time);

    private Function f;
    private int xMax;
    private int zMax;
    private float xScale;
    private float yScale;
    private float zScale;
    private Vector3 origin;

    public Mesh BakeMesh()
    {
        Mesh mesh = new Mesh();

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();

        mesh.MarkDynamic();
        mesh.RecalculateNormals();

        return mesh;
    }

    public void UpdateMeshFunction(ref Mesh mesh, float time)
    {
        Vector3[] kVertices = mesh.vertices;

        for (int i = 0; i < xMax; i++)
        {
            for (int j = 0; j < zMax; j++)
            {
                float y = origin.y + f(i, j, time) * yScale;
                float x = origin.x + i * xScale;
                float z = origin.z + j * zScale;

                kVertices[2 * (xMax * j + i)] = new Vector3(x, y, z);
                kVertices[2 * (xMax * j + i) + 1] = new Vector3(x, y, z);
            }
        }

        mesh.vertices = kVertices;

        mesh.RecalculateNormals();
    }

    public void UpdateScale(float xScale, float yScale, float zScale)
    {
        this.xScale = xScale;
        this.yScale = yScale;
        this.zScale = zScale;
    }

    public void UpdateOrigin(Vector3 origin)
    {
        this.origin = origin;
    }

    public void SetFunction(Function f, float time, int xMax, int zMax, Vector3 origin, float xScale, float yScale, float zScale)
    {
        this.xMax = xMax;
        this.zMax = zMax;
        this.xScale = xScale;
        this.yScale = yScale;
        this.zScale = zScale;
        this.origin = origin;
        this.f = f;

        Vector3[,] points = new Vector3[xMax * 2, zMax * 2];
        
        for (int i = 0; i < xMax; i++)
        {
            for (int j = 0; j < zMax; j++)
            {
                float y = origin.y + f(i, j, time) * yScale;
                float x = origin.x + i * xScale;
                float z = origin.z + j * zScale;

                vertices.Add(new Vector3(x, y, z));
      
                // Under side
                vertices.Add(new Vector3(x, y, z));
            }
        }

        for (int i = 0; i < xMax - 1; i++)
        {
            for (int j = 0; j < zMax - 1; j++)
            {
                triangles.Add(2 * (xMax * j + i));
                triangles.Add(2 * (xMax * j + i + 1));
                triangles.Add(2 * (xMax * (j + 1) + i + 1));

                triangles.Add(2 * (xMax * (j + 1) + i + 1));
                triangles.Add(2 * (xMax * (j + 1) + i));
                triangles.Add(2 * (xMax * j + i));

                // Under side
                triangles.Add(2 * (xMax * j + i) + 1);
                triangles.Add(2 * (xMax * (j + 1) + i + 1) + 1);
                triangles.Add(2 * (xMax * j + i + 1) + 1);

                triangles.Add(2 * (xMax * (j + 1) + i + 1) + 1);
                triangles.Add(2 * (xMax * j + i) + 1);
                triangles.Add(2 * (xMax * (j + 1) + i) + 1);
            }
        }
    }
}
