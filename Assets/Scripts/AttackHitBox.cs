using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitBox : MonoBehaviour
{
    public GameObject crystalhit;
    public GameObject enemyhit;
    public void OnTriggerEnter(Collider col)
    {
        if (col.transform.CompareTag("VulnerableSpot"))
        {
            GameObject boss = GameObject.FindGameObjectWithTag("Enemy");
            boss.GetComponent<BossController>().StartCoroutine("TakeDamage");
            
            GameObject hit = Instantiate(enemyhit, col.transform.position, Quaternion.identity);
            Destroy(hit, 1f);
        }
        else if (col.transform.CompareTag("Crystal"))
        {
            print("hit crystal");
            GameObject hit = Instantiate(crystalhit, col.transform.position, Quaternion.identity);
            Destroy(hit, 1f);
            GameObject boss = GameObject.FindGameObjectWithTag("Enemy");
            boss.GetComponent<BossController>().DestroyCrystal();
            Destroy(col.gameObject);
        }
    }
}
