using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character {

    public override void Start() {

        this.HealthPoint = 100;
        this.level = PlayerModel.Instance.Level;
        this.Strength = 15;
        this.Agility = 1;
        this.Speed = 1;

        base.Start();
    }

    //public void OnAttackFinish () {
    //    Combat.Instance.OnAttackFinish(this);
    //}

    //public void OnAttackHit () {
    //    Combat.Instance.OnAttackHit(this);
    //}

    //public void OnHitFinish() {
    //    Combat.Instance.OnHitFinish(this);
    //}

    //public void OnDeadFinish() {
    //    Combat.Instance.OnDeadFinish(this);
    //}
}
