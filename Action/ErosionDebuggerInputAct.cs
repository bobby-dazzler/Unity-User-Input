using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEventsFramework;

[CreateAssetMenu(menuName="Input/Actions/Erode Debugger Input")]
public class ErosionDebuggerInputAct : Action {

    public GameInputData gameInputData;

    public GridMgrState gridMgrState;

    public GridData gridData;

    public ErosionDebugger erosionDebugger;

    public override void Act(StateController controller) {
        //erosionDebugger.debugMode = true;
        erosionDebugger.startingPosition = gameInputData.mousePosition;       

        Coord coord = gridData.GetClosestPointCoordFromPosition(gameInputData.mousePosition);
        TestPositionFromCoord(coord.x, coord.y, coord.z);
        TestCoordFromPosition(gameInputData.mousePosition);

        float surface = gridMgrState.GetSurfaceValueAtXZ(coord.x, coord.z, gridData);
        Vector3 pointPosition = gridData.GetPointPositionAtCoord(coord);
        erosionDebugger.getSurfaceValue = new Vector3(pointPosition.x, surface, pointPosition.z);
        //erosionDebugger.getPointPositionAtCoord = pointPosition; 
        
        thisEvent.StartEvent(controller);  
    }

    public override void CreateHistory(StateController controller) {

    }

    void TestCoordFromPosition(Vector3 position) {
        Coord coord = gridData.GetClosestPointCoordFromPosition(position);
        Debug.Log("Testing GetPointFromCoord logic");
        Debug.Log("Point coord = " + coord);
        Coord tileCoord = new Coord(coord.x / gridData.maximumMarchingLOD, coord.y / gridData.maximumMarchingLOD, coord.z / gridData.maximumMarchingLOD);
        Debug.Log("Point should be in tile coord " + tileCoord);
        erosionDebugger.getCoordFromPositionTest = gridData.GetPointPositionAtCoord(coord);
    }

    void TestPositionFromCoord(int x, int y, int z) {
        Coord coord = new Coord(x,y,z);
        erosionDebugger.getPositionFromCoordList = new List<Vector3>();

        erosionDebugger.getPositionFromCoordList.Add(gridData.GetPointPositionAtCoord(coord));
        coord.x += 1;
        erosionDebugger.getPositionFromCoordList.Add(gridData.GetPointPositionAtCoord(coord));
        coord.x -= 2;
        erosionDebugger.getPositionFromCoordList.Add(gridData.GetPointPositionAtCoord(coord));
        coord.x += 1;
        coord.y += 1;
        erosionDebugger.getPositionFromCoordList.Add(gridData.GetPointPositionAtCoord(coord));
        coord.y -= 2;
        erosionDebugger.getPositionFromCoordList.Add(gridData.GetPointPositionAtCoord(coord));
        coord.y += 1;
        coord.z += 1;
        erosionDebugger.getPositionFromCoordList.Add(gridData.GetPointPositionAtCoord(coord));
        coord.z -= 2;
        erosionDebugger.getPositionFromCoordList.Add(gridData.GetPointPositionAtCoord(coord));

/*         coord.x = 0;
        coord.y = 0;
        coord.z = 0;
        erosionDebugger.getPositionFromCoordList.Add(gridData.GetPointPositionAtCoord(coord));
        coord.x = 26;
        coord.y = 0;
        coord.z = 0;
        erosionDebugger.getPositionFromCoordList.Add(gridData.GetPointPositionAtCoord(coord)); */
    }
}
