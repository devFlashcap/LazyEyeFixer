using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Block : MonoBehaviour
{
    private float previousTime;
    public float fallTime = 0.8f;

    public static int width = 10;
    public static int height = 25;
    private static Transform[,] grid = new Transform[width, height];

    private static BlockSpawner _blockSpawner;

    private static int[,,,] wallKicks = new int[,,,]
    {
        {                                               // J, L, T, S, Z wallkicks
            { {-1, 0}, {-1, 1}, {0, -2}, {-1, -2 } },   // 0 >> 1
            { {1, 0}, {1, -1}, {0, 2}, {1, 2} },        // 1 >> 0
            { {1, 0}, {1, -1}, {0, 2}, {1, 2} },        // 1 >> 2
            { {-1, 0}, {-1, 1}, {0, -2}, {-1, -2} },    // 2 >> 1
            { {1, 0}, {1, 1}, {0, -2}, {1, -2} },       // 2 >> 3
            { {-1, 0}, {-1, -1}, {0, 2}, {-1, 2} },     // 3 >> 2
            { {-1, 0}, {-1, -1}, {0, 2}, {-1, 2} },     // 3 >> 0
            { {1, 0}, {1, 1}, {0, -2}, {1, -2} }        // 0 >> 3
        },
        {                                               // I wallkicks
            { {-2, 0}, {1, 0}, {-2, -1}, {1, 2} },      // 0 >> 1
            { {2, 0}, {-1, 0}, {2, 1}, {-1, -2} },      // 1 >> 0
            { {-1, 0}, {2, 0}, {-1, 2}, {2, -1} },      // 1 >> 2
            { {1, 0}, {-2, 0}, {1, -2}, {-2, 1} },      // 2 >> 1
            { {2, 0}, {-1, 0}, {2, 1}, {-1, -2} },      // 2 >> 3
            { {-2, 0}, {1, 0}, {-2, -1}, {1, 2} },      // 3 >> 2
            { {1, 0}, {-2, 0}, {1, -2}, {-2, 1} },      // 3 >> 0
            { {-1, 0}, {2, 0}, {-1, 2}, {2, -1} }       // 0 >> 3
        }
    };

    public Vector3 RotationPoint;

    void Start()
    {
        _blockSpawner = FindObjectOfType<BlockSpawner>();
        MoveShadow();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1, 0, 0);
            if (IsValidPosition())
            {
                MoveShadow();
            }
            else
            {
                transform.position -= new Vector3(-1, 0, 0);
            }
                
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1, 0, 0);
            if (IsValidPosition())
            {
                MoveShadow();
            }
            else
            {
                transform.position -= new Vector3(1, 0, 0);
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            TryRotate(90);
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            TryRotate(-90);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 shadowPosition = _blockSpawner.ShadowBlock.transform.position;
            _blockSpawner.DestroyShadow();
            transform.position = shadowPosition;
        }
        else if(Input.GetKeyDown(KeyCode.C))
        {
            _blockSpawner.SpawnNext(true);
        }

        if (Time.time - previousTime > (Input.GetKey(KeyCode.DownArrow) ? fallTime / 10 : fallTime) || _blockSpawner.ShadowBlock == null)
        {
            transform.position += new Vector3(0, -1, 0);
            if (!IsValidPosition())
            {
                _blockSpawner.DestroyShadow();
                transform.position -= new Vector3(0, -1, 0);
                AddToGrid();
                ClearLines();
                this.enabled = false;
                _blockSpawner.SpawnNext();
            }
            else
            {
                if(_blockSpawner.CurrentBlock.transform.position.y <= 20)
                {
                    _blockSpawner.SetCurrentBlockVisible(true);
                }
            }
            previousTime = Time.time;
        }
    }

    private bool IsValidPosition(GameObject shadow = null)
    {
        foreach (Transform child in shadow == null ? transform : shadow.transform)
        {
            int x = Mathf.RoundToInt(child.transform.position.x);
            int y = Mathf.RoundToInt(child.transform.position.y);

            if (x < 0 || x >= width || y < 0 || y >= height || grid[x, y] != null) return false;
        }

        return true;
    }

    private void AddToGrid()
    {
        foreach (Transform child in transform)
        {
            int x = Mathf.RoundToInt(child.transform.position.x);
            int y = Mathf.RoundToInt(child.transform.position.y);

            grid[x, y] = child;
        }
    }

    private void TryRotate(float rotationAngle)
    {
        string blockName = _blockSpawner.GetCurrentBlock().name;
        if (string.IsNullOrEmpty(blockName) || blockName == "BlockO") return;

        Vector3 initialPosition = transform.position;
        float initialRotationAngle = transform.rotation.eulerAngles.z;

        transform.RotateAround(transform.TransformPoint(RotationPoint), new Vector3(0, 0, 1), rotationAngle);
        if (!IsValidPosition())
        {
            Vector3 initialRotatedPosition = transform.position;
            int wallkickTypeIndex = blockName == "BlockI" ? 1 : 0;
            int wallkickIndex = GetWallKickIndex(initialRotationAngle, transform.rotation.eulerAngles.z);
            if (wallkickTypeIndex >= 0 && 
                wallkickTypeIndex < wallKicks.Length && 
                wallkickIndex >= 0 && wallkickIndex < wallKicks.GetLength(1))
            {
                bool foundValidPosition = false;
                for(int wallKickTestIndex = 0; wallKickTestIndex < wallKicks.GetLength(2); wallKickTestIndex++)
                {
                    transform.position = initialRotatedPosition + new Vector3(wallKicks[wallkickTypeIndex, wallkickIndex, wallKickTestIndex, 0], wallKicks[wallkickTypeIndex, wallkickIndex, wallKickTestIndex, 1]);
                    if (IsValidPosition())
                    {
                        MoveShadow();
                        foundValidPosition = true;
                        break;
                    }
                }
                if (!foundValidPosition)
                {
                    transform.position = initialPosition;
                    transform.RotateAround(transform.TransformPoint(RotationPoint), new Vector3(0, 0, 1), -rotationAngle);
                }
            }
        }
        else
        {
            MoveShadow();
        }
    }

    private int GetWallKickIndex(float initialRotation, float desiredRotation)
    {
        if (initialRotation == 0 && desiredRotation == 90) return 0;    // 0 >> 1
        if (initialRotation == 90 && desiredRotation == 0) return 1;    // 1 >> 0
        if (initialRotation == 90 && desiredRotation == 180) return 3;  // 1 >> 2
        if (initialRotation == 180 && desiredRotation == 90) return 4;  // 2 >> 1
        if (initialRotation == 180 && desiredRotation == 270) return 5; // 2 >> 3
        if (initialRotation == 270 && desiredRotation == 180) return 6; // 3 >> 2
        if (initialRotation == 270 && desiredRotation == 0) return 7;   // 3 >> 0
        if(initialRotation == 0 && desiredRotation == 270) return 8;    // 0 >> 3
        return -1;
    }

    private void MoveShadow()
    {
        if(_blockSpawner.ShadowBlock != null)
        {
            _blockSpawner.ShadowBlock.transform.rotation = transform.rotation;
            _blockSpawner.ShadowBlock.transform.position = transform.position;
            for (float y = _blockSpawner.ShadowBlock.transform.position.y; y >= 0; y--)
            {
                _blockSpawner.ShadowBlock.transform.position -= new Vector3(0, 1, 0);
                if (!IsValidPosition(_blockSpawner.ShadowBlock))
                {
                    _blockSpawner.ShadowBlock.transform.position += new Vector3(0, 1, 0);
                    break;
                }
            }
        }
    }

    private void ClearLines()
    {
        for (int y = 0; y < height; y++)
        {
            if (IsLineFull(y))
            {
                ClearLine(y);
                y--;
            }
        }
    }

    private bool IsLineFull(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x, y] == null)
            {
                return false;
            }
        }
        return true;
    }

    private void ClearLine(int y)
    {
        for (int x = 0; x < width; x++)
        {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }

        for (int i = y; i < height - 1; i++)
        {
            for (int x = 0; x < width; x++)
            {
                if (grid[x, i + 1] != null)
                {
                    grid[x, i] = grid[x, i + 1];
                    grid[x, i + 1] = null;
                    grid[x, i].position += new Vector3(0, -1, 0);
                }
            }
        }
    }
}
