using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(JulienTerrainTools))]
public class JulienTerrainToolsInspector : Editor
{
    // internal variables
    Vector2 scrollPosition = new Vector2(0, 0);
    bool ToggleSplat = true;
    bool ToggleTrees = true;
    bool ToggleGrass = true;
    bool ToggleOverlay;
    bool ToggleMass;

    private Object MassMap;
    private bool DefaultsDone = false;

    public override void OnInspectorGUI()
    {
        JulienTerrainTools terrainTools = target as JulienTerrainTools;
        Terrain terrain = terrainTools.GetComponent<Terrain>();

        EditorGUILayout.BeginVertical(GUI.skin.box, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

        EditorGUILayout.Separator();

        ToggleSplat = EditorGUILayout.Foldout(ToggleSplat, "Texturing");
        if (ToggleSplat)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("First Splatmap");
            GUILayout.FlexibleSpace();
            terrainTools.SplatA = (Texture2D)EditorGUILayout.ObjectField("", terrainTools.SplatA, typeof(Texture2D));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Second Splatmap (optional)");
            GUILayout.FlexibleSpace();
            terrainTools.SplatB = (Texture2D)EditorGUILayout.ObjectField("", terrainTools.SplatB, typeof(Texture2D));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Apply Splatmap(s)"))
            {
                if (terrainTools.CheckSplatmap()) terrainTools.ApplySplatmap();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        EditorGUILayout.Separator();

        ToggleTrees = EditorGUILayout.Foldout(ToggleTrees, "Tree Distribution");
        if (ToggleTrees)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Tree map");
            GUILayout.FlexibleSpace();
            terrainTools.treemap = (Texture2D)EditorGUILayout.ObjectField("", terrainTools.treemap, typeof(Texture2D));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Clear Existing Trees?");
            GUILayout.FlexibleSpace();
            terrainTools.ResetTrees = EditorGUILayout.Toggle("remove trees", terrainTools.ResetTrees);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Tree Density");
            GUILayout.FlexibleSpace();
            terrainTools.TreeDensity = EditorGUILayout.Slider(terrainTools.TreeDensity, 0.1f, 1f);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Threshold");
            GUILayout.FlexibleSpace();
            terrainTools.TreeThreshold = EditorGUILayout.Slider(terrainTools.TreeThreshold, 0.01f, 0.99f);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Tree Size");
            GUILayout.FlexibleSpace();
            terrainTools.TreeSize = EditorGUILayout.Slider(terrainTools.TreeSize, 0.2f, 5f);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Size Variation");
            GUILayout.FlexibleSpace();
            terrainTools.SizeVariation = EditorGUILayout.Slider(terrainTools.SizeVariation, 0f, 1f);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Generate Trees"))
            {
                if (terrainTools.CheckTreemap()) terrainTools.ApplyTreemap();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

        }

        EditorGUILayout.Separator();

        ToggleGrass = EditorGUILayout.Foldout(ToggleGrass, "Grass and Details");
        if (ToggleGrass)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Grass map");
            GUILayout.FlexibleSpace();
            terrainTools.grassmap = (Texture2D)EditorGUILayout.ObjectField("", terrainTools.grassmap, typeof(Texture2D));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Bush/Detail map");
            GUILayout.FlexibleSpace();
            terrainTools.bushmap = (Texture2D)EditorGUILayout.ObjectField("", terrainTools.bushmap, typeof(Texture2D));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Grass Density");
            GUILayout.FlexibleSpace();
            terrainTools.grassmod = EditorGUILayout.Slider(terrainTools.grassmod, 0.1f, 3f);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Grass Clumping");
            GUILayout.FlexibleSpace();
            terrainTools.grassclumping = EditorGUILayout.Slider(terrainTools.grassclumping, 0f, 1f);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Bush/Detail Density");
            GUILayout.FlexibleSpace();
            terrainTools.bushmod = EditorGUILayout.Slider(terrainTools.bushmod, 0.1f, 2f);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Generate Grass and Details"))
            {
                if (terrainTools.CheckGrassmap()) terrainTools.ApplyGrassmap();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        EditorGUILayout.Separator();

        ToggleOverlay = EditorGUILayout.Foldout(ToggleOverlay, "Overlays (roads, rivers, etc)");
        if (ToggleOverlay)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Overlay map");
            GUILayout.FlexibleSpace();
            terrainTools.OverlayMap = (Texture2D)EditorGUILayout.ObjectField("", terrainTools.OverlayMap, typeof(Texture2D));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Threshold");
            GUILayout.FlexibleSpace();
            terrainTools.OverlayThreshold = EditorGUILayout.Slider(terrainTools.OverlayThreshold, 0.1f, 0.9f);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Overlay texture");
            GUILayout.FlexibleSpace();
            terrainTools.OverlayTexture = (Texture2D)EditorGUILayout.ObjectField("", terrainTools.OverlayTexture, typeof(Texture2D));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Tile size");
            GUILayout.FlexibleSpace();
            terrainTools.TileSize = EditorGUILayout.IntSlider(terrainTools.TileSize, 3, 127);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Clear trees");
            GUILayout.FlexibleSpace();
            terrainTools.ClearTrees = EditorGUILayout.Toggle("", terrainTools.ClearTrees);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Clear tree radius");
            GUILayout.FlexibleSpace();
            terrainTools.ClearRadius = EditorGUILayout.Slider(terrainTools.ClearRadius, 0.5f, 10.0f);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Clear grass/detail");
            GUILayout.FlexibleSpace();
            terrainTools.ClearGrass = EditorGUILayout.Toggle("", terrainTools.ClearGrass);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Raise/lower terrain");
            GUILayout.FlexibleSpace();
            terrainTools.ChangeTerrain = EditorGUILayout.Slider(terrainTools.ChangeTerrain, -50f, 50f);
            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Generate Overlay"))
            {
                if (terrainTools.CheckOverlaymap()) terrainTools.ApplyOverlaymap();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }
}
