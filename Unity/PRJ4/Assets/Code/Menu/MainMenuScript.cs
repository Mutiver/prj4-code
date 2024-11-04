using UnityEngine;
using UnityEngine.SceneManagement;

public interface ISceneManagement
{
    void LoadScene(string sceneName);
    void Quit();
}

public class UnitySceneManagement : ISceneManagement
{
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Quit()
    {
        Application.Quit();
    }
}

public class MainMenuScript : MonoBehaviour
{
    public int Difficulty { get; set; }

    private ISceneManagement _sceneManagement;

    private void Awake()
    {
        _sceneManagement = new UnitySceneManagement();
    }

    public void PlayGame()
    {
        _sceneManagement.LoadScene("Level 1");
    }

    public void PlayGame2()
    {
        _sceneManagement.LoadScene("Level 2");
    }
    public void PlayGame3()
    {
        _sceneManagement.LoadScene("Level 3");
    }
    public void PlayGame4()
    {
        _sceneManagement.LoadScene("Level 4");
    }
    

    public void QuitGame()
    {
        _sceneManagement.Quit();
    }

    // For testing purposes
    public void SetSceneManagement(ISceneManagement sceneManagement)
    {
        _sceneManagement = sceneManagement;
    }
}
