using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour {

    public GameObject HpBar;
    public Image HpBarRemain;
    public Text HpBarText;

    #region Parameter
    [HideInInspector]
    public int level;
    [HideInInspector]
    public float HealthPoint {
        get {
            return healthPoint;
        }
        set {
            this.HpBarText.text = ((int)value).ToString();
            healthPoint = value;
        }
    }
    protected float healthPoint;
    [HideInInspector]
    public int Strength;
    [HideInInspector]
    public int Agility;
    [HideInInspector]
    public int Speed;
    #endregion

    Animator animator;
    Character targetCharacter;

    public virtual void Start() {
        animator = GetComponent<Animator>();
    }

    public void PlayAttack(Character _targetCharacter) {
        animator.SetTrigger("PlayAttack");
        targetCharacter = _targetCharacter;
    }

    public void PlayHit() {
        animator.SetTrigger("PlayHit");
    }

    public void PlayDead() {
        animator.SetTrigger("PlayDead");
    }

    public void OnAttackFinish () {
        Combat.Instance.OnAttackFinish(this);
    }

    public void OnAttackHit() {
        Combat.Instance.OnAttackHit(this, targetCharacter);
        targetCharacter = null;
    }

    public void OnHitFinish() {
        Combat.Instance.OnHitFinish(this);
    }

    public void OnDeadFinish() {
        Combat.Instance.OnDeadFinish(this);
    }
    
}
