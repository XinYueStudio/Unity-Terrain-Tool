using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class TerrainCustomTool : ScriptableObject
{
    static void GetFilesRecursion( List<string> FilesList, string folder, string pattern)
    {
   
        DirectoryInfo root = new DirectoryInfo(folder);
        foreach (FileInfo f in root.GetFiles())
        {
            if(f.FullName.Contains(pattern)&& !f.FullName.Contains(".meta"))
            FilesList.Add(f.FullName);
        }

        if(FilesList.Count>0)
        {
          //  Debug.Log("Find TREE!");
        }
        foreach (DirectoryInfo f in root.GetDirectories())
        {          
            GetFilesRecursion(  FilesList, f.FullName, pattern);
        }
    }

    [MenuItem("Terrain/Add Folder Tree")]
    static void AddFolderTrees()
    {
        string folder = EditorUtility.OpenFolderPanel("Select the folder containing the tree", "Assets/", "");
        if (folder != "")
        {
            if (folder.IndexOf(Application.dataPath) == -1)
            {
                Debug.LogWarning("The folder must be in this project anywhere inside the Assets folder!");
                return;
            }
           
            List<string> FilesList = new List<string>();
            GetFilesRecursion(  FilesList, folder, ".prefab");

            string[] files = FilesList.ToArray();

            if (files.Length > 0)
            {
                TerrainData currentTerrainData = Selection.activeGameObject.GetComponent<Terrain>().terrainData;
                List<TreePrototype> treePrototypesList = new List<TreePrototype>(currentTerrainData.treePrototypes);
                 
                for (int i = 0; i < files.Length; i++)
                {
                    if (files[i].Contains("plant")|| files[i].Contains("tree"))
                    {
                        TreePrototype treePrototype = new TreePrototype();

                        int index = files[i].IndexOf("Assets\\");
                        string relativePath = files[i].Substring(index);
                        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(relativePath) ;
                        if (prefab != null)
                        {
                            treePrototype.prefab = prefab;
                            treePrototypesList.Add(treePrototype);
                        }
                    }
                }
                currentTerrainData.treePrototypes = treePrototypesList.ToArray();
                Selection.activeGameObject.GetComponent<Terrain>().Flush();
                currentTerrainData.RefreshPrototypes();
                EditorUtility.SetDirty(Selection.activeGameObject.GetComponent<Terrain>());
            }
        }
    }

    [MenuItem("Terrain/Clear Tree Editor")]
    static void ClearTreeEditor()
    {
        TerrainData currentTerrainData = Selection.activeGameObject.GetComponent<Terrain>().terrainData;
        currentTerrainData.treePrototypes = null;
        Selection.activeGameObject.GetComponent<Terrain>().Flush();
        currentTerrainData.RefreshPrototypes();
        EditorUtility.SetDirty(Selection.activeGameObject.GetComponent<Terrain>());
    }

    [MenuItem("Terrain/Add Folder Tree", true)]
    static bool ValidateAddFolderTrees()
    {
        if (Selection.activeGameObject == null || Selection.activeGameObject.GetComponent<Terrain>() == null)
        {
            Debug.LogWarning("You must have a Terrain selected to perform this action!");
            return false;
        }
        return true;
    }

    [MenuItem("Terrain/Clear Tree Editor", true)]
    static bool ValidateClearTreeEditor()
    {
        if (Selection.activeGameObject == null || Selection.activeGameObject.GetComponent<Terrain>() == null)
        {
            Debug.LogWarning("You must have a Terrain selected to perform this action!");
            return false;
        }
        return true;
    }

    [MenuItem("Terrain/Add Folder TerrainLayer")]
    static void AddFolderTerrainLayer()
    {
        string folder = EditorUtility.OpenFolderPanel("Select the folder containing the tree", "Assets/", "");
        if (folder != "")
        {
            if (folder.IndexOf(Application.dataPath) == -1)
            {
                Debug.LogWarning("The folder must be in this project anywhere inside the Assets folder!");
                return;
            }

            List<string> FilesList = new List<string>();
            GetFilesRecursion(FilesList, folder, ".terrainlayer");

            string[] files = FilesList.ToArray();

            if (files.Length > 0)
            {
                TerrainData currentTerrainData = Selection.activeGameObject.GetComponent<Terrain>().terrainData;
                List<TerrainLayer> treePrototypesList = new List<TerrainLayer>(currentTerrainData.terrainLayers);

                for (int i = 0; i < files.Length; i++)
                {

                 
                        int index = files[i].IndexOf("Assets\\");
                        string relativePath = files[i].Substring(index);
                    TerrainLayer treePrototype = AssetDatabase.LoadAssetAtPath<TerrainLayer>(relativePath);
                        if (treePrototype != null)
                        {
                            
                            treePrototypesList.Add(treePrototype);
                        }
                     
                }
                currentTerrainData.terrainLayers = treePrototypesList.ToArray();
                Selection.activeGameObject.GetComponent<Terrain>().Flush();
                currentTerrainData.RefreshPrototypes();
                EditorUtility.SetDirty(Selection.activeGameObject.GetComponent<Terrain>());
            }
        }
    }

    [MenuItem("Terrain/Clear TerrainLayer Editor")]
    static void CleaTerrainLayerEditor()
    {
        TerrainData currentTerrainData = Selection.activeGameObject.GetComponent<Terrain>().terrainData;
        currentTerrainData.terrainLayers = null;
        Selection.activeGameObject.GetComponent<Terrain>().Flush();
        currentTerrainData.RefreshPrototypes();
        EditorUtility.SetDirty(Selection.activeGameObject.GetComponent<Terrain>());
    }
    [MenuItem("Terrain/Add Folder TerrainLayer", true)]
    static bool ValidateAddFolderTerrainLayer()
    {
        if (Selection.activeGameObject == null || Selection.activeGameObject.GetComponent<Terrain>() == null)
        {
            Debug.LogWarning("You must have a Terrain selected to perform this action!");
            return false;
        }
        return true;
    }

    [MenuItem("Terrain/Clear TerrainLayer Editor", true)]
    static bool ValidateCleaTerrainLayerEditor()
    {
        if (Selection.activeGameObject == null || Selection.activeGameObject.GetComponent<Terrain>() == null)
        {
            Debug.LogWarning("You must have a Terrain selected to perform this action!");
            return false;
        }
        return true;
    }

}