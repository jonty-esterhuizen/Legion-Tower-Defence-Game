using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerConfig : MonoBehaviour
{
    public bool StartAsServer = false;
    public string IP = "127.0.0.1";
    public ushort Port = 5555; 
    public int MaxConnectedPlayers = 8;
    public string ServerListenAddress="0.0.0.0";
}
