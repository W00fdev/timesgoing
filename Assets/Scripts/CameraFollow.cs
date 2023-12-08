using Unity.VisualScripting;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Body;
    public RectTransform Crosshair;

    public float ScreenOffset;
    public float CameraMaxOffset;

    private Vector3 _crosshairPosition;

    private Quaternion _rotation;
    public float _distanceOffset;
    public float _upOffset;

    private Vector3 _offsetByMouse;

    private void Awake()
    {
        _distanceOffset = Vector3.Distance(Body.position, transform.position);
        _rotation = transform.rotation;
    }

    private void LateUpdate()
    {
        transform.position = Body.position + _rotation * (Vector3.back * _distanceOffset);

        /*
         * if (Input.mousePosition.x > 0.5f)
         * {
         *      if (Input.mousePosition.x > 1f - ScreenOffset)
         *      {
         *          // _offset = [0..1] = (Input.mousePosition.x - (1f - ScreenOffset)) / (1f - ScreenOffset)
         *          _offsetByMouse.x = (Input.mousePosition.x - (1f - ScreenOffset)) / (1f - ScreenOffset);
         *          _offsetByMouse.x *= CameraMaxOffset;
         *      }
         * }
         * else if (Input.mousePosition.x < 0.5f)
         * {
         * 
         * }
         * 
         * if (Input.mousePosition.y > 0.5f)
         * {
         * 
         * }
         * else if (Input.mousePosition.y < 0.5f)
         * {
         * 
         * }
         * 
         */

        UpdateCrosshair();

        //Debug.Log(Camera.main.ScreenToViewportPoint(Input.mousePosition));
    }

    private void UpdateCrosshair()
    {
        _crosshairPosition = Input.mousePosition;
        _crosshairPosition.x -= Screen.width / 2;
        _crosshairPosition.y -= Screen.height / 2;

        Crosshair.anchoredPosition = _crosshairPosition;
    }
}