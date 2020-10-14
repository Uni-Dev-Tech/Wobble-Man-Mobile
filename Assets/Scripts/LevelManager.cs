using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private int currentLevel; // текущий кровень
    static public LevelManager instance;
    private void Awake()
    {
        if(LevelManager.instance != null)
        {
            Destroy(gameObject);
            return;
        }
        LevelManager.instance = this;
        CheckTheProgress();
    }
    /// <summary>
    /// Запускает корутину перезагрузки уровня(для UI)
    /// </summary>
    public void RestartLevel()
    {
        StartCoroutine(RestartLevelAfter(0.25f));
    }
    /// <summary>
    /// Запускает корутину перехода на след уровень(для UI)
    /// </summary>
    public void NextLevel()
    {
        StartCoroutine(NextLevelAfter(0.25f));
    }
    /// <summary>
    /// Перезапускает уровень после проигрывания анимации
    /// </summary>
    /// <param name="second">Вреия проигрывания анимации</param>
    /// <returns></returns>
    public IEnumerator RestartLevelAfter(float second)
    {
        yield return new WaitForSeconds(second);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    /// <summary>
    /// Запускает следующий уровень после проигрывания анимации.Запускает первый уровень если последний был пройден
    /// </summary>
    /// <param name="seconds">Вреия анимации</param>
    /// <returns></returns>
    public IEnumerator NextLevelAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        currentLevel++;
        if (currentLevel > 2)
            currentLevel = 0;
        PlayerPrefs.SetInt("Progress", currentLevel);
        SceneManager.LoadScene(currentLevel);
    }
    /// <summary>
    /// Проверяет сохраненный прогресс и запускает последний непройденный уровень
    /// </summary>
    private void CheckTheProgress()
    {
        if (PlayerPrefs.HasKey("Progress"))
            currentLevel = PlayerPrefs.GetInt("Progress");
        else
        {
            currentLevel = 0;
            PlayerPrefs.SetInt("Progress", currentLevel);
        }
        if (currentLevel != SceneManager.GetActiveScene().buildIndex)
                SceneManager.LoadScene(currentLevel);
    }
}
