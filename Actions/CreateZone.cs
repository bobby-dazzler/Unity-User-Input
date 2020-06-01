using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEventsFramework;

[CreateAssetMenu(menuName="Input/Actions/Create Zone")]
public class CreateZone : Action {
   
    public GameInputData gameInputData;

    StateController portManagerController;

    public override void Act (StateController stateController) {
        if (portManagerController == null) {
           portManagerController = GameObject.Find("Port Manager").GetComponent<StateController>();
        }

        if (gameInputData.tileSelectionMode == GameInputData.TileSelectionMode.ContainerZone) {
            portManagerController.CallCurrentStateActionAtIndex(0); // CreateContainerZoneAct
        }
   }

   public override void CreateHistory (StateController controller) {

   }
}
