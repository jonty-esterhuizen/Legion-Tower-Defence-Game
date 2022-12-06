using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour
{

    void Update()
    {
        if (!IsServer)
        {
            return;
        }
        var step = 2 * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position,
           transform.forward * 20,
            step);
         
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, .2f);

        if (hitColliders.Length > 0)
        {
            Debug.Log(hitColliders[0]);
            try
            {
                hitColliders[0].GetComponent<EnemyController>().TakeDamageServerRpc();
                gameObject.GetComponent<NetworkObject>().Despawn(true);
            }
            catch (System.Exception)
            {
                gameObject.GetComponent<NetworkObject>().Despawn(true);
            }
        }

    }
}
