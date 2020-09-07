using System;
using System.Collections;
using System.Collections.Generic;
using Unisave;
using Unisave.Facets;
using Unisave.Facades;
using Unisave.Authentication.Middleware;

public class InitFacet : Facet
{
    /// <summary>
    /// Client can call this facet method and receive a greeting
    /// Replace this with your own server-side code
    /// </summary>
    public string GreetClient()
    {
        return "Hello client! I'm the server!";
    }

    public Player CreatePlayer(string deviceId)
    {
        List<Player> players = DB.TakeAll<Player>().Get();


        Player player = new Player();
        player.uiD = players.Count + 1;
        player.name = "Lord" + player.uiD;
        player.deviceId = deviceId;      
        player.Save();
        return player;
    }

    public Player LoadPlayer(int uID)
    {
        List<Player> players = DB.TakeAll<Player>().Get();

        Player player = players.Find(x => x.uiD.Equals(uID));
        return player;
    }

    public Unit CreateUnit()
    {
        Unit unit = new Unit();
        unit.quantity = 0;
        unit.subtype = "Fake";
        unit.Save();
        return unit;
    }

    public Player ChangePlayerName(string newName, string playerId)
    {
        Player currentPlayer = DB.Find<Player>(playerId);
        currentPlayer.Refresh();
        currentPlayer.name = newName;
        currentPlayer.Save();
        return currentPlayer;
    }

    public Player CreatePlayerCity(Player player)
    {
        player.Refresh();
        City city = new City();
        city.cityX = 0;
        city.cityY = 0;


        city.Save();
        player.city = city;


        player.Save();
        return player;
    }
}
