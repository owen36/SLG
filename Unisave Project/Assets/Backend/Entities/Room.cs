using System;
using System.Collections;
using System.Collections.Generic;
using Unisave;
using Unisave.Entities;
using Unisave.Facades;

public class Room : Entity
{
    public List<Player> players = new List<Player>();

    public int uId = 0;

    public int mapSizeX = 10;
    public int mapSizeY = 10;

    public bool isClosed = false;

    public int maxPlayerCount = 10;

    // TODO Make locations needs a list of locations
}
