using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEventsFramework;

public class UserInputMono : MonoBehaviour {

/*     public ShipForcesData shipForcesData;

    public Transform waterJetTransform; // temp for testing forces

    void FixedUpdate() {
        if (Input.GetKey("w")) {
            if (shipForcesData.currentJetPower < shipForcesData.maxPower) {
                shipForcesData.currentJetPower++;
            } 
        } else if (Input.GetKey("s")) {
            if (shipForcesData.currentJetPower > -shipForcesData.maxPower) {
                shipForcesData.currentJetPower--;
            } 
        } else {
            if (shipForcesData.currentJetPower > 0) {
                shipForcesData.currentJetPower--;
            } else if (shipForcesData.currentJetPower < 0) {
                shipForcesData.currentJetPower++;
            }
        }
        
        shipForcesData.horizontalInput = Input.GetAxis("Horizontal");

    } */

    public GameInputData gameInputData;

    StateController controller;

    void Awake () {
        controller = GetComponent<StateController>();
    }

    void FixedUpdate () {
        // Gets the input data
        // Calls some action to check and process whatever input is being set

        bool callControllerUpdate = false;

        // If left mouse is currently down
        if (Input.GetMouseButton(0)) {
            gameInputData.isMouse0 = true;
            gameInputData.mousePosition = GetSurfacePositionAtMousePosition();
            //gameInputData.currentCoord = 
            callControllerUpdate = true;
        } 

        // If left mouse was pressed this frame
        if (Input.GetMouseButtonDown(0)) {

        } 

        // If left mouse was released this frame
        if (Input.GetMouseButtonUp(0)) {
            gameInputData.isMouse0Up = true;
            gameInputData.mousePosition = GetSurfacePositionAtMousePosition();
            callControllerUpdate = true;
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
                gameInputData.mousePosition = GetSurfacePositionAtMousePosition();
                callControllerUpdate = true; 
            }
        } 

        if (callControllerUpdate == true) {
            if (gameInputData.isMouse0Up == true) {
                gameInputData.isMouse0Up = false;
                controller.CallCurrentStateActionAtIndex(1); // Debug Erosion action
                
            }

            if (gameInputData.isMouse1Up == true) {
                gameInputData.isMouse1Up = false;
                controller.CallCurrentStateActionAtIndex(2); // Debug Marching Points action
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

}
