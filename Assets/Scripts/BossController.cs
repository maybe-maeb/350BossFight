using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    public int health = 9;

    public enum CurrentStage{Start, Stage1, Stage2, Stage3, Dead}
    public CurrentStage currentStage;

    public List<GameObject> sideVulnerableSpots;
    public GameObject headVulnerableSpot;

    private GameObject activeVulnerableSpot;

    public float vulnerableDuration = 10f;

    public GameObject hitEffect;

    public float flyingBombardmentDuration = 15f;

    private Animator anim;

    public GameObject flyingBombardment;
    public GameObject burrowAttack;

    public GameObject playerTeleportEffect;
    public GameObject player;

    public Material invisibleMaterial;

    public GameObject firstRoom;

    public GameObject teleportPuff;

    private int protectingCrystals = 0;
    public GameObject crystalPrefab;
    public List<Transform> crystalSpawnPositions = new List<Transform>();

    public Transform dungeonPosition;

    public Slider healthSlider;

    public GameObject winScreen;

    public GameObject dragonShield;

    public void Start(){
        anim = GetComponent<Animator>();
        currentStage = CurrentStage.Start;
        protectingCrystals = 0;
    }

    public IEnumerator StartFight(){
        anim.SetTrigger("StartFight");
        yield return new WaitForSeconds(2f);
        anim.SetTrigger("StartAttacks");
        currentStage = CurrentStage.Stage1;
        healthSlider.gameObject.SetActive(true);
        ChooseAttack();
    }

    public void ChooseAttack(){
        if (currentStage == CurrentStage.Stage1) StartCoroutine("FlyingBombardment"); 
        else if (currentStage == CurrentStage.Stage2) StartCoroutine("BurrowAttack");
        else if (currentStage == CurrentStage.Stage3) StartCoroutine("DungeonAttack");
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
                if (health <= 0) StartCoroutine("Die");
                else StartCoroutine("GoToStage3");
            }
            else GoToStage2();
        }
    }

    public void GoToStage2(){
        print("Stage 2 activated.");
        currentStage = CurrentStage.Stage2;

        //Just in case
        Destroy(flyingBombardment);
        //Everything to start stage 2
    }

    public IEnumerator GoToStage3(){
        print("Stage 3 activated.");
        currentStage = CurrentStage.Stage3;

        //Just in case. Also frees resources
        Destroy(burrowAttack.GetComponent<BurrowingAttack>().fakeBoss);
        Destroy(burrowAttack);

        //Teleport the dragon
        this.transform.SetParent(dungeonPosition);
        transform.position = dungeonPosition.position;
        transform.rotation = dungeonPosition.rotation;

        //Start the player particle effects
        playerTeleportEffect.SetActive(true);
        playerTeleportEffect.GetComponent<ParticleSystem>().Play();
        Destroy(playerTeleportEffect, 1f);

        //Wait 1/4 seconds
        yield return new WaitForSeconds(0.25f);
        //Teleport the player to teh center
        player.transform.position = new Vector3(0, 0, 0);

        //Make the floor transparent and disable collisions
        Material[] mat = firstRoom.GetComponent<MeshRenderer>().materials;
        mat[0] = invisibleMaterial;
        firstRoom.GetComponent<MeshRenderer>().materials = mat;
        firstRoom.GetComponent<Collider>().enabled = false;


        //The player should now be in the center of the map and falling down the pit
    }

    public IEnumerator Die(){
        Debug.Log("Win!");
        currentStage = CurrentStage.Dead;
        anim.SetTrigger("Die");
        yield return new WaitForSeconds(3);
        winScreen.SetActive(true);
        yield return new WaitForSeconds(5f);
        Application.Quit();
    }

    public void BeVulnerable(bool forceHead = false){
        if (health == 7 || health == 4 || health == 1 || forceHead)
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
        if (currentStage != CurrentStage.Stage3) yield return new WaitForSeconds(vulnerableDuration);
        else yield return new WaitForSeconds(15f);
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

    public IEnumerator BurrowAttack(){
        //Go underground
        anim.SetTrigger("GoUnderground");
        yield return new WaitForSeconds(1f);
        teleportPuff.SetActive(true);
        teleportPuff.GetComponent<ParticleSystem>().Play();
        burrowAttack.SetActive(true);
        yield return new WaitForSeconds(10f);
        teleportPuff.SetActive(false);
        burrowAttack.SetActive(true);
        yield return new WaitForSeconds(10f);
        burrowAttack.SetActive(true);
        yield return new WaitForSeconds(10f);
        anim.SetTrigger("ReturnToSurface");
        teleportPuff.SetActive(true);
        teleportPuff.GetComponent<ParticleSystem>().Play();
        BeVulnerable();
    }

    public IEnumerator DungeonAttack(){
        yield return new WaitForSeconds(1f);
        dragonShield.SetActive(true);
        foreach(Transform point in crystalSpawnPositions){
            Instantiate(crystalPrefab, point.position, Quaternion.identity);
            protectingCrystals++;
        }
    }

    public void DestroyCrystal(){
        protectingCrystals--;
        if (protectingCrystals <= 0) 
        {
            dragonShield.SetActive(false);
            BeVulnerable(true);
        }
    }

    void Update(){
        healthSlider.value = health;
    }
}
