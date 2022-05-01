using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlatform : MonoBehaviour
{

    public List<GameObject> platforms = new List<GameObject>();
    public List<Transform> currentPlatforms = new List<Transform>();
    public int offset = 0;
    public int pSize = 29; // tamanho das plataformas

    private Transform player;
    private Transform currentPlatformEnd;
    private int platformIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        for (int i = 0; i < platforms.Count; i++)
        {
            Transform p = Instantiate(platforms[i], new Vector3(0, 0, i * pSize), transform.rotation).transform;
            currentPlatforms.Add(p);
            ShiftOffset();
        }

        currentPlatformEnd = currentPlatforms[platformIndex].GetComponent<Platform>().point;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = player.position.z - currentPlatformEnd.position.z;

        if (distance >= 2) {
            Recycle(currentPlatforms[platformIndex].gameObject);
            platformIndex = (platformIndex + 1) % currentPlatforms.Count;

            currentPlatformEnd = currentPlatforms[platformIndex].GetComponent<Platform>().point;
        }
    }

    private void ShiftOffset()
    {
        offset += pSize;
    }

    public void Recycle(GameObject platform)
    {
        platform.transform.position = new Vector3(0, 0, offset);
        ShiftOffset();
    }
}
