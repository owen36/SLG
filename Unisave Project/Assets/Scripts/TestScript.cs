using System;
using System.Collections;
using System.Collections.Generic;
using Unisave.Facades;
using UnityEngine;
using UnityEngine.UI;

public class TestScript : MonoBehaviour
{
    public Text playerName;
    public Button ChangeNameButton;
    public Text troopCount;
    public InputField newNameField;

    private Player currentPlayer;

    private void Awake()
    {
        //PlayerPrefs.DeleteAll();
    }

    void Start()
    {
        int playerUid = PlayerPrefs.GetInt("HasPlayer", -1);

        if (playerUid == -1)
            OnFacet<InitFacet>.Call<Player>(nameof(InitFacet.CreatePlayer), SystemInfo.deviceUniqueIdentifier).Then(PlayerCreated).Done();
        else
            OnFacet<InitFacet>.Call<Player>(nameof(InitFacet.LoadPlayer), playerUid).Then(PlayerLoaded).Done();


        ChangeNameButton.onClick.AddListener(ChangeName);
    }

    private void ChangeName()
    {
        string newName = newNameField.text;

        if(!string.IsNullOrEmpty(newName))
            OnFacet<InitFacet>.Call<Player>(nameof(InitFacet.ChangePlayerName), newName, currentPlayer.EntityId).Then(NameChanged).Done();
    }

    void NameChanged(Player player)
    {
        currentPlayer = player;
        playerName.text = player.name;
        newNameField.text = "";
    }

    void FacetMethodHasBeenCalled(string serverGreeting)
    {
        Debug.Log("Server greets you: " + serverGreeting);
    }

    void PlayerCreated(Player player)
    {
        PlayerPrefs.SetInt("HasPlayer", player.uiD);
        Debug.Log(player.name);
        currentPlayer = player;
        playerName.text = currentPlayer.name;

        // will always be 0 if they just got created 
        troopCount.text = "Troop count: 0";
    }

    void PlayerLoaded(Player player)
    {
        // Handle if the server cannot find the player. 
        if (player == null)
        {
            OnFacet<InitFacet>.Call<Player>(nameof(InitFacet.CreatePlayer), SystemInfo.deviceUniqueIdentifier).Then(PlayerCreated).Done();
        }
        else
            Debug.Log(player.name + " player loaded!");

        currentPlayer = player;
        playerName.text = currentPlayer.name;

        if (player.units.Count > 0)
        {
            Unit unit = player.units[0];
            if (unit != null)
                troopCount.text = "Troop count: " + unit.quantity.ToString();
            else
                troopCount.text = "Troop count: 0";
        }

        if(!player.currentRoom.IsNull)
        {
            if(player.currentRoom.Find() != null)
            {
                MapSpawner.Instance.SpawnTiles(player.currentRoom.Find());
            }           
        }
        else
            OnFacet<RoomManager>.Call<Room>(nameof(RoomManager.GetRoomViaId), currentPlayer.realmId).Then(SpawnRoom).Done();

    }

    void SpawnRoom(Room room)
    {
        MapSpawner.Instance.SpawnTiles(room);
    }

    public void TryTrainUnit()
    {
        OnFacet<UnitManager>.Call<Unit>(nameof(UnitManager.TrainUnit), currentPlayer.EntityId, "Fake", 4).Then(UnitTrained).Done();
    }

    void UnitTrained(Unit unit)
    {
        troopCount.text = "Troop count: " + unit.quantity.ToString();
        Debug.Log(unit.quantity);
    }

    public void GetAvalibleRoom()
    {
        if(currentPlayer == null)
        {
            return;
        }


        if (currentPlayer.realmId == -1)
            OnFacet<RoomManager>.Call<Room>(nameof(RoomManager.GetAvalibleRoom)).Then(CreateOrJoinRoom).Done();
        else
            Debug.Log("Already in Room UID: " + currentPlayer.realmId.ToString());
    }

    void CreateOrJoinRoom(Room room)
    {
        Debug.Log("Room Found UID: " + room.uId.ToString());
        OnFacet<RoomManager>.Call<Room>(nameof(RoomManager.JoinRoom), room, currentPlayer).Then(OnRoomJoined).Done();
    }

    void OnRoomJoined(Room room)
    {
        Debug.Log("Room Joined!!");
        OnFacet<RoomManager>.Call<Player>(nameof(RoomManager.SavePlayerRoom), room, currentPlayer).Then(SetPlayer).Done();
        //OnFacet<InitFacet>.Call<Player>(nameof(InitFacet.CreatePlayerCity), currentPlayer).Then(SetPlayer).Done();
        PlayerPrefs.SetInt("RoomId", room.uId);
        MapSpawner.Instance.SpawnTiles(room);
    }

    void SetPlayer(Player player)
    {
        currentPlayer = player;
    }
}
