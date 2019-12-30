using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEventsFramework;

[CreateAssetMenu(menuName="Input/Actions/Grid Editor Process Input")]
public class GridEditorProcessInputAct : Action {
    
    public GameInputData gameInputData;

    public GridChunksRuntimeSet allGridChunks;

    public GridData gridData;

    public MarchingPointRuntimeSet pointsToCheck;

    public MarchingPointRuntimeSet allMarchingPoints;

    public float valueIncrement = 0.01f;

    public bool isAddingValue = false;

    public GameEvent checkPointValuesEvent;

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

            
            checkPointValuesEvent.StartEvent(controller);

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

/*             MarchingPoint point = allMarchingPoints.GetPointFromPosition(gameInputData.mousePosition, gridData);             
            int index = tileState.marchingPoints.GetPointGOIndexFromCoord(point.localCoord.x, point.localCoord.z); */
            //int index = allMarchingPoints.GetPointGOIndexFromCoord(point.localCoord.x, point.localCoord.z);

/*             gameInputData.activeMarchingPoint = point;
            GameObject marchingPointObject = tileState.marchingPointGOs.items[index];
            if (marchingPointObject.activeSelf) {
                marchingPointObject.SetActive(false);
            } else {
                marchingPointObject.SetActive(true);
            }  */

            //Debug.Log("Mouse click at World Coord: " + tile.WorldCoord + ", Local Coord: " + tile.coord + ", Point Coord: " + point.localCoord);
    }

    void ChangePointValue () {
        int pointsY = (int)gridData.gridSize.y * (int)gridData.tilesPerChunk.y * gridData.maximumMarchingLOD;

        for (int y = gameInputData.activeMarchingPoint.worldCoord.y; y < pointsY; y++) {
            //Debug.Log(tileState);
            int index = allMarchingPoints.GetPointIndexFromCoord(gameInputData.activeMarchingPoint.worldCoord.x, y, gameInputData.activeMarchingPoint.worldCoord.z);
            MarchingPoint point = allMarchingPoints.items[index];
            ChangeAdditionalValue(point, valueIncrement);

            index = allMarchingPoints.GetPointIndexFromCoord(gameInputData.activeMarchingPoint.worldCoord.x + 1, y, gameInputData.activeMarchingPoint.worldCoord.z);
            point = allMarchingPoints.items[index];
            ChangeAdditionalValue(point, valueIncrement / 2);

            index = allMarchingPoints.GetPointIndexFromCoord(gameInputData.activeMarchingPoint.worldCoord.x - 1, y, gameInputData.activeMarchingPoint.worldCoord.z);
            point = allMarchingPoints.items[index];
            ChangeAdditionalValue(point, valueIncrement / 2);

            index = allMarchingPoints.GetPointIndexFromCoord(gameInputData.activeMarchingPoint.worldCoord.x, y, gameInputData.activeMarchingPoint.worldCoord.z + 1);
            point = allMarchingPoints.items[index];
            ChangeAdditionalValue(point, valueIncrement / 2);

            index = allMarchingPoints.GetPointIndexFromCoord(gameInputData.activeMarchingPoint.worldCoord.x, y, gameInputData.activeMarchingPoint.worldCoord.z - 1);
            point = allMarchingPoints.items[index];
            ChangeAdditionalValue(point, valueIncrement / 2);

            index = allMarchingPoints.GetPointIndexFromCoord(gameInputData.activeMarchingPoint.worldCoord.x + 1, y, gameInputData.activeMarchingPoint.worldCoord.z + 1);
            point = allMarchingPoints.items[index];
            ChangeAdditionalValue(point, valueIncrement / 2);

            index = allMarchingPoints.GetPointIndexFromCoord(gameInputData.activeMarchingPoint.worldCoord.x - 1, y, gameInputData.activeMarchingPoint.worldCoord.z - 1);
            point = allMarchingPoints.items[index];
            ChangeAdditionalValue(point, valueIncrement / 2);

            index = allMarchingPoints.GetPointIndexFromCoord(gameInputData.activeMarchingPoint.worldCoord.x - 1, y, gameInputData.activeMarchingPoint.worldCoord.z + 1);
            point = allMarchingPoints.items[index];
            ChangeAdditionalValue(point, valueIncrement / 2);

            index = allMarchingPoints.GetPointIndexFromCoord(gameInputData.activeMarchingPoint.worldCoord.x + 1, y, gameInputData.activeMarchingPoint.worldCoord.z - 1);
            point = allMarchingPoints.items[index];
            ChangeAdditionalValue(point, valueIncrement / 2); 

        }   
    }


    void ChangeAdditionalValue (MarchingPoint point, float value) {
        point.AdditionalValue += value;
        pointsToCheck.Add(point);
    }

    void ReDrawMesh() {
        StateController tileController = gameInputData.activeTile.tileObject.GetComponent<StateController>();
        tileController.CallCurrentStateActionAtIndex(7); // MarchTileAct
        tileController.CallCurrentStateActionAtIndex(8); // DrawMeshACt
        // Move MarchingPointGO
        TileMgrState tileMgrState = (TileMgrState)tileController.currentState;
        if (tileMgrState.marchingPointGOs.Count() > 0) {
                tileController.CallCurrentStateActionAtIndex(10); // InsMarchingPointGOsAct
        }

        if (gameInputData.activeTile.GetNeighbour(GridDirection.Xplus) != null && gameInputData.activeTile.GetNeighbour(GridDirection.Xplus).tileObject != null) {
            StateController neighbourController = gameInputData.activeTile.tileObject.GetComponent<StateController>();
/*             Debug.Log(gameInputData.activeTile.coord);
            Debug.Log((int)GridDirection.Xplus);
            Debug.Log(gameInputData.activeTile.GetNeighbour(GridDirection.Xplus).coord); */
            neighbourController = gameInputData.activeTile.GetNeighbour(GridDirection.Xplus).tileObject.GetComponent<StateController>();
            neighbourController.CallCurrentStateActionAtIndex(7); // MarchTileAct
            neighbourController.CallCurrentStateActionAtIndex(8); // DrawMeshACt
        }

        if (gameInputData.activeTile.GetNeighbour(GridDirection.Xminus) != null && gameInputData.activeTile.GetNeighbour(GridDirection.Xminus).tileObject != null) {
            StateController neighbourController = gameInputData.activeTile.tileObject.GetComponent<StateController>();
            //Debug.Log(gameInputData.activeTile.coord);
            //Debug.Log(gameInputData.activeTile.GetNeighbour(GridDirection.Xplus).coord);
            neighbourController = gameInputData.activeTile.GetNeighbour(GridDirection.Xminus).tileObject.GetComponent<StateController>();
            neighbourController.CallCurrentStateActionAtIndex(7); // MarchTileAct
            neighbourController.CallCurrentStateActionAtIndex(8); // DrawMeshACt
        }

        if (gameInputData.activeTile.GetNeighbour(GridDirection.Zplus) != null && gameInputData.activeTile.GetNeighbour(GridDirection.Zplus).tileObject != null) {
            StateController neighbourController = gameInputData.activeTile.tileObject.GetComponent<StateController>();
            //Debug.Log(gameInputData.activeTile.coord);
            //Debug.Log(gameInputData.activeTile.GetNeighbour(GridDirection.Xplus).coord);
            neighbourController = gameInputData.activeTile.GetNeighbour(GridDirection.Zplus).tileObject.GetComponent<StateController>();
            neighbourController.CallCurrentStateActionAtIndex(7); // MarchTileAct
            neighbourController.CallCurrentStateActionAtIndex(8); // DrawMeshACt
        }

        if (gameInputData.activeTile.GetNeighbour(GridDirection.Zminus) != null && gameInputData.activeTile.GetNeighbour(GridDirection.Zminus).tileObject != null) {
            StateController neighbourController = gameInputData.activeTile.tileObject.GetComponent<StateController>();
            //Debug.Log(gameInputData.activeTile.coord);
            //Debug.Log(gameInputData.activeTile.GetNeighbour(GridDirection.Xplus).coord);
            neighbourController = gameInputData.activeTile.GetNeighbour(GridDirection.Zminus).tileObject.GetComponent<StateController>();
            neighbourController.CallCurrentStateActionAtIndex(7); // MarchTileAct
            neighbourController.CallCurrentStateActionAtIndex(8); // DrawMeshACt
        }
    }
}
