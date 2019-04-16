using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInputMono : MonoBehaviour {

    public ShipForcesData shipForcesData;

    void Awake() {

    }

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
        
        //shipForcesData.horizontalInput = Input.GetAxis("Horizontal");

        if (Input.GetKey("a")) {
            shipForcesData.isSteeringLeft = true;
            //shipForcesData.horizontalInput = Input.GetAxis("Horizontal");
            shipForcesData.waterJetRotation_Y = shipForcesData.waterJetTransform.localEulerAngles.y + 2f;
        } else {
            shipForcesData.isSteeringLeft = false;
        } 
        
        if (Input.GetKey("d")) {
            shipForcesData.isSteeringRight = true;
            //shipForcesData.horizontalInput = Input.GetAxis("Horizontal");
            shipForcesData.waterJetRotation_Y = shipForcesData.waterJetTransform.localEulerAngles.y - 2f;
        } else {
            shipForcesData.isSteeringRight = false;
        }

    }
}
