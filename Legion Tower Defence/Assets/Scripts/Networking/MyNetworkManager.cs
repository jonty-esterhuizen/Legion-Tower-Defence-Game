using Newtonsoft.Json;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

[RequireComponent(typeof(ServerConfig))]
public class MyNetworkManager : NetworkManager
{
    public ServerConfig config;
    public SteamNetworkManager steamNetworkManager;

    // SteamScript SteamScript;
    private void Start()
    {
        steamNetworkManager = GetComponent<SteamNetworkManager>();
        config = GetComponent<ServerConfig>();

        OnClientConnectedCallback += MyNetworkManager_OnClientConnectedCallback;
        OnClientDisconnectCallback += MyNetworkManager_OnClientDisconnectCallback;
        OnServerStarted += MyNetworkManager_OnServerStarted;
    }

    public async void StartAsHost()
    {

        bool HostStarted = StartHost();
        steamNetworkManager.StartSteamHost(config);
        Debug.Log($"host started {HostStarted}");


    }
    private void OnDestroy()
    {

        OnClientConnectedCallback -= MyNetworkManager_OnClientConnectedCallback;
        OnClientDisconnectCallback -= MyNetworkManager_OnClientDisconnectCallback;
        OnServerStarted -= MyNetworkManager_OnServerStarted;
    }
    private void OnApplicationQuit()
    {
        Disconnect();
    }

    public void StartAsClient(SteamId steamId)
    {
        steamNetworkManager.facepunchTransport.targetSteamId = steamId;



        bool ClientStarted = StartClient();


    }


    #region Call backs
    private void MyNetworkManager_OnServerStarted()
    {
        Debug.Log("Server Started");
    }

    private void MyNetworkManager_OnClientConnectedCallback(ulong clientid)
    {
        Debug.Log($"connected {clientid}");


    }
    private void MyNetworkManager_OnClientDisconnectCallback(ulong clientid)
    {
        Debug.Log($"disconnected {clientid}");
    }

    #endregion


    public void Disconnect()
    {
        if (steamNetworkManager.currentLobby.HasValue)
        {
            steamNetworkManager.currentLobby.Value.Leave();

        }
        //  SteamAPI.Shutdown();
        NetworkManager.Singleton.Shutdown();

    }

}
