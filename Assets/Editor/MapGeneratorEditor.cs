using UnityEngine;
using System.Collections;
using UnityEditor;

//<Summary>
// custom editor, the type of class it is a custom editor of is "MapGenerator" class
//<Summary>
[CustomEditor(typeof(MapGenerator))]

//<Summary>
// Call from the editor and extend from the editor
//<Summary>
public class MapGeneratorEditor : Editor
{
    //<Summary>
    // Override on inspector GUI method
    //<Summary>
    public override void OnInspectorGUI()
    {
        //<Summary>
        // Reference to the map generator
        //<Summary>
        MapGenerator mapGen = (MapGenerator)target;

        //<Summary>
        // If any value changes within the GUI values automatically update the noise map in the editor
        //<Summary>
        if (DrawDefaultInspector())
        {
            if (mapGen.autoUpdate)
            {
                mapGen.GenerateMap();
            }
        }
        //<Summary>
        // Add button to the GUI, if pressed generate map
        //<Summary>
        if (GUILayout.Button("Generate"))
        {
            mapGen.GenerateMap();
        }
    }
}
