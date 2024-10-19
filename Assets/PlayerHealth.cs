using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    public float invulnerabilityTime = 1.5f;
    public bool isInvulnerable = false;
    public GameObject invulnerabilitySphere;

    public GameObject hitEffect;

    public float health = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider col){
        print("triggered");
        if (col.transform.CompareTag("EnemyHitBox")){
            print("Take damage");
            StartCoroutine("TakeDamage");
        }
    }

    public IEnumerator TakeDamage(){
        if (isInvulnerable == false){
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

        if (health <= 0){
            print("Player died!");
            // TODO: Game Over logic
        }
    }
}
