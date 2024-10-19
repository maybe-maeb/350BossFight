using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Animator anim;

    void Update(){
        if (Input.GetMouseButtonDown(0)) Attack();
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
