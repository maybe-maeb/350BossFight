using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{

    public float invulnerabilityTime = 1.5f;
    public bool isInvulnerable = false;
    public GameObject invulnerabilitySphere;

    public GameObject hitEffect;

    public float health = 10;

    public Slider healthSlider;
    public GameObject deathScreen;
    public List<GameObject> disableOnDeath = new List<GameObject>();

    void Start()
    {
        healthSlider.maxValue = health;
    }

    void Update()
    {
        healthSlider.value = health;
    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.transform.CompareTag("EnemyHitBox"))
        {
            print("Take damage");
            StartCoroutine("TakeDamage");
        }
    }

    public void OnTriggerStay(Collider col)
    {
        if (col.transform.CompareTag("EnemyHitBox"))
        {
            print("Take damage");
            StartCoroutine("TakeDamage");
        }
    }

    public IEnumerator TakeDamage()
    {
        if (isInvulnerable == false)
        {
            Vector3 pos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            GameObject hit = Instantiate(hitEffect, pos, Quaternion.identity);
            Destroy(hit, 1f);
            print("Took damage!");
            health--;
            isInvulnerable = true;
            invulnerabilitySphere.gameObject.SetActive(true);
            yield return new WaitForSeconds(invulnerabilityTime);
            isInvulnerable = false;
            invulnerabilitySphere.gameObject.SetActive(false);
            print("Player is now vulnerable again.");
        }

        if (health <= 0)
        {
            print("Player died!");
            deathScreen.SetActive(true);
            foreach(GameObject go in disableOnDeath){
                go.SetActive(false);
            }
            yield return new WaitForSeconds(3);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload current scene
        }
    }
}
