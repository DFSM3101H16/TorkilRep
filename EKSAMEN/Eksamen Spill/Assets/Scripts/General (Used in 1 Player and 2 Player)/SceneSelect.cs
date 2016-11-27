using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelect : MonoBehaviour {

    public GameObject pausePanel;

    void start()
    {
        pausePanel.SetActive(false);
    }

    //Funksjoner kjørt via knappene i UIen

    public void LoadScene(string gameScene)
    {
        SceneManager.LoadScene(gameScene);
    }

    public void showControls()
    {
        pausePanel.SetActive(true);
    }

    public void backToGame()
    {
        pausePanel.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
