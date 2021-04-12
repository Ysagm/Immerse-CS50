using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public float speed = 1f;
    float rotationSpeed = 4.0f;
    Vector3 averageHeading;
    Vector3 averagePosition;
    //value of the neighbor distance
    float neighbourDistance = 3.0f;
    float minDistance = 1f;

    bool turning = false;

    // Start is called before the first frame update
    void Start()
    {
        //generate a small difference in each neighbor's speed
        speed = Random.Range(0.5f, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, Vector3.zero) >= Boid.Instance.tankSize)
        {
            turning = true;
        }
        else
        {
            turning = false;
        }

        if (turning)
        {
            Vector3 direction = Vector3.zero - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
            speed = Random.Range(0.9f, 1f);
        }
        else
        {
            //flock value
            if (Time.frameCount % 5 == 0)
                ApplyRules(); //function called every 5 times
        }
        transform.Translate(0, 0, Time.deltaTime * speed);
    }

    void ApplyRules()
    {
        GameObject[] allFish = Boid.Instance.allFish;

        // Cohesion of boids
        Vector3 vcentre = Vector3.zero; //center of group
        Vector3 vavoid = Vector3.zero; //points away from any neighbors
        //group speed
        float gSpeed = 0.0f;

        //goal position
        Vector3 goalPos = Boid.Instance.goalPos;

        float dist; //distance variable

        //calculate group size
        int groupSize = 0;
        foreach (GameObject fish in allFish)
        {
            if (fish != this.gameObject)
            {
                dist = Vector3.Distance(fish.transform.position, this.transform.position);
                if (dist <= neighbourDistance)
                {
                    vcentre += fish.transform.position;
                    groupSize++;

                    if (dist < minDistance)
                    {
                        vavoid = vavoid + (this.transform.position - fish.transform.position);
                    }

                    //average speed of group by adding the speed of the flock
                    Flock anotherFlock = fish.GetComponent<Flock>();
                    gSpeed = gSpeed + anotherFlock.speed;
                }
            }
        }

        if (groupSize > 0)
        {
            //calculate average centre and average speed
            vcentre = vcentre / (float)groupSize + (goalPos - this.transform.position);
            speed = gSpeed / (float)groupSize;

            Vector3 direction = (vcentre + vavoid) - transform.position;
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
            }

        }
    }
}
