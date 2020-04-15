using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEventsFramework;
using Unity3DTileGrid;

[CreateAssetMenu(menuName="Input/Actions/Change TileType Input")]
public class ChangeTileTypeAct : Action {
    
    public GameInputData gameInputData;

    public TileTypesRuntimeSet allTileTypes;

    public override void Act (StateController controller) {
        GridTile tile;
        
        for (int i = 0; i < gameInputData.selectedTiles.Count; i++) {
            if (allTileTypes.activeTileType.tileTypeId == 0)  {
                tile = gameInputData.selectedTiles[i];
            } else {
                tile = gameInputData.selectedTiles[i].GetNeighbour(gameInputData.activeDirection);
            } 

            if (tile.tileType.tileTypeId != allTileTypes.activeTileType.tileTypeId) {
                tile.rotation = gameInputData.activeRotation;
                allTileTypes.ChangeTileType(tile, allTileTypes.activeTileType.tileTypeId, true);
            }

        }
    }
    
    public override void CreateHistory (StateController controller) {

    }
}
