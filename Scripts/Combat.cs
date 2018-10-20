using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;


public class Combat : Singleton<Combat> {

    public Player Player;    
    public Enemy Enemy;

    List<Character> characterList = new List<Character>();
    float activeTimeBoard = 1000;
    float activeTimeBaseSpeed = 10;
    float[] activeTimeStamp;

    List<Character> waitForActionTurn = new List<Character>();
    public bool EndGame = false;
    int turn = 0;
    int remainEnemy = 0;

    public GameObject[] Stands_Player = new GameObject[3];
    public GameObject[] Stands_Enemy = new GameObject[3];

    public GameObject Banner_Win;
    public GameObject Banner_Lose;

    void Start () {
        initPlayer();
        initEnemy();
        initEnemy();
        initEnemy();
        initActiveTimeBoard();

        Invoke("runActiveTime", 1);
    }

    void initPlayer () {
        characterList.Add(Player);
    }

    void initEnemy () {
        var _enemy = Instantiate(Enemy, Stands_Enemy[remainEnemy].transform, false);
        _enemy.gameObject.name += remainEnemy.ToString();
        _enemy.HealthPoint = 20;
        _enemy.level = 1;
        _enemy.Strength = 1;
        _enemy.Agility = 1;
        _enemy.Speed = 1;
        characterList.Add(_enemy);
        remainEnemy++;
    }

    void initActiveTimeBoard () {
        activeTimeStamp = new float[characterList.Count];
        for (int i = 0; i < characterList.Count; i++) {
            activeTimeStamp[i] = 0;
        }
    }

    void NextTurn () {
        turn++;
        if (EndGame || turn > 50) {
            Debug.LogError("turn: " + turn);
            return;
        }
        if (waitForActionTurn.Count <= 0) {
            Debug.Log("waitForActionTurn is empty"); 
            runActiveTime();
            return;
        }
        
        var characterTurn = waitForActionTurn[0];
        if (characterTurn.HealthPoint <= 0) {
            waitForActionTurn.RemoveAt(0);
            //var index = characterList.IndexOf(characterTurn);
            //characterList.RemoveAt(index);
            NextTurn();
            return;
        }
        Debug.Log(characterTurn + "\'s turn");
        playAttack(characterTurn);
    }

    void runActiveTime () {
        List<Character> arrived = new List<Character>();

        for (int i = 0; i < activeTimeStamp.Length; i++) {
            if (characterList[i].HealthPoint <= 0) {
                continue;
            }
            
            activeTimeStamp[i] += characterList[i].Speed + activeTimeBaseSpeed;
            if(activeTimeStamp[i] >= activeTimeBoard) {
                arrived.Add(characterList[i]);
            }
        }

        if (arrived.Count > 0) {
            Debug.Log(System.String.Join (",", activeTimeStamp.Select(p => p.ToString()).ToArray()));
            foreach (var item in arrived) {
                Debug.Log(item + " arrived");
                waitForActionTurn.Add(item);
            }

            NextTurn();
        } else {
            runActiveTime();
        }
    }

    void EndTurn() {
        float _timeStamp = activeTimeStamp[characterList.IndexOf(waitForActionTurn[0])];

        if (activeTimeStamp[characterList.IndexOf(waitForActionTurn[0])] >= activeTimeBoard) {
            activeTimeStamp[characterList.IndexOf(waitForActionTurn[0])] = 0;
        }
        Debug.Log(waitForActionTurn[0] + "timeStamp: " + _timeStamp + " --> " + activeTimeStamp[characterList.IndexOf(waitForActionTurn[0])]);

        waitForActionTurn.RemoveAt(0);

        if (EndGame) {
            Debug.Log("EndGame");
        }
        else {
            NextTurn();
        }
    }
    
    void playAttack (Character _actionCharacter, Character _targetCharacter = null) {
        if (_actionCharacter.GetType() == Player.GetType() && _targetCharacter == null) {
            _targetCharacter = getNextTargetEnemy();
        }
        else if (_actionCharacter.GetType() == Enemy.GetType() && _targetCharacter == null) {
            _targetCharacter = Player;
        }

        if (_targetCharacter != null) {
            _actionCharacter.PlayAttack(_targetCharacter);
        } else {
            Debug.Log("attack target == null");
        }
    }

    public void OnAttackFinish (Character _actionCharacter) {
        EndTurn();
    }

    public void OnAttackHit (Character _actionCharacter, Character _targetCharacter = null) {
        if (_actionCharacter.GetType() == Player.GetType() && _targetCharacter == null) {
            //targetCharacter = characterList[1];
            Debug.LogError("targetCharacter == null");
        }
        else if (_actionCharacter.GetType() == Enemy.GetType() && _targetCharacter == null) {
            _targetCharacter = Player;
        }

        float finalDamage = 0;
        finalDamage = _actionCharacter.Strength;
        Debug.Log(_actionCharacter + " attack " + _targetCharacter);

        _targetCharacter.HealthPoint -= finalDamage;
        Debug.Log(_targetCharacter + " hp " + (finalDamage * -1f).ToString() + " --> " + _targetCharacter.HealthPoint);

        playHit(_targetCharacter);
    }

    void playHit (Character _actionCharacter) {
        Debug.Log(_actionCharacter + " get hit");
        _actionCharacter.PlayHit();
    }

    public void OnHitFinish (Character _actionCharacter) {
        if (_actionCharacter.HealthPoint <= 0) {
            if (waitForActionTurn.Contains(_actionCharacter)) {
                waitForActionTurn.Remove(_actionCharacter);
            }
            playDead(_actionCharacter);
        }
    }

    void playDead (Character _actionCharacter) {
        Debug.Log(_actionCharacter + " is dead");
        _actionCharacter.PlayDead();
        
    }

    public void OnDeadFinish (Character _actionCharacter) {
        GameObject.Destroy(_actionCharacter.gameObject);
        if (_actionCharacter.GetType() == Player.GetType()) {
            Debug.Log("Lose\nExp + 10\n Point + 0");
            this.Banner_Lose.GetComponent<TextMeshProUGUI>().text = "Lose\nExp + 10\n Point + 0";
            PlayerModel.Instance.Exp += 10;
            this.Banner_Lose.SetActive(true);
            PlayerModel.Instance.AutoSave();
            Invoke("goLobby", 2);
        }
        else if (_actionCharacter.GetType() == Enemy.GetType()) {
            remainEnemy--;
            if (remainEnemy <= 0) {
                //EndGame = true;
                Debug.Log("Win\nExp + 50\n Point + 1");
                this.Banner_Win.GetComponent<TextMeshProUGUI>().text = "Win\nExp + 50\n Point + 1";
                PlayerModel.Instance.Exp += 50;
                PlayerModel.Instance.Point += 1;
                this.Banner_Win.SetActive(true);
                PlayerModel.Instance.AutoSave();
                Invoke("goLobby", 2);
            }
        }
        
    }

    Character getNextTargetEnemy() {
        Character targetEnemy = null;
        for (int i = 1; i < characterList.Count; i++) {
            if (characterList[i].HealthPoint > 0) {
                targetEnemy = characterList[i];
                break;
            }
        }
        return targetEnemy;
    }

    void goLobby () {
        GameController.Instance.LoadScene("Lobby");
    }
}
