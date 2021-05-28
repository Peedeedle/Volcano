using UnityEngine;
using System.Collections;


public class MapGenerator : MonoBehaviour
{
    //<Summary>
    // Public integers
    // Draw the colour map and the noise map together
    //<Summary>
    public enum DrawMode {NoiseMap, ColourMap, Mesh}
    public DrawMode drawMode;

    //<Summary>
    // W = 241 - 1, W - 1 = 240
    //<Summary>
    const int mapChunkSize = 241;
    [Range(0,6)]
    public int levelOfDetail;
    public float noiseScale;

    public int octaves;

    //<Summary>
    // Persistance should always be 0-1, adds a slider
    //<Summary>
    [Range(0,1)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    //<Summary>
    // Allows the mesh to have a height value to turn it into 3D
    //<Summary>
    public float meshHeightMultiplier;
    public AnimationCurve meshHeightCurve;

    //<Summary>
    // automatically update the generated noise map if any value changes
    //<Summary>
    public bool autoUpdate;

    //<Summary>
    // Create a public array of terrain types
    //<Summary>
    public TerrainType[] regions;

    //<Summary>
    // Generate map with a specific width, height and noise scale
    //<Summary>
    public void GenerateMap ()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistance, lacunarity, offset) ;

        //<Summary>
        // Create a 1D colour map array
        //<Summary>
        Color[] colourMap = new Color[mapChunkSize * mapChunkSize];

        //<Summary>
        // Loop through noise map and find current height for regions to set colours for land, water etc 
        //<Summary>

        for (int y = 0; y < mapChunkSize; y++)
        {
            for (int x = 0; x < mapChunkSize; x++)
            {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++)
                { 
                    if (currentHeight <= regions[i].height)
                    {
                        colourMap[y * mapChunkSize + x] = regions[i].colour;
                        break;
                    }
                }

            }
        }
        //<Summary>
        // Reference map display with noise map and draw noise map with the colour map, textures and mesh. Include height in map
        //<Summary>
        MapDisplay display = FindObjectOfType<MapDisplay>();
        if (drawMode == DrawMode.NoiseMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        }
        else if (drawMode == DrawMode.ColourMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromColourMap(colourMap, mapChunkSize, mapChunkSize));
        }
        else if (drawMode == DrawMode.Mesh)
        {
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve, levelOfDetail), TextureGenerator.TextureFromColourMap(colourMap, mapChunkSize, mapChunkSize));
        }
           
    }
    //<Summary>
    // Automatically whenever one of the script variables is changed to a value which is not needed change back to a certain value
    //<Summary>
    void OnValidate() 
    { 
        if (lacunarity < 1) 
        {
            lacunarity = 1;
        }
        if (octaves < 0) 
        {
            octaves = 0;       
        }

    }

}
//<Summary>
// Allows it to be seen in the inspector
//<Summary>
[System.Serializable]
//<Summary>
// Gives the noise map terrain types allowing the use of height to be used to create different colours
//<Summary>
public struct TerrainType {
    public string name;
    public float height;
    public Color colour;
}
