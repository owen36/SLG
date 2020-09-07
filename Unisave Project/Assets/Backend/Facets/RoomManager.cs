using System;
using System.Collections;
using System.Collections.Generic;
using Unisave;
using Unisave.Facets;
using Unisave.Facades;
using Unisave.Authentication.Middleware;

public class RoomManager : Facet
{
    public Room CreateRoom()
    {
        List<Room> rooms = DB.TakeAll<Room>().Get();

        Room room = new Room();
        room.uId = rooms.Count + 1;
        room.Save();
        return room;
    }

    public Room GetAvalibleRoom()
    {
        Room room = DB.TakeAll<Room>().Filter(p => p.isClosed == false).First();

        if (room != null)
            return room;
        else
            return CreateRoom();
    }

    public Room JoinRoom(Room room, Player player)
    {
        player.Refresh();
        room.players.Add(player);
        room.Save();


        //player.currentRoom = room;
       // player.Save();
        return room;
    }

    public Room GetRoomViaId(int uid)
    {
        Room room = DB.TakeAll<Room>().Filter(m => m.uId == uid).First();
        return room;
    }

    public Player SavePlayerRoom(Room room, Player player)
    {
        player.Refresh();
        room.Refresh();

        City city = new City();
        city.cityX = 0;
        city.cityY = 0;


        city.Save();
        player.city = city;
        player.currentRoom = room;
        player.realmId = room.uId;

        player.SaveCarefully();
        return player;
    }

    public City GetPlayerCity(Player player)
    {
        player.Refresh();
        return player.city.Find();
    }

}
