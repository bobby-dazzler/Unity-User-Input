using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEventsFramework;

[CreateAssetMenu(menuName="Input/Actions/Grid Editor Process Input")]
public class GridEditorProcessInputAct : Action {
    
    public GameInputData gameInputData;

    public GridChunksRuntimeSet allGridChunks;

    public GridData gridData;

    public float valueIncrement = 0.01f;

    public bool isAddingValue = false;

    TileMgrState tileState;

    public override void Act (StateController controller) {

        if (gameInputData.isMouse0 || gameInputData.isMouse1) {
            GetMarchingPointAtMousePosition();

            if (gameInputData.isMouse0) {
                valueIncrement = 0.01f;
            } else if (gameInputData.isMouse1) {
                valueIncrement = -0.01f;
            }

            ChangePointValue();

            ReDrawMesh();

            gameInputData.isMouse0 = false;
            gameInputData.isMouse1 = false;
        }        

        if (gameInputData.isMouse1Up) {
/*             GetMarchingPointAtMousePosition();

            Debug.Log(gameInputData.activeMarchingPoint.noise);
            //Debug.Log(gameInputData.activeMarchingPoint.position.y % gameInputData.activeMarchingPoint.parms.x);
            //Debug.Log(gameInputData.activeMarchingPoint.FinalValue);

            gameInputData.isMouse1Up = false; */
        } 
    }

    public override void CreateHistory (StateController controller) {

    }

    void GetMarchingPointAtMousePosition() {
            GridChunk chunk = allGridChunks.GetChunkFromPosition(gameInputData.mousePosition, gridData);
            StateController chunkController = chunk.gridChunkObject.GetComponent<StateController>();
            ChunkMgrState chunkState = (ChunkMgrState)chunkController.currentState;

            GridTile tile = chunkState.allTiles.GetTileFromPosition(chunk.coord, gameInputData.mousePosition, gridData);
            gameInputData.activeTile = tile;
            StateController tileController = tile.tileObject.GetComponent<StateController>();
            tileState = (TileMgrState)tileController.currentState;

            MarchingPoint point = tileState.marchingPoints.GetPointFromPosition(tile.tileObject.transform.InverseTransformPoint(gameInputData.mousePosition), chunkState.gridData);
            int index = tileState.marchingPoints.GetPointGOIndexFromCoord(point.localCoord.x, point.localCoord.z);

            gameInputData.activeMarchingPoint = point;
            /* GameObject marchingPointObject = tileState.marchingPointGOs.items[index];
            if (marchingPointObject.activeSelf) {
                marchingPointObject.SetActive(false);
            } else {
                marchingPointObject.SetActive(true);
            } */

            //Debug.Log("Mouse click at World Coord: " + tile.WorldCoord + ", Local Coord: " + tile.coord + ", Point Coord: " + point.localCoord);
    }

    void ChangePointValue () {
        //Debug.Log(valueIncrement);

        int pointsY = (int)gridData.gridSize.y * (int)gridData.tilesPerChunk.y * gridData.maximumMarchingLOD;

        for (int y = gameInputData.activeMarchingPoint.worldCoord.y; y < pointsY; y++) {
            int index = tileState.allMarchingPoints.GetPointIndexFromCoord(gameInputData.activeMarchingPoint.worldCoord.x, y, gameInputData.activeMarchingPoint.worldCoord.z);
            MarchingPoint point = tileState.allMarchingPoints.items[index];
            point.additionalValue -= valueIncrement;

            index = tileState.allMarchingPoints.GetPointIndexFromCoord(gameInputData.activeMarchingPoint.worldCoord.x + 1, y, gameInputData.activeMarchingPoint.worldCoord.z);
            point = tileState.allMarchingPoints.items[index];
            point.additionalValue -= valueIncrement / 2;

            index = tileState.allMarchingPoints.GetPointIndexFromCoord(gameInputData.activeMarchingPoint.worldCoord.x - 1, y, gameInputData.activeMarchingPoint.worldCoord.z);
            point = tileState.allMarchingPoints.items[index];
            point.additionalValue -= valueIncrement / 2;

            index = tileState.allMarchingPoints.GetPointIndexFromCoord(gameInputData.activeMarchingPoint.worldCoord.x, y, gameInputData.activeMarchingPoint.worldCoord.z + 1);
            point = tileState.allMarchingPoints.items[index];
            point.additionalValue -= valueIncrement / 2;

            index = tileState.allMarchingPoints.GetPointIndexFromCoord(gameInputData.activeMarchingPoint.worldCoord.x, y, gameInputData.activeMarchingPoint.worldCoord.z - 1);
            point = tileState.allMarchingPoints.items[index];
            point.additionalValue -= valueIncrement / 2;

            index = tileState.allMarchingPoints.GetPointIndexFromCoord(gameInputData.activeMarchingPoint.worldCoord.x + 1, y, gameInputData.activeMarchingPoint.worldCoord.z + 1);
            point = tileState.allMarchingPoints.items[index];
            point.additionalValue -= valueIncrement / 2;

            index = tileState.allMarchingPoints.GetPointIndexFromCoord(gameInputData.activeMarchingPoint.worldCoord.x - 1, y, gameInputData.activeMarchingPoint.worldCoord.z - 1);
            point = tileState.allMarchingPoints.items[index];
            point.additionalValue -= valueIncrement / 2;

            index = tileState.allMarchingPoints.GetPointIndexFromCoord(gameInputData.activeMarchingPoint.worldCoord.x - 1, y, gameInputData.activeMarchingPoint.worldCoord.z + 1);
            point = tileState.allMarchingPoints.items[index];
            point.additionalValue -= valueIncrement / 2;

            index = tileState.allMarchingPoints.GetPointIndexFromCoord(gameInputData.activeMarchingPoint.worldCoord.x + 1, y, gameInputData.activeMarchingPoint.worldCoord.z - 1);
            point = tileState.allMarchingPoints.items[index];
            point.additionalValue -= valueIncrement / 2;

            


/*             index = tileState.allMarchingPoints.GetPointIndexFromCoord(gameInputData.activeMarchingPoint.worldCoord.x + 2, y, gameInputData.activeMarchingPoint.worldCoord.z);
            point = tileState.allMarchingPoints.items[index];
            point.additionalValue -= valueIncrement / 4;

            index = tileState.allMarchingPoints.GetPointIndexFromCoord(gameInputData.activeMarchingPoint.worldCoord.x - 2, y, gameInputData.activeMarchingPoint.worldCoord.z);
            point = tileState.allMarchingPoints.items[index];
            point.additionalValue -= valueIncrement / 4;

            index = tileState.allMarchingPoints.GetPointIndexFromCoord(gameInputData.activeMarchingPoint.worldCoord.x, y, gameInputData.activeMarchingPoint.worldCoord.z + 2);
            point = tileState.allMarchingPoints.items[index];
            point.additionalValue -= valueIncrement / 4;

            index = tileState.allMarchingPoints.GetPointIndexFromCoord(gameInputData.activeMarchingPoint.worldCoord.x, y, gameInputData.activeMarchingPoint.worldCoord.z - 2);
            point = tileState.allMarchingPoints.items[index];
            point.additionalValue -= valueIncrement / 4;

            index = tileState.allMarchingPoints.GetPointIndexFromCoord(gameInputData.activeMarchingPoint.worldCoord.x + 2, y, gameInputData.activeMarchingPoint.worldCoord.z + 2);
            point = tileState.allMarchingPoints.items[index];
            point.additionalValue -= valueIncrement / 4;

            index = tileState.allMarchingPoints.GetPointIndexFromCoord(gameInputData.activeMarchingPoint.worldCoord.x - 2, y, gameInputData.activeMarchingPoint.worldCoord.z - 2);
            point = tileState.allMarchingPoints.items[index];
            point.additionalValue -= valueIncrement / 4; */
        }   
    }

    void ReDrawMesh() {
        StateController tileController = gameInputData.activeTile.tileObject.GetComponent<StateController>();

        tileController.CallCurrentStateActionAtIndex(7); // MarchTileAct
        tileController.CallCurrentStateActionAtIndex(8); // DrawMeshACt
    }
}
