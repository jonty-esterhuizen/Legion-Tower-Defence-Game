using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EnemyController : NetworkBehaviour
{

    public float speed = 7f;
    GameObject path;
    Transform target;
    int CurrentTargetChild = 0; 
    public int health = 100;
    void Start()
    {
        if (!IsServer)
        {
            return;
        }
        path = GameObject.Find("Enemy path");
        target = path.transform.GetChild(CurrentTargetChild);
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsServer)
        {
            return;
        }
        MoveToPoint();
    }



    private void MoveToPoint()
    {     // Move our position a step closer to the target.
        var step = speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);

        // Check if the position of the cube and sphere are approximately equal.
        if (Vector3.Distance(transform.position, target.position) < 0.001f)
        {
            if (CurrentTargetChild < path.transform.childCount - 1)
            {
                CurrentTargetChild++;
                target = path.transform.GetChild(CurrentTargetChild);
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void TakeDamageServerRpc()
    {


        health--;
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }


}
