using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonLogic : MonoBehaviour
{
    [SerializeField] int generationMode;
    [SerializeField] string generationModes;

    [SerializeField] RoomObject testRoom;

    // references
    [Header("References")]

    [SerializeField] RoomObjectCreator roomObjectCreator;

    void Start()
    {
        roomObjectCreator = FindObjectOfType<RoomObjectCreator>();
    }

    void Update()
    {
        
    }

    public RoomObject SetupRoom(RoomObject room, RoomObject.WallStatus wall){
        return testRoom;
    }
}
