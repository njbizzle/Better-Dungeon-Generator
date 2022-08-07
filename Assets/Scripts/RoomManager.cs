using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{

    [SerializeField] RoomObject[,] roomMatrix; // define the array

    [SerializeField] int roomBorder; // cant have negative indexes so if it is, then add this to the absoulute value

    // references
    [Header("References")]

    [SerializeField] GridLayout gridLayout;
    [SerializeField] RoomDrawer roomDrawer;

    [SerializeField] int currentRoomID = 0;
    void Awake()
    {
        // assign refernces
        gridLayout = FindObjectOfType<GridLayout>();
        roomDrawer = FindObjectOfType<RoomDrawer>();

        roomMatrix = new RoomObject[roomBorder*2, roomBorder*2]; // define the size of the room matrix
    }

    void Start()
    {

    }

    void Update()
    {
        
    }

    public void AddRoom(RoomObject room, Vector3Int roomPos){
        if (roomPos.x < 0){ // if less than 0
            roomPos.x = Mathf.Abs(roomPos.x + roomBorder); // manages negative indexes x
        }
        if (roomPos.y < 0){ // if less than 0
            roomPos.y = Mathf.Abs(roomPos.y + roomBorder); // manages negative indexes y
        }
        roomMatrix[roomPos.x, roomPos.y] = room; // set the room at the pos to that room
    }

    public RoomObject GetRoom(Vector3Int roomPos){
        if (roomPos.x < 0){ // if less than 0
            roomPos.x = Mathf.Abs(roomPos.x + roomBorder); // manages negative indexes x
        }
        if (roomPos.y < 0){ // if less than 0
            roomPos.y = Mathf.Abs(roomPos.y + roomBorder); // manages negative indexes y
        }
        
        try{ // tries to do stuff
            RoomObject returnRoom = roomMatrix[roomPos.x, roomPos.y]; // stores the room at that pos
            return returnRoom; //  return the room
        }
        catch{ // if there is an error, meaning no room, return null
            return null; // else return nothing
        }
    }

    
    public Vector3Int GridCellToMatrixPos(Vector3Int gridCellPos){
        int roomDiameter = roomDrawer.GetRoomDiameter(); // get the room diameter
        return new Vector3Int( // divide the values and return the new vector
            gridCellPos.x/roomDiameter, 
            gridCellPos.y/roomDiameter, 
            gridCellPos.z);
    }

    public int MakeRoomID(){
        currentRoomID++; //  get the next room ID (to avoid overlap)
        return currentRoomID; // return the current room ID
    }
}
