using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int Score;
    public int[] PointsForLines = new int[4];
    private static GameObject _gameManager;
    private static LevelManager _levelManager;

    void Start()
    {
        _gameManager = this.gameObject;
        _levelManager = _gameManager.GetComponent<LevelManager>();
        Score = 0;
    }

    public void AddScore(int linesCleared)
    {
        Score += PointsForLines[linesCleared - 1] *  _levelManager.Level;
        Debug.Log(Score);
    }
}
