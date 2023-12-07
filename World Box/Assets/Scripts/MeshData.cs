using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshData
{
    // Public Fields \\
    public Vector3[] vertices;
    public int[] triangles;
    public Vector2[] uvs;

    // Private Fields \\
    private int triangleIndex;

    // Public Methods \\
    public MeshData(int _width, int _height)
    {
        vertices = new Vector3[_width * _height];
        uvs = new Vector2[_width * _height];
        triangles = new int[(_width - 1) * (_height - 1) * 6];
    }

    public void AddTriangle(int _a, int _b, int _c)
    {
        triangles[triangleIndex] = _a;
        triangles[triangleIndex + 1] = _b;
        triangles[triangleIndex + 2] = _c;

        triangleIndex += 3;
    }

    public Mesh CreateMesh()
    {
        Mesh newMesh = new Mesh();

        newMesh.vertices = vertices;
        newMesh.triangles = triangles;
        newMesh.uv = uvs;

        newMesh.RecalculateNormals();

        return newMesh;
    }
}