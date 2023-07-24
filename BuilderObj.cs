using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderObj : MonoBehaviour
{
    public Material mat;
    public static String FormatNumber(int number)
    {
        String temp = "";
        if (number < 0)
            temp += '-';
        else
            temp += '+';
        int numStrLen = Math.Abs(number).ToString().Length;
        temp += new String('0', 3 - numStrLen);
        temp += Math.Abs(number);
        Console.WriteLine(number);
        return temp;
    }

    void Start()
    {
        int counter = 0;
        GameObject parent = new GameObject("GeneratedObjects");

        for (int sectionX = -10; sectionX <= 10; sectionX++)
        {
            for (int sectionY = -10; sectionY <= 10; sectionY++)
            {
                GameObject sectionParent = new GameObject("Section_" + sectionX + "_" + sectionY);
                sectionParent.transform.SetParent(parent.transform);

                for (int blockX = 0; blockX <= 3; blockX++)
                {
                    for (int blockY = 0; blockY <= 3; blockY++)
                    {
                        GameObject blockParent = new GameObject("Block_" + blockX + "_" + blockY);
                        blockParent.transform.SetParent(sectionParent.transform);

                        for (int chunkX = 0; chunkX <= 3; chunkX++)
                        {
                            for (int chunkY = -0; chunkY <= 3; chunkY++)
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

                                    go.transform.SetParent(blockParent.transform);
                                }
                            }
                        }
                    }
                }
            }
        }
        Debug.Log(counter);
    }

    void Update()
    {
    }
}
