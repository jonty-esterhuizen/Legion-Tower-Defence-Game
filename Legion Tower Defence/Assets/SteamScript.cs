//using Steamworks;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using Unity.Netcode;
//using UnityEngine;

//public class SteamScript : MonoBehaviour
//{
//    protected Callback<LobbyCreated_t> LobbyCreated;
//    protected Callback<GameLobbyJoinRequested_t> LobbyJoinRequested;
//    protected Callback<LobbyEnter_t> LobbyEnter_t;
//    private string HostAddressKey = "HostAddress";
//    ServerConfig Serverconfig;




//    void Start()
//    {
//        if (!SteamManager.Initialized)
//        {

//            return;

//        }

//        LobbyCreated = Callback<LobbyCreated_t>.Create(onLobyCreated);
//        LobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnLobbyJoinRequested);
//        LobbyEnter_t = Callback<LobbyEnter_t>.Create(OnLobbyEnter_t);


//    }





//    public void HostLoby()
//    {
//        SteamAPI.Init(); 
//        Serverconfig = NetworkManager.Singleton.GetComponent<ServerConfig>();
//        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, Serverconfig.MaxConnectedPlayers);
//        NetworkManager.Singleton.StartHost();

//    }



//    private void onLobyCreated(LobbyCreated_t param)
//    {

//        if (param.m_eResult != EResult.k_EResultOK)
//        {
//            Debug.Log("error Creating Loby");
//            return;
//        }
//        bool serverStarted = NetworkManager.Singleton.StartHost();

//        SteamMatchmaking.SetLobbyData(
//            new CSteamID(param.m_ulSteamIDLobby),
//            HostAddressKey,
//            SteamUser.GetSteamID().ToString()
//            );
//    }




//    private void OnLobbyJoinRequested(GameLobbyJoinRequested_t param)
//    {
//        SteamMatchmaking.JoinLobby(param.m_steamIDLobby);
//    }
//    private void OnLobbyEnter_t(LobbyEnter_t param)
//    {

        
//        if (NetworkManager.Singleton.IsServer)
//        {
//            return;
//        }
//        string hostAddress = SteamMatchmaking.GetLobbyData(
//            new CSteamID(param.m_ulSteamIDLobby),
//            HostAddressKey
//            );


//        Serverconfig.IP = hostAddress;

//        GetComponent<MyNetworkManager>().UpdateServerConnection();

//        NetworkManager.Singleton.StartClient();
//    }



//}
