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
    [SerializeField] DungeonLogic dungeonLogic;
    [SerializeField] RoomManager roomManager;

    void Start()
    {
        // find references
        gridLayout = FindObjectOfType<GridLayout>();
        dungeonLogic = FindObjectOfType<DungeonLogic>();
        roomManager = FindObjectOfType<RoomManager>();

        StartDungeon(); // what do you think it does
    }

    void Update()
    {
        
    }

//ROOM CREATION METHODS

    public RoomObject SpawnRoom(RoomObject room, RoomObject.WallStatus wall){
        return dungeonLogic.SetupRoom(room, wall); // use dungeonLogic to figure out the walls and corner statuses
    }
    public List<RoomObject> SpawnRoom(RoomObject room, RoomObject.WallStatus[] walls){ // overload for a lot of rooms
        List<RoomObject> roomsToReturn = new List<RoomObject>(); // create the list of room objects
        RoomObject roomSpawned; // create another variable so its not initialized for each wall

        foreach(RoomObject.WallStatus wall in walls){ // loop through each wall
            if(wall != RoomObject.WallStatus.closed){ // if its not closed
                roomSpawned = SpawnRoom(room, wall); // recursion, but not
                roomsToReturn.Add(roomSpawned); // adds the room to the list for returning
            }
        }
        return roomsToReturn; // returns all the room objects
    }

    public RoomObject MakeRoomInstance(Vector3Int posInGridCells, List<RoomObject.WallStatus> wallStatuses, List<bool> cornerStatuses, bool disable = true){ // define function with info needed for a room
        if(hardStop>hardStopMax){return null;} // stop the function if too many rooms where spawned

        Vector3Int roomPos = posInGridCells; // get room position in world space

        RoomObject room = ScriptableObject.CreateInstance(roomPos.x.ToString() + ", " + roomPos.y.ToString()) as RoomObject; // create room instance and cast it to room type also name it based on its position
        room.init(roomPos, wallStatuses, cornerStatuses, false, false); // setup the room
        roomManager.AddRoom(room, roomPos.x, roomPos.y); // adds the room to the room matrix

        hardStop++; // keep track of the room count
        return room; // return the room
    }


    public void StartDungeon(){
        SpawnRoom(startingRoom, startingRoom.GetWallsStatus()); // spawn the starting room
    }
}
