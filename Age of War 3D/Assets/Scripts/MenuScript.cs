using UnityEngine;

public class MenuScript : MonoBehaviour
{
    [SerializeField] GameObject menuContainer;
    [SerializeField] GameObject menuOptions;
    [SerializeField] GameObject gameOverOptions;
    [SerializeField] GameObject gameWinOptions;

    private void Awake()
    {
        ResumeGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menuOptions.activeSelf)
            {
                ResumeGame();
            }
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        menuContainer.SetActive(true);
        
        menuOptions.SetActive(true);
        gameOverOptions.SetActive(false);
        gameWinOptions.SetActive(false);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        menuContainer.SetActive(false);
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        menuContainer.SetActive(true);
        
        menuOptions.SetActive(false);
        gameWinOptions.SetActive(false);
        gameOverOptions.SetActive(true);
    }

    public void GameWin()
    {
        Time.timeScale = 0f;
        menuContainer.SetActive(true);

        menuOptions.SetActive(false);
        gameOverOptions.SetActive(false);
        gameWinOptions.SetActive(true);
    }
}
