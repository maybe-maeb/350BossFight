using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurrowingAttack : MonoBehaviour
{
    public float followDuration;
    public float attackDelay;

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

    void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Vector3 pos = new Vector3(player.position.x, 0, player.position.z);
        transform.position = pos;
        hitBox.SetActive(false);
        startedAttack = false;
    }

    void Update()
    {
        if (lerping){
            if (timeElapsed < flyDuration)
            {
                boss.transform.position = Vector3.Lerp(bossStartPosition, bossTargetPosition, timeElapsed / flyDuration);
                timeElapsed += Time.deltaTime;
            }
            if (timeElapsed > flyDuration) {
                boss.transform.position = bossTargetPosition;
                lerping = false;
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
        Vector3 bossPos = new Vector3(transform.position.x, -10, transform.position.z);
        boss.transform.position = bossPos;
        bossStartPosition = bossPos;
        bossTargetPosition = new Vector3(bossPos.x, 30, bossPos.z);
        timeElapsed = 0;
        yield return new WaitForSeconds(attackDelay);
        lerping = true;
        hitBox.SetActive(true);
        particleIndicator.SetActive(false);
        yield return new WaitForSeconds(0.25f);
        hitBox.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
