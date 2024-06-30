using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BlockSpawner : MonoBehaviour 
{
    public GameObject[] Blocks;
    public GameObject[] BlockShadows;
    public Sprite[] BlockSprites;
    private static Queue<int> _blockQueue = new Queue<int>();
    private static int[] _blocksIndexes;
    private static int _currentBlockIndex;
    public GameObject ShadowBlock;
    public GameObject CurrentBlock;
    private static int _heldBlockIndex;

    public Image[] QueueImages;

    void Start()
    {
        _blocksIndexes = Enumerable.Range(0, Blocks.Length).ToArray();
        _heldBlockIndex = -1;

        GenerateNewBag(2);
        SpawnNext();
    }

    public void SpawnNext(bool onHold = false)
    {
        if(CurrentBlock != null)
        {
            SetLayer(CurrentBlock, SettingsManager.FallingBlocksSettings.FallenBlockLayer);
        }

        _currentBlockIndex = GetNextBlock(onHold);
        if (_blockQueue.Count == 7)
        {
            GenerateNewBag(1);
        }

        CurrentBlock = Instantiate(Blocks[_currentBlockIndex], transform.position, Quaternion.identity);
        SetLayer(CurrentBlock, SettingsManager.FallingBlocksSettings.FallingBlockLayer);
        ShadowBlock = Instantiate(BlockShadows[_currentBlockIndex], transform.position, Quaternion.identity);
        SetLayer(ShadowBlock, SettingsManager.FallingBlocksSettings.ShadowLayer);
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
        int nextBlockIndex = -1;
        if(onHold)
        {
            int prevHeldBlockIndex = _heldBlockIndex;
            _heldBlockIndex = _currentBlockIndex;
            Destroy(CurrentBlock);
            DestroyShadow();
            nextBlockIndex = prevHeldBlockIndex != -1 ? prevHeldBlockIndex : _blockQueue.Dequeue();
        }
        else
        {
            nextBlockIndex = _blockQueue.Dequeue();
        }

        UpdateQueueUI();
        return nextBlockIndex;
    }

    public void SetLayer(GameObject gameObject, int layer)
    {
        gameObject.layer = layer;
        foreach (Transform child in gameObject.transform)
        {
            child.gameObject.layer = layer;
        }
    }

    private void UpdateQueueUI()
    {
        if(QueueImages.Length > 0)
        {
            int[] queueBlockIndexes = _blockQueue.Take(QueueImages.Length).ToArray();
            for(int blockImageIndex = 0; blockImageIndex < QueueImages.Length; blockImageIndex++)
            {
                QueueImages[blockImageIndex].sprite = BlockSprites[queueBlockIndexes[blockImageIndex]];
            }
        }
    }
}
