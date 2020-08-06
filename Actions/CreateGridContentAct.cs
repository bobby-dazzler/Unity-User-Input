using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEventsFramework;
using Unity3DTileGrid;

namespace UnityUserInput {
    [CreateAssetMenu(menuName="Input/Actions/Create Grid Content")]
    public class CreateGridContentAct : Action {

        public GameInputData gameInputData;

        public GridData gridData;

        public int activeContentId;

        public DisplayCurrentTile displayCurrentTileAct;

        public InputField posisionXInputField; // assigned by UIinitializer, scrap these when creating proper UI
        public InputField posisionYInputField;
        public InputField posisionZInputField;
        public InputField scaleXInputField;
        public InputField scaleYInputField;
        public InputField scaleZInputField;

        Vector3 positionOffset;

        Vector3 scale;

        public enum ParentMode {
            Tile, Chunk
        };

        public ParentMode parentMode;

        public override void Act (StateController controller)  {
            GridTile mouseTile = gridData.GetTileFromPosition(gameInputData.mouseCurrentPosition);
            GridTile tile = mouseTile.GetNeighbour(GridDirection.Yplus);

            // Check all tiles first
            if (gridData.tileGridContentFactory.CheckTileTypes(activeContentId, tile, gameInputData.horizontalTileSelection)) {
                Debug.Log("Trying to create content on a tile type which is incompatible");
                controller.CallCurrentStateActionAtIndex(6); // deselct tiles
                return;
            }
            if (gridData.tileGridContentFactory.CheckTileContents(activeContentId, tile, gameInputData.horizontalTileSelection)) {
                Debug.Log("Trying to create content on a tile which already has content, delete it first");
                controller.CallCurrentStateActionAtIndex(6); // deselct tiles
                return;
            }

            if (parentMode == ParentMode.Tile) {
                gridData.tileGridContentFactory.CreateGridContent(tile, activeContentId, gameInputData.horizontalTileSelection, positionOffset, scale);
            } else {
                gridData.chunkGridContentFactory.CreateGridContent(tile, activeContentId, gameInputData.horizontalTileSelection, positionOffset, scale, tile.chunk);
            }       

            controller.CallCurrentStateActionAtIndex(6); // deselct tiles
        }

        public override void CreateHistory (StateController controller) {

        }

        public void ChangeActiveContentId (int index) {
            activeContentId = index;

            // Change the tiles to display
            GridContentType gridContent;
            if (parentMode == ParentMode.Tile) {
                gridContent = gridData.tileGridContentFactory.items[activeContentId] as GridContentType;
            } else {
                gridContent = gridData.chunkGridContentFactory.items[activeContentId] as GridContentType;
            }
            if (gameInputData.horizontalTileSelection) {
                displayCurrentTileAct.tilesToDisplay.x = gridContent.tileSizeX;
                displayCurrentTileAct.tilesToDisplay.y = gridContent.tileSizeY;
                displayCurrentTileAct.tilesToDisplay.z = gridContent.tileSizeZ;
            } else if (gameInputData.verticalTileSelection) {
                displayCurrentTileAct.tilesToDisplay.x = gridContent.tileSizeZ;
                displayCurrentTileAct.tilesToDisplay.y = gridContent.tileSizeY;
                displayCurrentTileAct.tilesToDisplay.z = gridContent.tileSizeX;
            }
        }

        public void UpdatePositionOffset () {
            Vector3 value = new Vector3(float.Parse(posisionXInputField.text), float.Parse(posisionYInputField.text), float.Parse(posisionZInputField.text));
            positionOffset = value;
        }

        public void UpdateScale () {
            Vector3 value = new Vector3(float.Parse(scaleXInputField.text), float.Parse(scaleYInputField.text), float.Parse(scaleZInputField.text));
            scale = value;
        }
    }
}