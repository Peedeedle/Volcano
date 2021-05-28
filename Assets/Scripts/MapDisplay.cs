using UnityEngine;
using System.Collections;

public class MapDisplay : MonoBehaviour 
{
    //<Summary>
    // Reference to the renderer of the "plane" to set the texture and mesh filter and renderer
    //<Summary>
    public Renderer textureRender;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;

    public void DrawTexture(Texture2D texture)
    {
        //<Summary>
        // Apply texture to texture renderer, set size of the plane to the same size as the noise map
        //<Summary>
        textureRender.sharedMaterial.mainTexture = texture;
        textureRender.transform.localScale = new Vector3(texture.width, 1, texture.height);
    }
    //<Summary>
    // Draws mesh in the map display with the given textures and mesh data
    //<Summary>
    public void DrawMesh(MeshData meshData, Texture2D texture)
    {
        meshFilter.sharedMesh = meshData.CreateMesh();
        meshRenderer.sharedMaterial.mainTexture = texture;
    }

}

