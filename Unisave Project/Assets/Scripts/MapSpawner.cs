using System.Collections;
using System.Collections.Generic;
using Unisave.Facades;
using UnityEngine;

public class MapSpawner : MonoBehaviour
{
    public GameObject cube;
    public GameObject playerObj;
    public static MapSpawner Instance;
    private List<City> Cities = new List<City>();
    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    public void SpawnTiles(Room room)
    {
        

        for (int x = 0; x < room.mapSizeX; x++)
        {
            for (int z = 0; z < room.mapSizeY; z++)
            {
                GameObject block = Instantiate(cube, Vector3.zero, cube.transform.rotation) as GameObject;
                block.transform.parent = transform;
                block.transform.localPosition = new Vector3(x +0.5f, 0, z + 0.5f);

                //foreach (Player player in room.players)
                //{
                //    City city = OnFacet<RoomManager>.Call<City>(nameof(RoomManager.GetPlayerCity), player);

                //    if (city != null)
                //    {
                //        GameObject cityObj = Instantiate(cube, Vector3.zero, cube.transform.rotation) as GameObject;
                //        cityObj.transform.parent = block.transform;
                //        cityObj.transform.localPosition = new Vector3(x + 0.5f, 0, z + 0.5f);
                //    }
                //}
            }
        }  
    }
}
