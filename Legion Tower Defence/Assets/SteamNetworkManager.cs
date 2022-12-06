using Netcode.Transports.Facepunch;
using Steamworks;
using Steamworks.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamNetworkManager : MonoBehaviour
{
    //https://youtu.be/9CYsQ2Rsr2c
    public static SteamNetworkManager Instance { get; private set; }
    public FacepunchTransport facepunchTransport;
    MyNetworkManager myNetworkManager;
    public Lobby? currentLobby { get; private set; } = null;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        myNetworkManager = GetComponent<MyNetworkManager>();
        facepunchTransport = GetComponent<FacepunchTransport>();

        SteamMatchmaking.OnLobbyCreated += OnLobbyCreated;
        SteamMatchmaking.OnLobbyEntered += SteamMatchmaking_OnLobbyEntered;
        SteamMatchmaking.OnLobbyMemberJoined += SteamMatchmaking_OnLobbyMemberJoined;
        SteamMatchmaking.OnLobbyMemberLeave += SteamMatchmaking_OnLobbyMemberLeave;
        SteamMatchmaking.OnLobbyInvite += SteamMatchmaking_OnLobbyInvite;
        SteamMatchmaking.OnLobbyGameCreated += SteamMatchmaking_OnLobbyGameCreated;
        SteamFriends.OnGameLobbyJoinRequested += LobbyInviteAccepted;

    }


    private void OnDestroy()
    {

        SteamMatchmaking.OnLobbyCreated -= OnLobbyCreated;
        SteamMatchmaking.OnLobbyEntered -= SteamMatchmaking_OnLobbyEntered;
        SteamMatchmaking.OnLobbyMemberJoined -= SteamMatchmaking_OnLobbyMemberJoined;
        SteamMatchmaking.OnLobbyMemberLeave -= SteamMatchmaking_OnLobbyMemberLeave;
        SteamMatchmaking.OnLobbyInvite -= SteamMatchmaking_OnLobbyInvite;
        SteamMatchmaking.OnLobbyGameCreated -= SteamMatchmaking_OnLobbyGameCreated;
        SteamFriends.OnGameLobbyJoinRequested -= LobbyInviteAccepted;
    }


    #region Host

    public async void StartSteamHost(ServerConfig serverConfig)
    {
        currentLobby = await SteamMatchmaking.CreateLobbyAsync(serverConfig.MaxConnectedPlayers);
    }

    #endregion





    #region Create
    private void OnLobbyCreated(Result result, Steamworks.Data.Lobby lobby)
    {
        if (result != Result.OK)
        {
            Debug.LogError($" lobby status {result} {this}");
            return;
        }

        lobby.SetFriendsOnly();
        lobby.SetData("Lobby Name","lobby");
        lobby.GetData("Lobby Name");
        lobby.SetJoinable(true);





    }


    private void SteamMatchmaking_OnLobbyGameCreated(Steamworks.Data.Lobby lobby, uint ip, ushort port, SteamId id)
    {

    }

    #endregion



    #region invite
    private void SteamMatchmaking_OnLobbyInvite(Friend friend, Steamworks.Data.Lobby Lobby)
    {
        Debug.Log($"{friend.Name}");

    }


    private void SteamMatchmaking_OnLobbyMemberJoined(Steamworks.Data.Lobby lobby, Friend friend)
    {

    }
    #endregion



    #region Leave and Enter 


    private void LobbyInviteAccepted(Steamworks.Data.Lobby lobby, SteamId friend)
    {
        myNetworkManager.StartAsClient(lobby.Id);

        Debug.Log(lobby.GetData("Lobby Name"));

    }


    private void SteamMatchmaking_OnLobbyEntered(Steamworks.Data.Lobby lobby)
    {
        if (myNetworkManager.IsHost)
        {
            return;
        }
        myNetworkManager.StartAsClient(lobby.Id);
    }

    private void SteamMatchmaking_OnLobbyMemberLeave(Steamworks.Data.Lobby lobby, Friend friend)
    {

    }
    #endregion



}
