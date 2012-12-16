using UnityEditor;
using UnityEngine;
using System.Collections;
using System.IO;

//A helper script for setting up terrain splat maps and other features

[RequireComponent(typeof(Terrain))]
public class JulienTerrainTools : MonoBehaviour
{
    public Texture2D[] SplatTex = new Texture2D[4];
    public GameObject[] TreeObj = new GameObject[3];
    public Texture2D GrassTex;

    public Texture2D SplatA;
    public Texture2D SplatB;

    public Texture2D treemap;
    public bool ResetTrees = true;
    public float TreeDensity = 0.5f;
    public float TreeThreshold = 0.2f;
    public float TreeSize = 1f;
    public float SizeVariation = 0.2f;

    public Texture2D grassmap;
    public Texture2D bushmap;
    public float grassmod = 1.0f;
    public float grassclumping = 0.5f;
    public float bushmod = 0.25f;

    public Texture2D OverlayMap;
    public float OverlayThreshold = 0.1f;
    public Texture2D OverlayTexture;
    public int TileSize = 15;
    public bool ClearTrees = false;
    public float ClearRadius = 1.0f;
    public bool ClearGrass = true;
    public float ChangeTerrain = 0.0f;

    void FixFormat(Texture2D texture)
    {
        string path = AssetDatabase.GetAssetPath(texture);
        TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
        if (texture.format != TextureFormat.RGB24 || !textureImporter.isReadable)
        {
            Debug.Log(path + " needs fixing");
            textureImporter.mipmapEnabled = false;
            textureImporter.isReadable = true;
            textureImporter.textureFormat = TextureImporterFormat.RGB24;
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            Debug.Log("fixed texture format for " + path);
        }
    }

    SplatPrototype[] SetupTextures()
    {
        Vector2 tilesize = new Vector2(40f, 40f);
        SplatPrototype[] Splat = new SplatPrototype[4];
        Splat[0] = new SplatPrototype();
        Splat[0].texture = SplatTex[0];
        Splat[0].tileSize = tilesize;
        Splat[1] = new SplatPrototype();
        Splat[1].texture = SplatTex[1];
        Splat[1].tileSize = tilesize;
        Splat[2] = new SplatPrototype();
        Splat[2].texture = SplatTex[2];
        Splat[2].tileSize = tilesize;
        Splat[3] = new SplatPrototype();
        Splat[3].texture = SplatTex[3];
        Splat[3].tileSize = tilesize;

        return Splat;
    }

    TreePrototype[] SetupTrees()
    {
        TreePrototype[] Trees = new TreePrototype[3];
        Trees[0] = new TreePrototype();
        Trees[0].prefab = TreeObj[0];
        Trees[1] = new TreePrototype();
        Trees[1].prefab = TreeObj[1];
        Trees[2] = new TreePrototype();
        Trees[2].prefab = TreeObj[2];
        return Trees;
    }

    DetailPrototype[] SetupGrass()
    {
        DetailPrototype[] Grass = new DetailPrototype[3];
        Grass[0] = new DetailPrototype();
        Grass[0].prototypeTexture = GrassTex;
        Grass[1] = new DetailPrototype();
        Grass[1].prototypeTexture = GrassTex;
        Grass[2] = new DetailPrototype();
        Grass[2].prototypeTexture = GrassTex;
        return Grass;
    }

    public bool CheckSplatmap()
    {
        if (SplatA == null) return false;
        FixFormat(SplatA);

        if (SplatA.height != SplatA.width)
        {
            EditorUtility.DisplayDialog("Wrong size", "First splatmap must be square (width and height must be the same)", "Cancel");
            return false;
        }
        if (Mathf.ClosestPowerOfTwo(SplatA.width) != SplatA.width)
        {
            EditorUtility.DisplayDialog("Wrong size", "Splatmap width and height must be a power of two", "Cancel");
            return false;
        }

        if (SplatB != null)
        {
            FixFormat(SplatB);
            if (SplatB.height != SplatB.width)
            {
                EditorUtility.DisplayDialog("Wrong size", "Second splatmap must be square (width and height must be the same)", "Cancel");
                return false;
            }
            if (Mathf.ClosestPowerOfTwo(SplatB.width) != SplatB.width)
            {
                EditorUtility.DisplayDialog("Wrong size", "Second splatmap width and height must be a power of two", "Cancel");
                return false;
            }
        }

        return true;
    }

    public void ApplySplatmap()
    {
        Terrain terrain = GetComponent<Terrain>();
        TerrainData terrainData = terrain.terrainData;
        if (terrainData.alphamapLayers < 4 || (SplatB != null && terrainData.alphamapLayers < 8))
        {
            EditorUtility.DisplayDialog("Missing Textures", "Please set up at least 4 (one splatmap) or 8 (two splatmaps) textures in the terrain painter dialog.", "Cancel");
            return;
        }
        Undo.RegisterUndo(terrainData, "Apply splatmap(s)");
        int w = SplatA.width;
        bool TwoMaps = false;
        if (SplatB == null)
        {
            SplatB = new Texture2D(w, w, TextureFormat.ARGB32, false);
        }
        else
        {
            TwoMaps = true;
        }
        terrainData.alphamapResolution = w;

        float[, ,] splatmapData = terrainData.GetAlphamaps(0, 0, w, w);
        Color[] splatmapColors = SplatA.GetPixels();
        Color[] splatmapColors_b = SplatB.GetPixels();
        if (!TwoMaps)
        {
            DestroyImmediate(SplatB);
            SplatB = null;
        }

        for (int y = 0; y < w; y++)
        {
            if (y % 10 == 0) EditorUtility.DisplayProgressBar("Applying splatmap", "calculating...", Mathf.InverseLerp(0.0f, (float)w, (float)y));
            for (int x = 0; x < w; x++)
            {
                float sum;
                Color col = splatmapColors[((w - 1) - x) * w + y];
                Color col_b = splatmapColors_b[((w - 1) - x) * w + y];
                if (!TwoMaps)
                {
                    sum = col.r + col.g + col.b;
                }
                else
                {
                    sum = col.r + col.g + col.b + col_b.r + col_b.g + col_b.b;
                }
                if (sum > 1.0f)
                {
                    // no final channel, and scale down
                    splatmapData[x, y, 0] = col.r / sum;
                    splatmapData[x, y, 1] = col.g / sum;
                    splatmapData[x, y, 2] = col.b / sum;
                    if (!TwoMaps)
                    {
                        splatmapData[x, y, 3] = 0.0f;
                    }
                    else
                    {
                        splatmapData[x, y, 3] = col_b.r / sum;
                        splatmapData[x, y, 4] = col_b.g / sum;
                        splatmapData[x, y, 5] = col_b.b / sum;
                        splatmapData[x, y, 6] = 0.0f;
                    }
                }
                else
                {
                    // derive final channel from black
                    splatmapData[x, y, 0] = col.r;
                    splatmapData[x, y, 1] = col.g;
                    splatmapData[x, y, 2] = col.b;
                    if (!TwoMaps)
                    {
                        splatmapData[x, y, 3] = 1.0f - sum;
                    }
                    else
                    {
                        splatmapData[x, y, 3] = col_b.r;
                        splatmapData[x, y, 4] = col_b.g;
                        splatmapData[x, y, 5] = col_b.b;
                        splatmapData[x, y, 6] = 1.0f - sum;
                    }
                }
            }
        }
        EditorUtility.ClearProgressBar();

        terrainData.SetAlphamaps(0, 0, splatmapData);
        Debug.Log("Splatmaps applied.");
    }


    public bool CheckTreemap()
    {
        if (treemap == null) return false;
        FixFormat(treemap);

        if (treemap.height != treemap.width)
        {
            EditorUtility.DisplayDialog("Wrong size", "treemap width and height must be the same", "Cancel");
            return false;
        }
        if (Mathf.ClosestPowerOfTwo(treemap.width) != treemap.width)
        {
            EditorUtility.DisplayDialog("Wrong size", "treemap width and height must be a power of two", "Cancel");
            return false;
        }

        if (TreeDensity < 0.1f || TreeDensity > 1.0f)
        {
            EditorUtility.DisplayDialog("Invalid Value", "Tree Density must be between 0.1 and 1.0", "Cancel");
            return false;
        }
        if (TreeThreshold < 0.0f || TreeThreshold > 1.0f)
        {
            EditorUtility.DisplayDialog("Invalid Value", "Threshold must be between 0.0 and 1.0", "Cancel");
            return false;
        }

        return true;
    }

    public void ApplyTreemap()
    {
        // set up my data
        Terrain terrain = GetComponent<Terrain>();
        TerrainData data = terrain.terrainData;
        Undo.RegisterUndo(data, "Apply tree map");

        int w = treemap.width;

        Color[] mapColors = treemap.GetPixels();

        int index = -1;
        int trees = 0;
        int Step = Mathf.RoundToInt(1.0f / TreeDensity);
        float PositionVariation = (float)(Step * 0.5f / (float)w);

        if (ResetTrees)
        {
            data.treeInstances = new TreeInstance[0];
        }

        for (int y = 1; y < w - 1; y += Step)
        {
            if (y % 10 == 0) EditorUtility.DisplayProgressBar("Placing trees", "placed " + trees + " trees so far", Mathf.InverseLerp(0.0f, (float)w, (float)y));
            for (int x = 1; x < w - 1; x += Step)
            {
                // place the chosen tree, if the colours are right
                index = -1;
                Color col = mapColors[y * w + x];
                if (col.r > TreeThreshold + Random.Range(0.0f, 1.0f))
                {
                    index = 0;
                }
                else if (col.g > TreeThreshold + Random.Range(0.0f, 1.0f))
                {
                    index = 1;
                }
                else if (col.b > TreeThreshold + Random.Range(0.0f, 1.0f))
                {
                    index = 2;
                }

                if (index >= 0)
                {
                    // place a tree
                    trees++;

                    TreeInstance instance = new TreeInstance();

                    // random placement modifier for a more natural look
                    float xpos = (float)x / (float)w; float ypos = (float)y / (float)w;
                    xpos = Mathf.Clamp01(xpos + Random.Range(-PositionVariation, PositionVariation));
                    ypos = Mathf.Clamp01(1 - ypos + Random.Range(-PositionVariation, PositionVariation));
                    instance.position = new Vector3(xpos, 0f, ypos);

                    instance.color = Color.white;
                    instance.lightmapColor = Color.white;
                    instance.prototypeIndex = index;

                    instance.widthScale = TreeSize * (1f + Random.Range(-SizeVariation, SizeVariation));
                    instance.heightScale = TreeSize * (1f + Random.Range(-SizeVariation, SizeVariation));

                    terrain.AddTreeInstance(instance);

                }
            }
        }
        EditorUtility.ClearProgressBar();
        Debug.Log("placed " + trees + " trees");
    }


    public bool CheckGrassmap()
    {
        if (grassmap != null)
        {
            FixFormat(grassmap);

            int w = grassmap.width;
            if (grassmap.height != w)
            {
                EditorUtility.DisplayDialog("Wrong size", "grassmap width and height must be the same", "Cancel");
                return false;
            }
            if (Mathf.ClosestPowerOfTwo(w) != w)
            {
                EditorUtility.DisplayDialog("Wrong size", "grassmap width and height must be a power of two", "Cancel");
                return false;
            }
        }

        if (bushmap != null)
        {
            FixFormat(bushmap);

            int w = bushmap.width;
            if (bushmap.height != w)
            {
                EditorUtility.DisplayDialog("Wrong size", "bushmap width and height must be the same", "Cancel");
                return false;
            }
            if (Mathf.ClosestPowerOfTwo(w) != w)
            {
                EditorUtility.DisplayDialog("Wrong size", "bushmap width and height must be a power of two", "Cancel");
                return false;
            }
        }

        return true;
    }

    public void ApplyGrassmap()
    {
        Terrain terrain = GetComponent<Terrain>();
        TerrainData terrainData = terrain.terrainData;
        Undo.RegisterUndo(terrainData, "Apply grass and bush maps");

        if (grassmap != null)
        {
            SetDetailmap(grassmap, grassmod, 0, grassclumping, "Grass map", terrain);
            Debug.Log("Grass map applied.");
        }
        if (bushmap != null)
        {
            SetDetailmap(bushmap, bushmod, 3, 0.0f, "Bush map", terrain);
            Debug.Log("Bush map applied.");
        }
        EditorUtility.ClearProgressBar();
    }

    void SetDetailmap(Texture2D map, float mod, int firstlayer, float clumping, string MapName, Terrain terrain)
    {
        if (!terrain) terrain = Terrain.activeTerrain;
        TerrainData data = terrain.terrainData;

        Color[] detailColors = map.GetPixels();
        int w = map.width;
        int res = data.detailResolution;

        int[,] detail_a = new int[res, res];
        int[,] detail_b = new int[res, res];
        int[,] detail_c = new int[res, res];

        float scale = (float)w / (float)res;

        for (int y = 0; y < res; y++)
        {
            EditorUtility.DisplayProgressBar("Applying " + MapName, "Calculating...", Mathf.InverseLerp(0.0f, (float)res, (float)y));
            for (int x = 0; x < res; x++)
            {
                // place detail, depending on colours in map image
                int sx = Mathf.FloorToInt((float)(x) * scale);
                int sy = Mathf.FloorToInt((float)(y) * scale);
                Color col = detailColors[((w - 1) - sx) * w + sy];

                detail_a[x, y] = DetailValue(col.r * mod);
                detail_b[x, y] = DetailValue(col.g * mod);
                detail_c[x, y] = DetailValue(col.b * mod);
            }
        }

        if (clumping > 0.01f)
        {
            detail_a = MakeClumps(detail_a, clumping);
            detail_b = MakeClumps(detail_b, clumping);
            detail_c = MakeClumps(detail_c, clumping);
        }

        data.SetDetailLayer(0, 0, firstlayer + 0, detail_a);
        data.SetDetailLayer(0, 0, firstlayer + 1, detail_b);
        data.SetDetailLayer(0, 0, firstlayer + 2, detail_c);
    }

    int DetailValue(float col)
    {
        // linearly translates a 0.0-1.0 number to a 0-16 integer
        int c = Mathf.FloorToInt(col * 16);
        float r = col * 16 - Mathf.FloorToInt(col * 16);

        if (r > Random.Range(0.0f, 1.0f)) c++;
        return Mathf.Clamp(c, 0, 16);
    }

    int[,] MakeClumps(int[,] map, float clumping)
    {
        int res = map.GetLength(0);
        int[,] clumpmap = new int[res, res];

        // init - there's probably a better way to do this in C# that I just don't know
        for (int y = 0; y < res; y++)
        {
            for (int x = 0; x < res; x++)
            {
                clumpmap[x, y] = 0;
            }
        }

        for (int y = 0; y < res; y++)
        {
            for (int x = 0; x < res; x++)
            {
                clumpmap[x, y] += map[x, y];
                if (map[x, y] > 0)
                {
                    // there's grass here, we might want to raise the grass value of our neighbours
                    for (int a = -1; a <= 1; a++) for (int b = -1; b <= 1; b++)
                        {
                            if (x + a < 0 || x + a >= res || y + b < 0 || y + b >= res) continue;
                            if (a != 0 || b != 0 && Random.Range(0.0f, 1.0f) < clumping) clumpmap[x + a, y + b]++;
                        }
                }
            }
        }

        // values above 9 yield strange results
        for (int y = 0; y < res; y++)
        {
            for (int x = 0; x < res; x++)
            {
                if (clumpmap[x, y] > 9) clumpmap[x, y] = 9;
            }
        }

        return clumpmap;
    }


    public bool CheckOverlaymap()
    {
        if (OverlayMap == null) return false;
        FixFormat(OverlayMap);

        if (OverlayMap.height != OverlayMap.width)
        {
            EditorUtility.DisplayDialog("Wrong size", "OverlayMap width and height must be the same", "Cancel");
            return false;
        }
        if (Mathf.ClosestPowerOfTwo(OverlayMap.width) != OverlayMap.width)
        {
            EditorUtility.DisplayDialog("Wrong size", "OverlayMap width and height must be a power of two", "Cancel");
            return false;
        }

        if (OverlayMap.width != GetComponent<Terrain>().terrainData.alphamapResolution)
        {
            EditorUtility.DisplayDialog("Wrong size", "OverlayMap must have same size as existing splatmap (" + GetComponent<Terrain>().terrainData.alphamapResolution + ")", "Cancel");
            return false;
        }

        return true;
    }

    public void ApplyOverlaymap()
    {
        Terrain terrain = GetComponent<Terrain>();
        if (!terrain) terrain = Terrain.activeTerrain;
        TerrainData terrainData = terrain.terrainData;
        Undo.RegisterUndo(terrainData, "Apply overlay map");
        int w = OverlayMap.width;
        Color[] OverlayMapColors = OverlayMap.GetPixels();
        int layer = terrain.terrainData.alphamapLayers;

        int detailRes = terrainData.detailWidth;
        int[] detailLayers = terrainData.GetSupportedLayers(0, 0, detailRes, detailRes);
        int LayerCount = detailLayers.Length;

        AddTexture();
        float[, ,] splatmapData = terrainData.GetAlphamaps(0, 0, w, w);

        float terrainScale = (float)w / ((float)terrainData.heightmapWidth - 1);
        float terrainHeight = terrainData.size.y;
        int terrainSample = Mathf.CeilToInt(terrainScale);

        ArrayList NewTrees = new ArrayList(terrainData.treeInstances);
        if (ClearTrees)
        {
            /*			
                        int count = 0;
                        foreach (TreeInstance tree in terrain.treeInstances) {
                            count++;
                            EditorUtility.DisplayProgressBar("Overlay map", "gathering tree data", (float)count/(float)terrain.treeInstances.Length);
                            NewTrees.Add(tree);
                        }
            */
        }
        int TreesRemoved = 0;
        for (int y = 0; y < w; y++)
        {
            if (y % 10 == 0)
            {
                if (ClearTrees)
                {
                    EditorUtility.DisplayProgressBar("Overlay map", "updating terrain and trees (" + TreesRemoved + " trees removed)", Mathf.InverseLerp(0.0f, (float)w, (float)y));
                }
                else
                {
                    EditorUtility.DisplayProgressBar("Overlay map", "updating terrain", Mathf.InverseLerp(0.0f, (float)w, (float)y));
                }
            }
            for (int x = 0; x < w; x++)
            {
                float value = OverlayMapColors[((w - 1) - x) * w + y].grayscale;
                if (value > OverlayThreshold)
                {
                    splatmapData[x, y, layer] = value;
                    // fix the other layers
                    for (int l = 0; l < layer; l++)
                    {
                        splatmapData[x, y, l] *= (1.0f - value);
                    }

                    if (ChangeTerrain > 0.01f || ChangeTerrain < -0.01f)
                    {
                        if (value > OverlayThreshold)
                        {
                            float change = ChangeTerrain * value / terrainHeight;
                            int sx = Mathf.FloorToInt((float)y * terrainScale);
                            int sy = Mathf.FloorToInt((float)x * terrainScale);
                            float[,] data = terrainData.GetHeights(sx, sy, terrainSample, terrainSample);
                            for (int a = 0; a < terrainSample; a++) for (int b = 0; b < terrainSample; b++)
                                {
                                    data[a, b] = Mathf.Max(0.0f, data[a, b] + change);
                                }
                            terrainData.SetHeights(sx, sy, data);
                        }
                    }

                    if (ClearTrees)
                    {
                        for (int i = NewTrees.Count - 1; i >= 0; i--)
                        {
                            TreeInstance MyTree = (TreeInstance)NewTrees[i];
                            float distance = Vector2.Distance(new Vector2(MyTree.position.z * w, MyTree.position.x * w), new Vector2((float)x, (float)y));
                            if (distance < ClearRadius)
                            {
                                NewTrees.RemoveAt(i);
                                TreesRemoved++;
                            }
                        }
                    }

                }
                else
                {
                    splatmapData[x, y, layer] = 0.0f;
                }
            }
        }
        if (ClearTrees)
        {
            terrainData.treeInstances = (TreeInstance[])NewTrees.ToArray(typeof(TreeInstance));
        }


        terrainData.SetAlphamaps(0, 0, splatmapData);
        Debug.Log("Overlay map applied.");

        if (ClearGrass)
        {
            float scale = (float)w / (float)detailRes;
            for (int l = 0; l < LayerCount; l++)
            {
                EditorUtility.DisplayProgressBar("Overlay map", "clearing away grass", Mathf.InverseLerp(0.0f, (float)l, (float)LayerCount));
                int[,] grass = terrainData.GetDetailLayer(0, 0, detailRes, detailRes, l);
                for (int y = 0; y < detailRes; y++)
                {
                    for (int x = 0; x < detailRes; x++)
                    {
                        int sx = Mathf.FloorToInt((float)(x) * scale);
                        int sy = Mathf.FloorToInt((float)(y) * scale);
                        float value = OverlayMapColors[((w - 1) - sx) * w + sy].grayscale;
                        if (value > OverlayThreshold && grass[x, y] > 0)
                        {
                            if (value > 0.5f) grass[x, y] = 0; else grass[x, y] = 1;
                        }
                    }
                }
                terrainData.SetDetailLayer(0, 0, l, grass);
            }
        }

        EditorUtility.ClearProgressBar();
    }

    void AddTexture()
    {
        Terrain terrain = GetComponent<Terrain>();
        SplatPrototype[] oldPrototypes = terrain.terrainData.splatPrototypes;
        SplatPrototype[] newPrototypes = new SplatPrototype[oldPrototypes.Length + 1];
        for (int x = 0; x < oldPrototypes.Length; x++)
        {
            newPrototypes[x] = oldPrototypes[x];
        }
        newPrototypes[oldPrototypes.Length] = new SplatPrototype();
        newPrototypes[oldPrototypes.Length].texture = OverlayTexture;
        Vector2 vector = new Vector2(TileSize, TileSize);
        newPrototypes[oldPrototypes.Length].tileSize = vector;
        terrain.terrainData.splatPrototypes = newPrototypes;
        EditorUtility.SetDirty(terrain);
    }



    // these are copied from UnityEditor.dll
    private void CreateTerrain(string name, int size)
    {
        TerrainData theAsset = new TerrainData();
        //		theAsset.heightmapResolution = 0x201;
        theAsset.heightmapResolution = size + 1;
        theAsset.size = new Vector3(2000f, 1000f, 2000f);
        //		AssetUtility.CreateAsset(theAsset, name);
        AssetDatabase.CreateAsset(theAsset, name + ".asset");
        //		theAsset.heightmapResolution = 0x200;
        theAsset.heightmapResolution = size;
        theAsset.baseMapResolution = 0x400;
        //		theAsset.SetDetailResolution(0x400, theAsset.detailResolutionPerPatch);
        theAsset.SetDetailResolution(0x400, 16); // recommended value from documentation
        //		AssetUtility.SaveAsset(theAsset);
        Selection.activeObject = Terrain.CreateTerrainGameObject(theAsset);
        AssetDatabase.SaveAssets();
    }


    private void ReadRaw(string path, int size)
    {
        //		Debug.Log("reading heightmap "+path+" / size = "+size);
        byte[] buffer;
        using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open, FileAccess.Read)))
        {
            buffer = reader.ReadBytes((size * size) * 2);
            reader.Close();
        }
        Terrain terrain = GetComponent<Terrain>();
        int heightmapWidth = terrain.terrainData.heightmapWidth;
        int heightmapHeight = terrain.terrainData.heightmapHeight;
        //		Debug.Log("w/h = "+heightmapWidth+" / "+heightmapHeight);
        float[,] heights = new float[heightmapHeight, heightmapWidth];
        float num3 = 1.525879E-05f;
        for (int i = 0; i < heightmapHeight; i++)
        {
            if (i % 10 == 0) EditorUtility.DisplayProgressBar("Importing heightmap", "calculating...", Mathf.InverseLerp(0.0f, (float)heightmapHeight, (float)i));
            for (int j = 0; j < heightmapWidth; j++)
            {
                int num6 = Mathf.Clamp(j, 0, size - 1) + (Mathf.Clamp(i, 0, size - 1) * size);
                byte num7 = buffer[num6 * 2];
                buffer[num6 * 2] = buffer[(num6 * 2) + 1];
                buffer[(num6 * 2) + 1] = num7;
                float num9 = System.BitConverter.ToUInt16(buffer, num6 * 2) * num3;
                heights[i, j] = num9;
            }
        }
        terrain.terrainData.SetHeights(0, 0, heights);
    }

}

