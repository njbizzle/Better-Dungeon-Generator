using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomObject : MonoBehaviour
{
    public enum WallStatus {closed, open, door}; // define a wallstatus varible type

    [Header("Walls")]

    [SerializeField] WallStatus topWall; // define wallStaus variables 
    [SerializeField] WallStatus bottomWall;
    [SerializeField] WallStatus leftWall;
    [SerializeField] WallStatus rightWall;

    [Header("Corners")]

    [SerializeField] bool topLeftCorner; // define bool for the state of the corners
    [SerializeField] bool topRightCorner;
    [SerializeField] bool bottomLeftCorner;
    [SerializeField] bool bottomRightCorner;

    [Header("Draw/Redraw Room")]
    [SerializeField] bool drawRoom; // when true will draw the room using room drawer

    // references
    [Header("References")]

    [SerializeField] RoomDrawer roomDrawer;

    void Start()
    {
        roomDrawer = FindObjectOfType<RoomDrawer>();
    }

    void Update()
    {
        if (drawRoom) {
            roomDrawer.PlaceRoomObjectTiles(this); // pass in self to draw in tiles to the screen
            drawRoom = false; // disable it, like a button
        }
    }

    public WallStatus[] GetWallStatus(){
        WallStatus[] wallStatuses = new WallStatus[] 
            {topWall, bottomWall, leftWall, rightWall}; // make array of wall statuses

        return wallStatuses; // return created array
    }

    public bool[] GetCornersStatus(){
        bool[] cornerStatuses = new bool[] 
            {topLeftCorner, topRightCorner, bottomLeftCorner, bottomRightCorner}; // make array of corner bools

        return cornerStatuses; // return created array 
    }

    public Vector3Int GetPosInInts(){
        Vector3Int postionInt = new Vector3Int(
            Mathf.RoundToInt(transform.position.x), // get postion in integers
            Mathf.RoundToInt(transform.position.y), 
            Mathf.RoundToInt(transform.position.z));

        return postionInt; // returns created vector3int 
    }
}
