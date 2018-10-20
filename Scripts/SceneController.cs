using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : Singleton<SceneController> {
    
    public void LoadScene (string _sceneName) {
        SceneManager.LoadScene(_sceneName, LoadSceneMode.Single);
    }

    void Awake() {

    }
}
