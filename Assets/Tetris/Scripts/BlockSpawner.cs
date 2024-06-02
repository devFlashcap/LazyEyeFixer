using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    public GameObject[] Blocks;
    private static Queue<int> blockQueue = new Queue<int>();
    private static int QUEUE_SIZE = 5;

    void Start()
    {
        for(int i = 0; i < QUEUE_SIZE; i++)
        {
            blockQueue.Enqueue(Random.Range(0, Blocks.Length));
        }

        SpawnNext();
    }

    void Update()
    {
        
    }

    public void SpawnNext()
    {
        int nextBlockIndex = blockQueue.Dequeue();
        Instantiate(Blocks[nextBlockIndex], transform.position, Quaternion.identity);

        blockQueue.Enqueue(Random.Range(0, Blocks.Length));
    }
}
