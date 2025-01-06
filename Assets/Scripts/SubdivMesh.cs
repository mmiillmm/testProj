using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class PlaneSubdivider : MonoBehaviour
{
    public int subdivisions = 10;
    public float size = 1f;

    void Start()
    {
        CreateSubdividedPlane();
    }

    void CreateSubdividedPlane()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        mesh.name = "Subdivided Plane";

        int numVertices = (subdivisions + 1) * (subdivisions + 1);
        Vector3[] vertices = new Vector3[numVertices];
        Vector2[] uv = new Vector2[numVertices];
        int[] triangles = new int[subdivisions * subdivisions * 6];

        float step = size / subdivisions;
        int vertIndex = 0;
        int triIndex = 0;

        for (int y = 0; y <= subdivisions; y++)
        {
            for (int x = 0; x <= subdivisions; x++)
            {
                vertices[vertIndex] = new Vector3(x * step - size / 2, 0, y * step - size / 2);
                uv[vertIndex] = new Vector2((float)x / subdivisions, (float)y / subdivisions);

                if (x < subdivisions && y < subdivisions)
                {
                    triangles[triIndex] = vertIndex;
                    triangles[triIndex + 1] = vertIndex + subdivisions + 1;
                    triangles[triIndex + 2] = vertIndex + 1;

                    triangles[triIndex + 3] = vertIndex + 1;
                    triangles[triIndex + 4] = vertIndex + subdivisions + 1;
                    triangles[triIndex + 5] = vertIndex + subdivisions + 2;

                    triIndex += 6;
                }

                vertIndex++;
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
    }
}
