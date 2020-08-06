using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using UnityEventsFramework;
using UnityEngine.EventSystems;
using Unity3DTileGrid;

namespace UnityUserInput {
    public class UserInputMono : MonoBehaviour {

        public Transform cameraTransform;

        public GameInputData gameInputData;

        public GridData gridData;

        public DisplayCurrentTile displayCurrentTile;

        public DebugMarchingValues debugMarchingValues;

        public bool debugMode;

        StateController controller;

        void Awake () {
            controller = GetComponent<StateController>();
            gameInputData.cameraTransform = cameraTransform;
            gameInputData.cameraSwivel = cameraTransform.GetChild(0);
            gameInputData.cameraStick = gameInputData.cameraSwivel.GetChild(0);

            gameInputData.selectedTiles  = new List<GridTile>();

            gameInputData.rotationAngle = 0f;

            gameInputData.horizontalTileSelection = true;
            gameInputData.verticalTileSelection = false;

            displayCurrentTile.SetTileSize(0);
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
            gameInputData.rotationDelta = Input.GetAxis("Rotation");

            gameInputData.mouseCurrentPosition = GetSurfacePositionAtMousePosition();
            
            GetObjectsAtMousePoition();
            gameInputData.currentFace = GetFaceAtMousePosition();

            // If left mouse was pressed this frame
            if (Input.GetMouseButtonDown(0)) {
                gameInputData.isMouse0Down = true;

                if (!EventSystem.current.IsPointerOverGameObject()) {
                    gameInputData.mouseDownPosition = GetSurfacePositionAtMousePosition();
                    gameInputData.mouseDownFace = GetFaceAtMousePosition();
                }

                callControllerUpdate = true;
            } 

            // If left mouse is currently down
            if (Input.GetMouseButton(0)) {
                if (!EventSystem.current.IsPointerOverGameObject()) {
                    gameInputData.isMouse0 = true;

                    //gameInputData.mouseCurrentPosition = GetSurfacePositionAtMousePosition();

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
                if (!EventSystem.current.IsPointerOverGameObject()) {
                    gameInputData.isMouse1 = true;
                    callControllerUpdate = true; 
                }
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

            // If spacebar was released this frame
            if (Input.GetKeyUp(KeyCode.Space)) {
                if (gameInputData.isSpacebar == false) {
                    gameInputData.isSpacebar = true;
                    callControllerUpdate = true; 
                }
            }

            if (Input.GetKeyUp(KeyCode.Tab)) {
                if (gameInputData.horizontalTileSelection == true) {
                    gameInputData.horizontalTileSelection = false;
                    gameInputData.verticalTileSelection = true;
                } else {
                    gameInputData.horizontalTileSelection = true;
                    gameInputData.verticalTileSelection = false;
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

            if (gameInputData.rotationDelta != 0f) {
                gameInputData.isRotationChanged = true;
                callControllerUpdate = true;
            }

            //
            // Calling the actions for each input
            //

            // Called every frame
            CallControllerActionAtIndex(0);

            if (callControllerUpdate == true) {
                if (gameInputData.isMouse0Down == true) {
                    gameInputData.isMouse0Down = false;
                    //controller.CallCurrentStateActionAtIndex(1); 
                }
                if (gameInputData.isMouse0 == true) {
                    gameInputData.isMouse0 = false;
                    CallControllerActionAtIndex(2);
                }
                if (gameInputData.isMouse0Up == true) {
                    gameInputData.isMouse0Up = false;
                    CallControllerActionAtIndex(3);
                } 
                if (gameInputData.isMouse1Down == true) {
                    gameInputData.isMouse1Down = false;
                    CallControllerActionAtIndex(4);
                }
                if (gameInputData.isMouse1 == true) {
                    gameInputData.isMouse1 = false;
                    CallControllerActionAtIndex(5);
                }
                if (gameInputData.isMouse1Up == true) {
                    gameInputData.isMouse1Up = false;
                    CallControllerActionAtIndex(6);
                }
                if (gameInputData.isMouseScroll == true) {
                    gameInputData.isMouseScroll = false;
                    CallControllerActionAtIndex(7);
                }
                if (gameInputData.isXZChanged == true) {
                    gameInputData.isXZChanged = false;
                    CallControllerActionAtIndex(7);
                }
                if (gameInputData.isRotationChanged == true) {
                    gameInputData.isRotationChanged = false;
                    CallControllerActionAtIndex(7);
                }
                if (gameInputData.isSpacebar == true) {
                    gameInputData.isSpacebar = false;
                    CallControllerActionAtIndex(8);
                    debugMarchingValues.GetValuesAtXZ(gameInputData.mouseCurrentPosition);
                }
            }
        }

        void CallControllerActionAtIndex (int index) {
            if (controller.currentState.actions[index] != null) {
                controller.CallCurrentStateActionAtIndex(index);
            }
        }

        Vector3 GetSurfacePositionAtMousePosition() {
            Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(inputRay, out hit)) {
                return hit.point;
            } 
            else return new Vector3(-1,-1,-1); // dont do this 
        }

        void GetObjectsAtMousePoition () {
            Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(inputRay, out hit)) {
                GridTileObject obj = hit.collider.transform.root.GetComponent<GridTileObject>();
                if (obj != null) {
                    gameInputData.mouseCurrentChunk = gridData.GetChunkFromPosition(gameInputData.mouseCurrentPosition);
                } else {
                    gameInputData.currentTileObject = null;
                }

                GridChunk chunk = gridData.GetChunkFromPosition(gameInputData.mouseCurrentPosition);
                if (chunk != null) {
                    gameInputData.mouseCurrentChunk = chunk;
                } else {
                    gameInputData.mouseCurrentChunk = null;
                }

                GridContentObject content = hit.collider.transform.GetComponentInParent<GridContentObject>();
                if (content != null) {
                    gameInputData.currentGridContentObject = content;
                } else {
                    gameInputData.currentGridContentObject = null;
                }
            } else {
                gameInputData.currentTileObject = null;
                gameInputData.mouseCurrentChunk = null;
                gameInputData.currentGridContentObject = null;
            }
        }

        GridDirection GetFaceAtMousePosition () {
            GridDirection direction = GridDirection.Yplus;
            Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(inputRay, out hit)) {
                //GridDirection direction;
                // enums are non nullable, if this causes problems change gameInputData.currentFace to something other than a GridDirection
                if (hit.collider.transform.name == "xPlus") {
                    direction = GridDirection.Xplus;
                } else if (hit.collider.transform.name == "zPlus") {
                    direction = GridDirection.Zplus;
                } else if (hit.collider.transform.name == "xMinus") {
                    direction = GridDirection.Xminus;
                } else if (hit.collider.transform.name == "yMinus") {
                    direction = GridDirection.Yminus;
                } else if (hit.collider.transform.name == "zMinus") {
                    direction = GridDirection.Zminus;
                } else {
                    direction = GridDirection.Yplus;
                }
            }
            return direction;
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
            gameInputData.isSpacebar = false;
        }

    }
}
