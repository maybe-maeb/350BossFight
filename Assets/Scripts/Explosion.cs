using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float hitDelay;
    private Collider hitBoxCollider;
    public float hitDuration = 0.25f;

    public ParticleSystem particles;

    void OnEnable()
    {
        hitBoxCollider = this.GetComponent<Collider>();
        hitBoxCollider.enabled = false;
        particles.Play();
        StartCoroutine("WaitToHit");
    }

    public IEnumerator WaitToHit(){
        yield return new WaitForSeconds(hitDelay);
        hitBoxCollider.enabled = true;
        yield return new WaitForSeconds(hitDuration);
        hitBoxCollider.enabled = false;
        this.gameObject.SetActive(false);
    }
}
