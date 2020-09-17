using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{

    int score = 0;


    private void Awake()
    {
        SetUpSingleton();
    }

    private void Update()
    {
        
    }

    public void HyperSpace()
    {
        StartCoroutine(SetHyperSpace());
    }

    IEnumerator SetHyperSpace()
    {
        yield return new WaitForSeconds(2);
        HyperSpace hyperSpace = FindObjectOfType<HyperSpace>();
        hyperSpace.SetHyperSpace();
        hyperSpace.PlayHyperSpaceSound();
    }

    private void SetUpSingleton()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public int GetScore()
    {
        return score;
    }

    public void AddToScore(int points)
    {
        score += points;
    }

    public void ResetGame()
    {
        Destroy(gameObject);

    }

}
