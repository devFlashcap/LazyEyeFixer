using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    public GameObject[] Blocks;
    public GameObject[] BlockShadows;
    private static Queue<int> _blockQueue = new Queue<int>();
    private static int[] _blocksIndexes;
    private static int _currentBlockIndex;
    public GameObject ShadowBlock;
    public GameObject CurrentBlock;
    private static int _heldBlockIndex;
    private bool? _isCurrentBlockVisible;

    void Start()
    {
        _blocksIndexes = Enumerable.Range(0, Blocks.Length).ToArray();
        _heldBlockIndex = -1;
        _isCurrentBlockVisible = null;

        GenerateNewBag(2);
        SpawnNext();
    }

    public void SpawnNext(bool onHold = false)
    {
        _currentBlockIndex = GetNextBlock(onHold);
        if (_blockQueue.Count == 7)
        {
            GenerateNewBag(1);
        }

        CurrentBlock = Instantiate(Blocks[_currentBlockIndex], transform.position, Quaternion.identity);
        ShadowBlock = Instantiate(BlockShadows[_currentBlockIndex], transform.position, Quaternion.identity);
        SetCurrentBlockVisible(false);
    }

    public void SetCurrentBlockVisible(bool isVisible)
    {
        if (_isCurrentBlockVisible != isVisible)
        {
            foreach (var renderer in CurrentBlock.GetComponentsInChildren<Renderer>())
            {
                renderer.enabled = isVisible;
            }

            foreach (var renderer in ShadowBlock.GetComponentsInChildren<Renderer>())
            {
                renderer.enabled = isVisible;
            }
            _isCurrentBlockVisible = isVisible;
        }
    }

    private void GenerateNewBag(int numberOfBags)
    {
        for(int i = 0; i < numberOfBags; i++)
        {
            _blocksIndexes.Shuffle();
            foreach (var item in _blocksIndexes)
            {
                _blockQueue.Enqueue(item);
            }
        }
    }

    public GameObject GetCurrentBlock()
    {
        return Blocks[_currentBlockIndex];
    }

    public void DestroyShadow()
    {
        Destroy(ShadowBlock);
        ShadowBlock = null;
    }

    private int GetNextBlock(bool onHold)
    {
        if(onHold)
        {
            int prevHeldBlockIndex = _heldBlockIndex;
            _heldBlockIndex = _currentBlockIndex;
            Destroy(CurrentBlock);
            DestroyShadow();
            return prevHeldBlockIndex != -1 ? prevHeldBlockIndex : _blockQueue.Dequeue();
        }
        else
        {
            return _blockQueue.Dequeue();
        }
    }
}
