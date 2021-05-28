using UnityEngine;
using System.Collections;


public static class MeshGenerator
{
    //<Summary>
    //  Generate terrain mesh with the argument of the 2D float array and height map
    //<Summary>
    public static MeshData GenerateTerrainMesh(float[,] heightMap, float heightMultiplier, AnimationCurve heightCurve, int levelOfDetail)
    {
        //<Summary>
        // Find out width and height of height map
        //<Summary>
        int width = heightMap.GetLength (0);
        int height = heightMap.GetLength (1);

        //<Summary>
        // calculate floats for each point of the vertices
        //<Summary>
        float topLeftX = (width - 1) / -2f;
        float topLeftZ = (height - 1) / 2f;

        //<Summary>
        // If level of detail = 0, mesh simplication = 1 otherwise for loops will not work, work out number or vertices per line
        //<Summary>
        int meshSimplificationIncrement = (levelOfDetail == 0) ?1: levelOfDetail * 2;
        int verticesPerLine = (width - 1) / meshSimplificationIncrement + 1;

        //<Summary>
        // Create mesh data variable and calculate vertices per line, set integer of vertexindex = 0
        //<Summary>
        MeshData meshData = new MeshData(verticesPerLine, verticesPerLine);
        int vertexIndex = 0;

        //<Summary>
        // Loop through height map
        //<Summary>
        for (int y =0; y < height; y += meshSimplificationIncrement)
        {
            for (int x = 0; x < width; x += meshSimplificationIncrement)
            {
                //<Summary>
                // Centre all 3 points of the vertices and uvs, value on x axis of curve, respons corresponding y value
                //<Summary>
                meshData.vertices [vertexIndex] = new Vector3 (topLeftX + x, heightCurve.Evaluate(heightMap [x, y]) * heightMultiplier, topLeftZ - y);
                meshData.uvs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);

                //<Summary>
                // adds triangles into the map and ignores outside numbers as adding triangles to those would add triangles past the edges/barriers
                //<Summary>
                if (x < width-1 && y < height - 1)
                {
                    meshData.AddTriangles(vertexIndex, vertexIndex + verticesPerLine + 1, vertexIndex + verticesPerLine);
                    meshData.AddTriangles(vertexIndex + verticesPerLine + 1, vertexIndex, vertexIndex + 1);
                }

                //<Summary>
                // At the end of each loop incrememnt by 1
                //<Summary>
                vertexIndex++;
            }

        }

        
        return meshData;
    
    }


}
//<Summary>
// Public 3D vector array for vertices and triangles
//<Summary>
public class MeshData
{
    public Vector3[] vertices;
    public int[] triangles;
    public Vector2[] uvs;

    //<Summary>
    // Keep track of current index of triangles array
    //<Summary>
    int triangleIndex;

    //<Summary>
    // Constructor for the vertices, triangles and uvs for each vertex
    //<Summary>
    public MeshData(int meshWidth, int meshHeight)
    {
        vertices = new Vector3[meshWidth * meshHeight];
        uvs = new Vector2[meshWidth * meshHeight];
        triangles = new int[(meshWidth - 1)*(meshHeight-1)*6];
    }
    //<Summary>
    // Method for adding triangles
    //<Summary>
    public void AddTriangles(int a, int b, int c)
    {
        triangles[triangleIndex] = a;
        triangles[triangleIndex+1] = b;
        triangles[triangleIndex+2] = c;

        //<Summary>
        // Increment triangle index by 3
        //<Summary>
        triangleIndex += 3;
    }
    //<Summary>
    // Create method for getting mesh from mesh data
    //<Summary>
    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        return mesh;
    }

}

