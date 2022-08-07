using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonLogic : MonoBehaviour
{
    [SerializeField] int generationMode;
    [SerializeField] string generationModes;

    [SerializeField] RoomObject testRoom;
    [SerializeField] List<RoomObject.WallStatus> testWallStatuses;
    [SerializeField] List<bool> testCornerStatuses;

    Dictionary<int,int> oppositeWallsKey = new Dictionary<int,int>();

    // references
    [Header("References")]

    [SerializeField] GridLayout gridLayout;
    [SerializeField] DungeonLogic dungeonLogic;
    [SerializeField] RoomManager roomManager;
    [SerializeField] RoomDrawer roomDrawer;

    void Awake()
    {
        // find references
        gridLayout = FindObjectOfType<GridLayout>();
        dungeonLogic = FindObjectOfType<DungeonLogic>();
        roomManager = FindObjectOfType<RoomManager>();
        roomDrawer = FindObjectOfType<RoomDrawer>();

        oppositeWallsKey.Add(0,1);
        oppositeWallsKey.Add(1,0);
        oppositeWallsKey.Add(2,3);
        oppositeWallsKey.Add(3,2);
    }

    void Start()
    {

    }

    void Update()
    {
        
    }

    public RoomObject GenerateChildRoom(RoomObject parentRoom, RoomObject.WallStatus parentWallStatus, int parentWallIndex){

        // wall index orders: 0 = top, 1 = bottom, 2 = left, 3 = right

        List<RoomObject.WallStatus> wallStatuses = new List<RoomObject.WallStatus>(){ // define the base list of just a lot of closed walls
            RoomObject.WallStatus.closed, 
            RoomObject.WallStatus.closed, 
            RoomObject.WallStatus.closed, 
            RoomObject.WallStatus.closed
        };
        List<bool> cornerStatuses = new List<bool>(){true, true, true, true}; // define a base list of closed corners

        Vector3Int parentRoomPos = parentRoom.GetPosInGridCells(); // get room position
        Vector3Int roomPos = parentRoomPos;
        int roomDiameter = roomDrawer.GetRoomDiameter(); // get the room diameter

        // offset room position
        if(parentWallIndex == 0){ // top
            roomPos = new Vector3Int(
                parentRoomPos.x, 
                parentRoomPos.y + roomDiameter*2, 
                parentRoomPos.z);
        }
        else if(parentWallIndex == 1){ // bottom
            roomPos = new Vector3Int(
                parentRoomPos.x, 
                parentRoomPos.y - roomDiameter*2, 
                parentRoomPos.z);
        }
        else if(parentWallIndex == 2){ // left
            roomPos = new Vector3Int(
                parentRoomPos.x - roomDiameter*2, 
                parentRoomPos.y, 
                parentRoomPos.z);
        }
        else if(parentWallIndex == 3){ // right
            roomPos = new Vector3Int(
                parentRoomPos.x + roomDiameter*2, 
                parentRoomPos.y, 
                parentRoomPos.z);
        }
        else {Debug.Log("another problem: parentWallIndex = " + parentWallIndex);} // woo hoo

        // make sure rooms line up
        wallStatuses[oppositeWallsKey[parentWallIndex]] = parentWallStatus; // find the opposite of the parent wall (the one facing the spawned room) and set it 

        Vector3Int roomPosMatrixPos = roomManager.GridCellToMatrixPos(roomPos); // get room pos in the room matrix
        if(roomManager.GetRoom(roomPosMatrixPos) != null){ // if there is already a room there don't replace it
            return null;
        }

        // spawn and setup room
        RoomObject room = ScriptableObject.CreateInstance<RoomObject>(); // create room instance and cast it to room type
        room.init(roomPos, wallStatuses, cornerStatuses, roomManager.MakeRoomID()); // setup the room

        SetupRoom(room); // sets up room duh

        Debug.Log(room.GetRoomID());

        return room; // return the room
    }
    public void SetupRoom(RoomObject room){
        Vector3Int roomPosMatrixPos = roomManager.GridCellToMatrixPos(room.GetPosInGridCells()); // get room position in the room array
        room.name = roomPosMatrixPos.x.ToString() + ", " + roomPosMatrixPos.y.ToString(); // name the room
        roomManager.AddRoom(room, roomPosMatrixPos); // adds the room to the room matrix
        roomDrawer.PlaceRoomObjectTiles(room); //draws the room
    }
}
