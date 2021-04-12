//using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class Boid : MonoBehaviour
{

    private static Boid _instance = null;
    private static object _lock = new object();

    public static Boid Instance { 
        get
        {
            lock (_lock)
            {
                if (_instance != null) return _instance;
                Boid boid = FindObjectOfType<Boid>();
                if (boid == null)
                {
                    GameObject go = new GameObject("[Boid]", typeof(Boid));
                    boid = go.GetComponent<Boid>();
                }
                _instance = boid;
            }
            return _instance;
        }
    }

    public GameObject fishPrefab;
    public GameObject goalPrefab;
    public float goalRange = 1f;
    public int tankSize = 5;

    //generate number of fishes
    static int numFish = 80;
  
    private GameObject[] _allFish = new GameObject[numFish];
    public GameObject[] allFish
    {
        get { return _allFish; }
    }

    //public static Vector3 goalPos = Vector3.zero;
    public Vector3 goalPos {
        get {
            if(goalPrefab != null) return goalPrefab.transform.position;
            return Vector3.zero;
        } 
    }

    // Start is called before the first frame update
    void Start()
    {
        //render fog 
        RenderSettings.fogColor = Camera.main.backgroundColor;
        RenderSettings.fogDensity = 0.01f;
        RenderSettings.fog = true;

        //create loop interating through the numFish array
        for (int i = 0; i < numFish; i++)
        {
            //create a postion to our fishes (in a 3Dimensional space)
            allFish[i] = (GameObject)Instantiate(fishPrefab, 
                Random.insideUnitSphere * tankSize, 
                Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Reset where the goal position is randomly 
        foreach(var fish in allFish)
        {
            if((fish.transform.position - goalPos).magnitude <= goalRange)
            {
                goalPrefab.transform.position = Random.insideUnitSphere * tankSize * 0.75f;
                break;
            }
        }
    }
}
