using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEventsFramework;
using Unity3DTileGrid;

[CreateAssetMenu(menuName=("Input/Actions/Control Camera Action"))]
public class ControlCameraAction : Action {

    public GameInputData gameInputData;

    public GridData gridData;

    public float cameraMoveSpeedMinZoom = 400f;
    public float cameraMoveSpeedMaxZoom = 100f;
    public float stickMinZoom = -250f;
    public float stickMaxZoom = -45f;
    public float swivelMinZoom = 90f;
    public float swivelMaxZoom = 45f;
    public float rotationSpeed = 50f;
    
    

    float zoom = 1f;

    

    public override void Act (StateController controller) {
        if (gameInputData.scrollWheelDelta != 0) {
            AdjustZoom(gameInputData.scrollWheelDelta);
        }
        if(gameInputData.xDelta != 0f || gameInputData.zDelta != 0f) {
            AdjustPosition(gameInputData.xDelta, gameInputData.zDelta);
        }
        if (gameInputData.rotationDelta != 0f) {
            AdjustRotation(gameInputData.rotationDelta);
        }
    }

    public override void CreateHistory (StateController controller) {

    }

    void AdjustZoom (float delta) {
        zoom = Mathf.Clamp01(zoom + delta);

        float distance = Mathf.Lerp(stickMinZoom, stickMaxZoom, zoom);
        gameInputData.cameraStick.localPosition = new Vector3(0f, 0f, distance);

        float angle = Mathf.Lerp(swivelMinZoom, swivelMaxZoom, zoom);
        gameInputData.cameraSwivel.localRotation = Quaternion.Euler(angle, 0f, 0f);
    }

    void AdjustPosition(float xDelta, float zDelta) {
        Vector3 direction = gameInputData.cameraTransform.localRotation * new Vector3(gameInputData.xDelta, 0f, gameInputData.zDelta).normalized;
        float damping = Mathf.Max(Mathf.Abs(xDelta), Mathf.Abs(zDelta));
        float distance = Mathf.Lerp(cameraMoveSpeedMinZoom, cameraMoveSpeedMaxZoom, zoom) * damping * Time.deltaTime;

        Vector3 position = gameInputData.cameraTransform.localPosition;
        position += direction * distance;
        gameInputData.cameraTransform.localPosition = ClampPosition(position);
    }

    void AdjustRotation(float delta) {
        gameInputData.rotationAngle += delta * rotationSpeed * Time.deltaTime;
        if (gameInputData.rotationAngle < 0f) {
            gameInputData.rotationAngle += 360f;
        } else if (gameInputData.rotationAngle >= 360f) {
            gameInputData.rotationAngle -= 360f;
        }
        gameInputData.cameraTransform.localRotation = Quaternion.Euler(0f, gameInputData.rotationAngle, 0f);
    }

    Vector3 ClampPosition (Vector3 position) {
        float xMax = gridData.numChunks.x * gridData.numTiles.x * gridData.spacing;
        position.x = Mathf.Clamp(position.x, 0f, xMax);

        float zMax = gridData.numChunks.z * gridData.numTiles.z * gridData.spacing;
        position.z = Mathf.Clamp(position.z, 0f, zMax);
        return position;
    }
}