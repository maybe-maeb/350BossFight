using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public Animator anim;
    public InputActionReference attacking;

    void Update(){
        if (attacking.action.triggered) Attack();
    }

    void Attack(){
        anim.SetTrigger("SwingSword");
        StartCoroutine(FinishAttack());
    }

    public IEnumerator FinishAttack(){
        yield return new WaitForSeconds(0.5f);
        anim.SetTrigger("FinishAttack");
    }
}
