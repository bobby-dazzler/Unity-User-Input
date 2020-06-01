using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEventsFramework;
using Unity3DTileGrid;

[CreateAssetMenu(menuName="Input/Actions/Select Tiles Act")]
public class SelectTilesAct : Action {
        
    public GameInputData gameInputData;

    public GridData gridData;

    GridTile previousTile;

    public override void Act (StateController controller) {

        GridTile mouseDownTile = gridData.GetTileFromPosition(gameInputData.mouseDownPosition);
        GridTile currentTile = gridData.GetTileFromPosition(gameInputData.mouseCurrentPosition);

        if (mouseDownTile != currentTile || gameInputData.selectedTiles.Count == 0) {

            controller.CallCurrentStateActionAtIndex(6); // Deselect tiles            
            
            Coord mouseDownWorldCoord = new Coord(mouseDownTile.worldCoord.x, mouseDownTile.worldCoord.y, mouseDownTile.worldCoord.z);
            Coord currentWorldCoord = new Coord(currentTile.worldCoord.x, currentTile.worldCoord.y, currentTile.worldCoord.z);
            if (currentWorldCoord.x <= mouseDownWorldCoord.x - gameInputData.tileSelectionClamp.x) {
                currentWorldCoord.x =  mouseDownWorldCoord.x - gameInputData.tileSelectionClamp.x + 1;
            }
            if (currentWorldCoord.x >= mouseDownWorldCoord.x + gameInputData.tileSelectionClamp.x - 1) {
                currentWorldCoord.x = mouseDownWorldCoord.x + gameInputData.tileSelectionClamp.x - 1;
            }
            if (currentWorldCoord.z <= mouseDownWorldCoord.z - gameInputData.tileSelectionClamp.z) {
                currentWorldCoord.z = mouseDownWorldCoord.z - gameInputData.tileSelectionClamp.z + 1;
            }
            if (currentWorldCoord.z >= mouseDownWorldCoord.z + gameInputData.tileSelectionClamp.z - 1) {
                currentWorldCoord.z = mouseDownWorldCoord.z + gameInputData.tileSelectionClamp.z - 1;
            }       

            gameInputData.minX = Mathf.Min(mouseDownWorldCoord.x - (gameInputData.tileOffsetsToSelect.x / 2), currentWorldCoord.x);
            gameInputData.minY = Mathf.Min(mouseDownWorldCoord.y, currentWorldCoord.y);
            gameInputData.minZ = Mathf.Min(mouseDownWorldCoord.z - (gameInputData.tileOffsetsToSelect.z / 2), currentWorldCoord.z);
            gameInputData.maxX = Mathf.Max(mouseDownWorldCoord.x + (gameInputData.tileOffsetsToSelect.x / 2), currentWorldCoord.x);
            gameInputData.maxY = Mathf.Max(mouseDownWorldCoord.y, currentWorldCoord.y);
            gameInputData.maxZ = Mathf.Max(mouseDownWorldCoord.z + (gameInputData.tileOffsetsToSelect.z / 2), currentWorldCoord.z); 
            
            for (int y = gameInputData.minY; y <= gameInputData.maxY; y++) {
                for (int z = gameInputData.minZ; z <= gameInputData.maxZ; z++) {
                    for (int x = gameInputData.minX; x <= gameInputData.maxX; x++) {
                        Coord tileWorldCoord = new Coord(x,y,z);
                        GridTile tile = gridData.GetTileFromWorldCoord(tileWorldCoord);
                        gameInputData.selectedTiles.Add(tile);
                        tile.SetProjector(true, (int)gameInputData.mouseDownFace);
                        tile.isSelected = true;
                    }
                }
            } 

            previousTile = currentTile;
        }
    }
    
    public override void CreateHistory (StateController controller) {

    }
}
