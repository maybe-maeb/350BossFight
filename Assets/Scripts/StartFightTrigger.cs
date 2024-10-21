using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartFightTrigger : MonoBehaviour
{
    public BossController boss;
    public void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            boss.StartCoroutine("StartFight");
            this.gameObject.SetActive(false);
        }
    }
}
