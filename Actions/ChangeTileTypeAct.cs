using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEventsFramework;
using Unity3DTileGrid;

[CreateAssetMenu(menuName="Input/Actions/Change TileType")]
public class ChangeTileTypeAct : Action {

    public GameInputData gameInputData;

    public TileFactory tileFactory;

    public int activeTileTypeIndex;

    public enum Mode {
        tileMode, neighbourMode
    }

    public bool devMode;

    public Mode mode;

    public GridDirection activeDirection;

    public override void Act (StateController controller) {
        for (int i = 0; i < gameInputData.selectedTiles.Count; i++) {
            GridTile tile;

            if (mode == Mode.tileMode) {
                if (gameInputData.selectedTiles[i].worldCoord.y == 0 && activeTileTypeIndex == 0 && !devMode) {
                    break;
                } else {
                    tileFactory.ChangeTileType(gameInputData.selectedTiles[i], activeTileTypeIndex, true);
                }
            } else if (mode == Mode.neighbourMode) {
                activeDirection = gameInputData.mouseDownFace;
                if (gameInputData.selectedTiles[i].GetNeighbour(activeDirection) != null) {
                    tileFactory.ChangeTileType(gameInputData.selectedTiles[i].GetNeighbour(activeDirection), activeTileTypeIndex, true);
                }
            }
        }

        controller.CallCurrentStateActionAtIndex(6); // deselect tiles
    }
    
    public override void CreateHistory (StateController controller) {

    }

    public void ChangeActiveTileType (int index) {
        activeTileTypeIndex = index;
    }

    public void ChangeMode(int index) {
        if (index == 0) {
            mode = Mode.tileMode;
        } else if (index == 1) {
            mode = Mode.neighbourMode;
        }
    }

    public void ChangeTileType(int index) {
        activeTileTypeIndex = index;
    }
}
