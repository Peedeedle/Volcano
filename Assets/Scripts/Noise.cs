using UnityEngine;
using System.Collections;


public static class Noise 
{
    //<Summary>
    // Returns a 2D array of float values of integers for the map height and width etc to generate for the noise map
    //<Summary>
    public static float [,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];

        //<Summary>
        // System for random generator, assigned to seed, prng = "Pseudo random number generator"
        //<Summary>
        System.Random prng = new System.Random(seed);

        //<Summary>
        // Create and array of vector 2 between the values of -100,000 and +100,000 for an efficient random noise map
        //<Summary>
        Vector2[] octaveOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++)  {
            float offsetX = prng.Next (-100000, 100000) + offset.x;
            float offsetY = prng.Next (-100000, 100000) + offset.y;
            octaveOffsets [i] = new Vector2 (offsetX, offsetY);
        }

        //<Summary>
        // If scale is <=0 give the value of 0.0001 and a half
        //<Summary>
        if (scale <= 0)
        {
            scale = 0.0001f;
        }

        //<Summary>
        // Normalise values to be back in the range of 0-1, keep track of lowest to highest values in noise map
        //<Summary>
        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        //<Summary>
        // When scrolling through noise map in the editor allows to zoom in to the centre of the map and not the top right.
        //<Summary>
        float halfWidth = mapWidth / 2f;
        float halfHeight = mapHeight / 2f;

        //<Summary>
        // Loop through noise map
        //<Summary>
        for (int y = 0; y < mapHeight; y++) { 
        
            for (int x = 0; x < mapWidth; x++) {

                //<Summary>
                // Create frequency and amplitude variables
                //<Summary>
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                //<Summary>
                // For loop for all of the octaves
                //<Summary>
                for (int i = 0; i < octaves; i++) {

                    //<Summary>
                    // Used to sample non integer height values and * by frequency, means height values will change more rapidly
                    //<Summary>
                    float sampleX = (x-halfWidth) / scale * frequency + octaveOffsets[i].x;
                    float sampleY = (y-halfHeight) / scale * frequency + octaveOffsets[i].y;

                    //<Summary>
                    // Find the perlin noise map value and apply it to the noise map, increase noiseheight of the perlin value of each octave, create perlin value to sometimes be negative so noise height would decrease
                    //<Summary>
                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    //<Summary>
                    // At the end of each octave the amplitude gets multiplied by the persistance value which decreases each octave, frequency * lacunarity which increases each octave
                    //<Summary>
                    amplitude *= persistance;
                    frequency *= lacunarity;
                }
                //<Summary>
                // Once finished proccessing octaves, if the current noise height is greater than the current maximum noise height update max noise height = noise height otherwise if noiseheight<minimum update to new minimum noise height
                //<Summary>
                if (noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < minNoiseHeight) {
                    minNoiseHeight = noiseHeight;
                }
                //<Summary>
                // Apply noise height to the noise map
                //<Summary>
                noiseMap[x, y] = noiseHeight;
            }
        }
        //<Summary>
        // Loop through noise map
        //<Summary>
        for (int y = 0; y < mapHeight; y++) {        
            for (int x = 0; x < mapWidth; x++) {

                //<Summary>
                // For each value in the noise map set equal to these values, inverselerp returns a value between 0-1
                //<Summary>
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);    

            }
        }
                return noiseMap;
    }

}

