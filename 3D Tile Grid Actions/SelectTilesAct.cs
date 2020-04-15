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
        for (int i = 0; i < gameInputData.selectedTiles.Count; i++) {
            gameInputData.selectedTiles[i].SetProjector(false, 1);
        }
        gameInputData.selectedTiles.Clear();

        GridTile mouseDownTile = gridData.GetTileFromPosition(gameInputData.mouseDownPosition);
        GridTile currentTile = gridData.GetTileFromPosition(gameInputData.mouseCurrentPosition);
        Coord mouseDownWorldCoord = gridData.LocalToWorldCoord(mouseDownTile.coord.x, mouseDownTile.coord.y, mouseDownTile.coord.z, mouseDownTile.chunk.coord);
        Coord currentWorldCoord = gridData.LocalToWorldCoord(currentTile.coord.x, currentTile.coord.y, currentTile.coord.z, currentTile.chunk.coord);
        int minX = Mathf.Min(mouseDownWorldCoord.x, currentWorldCoord.x);
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
                }
            }
        }
        previousTile = currentTile;
    }
    
    public override void CreateHistory (StateController controller) {

    }
}
