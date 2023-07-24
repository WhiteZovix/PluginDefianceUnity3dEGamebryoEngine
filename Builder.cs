using System;
using System.Text;
using UnityEngine;

public class Builder : MonoBehaviour
{
    public Material mat;

    private const int ChunkCount = 3;
    private const int SectionCount = 21;

    public static string FormatNumber(int number)
    {
        StringBuilder temp = new StringBuilder();
        temp.Append(number < 0 ? '-' : '+');

        int absNumber = Math.Abs(number);
        temp.Append(new string('0', 3 - absNumber.ToString().Length));
        temp.Append(absNumber);

        return temp.ToString();
    }

    private void Start()
    {
        CombineInstance[] combine = new CombineInstance[ChunkCount * ChunkCount * SectionCount * SectionCount];
        int counter = 0;

        for (int sectionX = -10; sectionX <= 10; sectionX++)
        {
            for (int sectionY = -10; sectionY <= 10; sectionY++)
            {
                for (int blockX = 0; blockX <= ChunkCount; blockX++)
                {
                    for (int blockY = 0; blockY <= ChunkCount; blockY++)
                    {
                        for (int chunkX = 0; chunkX <= ChunkCount; chunkX++)
                        {
                            for (int chunkY = -0; chunkY <= ChunkCount; chunkY++)
                            {
                                Mesh m = Resources.Load<Mesh>(
                                    "DFF/dtc_zon_[Channel2_zone_[" + FormatNumber(sectionX) + FormatNumber(sectionY) + "]]_client_terrain_[" + blockX + "_" + blockY + "]_[" + chunkX +
                                    "_" + chunkY + "].");

                                if (m != null)
                                {
                                    counter++;

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
        Debug.Log(counter);
    }

    private void Update()
    {
    }
}
