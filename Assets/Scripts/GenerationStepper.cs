using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationStepper : MonoBehaviour
{
    bool nextGen = false;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown("space")){ // readty for next gen when space is pressed
            nextGen = true;
        }
    }

    public bool IsNextGen(){
        if(!nextGen){return false;} // if not ready return false then stop
        nextGen = false; // reset nextGen
        return true; // ready for next gen
    }
}
