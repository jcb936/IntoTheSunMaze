using UnityEngine;

// Cell wrapper class for the maze generation algorithm. It has references to each cell's wall.

public class Cell : MonoBehaviour {

    // Public variables are serialiazed in the inspector.

    public GameObject wallL;

    public GameObject wallR;

    public GameObject wallU;

    public GameObject wallD;

}
