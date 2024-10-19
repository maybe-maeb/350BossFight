using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitBox : MonoBehaviour
{
    public void OnTriggerEnter(Collider col){
        if (col.transform.CompareTag("VulnerableSpot")){
            GameObject boss = GameObject.FindGameObjectWithTag("Enemy");
            boss.GetComponent<BossController>().StartCoroutine("TakeDamage");
        }
    }
}
