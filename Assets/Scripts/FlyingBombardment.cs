using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingBombardment : MonoBehaviour
{
    public List<GameObject> explosionLocations = new List<GameObject>();

    public float explosionEveryXSeconds = 0.25f;
    private float timeSinceLastExplosion = 0.0f;

    public void OnEnable(){
        foreach(GameObject obj in explosionLocations) obj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastExplosion += Time.deltaTime;
        if (timeSinceLastExplosion > explosionEveryXSeconds) NewExplosion();
    }

    public void NewExplosion(){
        timeSinceLastExplosion = 0.0f;
        List<GameObject> possibleLocations = new List<GameObject>();
        foreach (GameObject obj in explosionLocations){
            if (obj.activeSelf == false) possibleLocations.Add(obj);
        }

        int r = Random.Range(0, possibleLocations.Count);
        possibleLocations[r].SetActive(true);
    }
}
