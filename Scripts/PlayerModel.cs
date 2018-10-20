using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class LevelExpTranslate : Singleton<LevelExpTranslate> {
    public List<int> LevelExpList = new List<int>();

    private float exponent = 1.5f;
    private float baseXP = 100f;

    void Awake() {
        for (int i = 0; i < 100; i++) {
            LevelExpList.Add(NextLevel(i));
        }
    }

    public int NextLevel(int _level) {
        float _exponent = exponent;
        float _baseXP = baseXP;
        return Mathf.FloorToInt(_baseXP * (Mathf.Pow((float)_level, _exponent)));
    }

    public int GetLevel(int _exp) {
        for (int i = 0; i < this.LevelExpList.Count; i++) {
            if (_exp < LevelExpList[i]) {
                return i;
            }
        }
        return -1;
    }
}

public class PlayerModel : Singleton<PlayerModel> {
    public int Exp = 0;

    private int level = 1;
    public int Level {
        get { return level; }
        set {
            Exp = LevelExpTranslate.Instance.NextLevel(value);
            level = LevelExpTranslate.Instance.GetLevel(Exp);
        }
    }

    public int Point = 0;

    private void Awake() {
        Load();
    }

    void Start() {
        InvokeRepeating("AutoSave", 30, 30);
    }

    public void AutoSave () {
        string json = JsonUtility.ToJson(this);
        PlayerPrefs.SetString("PlayerModel", json);
    }

    public void Load() {
        JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString("PlayerModel"), this);
    }
}
