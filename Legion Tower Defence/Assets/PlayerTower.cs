using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using static Unity.Netcode.NetworkObject;

public class PlayerTower : NetworkBehaviour
{


    public float Range = 20f;
    public GameObject Towerhead;
    public GameObject Bullet;
    public LayerMask LayerMask;
    public List<Transform> ShootingPoints = new List<Transform>();
    public float ShootSpeed = 2;
    void Start()
    { 
        
    }


    public GameObject EnemyClosest;

    void Update()
    {
        if (!IsOwner)
        {
            return;
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, Range, LayerMask);

        if (hitColliders.Length > 0)
        {
            EnemyClosest = hitColliders[0].gameObject;
        }
        if (EnemyClosest != null)
        {
            Towerhead.transform.LookAt(EnemyClosest.transform);
            StartCoroutine(Fire());
        }
    }

    bool ticking = false;
    private IEnumerator Fire()
    {
        if (!ticking)
        {
            ticking = true;
            ShootEnemy(EnemyClosest);
            yield return new WaitForSeconds(ShootSpeed);
            ticking = false;
        }
    }

    private void ShootEnemy(GameObject Enemy)
    {
        foreach (var item in ShootingPoints)
        {
            SpawnBulletServerRpc(
                Enemy.transform.position,
                item.transform.position,
                NetworkManager.Singleton.LocalClientId
                );
        }
    }
    [ServerRpc]
    public void SpawnBulletServerRpc(Vector3 EnemyPosition, Vector3 startPoint, ulong ClientID)
    {

        GameObject temp = Instantiate(Bullet, startPoint, Quaternion.identity);
        Destroy(temp, 3);
        temp.transform.LookAt(EnemyPosition);
        temp.GetComponent<NetworkObject>().Spawn();  

    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, Range);
    }
}
