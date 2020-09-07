using System;
using System.Collections;
using System.Collections.Generic;
using Unisave;
using Unisave.Entities;
using Unisave.Facades;

public class Player : Entity
{
    public string name = "Lord";


    public string deviceId = "unset";


    public int uiD = 0;


    public int realmId = -1;
    public EntityReference<Room> currentRoom;

    public EntityReference<City> city;

    public List<Item> items = new List<Item>();
    public List<Unit> units = new List<Unit>();
    public List<Currency> currencies = new List<Currency>();
}
