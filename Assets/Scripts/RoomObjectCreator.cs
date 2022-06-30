using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomObjectCreator : MonoBehaviour
{
    // references
    [Header("References")]

    [SerializeField] GridLayout gridLayout;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public RoomObject MakeRoomInstance(Vector3Int posInGridCells, List<RoomObject.WallStatus> wallStatuses, List<bool> cornerStatuses){ // define function with info needed for a room
        Vector3 roomPos = gridLayout.CellToWorld(posInGridCells); // get room position in world space
        //Instantiate(roomPos, RoomObject, Quaternion.Identity());
        return null;
    }
}
