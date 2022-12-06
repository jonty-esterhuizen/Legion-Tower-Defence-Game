using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class PlayerInteractionController : NetworkBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> buildings = new List<GameObject>();
    [SerializeField]
    LayerMask layerMask;
    GameObject previewObject;
    void Start()
    {
        if (!IsOwner)
        {
            return;
        }
       
    }
    int index = 0;

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner)
        {
            return;
        }
        if (GetComponent<PlayerController>().camera == null)
        {


        }
        else
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
            {
                index++;
                if (index > buildings.Count - 1)
                {
                    index = 0;
                }
                Debug.Log($"{index } ,{ buildings.Count}");
                previewObject = InstantiatePreviewBuilding(buildings[index]);
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
            {
                index--;
                Debug.Log($"{index } ,{ buildings.Count}");
                if (index < 0)
                {
                    index = buildings.Count - 1;
                }
                previewObject = InstantiatePreviewBuilding(buildings[index]);
            }


            Ray ray = GetComponent<PlayerController>().camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 500, layerMask))
            {
                if (previewObject != null)
                {
                    var step = 22 * Time.deltaTime;

                    previewObject.transform.position = Vector3.MoveTowards(
                        previewObject.transform.position,
                       new Vector3(hit.point.x, 1, hit.point.z),
                    step
                    );

                }
                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    PlaceBuildingServerRpc(NetworkManager.LocalClientId, buildings[index].gameObject.name, new Vector3(hit.point.x, 1, hit.point.z));
                }
            }
        }


    }



    [ServerRpc]
    private void PlaceBuildingServerRpc(ulong ClientID, string buildingName,Vector3 position)
    {
        GameObject tempBuilding =
            Instantiate(buildings.FirstOrDefault(ol => ol.name == buildingName),
            position,
            Quaternion.identity);
        tempBuilding.GetComponent<NetworkObject>().SpawnWithOwnership(ClientID);
    }

    private GameObject InstantiatePreviewBuilding(GameObject building)
    {
        if (previewObject != null)
        {
            Destroy(previewObject);
            previewObject = null;
        }
        GameObject temp = Instantiate(building);
        return temp;

    }
}
