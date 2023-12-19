using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ContourMap : MonoBehaviour
{
    // Creates contour map from terrain heightmap data as Texture2D

    // use default colours and optional parameter numberOfBands
    public static Texture2D FromTerrain(Terrain terrain, int numberOfBands = 12)
    {
        return FromTerrain(terrain, numberOfBands, Color.white, Color.clear);
    }

    // define all parameters
    public static Texture2D FromTerrain(Terrain terrain, int numberOfBands, Color bandColor, Color bkgColor)
    {
        //color gradient setup
        Gradient gradient = new Gradient();
        GradientColorKey[] colors = new GradientColorKey[2];
        //time is the value to color key, just need to normalize the band number to a value between 0 and 1.
        GradientAlphaKey[] alphas = new GradientAlphaKey[2];

        //set low color and high color.
        colors[0] = new GradientColorKey(Color.green, 0.0f);
        colors[1] = new GradientColorKey(Color.red, 1.0f);

        //no need for alpha changes yet
        alphas[0] = new GradientAlphaKey(1.0f, 0.0f);
        alphas[1] = new GradientAlphaKey(1.0f, 1.0f);

        gradient.SetKeys(colors, alphas);

        // dimensions
        int width = terrain.terrainData.heightmapResolution;
        int height = terrain.terrainData.heightmapResolution;

        // heightmap data
        float[,] heightmap = terrain.terrainData.GetHeights(0, 0, width, height);

        // Create Output Texture2D with heightmap dimensions
        Texture2D topoMap = new Texture2D(width, height);
        topoMap.anisoLevel = 16;

        // array for storing colours to be applied to texture
        Color[] colourArray = new Color[width * height];

        // Set background
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                colourArray[(y * width) + x] = bkgColor;
            }
        }

        // Initial Min/Max values for normalized terrain heightmap values
        float minHeight = 1f;
        float maxHeight = 0;

        // Find lowest and highest points
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (minHeight > heightmap[y, x])
                {
                    minHeight = heightmap[y, x];
                }
                if (maxHeight < heightmap[y, x])
                {
                    maxHeight = heightmap[y, x];
                }
            }
        }

        // Create height band list
        float bandDistance = (maxHeight - minHeight) / (float)numberOfBands; // Number of height bands to create

        //normalize number of bands to color scale.
        float colorScale = 1.0f / (float) numberOfBands;

        List<float> bands = new List<float>();

        // Get ranges
        float r = minHeight + bandDistance;
        while (r < maxHeight)
        {
            bands.Add(r);
            r += bandDistance;
        }

        // Create slice buffer
        bool[,] slice = new bool[width, height];

        // Draw bands
        for (int b = 0; b < bands.Count; b++)
        {
            // Get Slice
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (heightmap[y, x] >= bands[b])
                    {
                        slice[x, y] = true;
                    }
                    else
                    {
                        slice[x, y] = false;
                    }
                }
            }

            // Detect edges on slice and write to output
            for (int y = 1; y < height - 1; y++)
            {
                for (int x = 1; x < width - 1; x++)
                {
                    if (slice[x, y] == true)
                    {
                        if (
                            slice[x - 1, y] == false ||
                            slice[x + 1, y] == false ||
                            slice[x, y - 1] == false ||
                            slice[x, y + 1] == false)
                        {
                            // heightmap is read y,x from bottom left
                            // texture is read x,y from top left
                            // magic equation to find correct array index
                            int ind = ((height - y - 1) * width) + (width - x - 1);

                            //set color to scale in gradient
                            colourArray[ind] = gradient.Evaluate(b*colorScale);
                        }
                    }
                }
            }

        }

        // apply colour array to texture
        topoMap.SetPixels(colourArray);
        topoMap.Apply();

        // Return result
        return topoMap;
    }
}
