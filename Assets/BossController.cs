using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public int health = 9;

    public enum CurrentStage{Stage1, Stage2, Stage3, Dead}
    public CurrentStage currentStage;

    public List<GameObject> sideVulnerableSpots;
    public GameObject headVulnerableSpot;

    private GameObject activeVulnerableSpot;

    public float vulnerableDuration = 10f;

    public GameObject hitEffect;

    public float flyingBombardmentDuration = 15f;

    private Animator anim;

    public GameObject flyingBombardment;

    public void Start(){
        anim = GetComponent<Animator>();
        currentStage = CurrentStage.Stage1;
        ChooseAttack();
    }

    public void ChooseAttack(){
        if (currentStage == CurrentStage.Stage1) StartCoroutine("FlyingBombardment"); 
    }

    public IEnumerator TakeDamage(){
        GameObject hit = Instantiate(hitEffect, activeVulnerableSpot.transform.position, Quaternion.identity);
        Destroy(hit, 1f);
        activeVulnerableSpot.SetActive(false);
        anim.SetTrigger("TakeDamage");
        health--;
        CheckStages();
        yield return new WaitForSeconds(1.5f);
        ChooseAttack();
    }

    public void CheckStages(){
        if (health <= 6){
            if (health <= 3){
                if (health <= 0) Die();
                else GoToStage3();
            }
            else GoToStage2();
        }
    }

    public void GoToStage2(){
        print("Stage 2 activated.");
        currentStage = CurrentStage.Stage2;
        //Everything to start stage 2
    }

    public void GoToStage3(){
        print("Stage 3 activated.");
        currentStage = CurrentStage.Stage3;
        //Everything to start stage 3
    }

    public void Die(){
        Debug.Log("Win!");
        currentStage = CurrentStage.Dead;
        anim.SetTrigger("Die");
    }

    public void BeVulnerable(){
        if (health == 7 || health == 4 || health == 1)
        {
            headVulnerableSpot.SetActive(true);
            activeVulnerableSpot = headVulnerableSpot;
        } 
        else {
            int rand = Random.Range(0, sideVulnerableSpots.Count);
            sideVulnerableSpots[rand].SetActive(true);
            activeVulnerableSpot = sideVulnerableSpots[rand];
        }
        StartCoroutine(DeactivateVulnerableSpot());
    }

    public IEnumerator DeactivateVulnerableSpot(){
        int startHealth = health;
        yield return new WaitForSeconds(vulnerableDuration);
        //If it took damage, don't do anything
        if (health == startHealth){
            activeVulnerableSpot.SetActive(false);
            ChooseAttack();
        }
    }

    public IEnumerator FlyingBombardment(){
        anim.SetTrigger("FlyingBombardment");
        yield return new WaitForSeconds(5f);
        flyingBombardment.SetActive(true);
        print("Shooting fireballs!");
        yield return new WaitForSeconds(flyingBombardmentDuration);
        anim.SetTrigger("FinishFlyingBombardment");
        flyingBombardment.SetActive(false);
        yield return new WaitForSeconds(5f);
        BeVulnerable();
    }
}
