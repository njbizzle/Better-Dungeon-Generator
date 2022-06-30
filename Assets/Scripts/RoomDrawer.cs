using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomDrawer : MonoBehaviour
{
    [SerializeField] Tilemap borderTilemap;
    [SerializeField] Tilemap insideTilemap;

    [SerializeField] TileBase borderTile;
    [SerializeField] TileBase insideTile;

    [SerializeField] int roomDiameter;
    int wallLength;
    
    [SerializeField] int doorDiameter;
    int doorLength;

    // references
    [Header("References")]

    [SerializeField] GridLayout gridLayout;

    void Start()
    {
    }

    void Update()
    {
    }

    public void PlaceRoomObjectTiles(RoomObject room){

        // --get info on room--

        RoomObject.WallStatus[] wallStatuses = room.GetWallStatus(); // get array of the statuses of the differnt wall with type wallstatus
        
        RoomObject.WallStatus roomTopWall = wallStatuses[0]; // split array into differnt variables
        RoomObject.WallStatus roomBottomWall = wallStatuses[1];
        RoomObject.WallStatus roomLeftWall = wallStatuses[2];
        RoomObject.WallStatus roomRightWall = wallStatuses[3];

        bool[] cornerStatuses = room.GetCornersStatus(); // get status of corners

        bool roomTopLeftCorner = cornerStatuses[0]; // split array into differnt variables
        bool roomTopRightCorner = cornerStatuses[1];
        bool roomBottomLeftCorner = cornerStatuses[2];
        bool roomBottomRightCorner = cornerStatuses[3];

        Vector3Int roomPos = room.GetPosInInts(); // get postion of room in integers
        Vector3Int roomPosInGridCells = gridLayout.WorldToCell(roomPos); // get room position in grid cells        

        wallLength = roomDiameter*2+1; // wall length
        doorLength = doorDiameter*2+1; // door length

        // --place tiles--

        List<List<Vector3Int>> wallPoeses2d = PlaceWall(roomPosInGridCells); // get a 2d list of the differnt positions for the wall tiles

        int wallIndex = 0;

        foreach (List<Vector3Int> wallPosList in wallPoeses2d){ // iterate though the differnt lists of wall tile positions

            if (wallPosList.Count % 2 == 0){ // if the remainder of wallPosList/2 is not 0 (its even)
                Debug.Log("there is a problem"); // this should not happen
            }

            else if (wallStatuses[wallIndex] == RoomObject.WallStatus.closed){ // if the wall is closed
                BulkPlace(wallPosList); // call bulk place with the list for the walls
            }
            else if (wallStatuses[wallIndex] == RoomObject.WallStatus.open){ // if the wall is closed
                wallPosList.RemoveAt(0); // remove the corners, or the first and last tiles, from the list
                wallPosList.RemoveAt(wallPosList.Count - 1); // same as above but adjust for indexes starting at 0 

                BulkPlace(wallPosList, null, null); // if open make sure there are no tiles, pass in null for border and inside tile
            }
            else if (wallStatuses[wallIndex] == RoomObject.WallStatus.door){ // if the wall is closed
                
                List<Vector3Int> doorPoses = new List<Vector3Int>(); // define a list to keep track of the empty door spots
                int doorStartIndex = roomDiameter - doorDiameter; // the index where the door tiles start at

                for (int i = 0; i < doorLength; i++){ // loop though the length of the door
                    doorPoses.Add(wallPosList[doorStartIndex]); // add the postion to the list of door tile positions
                    wallPosList.RemoveAt(doorStartIndex); // find the bottom of the door and remove it
                }

                foreach (Vector3Int doorPos in doorPoses){ // go though all the door tiles postions
                    DeleteTile(doorPos); // delete the door tile
                }
                BulkPlace(wallPosList); // place the updated list
            }
            else {
                Debug.Log("there is a problem"); // this should not happen
            }
            wallIndex++; // add 1 to the index (at the end of the loop)
        }

        // --place corners--

        if(!roomTopLeftCorner){ // if the corner shouldn't be there
            DeleteTile(new Vector3Int(
                roomPosInGridCells.x - roomDiameter, 
                roomPosInGridCells.y + roomDiameter, 
                roomPosInGridCells.z)); // replace it with nothing
        }
        if(!roomTopRightCorner){ // if the corner shouldn't be there
            DeleteTile(new Vector3Int(
                roomPosInGridCells.x + roomDiameter, 
                roomPosInGridCells.y + roomDiameter, 
                roomPosInGridCells.z)); // replace it with nothing
        }
        if(!roomBottomLeftCorner){ // if the corner shouldn't be there
            DeleteTile(new Vector3Int(
                roomPosInGridCells.x - roomDiameter, 
                roomPosInGridCells.y - roomDiameter, 
                roomPosInGridCells.z)); // replace it with nothing
        }
        if(!roomBottomRightCorner){ // if the corner shouldn't be there
            DeleteTile(new Vector3Int(
                roomPosInGridCells.x + roomDiameter, 
                roomPosInGridCells.y - roomDiameter, 
                roomPosInGridCells.z)); // replace it with nothing
        }
        
    }

    private List<List<Vector3Int>> PlaceWall(Vector3Int roomPosInGridCells){

        List<Vector3Int> topWallPoses = new List<Vector3Int>(); // define lists for the differnt cell postions of the walls
        List<Vector3Int> bottomWallPoses = new List<Vector3Int>(); 
        List<Vector3Int> leftWallPoses = new List<Vector3Int>();
        List<Vector3Int> rightWallPoses = new List<Vector3Int>();

        for (int i = 0; i < wallLength; i++){ // loop as many times as the the length of the wall

            topWallPoses.Add(new Vector3Int(
                roomPosInGridCells.x - roomDiameter + i, // this is hard to explain in comments, so idk
                roomPosInGridCells.y + roomDiameter, 
                roomPosInGridCells.z));

            bottomWallPoses.Add(new Vector3Int(
                roomPosInGridCells.x - roomDiameter + i,
                roomPosInGridCells.y - roomDiameter, 
                roomPosInGridCells.z));

            leftWallPoses.Add(new Vector3Int(
                roomPosInGridCells.x - roomDiameter,
                roomPosInGridCells.y - roomDiameter + i, 
                roomPosInGridCells.z));

            rightWallPoses.Add(new Vector3Int(
                roomPosInGridCells.x + roomDiameter,
                roomPosInGridCells.y - roomDiameter + i, 
                roomPosInGridCells.z));
        }

        List<List<Vector3Int>> returnList = new List<List<Vector3Int>>(); // make a 2d list that will be returned

        returnList.Add(topWallPoses); // add all of the created lists
        returnList.Add(bottomWallPoses);
        returnList.Add(leftWallPoses);
        returnList.Add(rightWallPoses);

        return returnList; // return

    }

    private void BulkPlace(List<Vector3Int> cellPositions){

        foreach (Vector3Int cellPos in cellPositions){ // loop though all cell position passed in
            PlaceTile(borderTilemap, borderTile, cellPos); // place border tile at cell pos
            PlaceTile(insideTilemap, insideTile, cellPos); // place border tile at cell pos
        }
    }
    private void BulkPlace(List<Vector3Int> cellPositions, TileBase placeBorderTile, TileBase placeInsideTile){ // overload if inside tile is specified

        foreach (Vector3Int cellPos in cellPositions){ // loop though all cell position passed in
            PlaceTile(borderTilemap, placeBorderTile, cellPos); // place border tile at cell pos
            PlaceTile(insideTilemap, placeInsideTile, cellPos); // place border tile at cell pos
        }
    }

    private void PlaceTile(Tilemap tilemap, TileBase tileBase, Vector3 position){

        Vector3Int postionInt = new Vector3Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y), Mathf.RoundToInt(position.z)); // get postion in integers if vector3 is passed in
        tilemap.SetTile(postionInt, tileBase); // set tile at postion to tileBase
    }
    private void PlaceTile(Tilemap tilemap, TileBase tileBase, Vector3Int cellPosition){ // overload if tile is already an int

        tilemap.SetTile(cellPosition, tileBase); // set tile at postion to tileBase
    }
    private void DeleteTile(Vector3Int cellPosition){
        borderTilemap.SetTile(cellPosition, null); // set the border to nothing
        insideTilemap.SetTile(cellPosition, null); // set the inside to nothing
    }
}
