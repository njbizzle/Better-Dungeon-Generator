using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{

    [SerializeField] RoomObject[][] roomMatrix; // make an array of arrays of rooms

    [SerializeField] int roomBorder; // cant have negative indexes so if it is, then add this to the absoulute value

    // references
    [Header("References")]

    [SerializeField] GridLayout gridLayout;
    [SerializeField] RoomDrawer roomDrawer;
    
    void Start()
    {
        // assign refernces
        gridLayout = FindObjectOfType<GridLayout>();
        roomDrawer = FindObjectOfType<RoomDrawer>();

        Debug.Log(gridLayout.cellSize);
    }

    void Update()
    {
        
    }

    public void AddRoom(RoomObject room, int xIndex, int yIndex){
        if (xIndex < 0){ // if less than 0
            xIndex = Mathf.Abs(xIndex + roomBorder); // manages negative indexes x
        }
        if (yIndex < 0){ // if less than 0
            yIndex = Mathf.Abs(yIndex + roomBorder); // manages negative indexes y
        }
        roomMatrix[xIndex][yIndex] = room; // set the room at the pos to that room
    }

    public RoomObject GetRoom(int xIndex, int yIndex){
        if (xIndex < 0){ // if less than 0
            xIndex = Mathf.Abs(xIndex + roomBorder); // manages negative indexes x
        }
        if (yIndex < 0){ // if less than 0
            yIndex = Mathf.Abs(yIndex + roomBorder); // manages negative indexes y
        }
        
        try{ // tries to do stuff
            RoomObject returnRoom = roomMatrix[xIndex][yIndex]; // stores the room at that pos
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
}
