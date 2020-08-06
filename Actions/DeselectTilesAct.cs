using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEventsFramework;
using Unity3DTileGrid;

namespace UnityUserInput {
    [CreateAssetMenu(menuName="Input/Actions/Deselect Tiles")]
    public class DeselectTilesAct : Action {
        
        public GridData gridData;

        public GameInputData gameInputData;

        public GridTileRuntimeSet selectedTileTypeTiles; // tiles whichc are selected but are currently empty tiles will be changed to the selecting tile type, stor them in this list to change them back if required

        public override void Act (StateController controller) {
            for (int i = 0; i < gameInputData.selectedTiles.Count; i++) {
                for (int d = 0; d < 6; d++) {
                    gameInputData.selectedTiles[i].SetProjector(false, d);
                }
                gameInputData.selectedTiles[i].isSelected = false;
            }
            gameInputData.selectedTiles.Clear();

            for (int i = 0; i < selectedTileTypeTiles.Count(); i++) {
                gridData.tileFactory.ChangeTileType(selectedTileTypeTiles.items[i], 0, true);
            }
            selectedTileTypeTiles.Clear();
        }

        public override void CreateHistory (StateController controller) {

        }
    }
}