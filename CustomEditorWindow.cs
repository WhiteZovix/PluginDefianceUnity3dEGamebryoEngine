using UnityEditor;
using UnityEngine;

public class CustomEditorWindow : EditorWindow
{
    public Material mat;

    [MenuItem("DefianceGame/TerrainLoader")]
    public static void ShowWindow()
    {
        GetWindow<CustomEditorWindow>("Load Terrain");
    }

    private void OnGUI()
    {
        mat = EditorGUILayout.ObjectField("Material", mat, typeof(Material), false) as Material;

        if (GUILayout.Button("Generate Terrain"))
        {
            GenerateCubes();
        }
    }

    private static string FormatNumber(int number)
    {
        string temp = "";
        if (number < 0)
            temp += '-';
        else
            temp += '+';
        int numStrLen = Mathf.Abs(number).ToString().Length;
        temp += new string('0', 3 - numStrLen);
        temp += Mathf.Abs(number);
        Debug.Log(number);
        return temp;
    }

    private void GenerateCubes()
    {
        int counter = 0;
        for (int sectionX = -10; sectionX <= 10; sectionX++)
        {
            for (int sectionY = -10; sectionY <= 10; sectionY++)
            {
                for (int blockX = 0; blockX <= 3; blockX++)
                {
                    for (int blockY = 0; blockY <= 3; blockY++)
                    {
                        for (int chunkX = 0; chunkX <= 3; chunkX++)
                        {
                            for (int chunkY = 0; chunkY <= 3; chunkY++)
                            {
                                Mesh m = Resources.Load<Mesh>(
                                    "DTC/dtc_zon_[Channel2_zone_[" + FormatNumber(sectionX) + FormatNumber(sectionY) + "]]_client_terrain_[" + blockX + "_" + blockY + "]_[" + chunkX +
                                    "_" + chunkY + "].");
                                if (m != null)
                                {
                                    counter++;
                                    //Debug.Log("dtc_zon_[Channel2_zone_[+000+000]]_client_terrain_[0_0]_[" + x + "_" + y + "].");

                                    GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                                    go.GetComponent<MeshFilter>().sharedMesh = m;
                                    go.transform.rotation = Quaternion.Euler(90, 0, 0);
                                    go.name = "{" + sectionX + ", " + sectionY + "}:[" + blockX + ", " + blockY +
                                              "]: " + chunkX + ", " + chunkY;
                                    int ox = blockX * 512 + (sectionX * 2048);
                                    int oy = blockY * -512 + (sectionY * -2048);
                                    go.transform.position = new Vector3(chunkX * 128 + ox, 0, chunkY * -128 + oy);
                                    go.GetComponent<MeshRenderer>().material = mat;
                                }
                            }
                        }
                    }
                }
            }
        }
        Debug.Log("Generated " + counter + " cubes.");
    }
}
