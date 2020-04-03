using System.Collections.Generic;
using UnityEngine;
using AlgoConstants;

// MazeMaker is where the maze generation algorithm is implemented. It spawns
// the actual maze.

public class MazeMaker : MonoBehaviour {

    // Make it a singleton
    public static MazeMaker instance;

    // Initialise global variables, public variables are serialized via the inspector.

    public GameObject[] floorTiles;

    // Lists are used by the maze algorithm the generate the maze.

    private List<Vector3> grid;

    private List<GameObject> walls;

    private List<Vector2> marked;

    // boardHolder is a wrapper parent object for the cells in final gameobjects hierarchy.

    private Transform boardHolder;

    // Initialise

    private void Awake() {
        boardHolder = new GameObject("Board").transform;
        if (instance == null)
            instance = this;
        grid = new List<Vector3>();
        walls = new List<GameObject>();
        marked = new List<Vector2>();
    }

    // Actual maze generator, using Prim's algorithm, with the possibility to extend it with other algorithms
    // via the switch statement.

    public void GenerateMaze(int height, int width, int algorithm) {
        switch (algorithm) {
            case Constants.PRIM_ALGO:
                GenerateMazePrims(height, width);
                break;
        }
    }

    // Initialise grid of Vector3 position which will make our grid.

    private void InitialiseGrid(int height, int width) {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                Vector3 vec = new Vector3(x, y, 0);
                grid.Add(vec);
            }
        }
    }

    // Implementation of Prim's algorithm for maze generation. Each cell is made by a
    // Cell object which has reference to the wall around it, to smooth the implementation.

    private void GenerateMazePrims(int height, int width) {
        InitialiseGrid(height, width);
        Vector2 start = grid[(int)Random.Range(0, grid.Count)];
        Cell startCell = MarkGridCell(start);

        while (walls.Count > 0) {
            GameObject wall = walls[(int)Random.Range(0, walls.Count)];
            Vector2 direction = new Vector2(0, 0);
            Vector3 newCellPosition = new Vector3();

            switch (WhichWall(wall, out direction)) {
                case 0:
                    newCellPosition = wall.transform.parent.transform.position + (Vector3)direction;
                    if (grid.Contains(newCellPosition) && !marked.Contains(newCellPosition)) {
                        Cell cellToPut = MarkGridCell(newCellPosition);
                        cellToPut.wallR.SetActive(false);
                        walls.Remove(cellToPut.wallR);
                        wall.SetActive(false);
                    }
                    break;
                case 1:
                    newCellPosition = wall.transform.parent.transform.position + (Vector3)direction;
                    if (grid.Contains(newCellPosition) && !marked.Contains(newCellPosition)) {
                        Cell cellToPut = MarkGridCell(newCellPosition);
                        cellToPut.wallU.SetActive(false);
                        walls.Remove(cellToPut.wallU);
                        wall.SetActive(false);
                    }
                    break;
                case 2:
                    newCellPosition = wall.transform.parent.transform.position + (Vector3)direction;
                    if (grid.Contains(newCellPosition) && !marked.Contains(newCellPosition)) {
                        Cell cellToPut = MarkGridCell(newCellPosition);
                        cellToPut.wallL.SetActive(false);
                        walls.Remove(cellToPut.wallL);
                        wall.SetActive(false);
                    }
                    break;
                case 3:
                    newCellPosition = wall.transform.parent.transform.position + (Vector3)direction;
                    if (grid.Contains(newCellPosition) && !marked.Contains(newCellPosition)) {
                        Cell cellToPut = MarkGridCell(newCellPosition);
                        cellToPut.wallD.SetActive(false);
                        walls.Remove(cellToPut.wallD);
                        wall.SetActive(false);
                    }
                    break;
            }

            walls.Remove(wall);
        }
    }

    // Mark grid cell, in accord to Prim's algorithm

    private Cell MarkGridCell(Vector2 cellPosition) {
        Cell newCell = Instantiate(floorTiles[Random.Range(0, floorTiles.Length)], cellPosition, Quaternion.identity).GetComponent<Cell>();
        newCell.transform.SetParent(boardHolder);
        marked.Add(cellPosition);
        walls.Add(newCell.wallL);
        walls.Add(newCell.wallR);
        walls.Add(newCell.wallU);
        walls.Add(newCell.wallD);
        return newCell;
    }

    // Determine which wall has been selected by the algorithm
    
    private int WhichWall(GameObject wall, out Vector2 direction) {
        Cell parentCell = wall.GetComponentInParent<Cell>();
        if (wall == parentCell.wallL) {
            direction = Vector2.left;
            return 0;
        }
        else if (wall == parentCell.wallD) {
            direction = Vector2.down;
            return 1;
        }
        else if (wall == parentCell.wallR) {
            direction = Vector2.right;
            return 2;
        }
        else {
            direction = Vector2.up;
            return 3;
        }
    }
}
