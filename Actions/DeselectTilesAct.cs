using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEventsFramework;

[CreateAssetMenu(menuName="Input/Actions/Deselect Tiles")]
public class DeselectTilesAct : Action {
    
    public GameInputData gameInputData;

    public override void Act (StateController controller) {
        for (int i = 0; i < gameInputData.selectedTiles.Count; i++) {
            for (int d = 0; d < 6; d++) {
                gameInputData.selectedTiles[i].SetProjector(false, d);
            }
            gameInputData.selectedTiles[i].isSelected = false;
        }
        gameInputData.selectedTiles.Clear();
    }

    public override void CreateHistory (StateController controller) {

    }
}
