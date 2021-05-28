using UnityEngine;
using System.Collections;


public static class TextureGenerator
{
    //<Summary>
    // Create texture from 1D colour map, sharpen edges and not allow the edges to wrap to the other side of the map
    //<Summary>
    public static Texture2D TextureFromColourMap (Color[] colourMap, int width, int height)
    {
        Texture2D texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(colourMap);
        texture.Apply();
        return texture;
    }
    //<Summary>
    // Getting texture from 2D height map
    //<Summary>
    public static Texture2D TextureFromHeightMap(float[,] heightMap)
    {
        //<Summary>
        // Calculate width and height of the noise map
        //<Summary>
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        //<Summary>
        // Set colour of each pixel in the current texture, array of all the colours used to set them all at the same time as it is more time efficient
        //<Summary>
        Color[] colourMap = new Color[width * height];

        //<Summary>
        // Loop throught all the values in the noise map and set the colour map to black and white
        //<Summary>
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                //<Summary>
                // colourmap is a 1 dimentional array, set colour between black and white, give a percentage between 0-1 noise map = 0-1
                //<Summary>
                colourMap[y * width + x] = Color.Lerp(Color.black, Color.white, heightMap[x, y]);
            }
        }

        return TextureFromColourMap(colourMap, width, height);
  
    }


}
