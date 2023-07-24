using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

public class YourEditorScript : EditorWindow
{
    private List<string> filePathsToLoad = new List<string>();
    private bool isLoading = false;
    private GameObject terrainPrefab; // Make sure to assign a terrain prefab in the Inspector.
    private Vector2 scrollPos; // Move the declaration of scrollPos here.
    private int maxFilesToSelect = 10; // Set the maximum number of files to select here.

    [MenuItem("DefianceGame/Builder Terrain Objects")]
    public static void ShowWindow()
    {
        GetWindow<YourEditorScript>("Builder Terrain Objects");
    }

    private void OnGUI()
    {
        GUILayout.Label("Builder Terrain Objects", EditorStyles.boldLabel);

        if (GUILayout.Button("Select .ini Files") && !isLoading)
        {
            isLoading = true;
            filePathsToLoad.Clear();

            int fileCount = 0;
            while (fileCount < 15)
            {
                string iniPath = EditorUtility.OpenFilePanel("Select .ini File", "", "ini");
                if (!string.IsNullOrEmpty(iniPath))
                {
                    filePathsToLoad.Add(iniPath);
                    fileCount++;
                }
                else
                {
                    // User canceled the selection or reached the maximum of 15 files, exit the loop.
                    break;
                }
            }

            // Load the terrain prefab here
            terrainPrefab = LoadTerrainPrefab(); // Implement the function to load the terrain prefab

            EditorApplication.update += LoadObjectsUpdate;
        }
    }

    private GameObject LoadTerrainPrefab()
    {
        // Implement the code to load the terrain prefab here
        // You can load it from Resources, or use AssetDatabase.LoadAssetAtPath, etc.
        // Example:
        return Resources.Load<GameObject>("E:\\Defiance\\UnityProject\\DefianceGames\\DefianceGame\\Assets\\Resources\\Terrain.prefab");
    }

    private void LoadObjectsUpdate()
    {
        if (filePathsToLoad.Count > 0)
        {
            string filePath = filePathsToLoad[0];
            filePathsToLoad.RemoveAt(0);

            string iniFile = System.IO.File.ReadAllText(filePath);
            string[] lines = iniFile.Split('\n');

            CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            ci.NumberFormat.CurrencyDecimalSeparator = ".";

            bool finished = false;
            string mes = "";
            string cmp = "";
            double posX = 0;
            double posY = 0;
            double posZ = 0;
            double rotX = 0;
            double rotY = 0;
            double rotZ = 0;
            double scale = 0;

            foreach (string line in lines)
            {
                if (line.StartsWith("["))
                {
                    finished = false;
                }
                else if (line.Contains("="))
                {
                    string[] parts = line.Split('=');
                    string key = parts[0].Trim();
                    string _value = parts[1].Trim();

                    UnityEngine.Debug.Log(key + "=" + _value);

                    switch (key)
                    {
                        case "mes":
                            mes = _value;
                            break;
                        case "cmp":
                            cmp = _value;
                            break;
                        case "posX":
                            posX = double.Parse(_value, NumberStyles.Any, ci);
                            break;
                        case "posY":
                            posY = double.Parse(_value, NumberStyles.Any, ci);
                            break;
                        case "posZ":
                            posZ = double.Parse(_value, NumberStyles.Any, ci);
                            break;
                        case "rotX":
                            rotX = double.Parse(_value, NumberStyles.Any, ci);
                            break;
                        case "rotY":
                            rotY = double.Parse(_value, NumberStyles.Any, ci);
                            break;
                        case "rotZ":
                            rotZ = double.Parse(_value, NumberStyles.Any, ci);
                            break;
                        case "scale":
                            scale = double.Parse(_value, NumberStyles.Any, ci);
                            finished = true;
                            break;
                    }

                    if (finished)
                    {
                        GameObject prefab = Resources.Load<GameObject>("Objects/Mes/temp/" + mes);

                        if (prefab != null)
                        {
                            GameObject _obj = Instantiate(prefab);

                            _obj.transform.position = new Vector3((float)posX, (float)posY, (float)posZ);
                            _obj.transform.rotation = Quaternion.Euler((float)rotX, (float)rotY, (float)rotZ);
                            _obj.transform.localScale = new Vector3((float)scale, (float)scale, (float)scale);

                            // Attach the object to the terrain prefab
                            AttachObjectToTerrainPrefab(_obj);

                            _obj.AddComponent<MeshCollider>();
                        }
                    }
                }
            }
        }
        isLoading = false;
        EditorApplication.update -= LoadObjectsUpdate;
    }

    private void AttachObjectToTerrainPrefab(GameObject obj)
    {
        // Attach the object to the terrain prefab using the terrainPrefab GameObject
        // Similar to attaching it to the active Terrain in the previous function
        if (terrainPrefab != null)
        {
            Terrain terrain = terrainPrefab.GetComponent<Terrain>();
            if (terrain != null)
            {
                Vector3 position = obj.transform.position;
                position.y = terrain.SampleHeight(obj.transform.position);
                obj.transform.position = position;
            }
        }
    }

    public static string FormatNumber(int number)
    {
        string temp = "";
        if (number < 0)
            temp += '-';
        else
            temp += '+';
        int numStrLen = Math.Abs(number).ToString().Length;
        temp += new String('0', 3 - numStrLen);
        temp += Math.Abs(number);
        return temp;
    }
}
