using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu()]
public class RoomObject : ScriptableObject
{
    public enum WallStatus {closed, open, door}; // define a wallstatus varible type
    [SerializeField] Vector3Int centerCellPosition;

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

    [Header("Tile Drawn")]
    [SerializeField] List<Vector3Int> tilePoints; // stores postions of all tiles associated with the room

    // references
    [Header("References")]

    [SerializeField] RoomDrawer roomDrawer;

    public void init(Vector3Int pos, List<RoomObject.WallStatus> wallStatuses, List<bool> cornerStatuses){
        centerCellPosition = pos; // update position
        SetWallsAndCorners(wallStatuses, cornerStatuses); // call function to change the walls and corners
    }

    public void SetWallsAndCorners(List<RoomObject.WallStatus> wallStatuses, List<bool> cornerStatuses){
        topWall = wallStatuses[0]; // set wall values
        bottomWall = wallStatuses[1];
        leftWall = wallStatuses[2];
        rightWall = wallStatuses[3];

        topLeftCorner = cornerStatuses[0];
        topRightCorner = cornerStatuses[1];
        bottomLeftCorner = cornerStatuses[2];
        bottomRightCorner = cornerStatuses[3];
    }

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

    public Vector3Int GetPosInGridCells(){
        return centerCellPosition; // returns position
    }
    public List<Vector3Int> GetTilePoints(){
        return tilePoints;
    }

    public void AddTileToList(Vector3Int tilePos){
        tilePoints.Add(tilePos);
    }
    
    public void RemoveTileFromList(Vector3Int tilePos){
        tilePoints.Remove(tilePos);
    }
    
    public void RemoveTilesFromList(List<Vector3Int> tilePoses){
        tilePoints = tilePoints.Except(tilePoses).ToList(); // subtract the lists
        // someone on unity discord said this if its too laggy, too lazy to do rn so ill do it if i have optomizing problems. but apperently im not lazy enough to write this stupidly long comment.
        // unless the ordering of the list doesnt matter, in that case you could do a simple find, swap with last element, then remove the last element
    }
}
