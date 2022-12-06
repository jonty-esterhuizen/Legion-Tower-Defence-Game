using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CameraController : NetworkBehaviour
{
    public float speed = 7f;
    [SerializeField]
    Transform target;
    public string CamName = "";
    void Start()
    {
        if (!IsOwner)
        {

            return;
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner)
        {
            return;
        }

        CamName = "cam" + NetworkManager.Singleton.LocalClient.ClientId;
        GetComponent<Camera>().depth = 1;
        gameObject.name = CamName;
        target = NetworkManager.Singleton.LocalClient.PlayerObject.transform;
        MoveToPoint();
    }



    private void MoveToPoint()
    {     // Move our position a step closer to the target.
        var step = speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position,

            new Vector3(target.position.x, 20, target.position.z),

            step);


    }
}
