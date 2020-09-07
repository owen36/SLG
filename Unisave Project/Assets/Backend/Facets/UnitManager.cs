using System;
using System.Collections;
using System.Collections.Generic;
using Unisave;
using Unisave.Facets;
using Unisave.Facades;
using Unisave.Authentication.Middleware;
using Unisave.Entities;

public class UnitManager : Facet
{
    /// <summary>
    /// Client can call this facet method and receive a greeting
    /// Replace this with your own server-side code
    /// </summary>
    public Unit TrainUnit(string playerId, string unitSubtype, int quantity)
    {
        // this will be stored in the resource manager late. 
        Player currentPlayer = DB.Find<Player>(playerId);
        currentPlayer.Refresh();
        List<Unit> units = DB.TakeAll<Unit>().Get();
        Unit unit = units.Find(x => x.subtype.Equals(unitSubtype));

        Unit playerUnit = currentPlayer.units.Find(x => x.subtype.Equals(unitSubtype));

        if (playerUnit != null)
        {
            currentPlayer.units.Remove(playerUnit);
            unit.quantity = playerUnit.quantity + quantity;
        }
        else
        {
            unit.quantity = quantity;           
        }

        currentPlayer.units.Add(unit);
        currentPlayer.Save(true);
        return unit;
    }
}
