using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{

    [SerializeField] float delayInSeconds = 2f;
    [SerializeField] AudioClip buttonPressed;
    [SerializeField] [Range(0,1)] float buttonVolume = 0.2f;
    [SerializeField] float delayInSecondsGame = 0.8f;
    [SerializeField] List<Sprite> playerSprites;
    [SerializeField] GameObject resumePrefab;
    [SerializeField] int level = 1;

    public void LoadStartMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadGame()
    {
        StartCoroutine(WaitAndLoadGame("Game"));
        FindObjectOfType<GameSession>().ResetGame();
    }

    public int GetLevel()
    {
        return level;
    }

    public void NextLevel()
    {
        level += 1;
    }

    public void SelectPlayer(int selection)
    {
        FindObjectOfType<Player>().GetComponent<SpriteRenderer>().sprite = playerSprites[selection];
    }

    IEnumerator WaitAndLoadGame(string name)
    {
        AudioSource.PlayClipAtPoint(buttonPressed, Camera.main.transform.position, buttonVolume);
        yield return new WaitForSeconds(delayInSecondsGame);
        SceneManager.LoadScene(name);
    }

    public void LoadPlayerSelection()
    {
        StartCoroutine(WaitAndLoadGame("Player Selection"));
    }

    public void LoadGameOver()
    {
        StartCoroutine(WaitAndLoad());
    }

    IEnumerator WaitAndLoad()
    {
        yield return new WaitForSeconds(delayInSeconds);
        SceneManager.LoadScene("Game Over");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        Instantiate(resumePrefab, new Vector2(0f, 0f), transform.rotation);
        // make pause menu
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        Destroy(resumePrefab);
    }
}
