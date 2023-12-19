using UnityEngine;
using System.Collections;
using UnityEngine.Windows;


public class ContourLines : MonoBehaviour
{
    public Terrain terrain;

    public int numberOfBands = 12;

    public Color bandColor = Color.white;
    public Color bkgColor = Color.clear;

    public Renderer outputPlain;

    public Texture2D topoMap;
    public string outputPath;


    void Start()
    {
        GenerateTopoLines();
    }

    void GenerateTopoLines()
    {
        //topoMap = ContourMap.FromTerrain( terrain );
        //topoMap = ContourMap.FromTerrain( terrain, numberOfBands );
        topoMap = ContourMap.FromTerrain(terrain, numberOfBands, bandColor, bkgColor);

        if (outputPlain)
        {
            outputPlain.material.mainTexture = topoMap;
        }
        else
        {
            byte[] png = topoMap.EncodeToPNG();
            //Weird Unity compile error when buildig, uncomment when using at runtime.
            //File.WriteAllBytes(outputPath, png);
        }
    }
}
