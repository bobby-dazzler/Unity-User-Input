using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity3DTileGrid;

[CreateAssetMenu(menuName="Input/Data/Game Input Data")]
public class GameInputData : ScriptableObject {
    
    public Transform cameraTransform;

    public Transform cameraStick, cameraSwivel;

    public Vector3 mouseDownPosition;

    public Vector3 mouseCurrentPosition;

    public List<GridTile> selectedTiles;

    public List<GridTile> previousSelectedTiles;

    public List<int> previousSelectedTileTypeIDs; // used when changing tile type back to their previous type

    public GridDirection activeDirection; // used when building tiles in a direction

    public Vector3 activeRotation; // used when creating certain tiles in specific rotations e.g. wall tiles

    [HideInInspector]
    public float scrollWheelDelta;
    [HideInInspector]
    public float xDelta;
    [HideInInspector]
    public float zDelta;
    [HideInInspector]
    public bool isMouse0 = false;
    [HideInInspector]
    public bool isMouse0Down = false;
    [HideInInspector]
    public bool isMouse0Up = false;
    [HideInInspector]
    public bool isMouse1 = false;
    [HideInInspector]
    public bool isMouse1Down = false;
    [HideInInspector]
    public bool isMouse1Up = false;
    [HideInInspector]
    public bool isMouseScroll = false;
    [HideInInspector]   
    public bool isLeftShift = false;
    [HideInInspector]
    public bool isXZChanged = false;

    public void SetGridEditorDirection (int value) {
        if (value == 0) {
            activeDirection = GridDirection.Xplus;
        }
        if (value == 1) {
            activeDirection = GridDirection.Yplus;
        }
        if (value == 2) {
            activeDirection = GridDirection.Zplus;
        }
        if (value == 3) {
            activeDirection = GridDirection.Xminus;
        }
        if (value == 4) {
            activeDirection = GridDirection.Yminus;
        }
        if (value == 5) {
            activeDirection = GridDirection.Zminus;
        }
    }

    public void SetActiveRotation (int value) {
        if (value == 0) {
            activeRotation = new Vector3(0,0,0);
        }
        if (value == 1) {
            activeRotation = new Vector3(0,90,0);
        }
        if (value == 2) {
            activeRotation = new Vector3(0,180,0);
        }
        if (value == 3) {
            activeRotation = new Vector3(0,270,0);
        }
    }

}

