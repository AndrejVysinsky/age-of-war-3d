using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] GameObject grid;

    [Header("Camera Zoom")]
    [SerializeField] float zoomSensitivity;
    [SerializeField] float zoomAdjustingSpeed;
    [SerializeField] float minCameraY;
    [SerializeField] float maxCameraY;

    [Header("Position restrictions")]
    [SerializeField] float minX;
    [SerializeField] float maxX;
    [SerializeField] float minZ;
    [SerializeField] float maxZ;

    private float _targetCameraY;

    private void Awake()
    {
        _targetCameraY = transform.position.y;
    }

    private void LateUpdate()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            ZoomCamera();
        }

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            MoveCamera();
            ClampPosition();
        }

        if (_targetCameraY != transform.position.y)
        {
            AdjustHeightToTerrain();
        }
    }

    private void MoveCamera()
    {
        float xAxisValue = Input.GetAxis("Horizontal") * moveSpeed * GetBaseDeltaTime();
        float zAxisValue = Input.GetAxis("Vertical") * moveSpeed * GetBaseDeltaTime();

        var moveVector = new Vector3(xAxisValue, 0.0f, zAxisValue);

        transform.Translate(Quaternion.AngleAxis(transform.rotation.eulerAngles.y, Vector3.up) * moveVector, Space.World);
    }

    private void ClampPosition()
    {
        var position = transform.position;        

        position.x = Mathf.Clamp(position.x, minX, maxX);
        position.z = Mathf.Clamp(position.z, minZ, maxZ);

        transform.position = position;
    }

    private void ZoomCamera()
    {
        float zoomValue = Input.GetAxis("Mouse ScrollWheel") * -1000;

        _targetCameraY += zoomSensitivity * zoomValue * GetBaseDeltaTime();

        _targetCameraY = Mathf.Clamp(_targetCameraY, minCameraY, maxCameraY);
    }

    private void AdjustHeightToTerrain()
    {
        var currentDistanceFromGround = transform.position.y;

        if (Mathf.Approximately(_targetCameraY, currentDistanceFromGround))
        {
            return;
        }

        var diff = _targetCameraY - currentDistanceFromGround;

        var position = transform.position;

        position.y += diff * zoomAdjustingSpeed * GetBaseDeltaTime();

        transform.position = position;
    }

    private float GetBaseDeltaTime()
    {
        float timeScale = Time.timeScale == 0 ? 1 : Time.timeScale;

        return Time.deltaTime / timeScale;
    }
}
