using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity3DTileGrid;
using Unity.Mathematics;

namespace UnityUserInput {
    [CreateAssetMenu(menuName="Input/Data/Game Input Data")]
    public class GameInputData : ScriptableObject {

        public enum TileSelectionMode {
            Select, ContainerZone
        }
        public TileSelectionMode tileSelectionMode;
        
        public Transform cameraTransform;

        public Transform cameraStick, cameraSwivel;

        public Vector3 mouseDownPosition;

        public Vector3 mouseCurrentPosition;

        public GridChunk mouseCurrentChunk;
        
        public GridTileObject currentTileObject;

        public GridDirection currentFace;
        public GridDirection mouseDownFace;

        public GridContentObject currentGridContentObject;

        public List<GridTile> selectedTiles;

        public int minX, minY, minZ, maxX, maxY, maxZ; // selected tile boundaries

        //public List<GridTile> previousSelectedTiles;

        //public List<int> previousSelectedTileTypeIDs; // used when changing tile type back to their previous type

        //public GridDirection activeDirection; // used when building tiles in a direction

        //public Vector3 activeRotation; // used when creating certain tiles in specific rotations e.g. wall tiles

        public Vector3Int tileOffsetsToSelect;

        public Vector3Int tileSelectionClamp;

        public bool horizontalTileSelection;

        public bool verticalTileSelection;

        [HideInInspector]
        public float scrollWheelDelta;
        [HideInInspector]
        public float xDelta;
        [HideInInspector]
        public float zDelta;
        [HideInInspector]
        public float rotationDelta;
        [HideInInspector]
        public float rotationAngle;
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
        [HideInInspector]
        public bool isRotationChanged = false;
        [HideInInspector]   
        public bool isSpacebar = false;

        public void ChangeTileSelectionMode (int index) {
            if (index == 0) {
                tileSelectionMode = TileSelectionMode.Select;
            } else if (index == 1) {
                tileSelectionMode = TileSelectionMode.ContainerZone;
            }
        }

    }
}
