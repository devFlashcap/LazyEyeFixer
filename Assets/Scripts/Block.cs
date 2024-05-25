using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public float fallSpeed = 1.0f;
    private float previousTime;

    void Update()
    {
        if (Time.time - previousTime > (1 / fallSpeed))
        {
            transform.position += Vector3.down;
            if (!GameManager.IsValidPosition(this))
            {
                transform.position += Vector3.up;
                GameManager.UpdateGrid(this);
                GameManager.ClearLines();
                FindObjectOfType<GameManager>().SpawnNextBlock();
                enabled = false; // Disable this script when the Tetromino is locked in place.
            }
            previousTime = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left;
            if (!GameManager.IsValidPosition(this))
                transform.position += Vector3.right;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += Vector3.right;
            if (!GameManager.IsValidPosition(this))
                transform.position += Vector3.left;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            transform.position += Vector3.down;
            if (!GameManager.IsValidPosition(this))
                transform.position += Vector3.up;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.Rotate(0, 0, 90);
            if (!GameManager.IsValidPosition(this))
                transform.Rotate(0, 0, -90);
        }
    }
}
