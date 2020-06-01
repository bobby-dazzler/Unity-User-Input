using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEventsFramework;
using Unity3DTileGrid;
using UnityEngine.EventSystems;
using Unity.Mathematics;

[CreateAssetMenu(menuName="Input/Actions/Display Current Tile")]
public class DisplayCurrentTile : Action {
   
    public GridData gridData;

    public GameInputData gameInputData;

    GridTile previousTile;

    List<GridTile> enabledProjectors = new List<GridTile>();

    public Vector3Int tilesToDisplay;

    public Vector3Int clampSelectionAxis;

    public override void Act(StateController controller) {
        for (int i = 0; i < enabledProjectors.Count; i++) {
            if (!enabledProjectors[i].isSelected) {
                for (int d = 0; d < 6; d++) {
                    enabledProjectors[i].SetProjector(false, d);
                }
            }
        }
        enabledProjectors.Clear();

        if (!gameInputData.isMouse0 && !EventSystem.current.IsPointerOverGameObject()) {
            GridTile currentTile = gridData.GetTileFromPosition(gameInputData.mouseCurrentPosition);
            if (currentTile != null) {
                int numTilesX = gridData.numChunks.x * gridData.numTiles.x;
                int numTilesZ = gridData.numChunks.z * gridData.numTiles.z;

                if (currentTile.worldCoord.x > tilesToDisplay.x / 2 && currentTile.worldCoord.x < numTilesX - tilesToDisplay.x / 2 &&  currentTile.worldCoord.z > tilesToDisplay.z / 2 && currentTile.worldCoord.z < numTilesZ - tilesToDisplay.z / 2) {
                    
                    if (gameInputData.horizontalTileSelection) {
                        gameInputData.tileOffsetsToSelect = new Vector3Int(tilesToDisplay.z, tilesToDisplay.y, tilesToDisplay.x);
                        gameInputData.tileSelectionClamp = new Vector3Int(clampSelectionAxis.z, clampSelectionAxis.y, clampSelectionAxis.x);
                    } else {
                        gameInputData.tileOffsetsToSelect = new Vector3Int(tilesToDisplay.x, tilesToDisplay.y, tilesToDisplay.z);
                        gameInputData.tileSelectionClamp = new Vector3Int(clampSelectionAxis.x, clampSelectionAxis.y, clampSelectionAxis.z);
                    }

                    int xOffset = gameInputData.tileOffsetsToSelect.x / 2;
                    int zOffset = gameInputData.tileOffsetsToSelect.z / 2;
                    for (int z = currentTile.worldCoord.z - zOffset; z <= currentTile.worldCoord.z + zOffset; z++) {
                        for (int x = currentTile.worldCoord.x - xOffset; x <= currentTile.worldCoord.x + xOffset; x++) {
                            
                            GridTile neighbour = gridData.GetTileFromWorldCoord(new Coord(x, currentTile.worldCoord.y, z));
                            DisplayTile(neighbour, gameInputData.currentFace);
                        }
                    }

                    previousTile = currentTile; 
                }
            }
        }
    }

    void DisplayTile (GridTile tile, GridDirection direction) {
        tile.SetProjector(true, direction);
        enabledProjectors.Add(tile);
    }

    public override void CreateHistory (StateController controller) {

    }
}
