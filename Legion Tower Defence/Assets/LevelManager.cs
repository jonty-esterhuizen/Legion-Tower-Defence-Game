using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class LevelManager : NetworkBehaviour
{

    public Transform SpawnPoint;
    public List<GameObject> Enemies = new List<GameObject>();
    public List<GameObject> EnemiesSpawned = new List<GameObject>();

    private void Start()
    {
        foreach (var item in Enemies)
        { 
            NetworkManager.AddNetworkPrefab(item);
        }
    }

    void Update()
    {
        if (IsServer)
        {

            StartCoroutine(SpawnEnemies());
        }
        
    }
    bool ticking = false;
    private IEnumerator SpawnEnemies()
    {
        if (!ticking)
        {
            ticking = true;
              GameObject temp= Instantiate(Enemies[0]);
            temp.transform.position = SpawnPoint.transform.position;
            temp.GetComponent<NetworkObject>().Spawn();
            yield return new WaitForSeconds(1f);
            ticking = false;
        }
    }
}
