using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    public float speed = 7f;
    [SerializeField]
    Vector3 MovePoint;
    int CurrentTargetChild = 0;
    [SerializeField]
    public Camera camera;
    [SerializeField]
    GameObject CameraPrefab;
    [SerializeField]
    LayerMask layerMask;
    public bool Setupplayer = false;

    void Start()
    {
        if (!IsOwner)
        {
            return;
        }

    }

    [ServerRpc]
    public void SpawnPlayerPrefabsServerRpc(ulong ClinetID)
    {

        GameObject camTemp = Instantiate(CameraPrefab);
        camTemp.transform.SetParent(null, false);

        camTemp.GetComponent<NetworkObject>().SpawnWithOwnership(ClinetID, true);

    }
    // Update is called once per frame
    void Update()
    {
        if (!IsOwner)
        {
            return;
        }
        if (!Setupplayer)
        {
            Setupplayer = true;
            SpawnPlayerPrefabsServerRpc(NetworkManager.Singleton.LocalClient.ClientId);

        }
        gameObject.name = "Player" + NetworkManager.Singleton.LocalClient.ClientId;
        camera = GameObject.Find("cam" + NetworkManager.Singleton.LocalClient.ClientId).GetComponent<Camera>();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 500, layerMask))
            {
                MovePoint = hit.point;
                Debug.Log(hit.transform.name);
                Debug.Log("hit" + hit.collider.gameObject.name);
            }
        }

        MoveToPoint();
    }



    private void MoveToPoint()
    {     // Move our position a step closer to the target.
        var step = speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position,

            new Vector3(MovePoint.x, 1, MovePoint.z),

            step);
        transform.LookAt(new Vector3(MovePoint.x, 1, MovePoint.z));

    }
}
