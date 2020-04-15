using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEventsFramework;
using UnityEngine.EventSystems;
using Unity3DTileGrid;

public class UserInputMono : MonoBehaviour {

    public Transform cameraTransform;

    public GameInputData gameInputData;

    StateController controller;

    void Awake () {
        controller = GetComponent<StateController>();
        gameInputData.cameraTransform = cameraTransform;
        gameInputData.cameraSwivel = cameraTransform.GetChild(0);
        gameInputData.cameraStick = gameInputData.cameraSwivel.GetChild(0);

        gameInputData.selectedTiles  = new List<GridTile>();
        //gameInputData.previousSelectedTiles  = new List<GridTile>();
        //gameInputData.previousSelectedTileTypeIDs = new List<int>();
    }

    void Update () {
        // Gets the input data
        // Calls some action to check and process whatever input is being set

        bool callControllerUpdate = false;
        SetAllChecksFalse(); // prevent anything being called accidentally

        gameInputData.isLeftShift = Input.GetKeyDown(KeyCode.LeftShift);
        gameInputData.scrollWheelDelta = Input.GetAxis("Mouse ScrollWheel");
        gameInputData.xDelta = Input.GetAxis("Horizontal");
        gameInputData.zDelta = Input.GetAxis("Vertical");

        gameInputData.mouseCurrentPosition = GetSurfacePositionAtMousePosition();

        // If left mouse was pressed this frame
        if (Input.GetMouseButtonDown(0)) {
            gameInputData.isMouse0Down = true;

            if (!EventSystem.current.IsPointerOverGameObject()) {
                gameInputData.mouseDownPosition = GetSurfacePositionAtMousePosition();
            }

            callControllerUpdate = true;
        } 

        // If left mouse is currently down
        if (Input.GetMouseButton(0)) {
            if (!EventSystem.current.IsPointerOverGameObject()) {
                gameInputData.isMouse0 = true;

                gameInputData.mouseCurrentPosition = GetSurfacePositionAtMousePosition();

                callControllerUpdate = true;
            } 
        }

        // If left mouse was released this frame
        if (Input.GetMouseButtonUp(0)) {
            if (!EventSystem.current.IsPointerOverGameObject()) {
                gameInputData.isMouse0Up = true;
                callControllerUpdate = true;
            }
        } 

        // If right mouse is currently down
        if (Input.GetMouseButton(1)) {
/*             gameInputData.isMouse1 = true;
            gameInputData.mousePosition = GetSurfacePositionAtMousePosition();
            callControllerUpdate = true; */
        } 

        // If right mouse was pressed this frame
        if (Input.GetMouseButtonDown(1)) {

        } 

        // If right mouse was released this frame
        if (Input.GetMouseButtonUp(1)) {
            if (gameInputData.isMouse1Up == false) {
                gameInputData.isMouse1Up = true;
                //gameInputData.mousePosition = GetSurfacePositionAtMousePosition();
                callControllerUpdate = true; 
            }
        } 

        // Map Controls

        if (gameInputData.scrollWheelDelta != 0) {
            gameInputData.isMouseScroll = true;
            callControllerUpdate = true;
        }

        if (gameInputData.xDelta != 0f || gameInputData.zDelta != 0f) {
            gameInputData.isXZChanged = true;
            callControllerUpdate = true;
        }

        //
        // Calling the actions for each input
        //

        if (callControllerUpdate == true) {
            if (gameInputData.isMouse0Down == true) {
                gameInputData.isMouse0Down = false;
                //controller.CallCurrentStateActionAtIndex(0); // Grid Debug Input
            }
            if (gameInputData.isMouse0 == true) {
                gameInputData.isMouse0 = false;
                controller.CallCurrentStateActionAtIndex(1); // SelectTilesAct
            }
            if (gameInputData.isMouse0Up == true) {
                gameInputData.isMouse0Up = false;
                //controller.CallCurrentStateActionAtIndex(3); // ChangeTileTypeAct
            } 

            if (gameInputData.isMouse1Up == true) {
                gameInputData.isMouse1Up = false;
                //controller.CallCurrentStateActionAtIndex(2); 
            }

            if (gameInputData.isMouseScroll == true) {
                gameInputData.isMouseScroll = false;
                controller.CallCurrentStateActionAtIndex(0); // camera controller action
            }
            if (gameInputData.isXZChanged == true) {
                gameInputData.isXZChanged = false;
                controller.CallCurrentStateActionAtIndex(0); // camera controller action
            }
        }

        
    }
/* 
    Coord GetClosestCoordAtMousePosition() {

    } */

    Vector3 GetSurfacePositionAtMousePosition() {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit)) {
            return hit.point;
        } 
        else return new Vector3(-1,-1,-1); // dont do this 
    }

    void SetAllChecksFalse() {
        gameInputData.isMouse0 = false;
        gameInputData.isMouse0Down = false;
        gameInputData.isMouse0Up = false;
        gameInputData.isMouse1 = false;
        gameInputData.isMouse1Down = false;
        gameInputData.isMouse1Up = false;
        gameInputData.isMouseScroll = false;
        gameInputData.isLeftShift = false;
        gameInputData.isXZChanged = false;
    }

}
