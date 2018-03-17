using UnityEngine;

public class SceneTransition : MonoBehaviour {

    public void SceneLoad(int scene)
    {
        SceneController.instance.SceneLoad(scene);        
    }
}
