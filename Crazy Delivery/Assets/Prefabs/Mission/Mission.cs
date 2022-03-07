using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = System.Random;

public class Mission : MonoBehaviour
{
    
    [SerializeField] public GameObject mission;
    public GameObject myMission;
    // Start is called before the first frame update
    void Start()
    {
        SpawnMission();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SpawnMission()
    {
        var rnd = new System.Random();
        myMission = Instantiate(mission, new Vector3(0, (float)0.6, (float) rnd.Next(-20,20)), Quaternion.identity);
    }

    public void MissionDone()
    {
        Debug.Log("200 points");
        Destroy(myMission); 
    }

    private void OnTriggerEnter(Collider col) 
    {
        if(col.gameObject == myMission)
            {
                MissionDone();
                SpawnMission();
            }
    }
}
