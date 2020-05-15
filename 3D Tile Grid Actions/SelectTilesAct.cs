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

        if (mouseDownTile != currentTile) {

            controller.CallCurrentStateActionAtIndex(6); // Deselect tiles            
            
            //Coord mouseDownWorldCoord = gridData.LocalToWorldCoord(mouseDownTile.coord.x, mouseDownTile.coord.y, mouseDownTile.coord.z, mouseDownTile.chunk.coord);
            //Coord currentWorldCoord = gridData.LocalToWorldCoord(currentTile.coord.x, currentTile.coord.y, currentTile.coord.z, currentTile.chunk.coord);
            Coord mouseDownWorldCoord = new Coord(mouseDownTile.worldCoord.x, mouseDownTile.worldCoord.y, mouseDownTile.worldCoord.z);
            Coord currentWorldCoord = new Coord(currentTile.worldCoord.x, currentTile.worldCoord.y, currentTile.worldCoord.z);
            if (currentWorldCoord.x < mouseDownWorldCoord.x - gameInputData.tileSelectionClamp.x) {
                currentWorldCoord.x =  mouseDownWorldCoord.x - gameInputData.tileSelectionClamp.x;
            }
            if (currentWorldCoord.x > mouseDownWorldCoord.x + gameInputData.tileSelectionClamp.x) {
                currentWorldCoord.x = mouseDownWorldCoord.x + gameInputData.tileSelectionClamp.x;
            }
            if (currentWorldCoord.z < mouseDownWorldCoord.z - gameInputData.tileSelectionClamp.z) {
                currentWorldCoord.z =  mouseDownWorldCoord.z - gameInputData.tileSelectionClamp.z;
            }
            if (currentWorldCoord.z > mouseDownWorldCoord.z + gameInputData.tileSelectionClamp.z) {
                currentWorldCoord.z = mouseDownWorldCoord.z + gameInputData.tileSelectionClamp.z;
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
                        tile.SetProjector(true, 1);
                        tile.isSelected = true;
                    }
                }
            } 
    
/*         int minX = Mathf.Min(mouseDownWorldCoord.x, currentWorldCoord.x);
        int minY = Mathf.Min(mouseDownWorldCoord.y, currentWorldCoord.y);
        int minZ = Mathf.Min(mouseDownWorldCoord.z, currentWorldCoord.z);
        int maxX = Mathf.Max(mouseDownWorldCoord.x, currentWorldCoord.x);
        int maxY = Mathf.Max(mouseDownWorldCoord.y, currentWorldCoord.y);
        int maxZ = Mathf.Max(mouseDownWorldCoord.z, currentWorldCoord.z);

        for (int y = minY; y <= maxY; y++) {
            for (int z = minZ; z <= maxZ; z++) {
                for (int x = minX; x <= maxX; x++) {
                    Coord tileWorldCoord = new Coord(x,y,z);
                    GridTile tile = gridData.GetTileFromWorldCoord(tileWorldCoord);
                    gameInputData.selectedTiles.Add(tile);
                    tile.SetProjector(true, 1);
                    tile.isSelected = true;
                }
            }
        } */
            previousTile = currentTile;
        }
    }
    
    public override void CreateHistory (StateController controller) {

    }
}
