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

        if (Input.GetMouseButton(0)) {
            gameInputData.isMouse0 = true;
            gameInputData.mousePosition = GetSurfaceLevelAtMousePosition();
            callControllerUpdate = true;
        } 

        if (Input.GetMouseButtonDown(0)) {

        } 

        if (Input.GetMouseButtonUp(0)) {

        } 

        if (Input.GetMouseButton(1)) {
            gameInputData.isMouse1 = true;
            gameInputData.mousePosition = GetSurfaceLevelAtMousePosition();
            callControllerUpdate = true;
        } 

        if (Input.GetMouseButtonDown(1)) {

        } 

        if (Input.GetMouseButtonUp(1)) {
/*             gameInputData.isMouse1Up = true;
            gameInputData.mousePosition = GetSurfaceLevelAtMousePosition();
            callControllerUpdate = true; */
        } 

        if (callControllerUpdate == true) {
            controller.CallCurrentStateActionAtIndex(0); // Process input action
        }
    }

    Vector3 GetSurfaceLevelAtMousePosition() {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit)) {
            return hit.point;
        } 
        else return new Vector3(-1,-1,-1); // dont do this 
    }

}
