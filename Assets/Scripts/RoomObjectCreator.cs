using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomObjectCreator : MonoBehaviour
{
    [SerializeField] int hardStop;
    [SerializeField] int hardStopMax;

    [SerializeField] RoomObject startingRoom;

    // references
    [Header("References")]

    [SerializeField] GridLayout gridLayout;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public RoomObject MakeRoomInstance(Vector3Int posInGridCells, List<RoomObject.WallStatus> wallStatuses, List<bool> cornerStatuses, bool disable = true){ // define function with info needed for a room
        if(hardStop>hardStopMax){return null;} // stop the function if too many rooms where spawned

        Vector3Int roomPos = posInGridCells; // get room position in world space

        RoomObject room = (RoomObject)ScriptableObject.CreateInstance("temp name"); // create room instance and cast it to room type (later set the name to the array pos)
        room.init(roomPos, wallStatuses, cornerStatuses); // setup the room

        hardStop++; // keep track of the room count
        return room; // return the room
    }

    public void StartDungeon(){
    }
}
