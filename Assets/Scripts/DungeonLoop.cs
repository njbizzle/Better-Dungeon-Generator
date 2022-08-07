using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonLoop : MonoBehaviour
{
    [SerializeField] int hardStop;
    [SerializeField] int hardStopMax;

    [Header("Starting Room")]

    [SerializeField] RoomObject startingRoom;

    // references
    [Header("References")]

    [SerializeField] GridLayout gridLayout;
    [SerializeField] DungeonLogic dungeonLogic;
    [SerializeField] RoomManager roomManager;
    [SerializeField] RoomDrawer roomDrawer;
    [SerializeField] GenerationStepper generationStepper;

    void Awake(){
        // find references
        gridLayout = FindObjectOfType<GridLayout>();
        dungeonLogic = FindObjectOfType<DungeonLogic>();
        roomManager = FindObjectOfType<RoomManager>();
        roomDrawer = FindObjectOfType<RoomDrawer>();
        generationStepper = FindObjectOfType<GenerationStepper>();
    }

    void Start()
    {
        StartCoroutine(StartDungeonLoop()); // Starts the DungeonLoop, duh
    }

    void Update()
    {
        
    }

    IEnumerator StartDungeonLoop()
    {
        Stack<RoomObject> dungeonStack = new Stack<RoomObject>(); // make a stack
        dungeonStack.Push(startingRoom); // add the starting room

        dungeonLogic.SetupRoom(startingRoom); // sets up the starting room

        int wallIndex; // define wall index for the loop

        while (dungeonStack.Count > 0) // while there are rooms left in the queue
        {
            if(hardStop>hardStopMax){Debug.Log("get hardstopped");yield break;} // if the hardStop is too big than break

            yield return new WaitUntil(generationStepper.IsNextGen); // waits until generationstepper says its ok to go to next gen

            RoomObject room = dungeonStack.Pop(); // get a room
            
            wallIndex = 0; // reset wallIndex before loop
            foreach (RoomObject.WallStatus wall in room.GetWallsStatus())
            {
                if(wall != RoomObject.WallStatus.closed){ // if the room isnt closed
                    RoomObject childRoom = dungeonLogic.GenerateChildRoom(room, wall, wallIndex); // use dungeonLogic to figure out the walls and corner statuses
                    if(childRoom != null){
                        dungeonStack.Push(childRoom); // add the child room to the stack
                    }
                }
                wallIndex++; // count up each time
            }
            hardStop++; // keep track of the room count
        }
    }
}
