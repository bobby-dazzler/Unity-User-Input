using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEventsFramework;
using Unity3DTileGrid;

namespace UnityUserInput {
    [CreateAssetMenu(menuName="Input/Actions/Delete Grid Content At Mouse Position")]
    public class DeleteGridContentAtMousePosition : Action {
        
        public GameInputData gameInputData;

        public GridData gridData;

        public override void Act(StateController controller) {
            if (gameInputData.currentGridContentObject != null) {
                gameInputData.currentGridContentObject.gridContent.ReclaimInstance();
                gameInputData.currentGridContentObject.gridContent.ClearLocation();
            } else {
                Debug.Log("Called delete content at mouse position on null content");
            }

            controller.CallCurrentStateActionAtIndex(6); // Deselect tiles
        }

        public override void CreateHistory(StateController controller) {

        }
    }
}
