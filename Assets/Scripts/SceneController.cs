using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

    public static SceneController instance;


    private void Awake()
    {
        if (instance == null)
        {

            instance = this;

            DontDestroyOnLoad(this);
        }

        if (instance != this)
            Destroy(gameObject);
    }

    public void SceneLoad(int scene)
    {
        SceneManager.LoadScene(scene);
    }
}
