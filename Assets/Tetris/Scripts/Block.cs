using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Block : MonoBehaviour
{
    private float previousTime;
    public float fallTime = 0.8f;

    public static int width = 10;
    public static int height = 20;
    private static Transform[,] grid = new Transform[width, height];

    public Vector3 RotationPoint;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1, 0, 0);
            if(!IsValidPosition()) transform.position -= new Vector3(-1, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1, 0, 0);
            if (!IsValidPosition()) transform.position -= new Vector3(1, 0, 0);
        }
        else if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.RotateAround(transform.TransformPoint(RotationPoint), new Vector3(0,0,1), 90);    
            if (!IsValidPosition()) transform.RotateAround(transform.TransformPoint(RotationPoint), new Vector3(0, 0, 1), -90);
        }

        if(Time.time - previousTime > (Input.GetKey(KeyCode.DownArrow) ? fallTime / 10 : fallTime))
        {
            transform.position += new Vector3(0, -1, 0);
            if (!IsValidPosition())
            {
                transform.position -= new Vector3(0, -1, 0);
                AddToGrid();
                this.enabled = false;
                FindObjectOfType<BlockSpawner>().SpawnNext();
            }
            previousTime = Time.time;
        }
    }

    private bool IsValidPosition()
    {
        foreach (Transform child in transform)
        {
            int x = Mathf.RoundToInt(child.transform.position.x);
            int y = Mathf.RoundToInt(child.transform.position.y);

            if(x < 0 || x >= width || y < 0 || y >= height || grid[x,y] != null) return false;
        }

        return true;
    }

    private void TryRotate(RotationStates initialState, RotationStates desiredState)
    {
        // implement wall kicks
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
}
