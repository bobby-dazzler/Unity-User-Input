using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEventsFramework;
using Unity3DTileGrid;

namespace UnityUserInput {
    [CreateAssetMenu(menuName="Input/Actions/Add Marching Value")]
    public class AddMarchingValueAct : Action {
        
        public int radius = 4;
        
        public float valueIncrement;

        public bool blendMode;

        public float blendPercentage;

        public GameInputData gameInputData;

        public GridData gridData;

        float timer = 0f;

        public override void Act(StateController controller) {
            timer += Time.deltaTime;
            if (timer > 0.1f) {
                Vector2Int centerCoord = gridData.GetMarchingPointAtPosition(gameInputData.mouseCurrentPosition);
               
                float threshold = radius * radius;
                for (int z = -radius; z < radius; z++) {
                    for (int x = -radius; x < radius; x++) {
                        if (x * x + z * z <  threshold) {
                            Vector2Int coord = new Vector2Int(centerCoord.x + x, centerCoord.y + z);
                            int largestDistance = Mathf.Max(Mathf.Abs(x), Mathf.Abs(z));

                            if (blendMode) {
                                float blend = blendPercentage * largestDistance;
                            } else {
                                //gridData.terrainHeightMap[coord.x, coord.y] += valueIncrement;
                            }
                        }
                    }
                } 

                StateController chunkController = gameInputData.mouseCurrentChunk.chunkObject.GetComponent<StateController>();
                chunkController.CallCurrentStateActionAtIndex(4);
                
                GridTile tile = gridData.GetTileFromPosition(gameInputData.mouseCurrentPosition);
                int numTilesX = gridData.numTiles.x * gridData.numChunks.x;
                int numTilesZ = gridData.numTiles.z * gridData.numChunks.z;
                //GridChunk chunk = gridData.GetChunkFromPosition(gameInputData.mouseCurrentPosition);
                if (tile.LocalCoord.x - radius <= 0 && tile.worldCoord.x != 0) {
                    GridChunk neighbour = tile.GetNeighbour(GridDirection.Xminus).chunk;
                    StateController neighbourController = neighbour.chunkObject.GetComponent<StateController>();
                    neighbourController.CallCurrentStateActionAtIndex(4);
                }
                if (tile.LocalCoord.z - radius <= 0 && tile.worldCoord.z != 0) {
                    GridChunk neighbour = tile.GetNeighbour(GridDirection.Zminus).chunk;
                    StateController neighbourController = neighbour.chunkObject.GetComponent<StateController>();
                    neighbourController.CallCurrentStateActionAtIndex(4);
                }
                if (tile.LocalCoord.x + radius >= gridData.numTiles.x && tile.worldCoord.x != numTilesX) {
                    GridChunk neighbour = tile.GetNeighbour(GridDirection.Xplus).chunk;
                    StateController neighbourController = neighbour.chunkObject.GetComponent<StateController>();
                    neighbourController.CallCurrentStateActionAtIndex(4);
                }
                if (tile.LocalCoord.z + radius >= gridData.numTiles.z && tile.worldCoord.z != numTilesZ) {
                    GridChunk neighbour = tile.GetNeighbour(GridDirection.Zplus).chunk;
                    StateController neighbourController = neighbour.chunkObject.GetComponent<StateController>();
                    neighbourController.CallCurrentStateActionAtIndex(4);
                }

                timer = 0f;
            }
        }

        public override void CreateHistory (StateController controller ){

        }
    }
}
