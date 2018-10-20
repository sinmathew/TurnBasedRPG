using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController> {

    private static bool created = false;

    void Awake() {
        if (!created) {
            DontDestroyOnLoad(this.gameObject);
            created = true;
        } else {
            GameObject.Destroy(this.gameObject);
        }
    }

    void Start() {
        //for (int i = 1; i < 100; i++) {
        //    Debug.Log(i + " : " + PlayerModel.Instance.nextLevel(i));
        //}
    }


    public void LoadScene (string SceneName) {
        SceneController.Instance.LoadScene(SceneName);
    }

    
}
