using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBlocks : MonoBehaviour
{
    private GameObject _currentBlock;
    private string _nextBlock;

    private readonly string[] _blocks = new string[]
    {
        "Block-I",
        "Block-J",
        "Block-L",
        "Block-O",
        "Block-S",
        "Block-T",
        "Block-Z"
    };

    void Start()
    {
        var resource = Resources.Load<GameObject>(_blocks[Random.Range(0, _blocks.Length)]);
        _currentBlock = Instantiate(resource, new Vector3(-2, 10, 20), Quaternion.identity);
    }

    void Update()
    {
        
    }
}
