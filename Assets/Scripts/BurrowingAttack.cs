using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurrowingAttack : MonoBehaviour
{
    public float followDuration;
    public float attackDelay;

    public GameObject fakeBoss;
    public GameObject boss;

    public GameObject hitBox;

    public float flyDuration;
    public bool lerping;
    Vector3 bossTargetPosition;
    Vector3 bossStartPosition;
    private float timeElapsed;

    private bool startedAttack;

    public float moveSpeed;
    private float timeFollowing;

    Transform player;

    public GameObject particleIndicator;
    private bool bossAttacking;
    public GameObject attackParticles;

    void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Vector3 pos = new Vector3(player.position.x, 0, player.position.z);
        particleIndicator.SetActive(true);
        transform.position = pos;
        hitBox.SetActive(false);

        attackParticles.SetActive(false);
        startedAttack = false;
        lerping = false;
        bossAttacking = false;
        timeElapsed = 0;
        timeFollowing = 0;
    }

    void Update()
    {
        if (lerping && bossAttacking == false){
            if (timeElapsed < flyDuration)
            {
                fakeBoss.transform.position = Vector3.Lerp(bossStartPosition, bossTargetPosition, timeElapsed / flyDuration);
                timeElapsed += Time.deltaTime;
            }
            if (timeElapsed > flyDuration) {
                fakeBoss.transform.position = bossTargetPosition;
                lerping = false;
                fakeBoss.SetActive(false);
                this.gameObject.SetActive(false);
            }
        }
        
        if (startedAttack == false) Follow();
    }

    public void Follow(){
        Vector3 targetPosition = new Vector3(player.position.x, 0, player.position.z);

        // Move our position a step closer to the target.
        var step =  moveSpeed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

        timeFollowing += Time.deltaTime;
        if (timeFollowing > followDuration) StartCoroutine("Attack");
    }

    public IEnumerator Attack()
    {
        startedAttack = true;
        timeElapsed = 0;
        yield return new WaitForSeconds(attackDelay);
        Vector3 bossPos = new Vector3(transform.position.x, -20, transform.position.z);
        fakeBoss.transform.position = bossPos;
        bossStartPosition = bossPos;
        bossTargetPosition = new Vector3(bossPos.x, 50, bossPos.z);
        fakeBoss.SetActive(true);
        lerping = true;
        bossAttacking = true;
        hitBox.SetActive(true);
        particleIndicator.SetActive(false);
        attackParticles.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        hitBox.SetActive(false);
        yield return new WaitForSeconds(1f);
        bossAttacking = false;
    }
}
