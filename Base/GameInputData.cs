using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Input/Data/Game Input Data")]
public class GameInputData : ScriptableObject {
    
    public GridTile activeTile;

    public MarchingPoint activeMarchingPoint;

    public Vector3 mousePosition;

    public bool isMouse0 = false;

    public bool isMouse0Down = false;

    public bool isMouse0Up = false;

    public bool isMouse1 = false;

    public bool isMouse1Down = false;

    public bool isMouse1Up = false;
}
