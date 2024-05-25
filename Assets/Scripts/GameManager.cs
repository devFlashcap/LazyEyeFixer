using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static int width = 10;
    public static int height = 24;
    private static Transform[,] grid = new Transform[width, height];

    public GameObject[] blocks;

    void Start()
    {
        SpawnNextBlock();
    }

    public void SpawnNextBlock()
    {
        int index = Random.Range(0, blocks.Length);
        Instantiate(blocks[index], new Vector3(width / 2, height-4, 0), Quaternion.identity);
    }

    public static bool IsValidPosition(Block tetromino)
    {
        foreach (Transform mino in tetromino.transform)
        {
            Vector2 pos = Round(mino.position);

            if (!InsideBorder(pos))
                return false;

            if (grid[(int)pos.x, (int)pos.y] != null)
                return false;
        }
        return true;
    }

    public static void UpdateGrid(Block block)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (grid[x, y] != null)
                {
                    if (grid[x, y].parent == block.transform)
                    {
                        grid[x, y] = null;
                    }
                }
            }
        }

        foreach (Transform blockCube in block.transform)
        {
            Vector2 pos = Round(blockCube.position);
            grid[(int)pos.x, (int)pos.y] = blockCube;
        }
    }

    public static Vector2 Round(Vector2 v)
    {
        return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
    }

    public static bool InsideBorder(Vector2 pos)
    {
        return ((int)pos.x >= 0 && (int)pos.x < width && (int)pos.y >= 0);
    }

    public static void ClearLines()
    {
        for (int y = 0; y < height; y++)
        {
            if (IsLineFull(y))
            {
                ClearLine(y);
                // After clearing a line, we need to check the same line again,
                // as lines above have been moved down.
                y--;
            }
        }
    }

    public static bool IsLineFull(int y)
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

    public static void ClearLine(int y)
    {
        // Destroy all blocks in the line
        for (int x = 0; x < width; x++)
        {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }

        // Move all lines above down
        for (int i = y; i < height - 1; i++)
        {
            for (int x = 0; x < width; x++)
            {
                if (grid[x, i + 1] != null)
                {
                    // Move the block one step down
                    grid[x, i] = grid[x, i + 1];
                    grid[x, i + 1] = null;
                    // Update the block's position
                    grid[x, i].position += new Vector3(0, -1, 0);
                }
            }
        }
    }
}
