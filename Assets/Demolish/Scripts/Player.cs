using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class Player : MonoBehaviour
{
    private const float Distance = 50f;

    [SerializeField] private Transform tower;
    [SerializeField] private Transform gun;
    [SerializeField] private float rotationSpeed = 10f;

    private Camera _camera;
    
    private void Awake()
    {
        _camera = Camera.main;
    }

    private void RotateTower(Vector3 lookAt)
    {
        var originalRot = tower.rotation;
        lookAt.y = tower.position.y;
        tower.LookAt(lookAt, Vector3.up);
        var newRot = tower.rotation;

        tower.rotation = Quaternion.Lerp(originalRot, newRot, rotationSpeed * Time.deltaTime);
    }

    private void RotateGun(Vector3 lookAt)
    {
        var originalRot = gun.rotation;
        gun.LookAt(lookAt, Vector3.up);

        var newRot = gun.rotation;
        var angles = newRot.eulerAngles;
        angles.y = tower.rotation.eulerAngles.y;
        newRot.eulerAngles = angles;

        gun.rotation = Quaternion.Lerp(originalRot, newRot, rotationSpeed * Time.deltaTime);
    }

    private void Update()
    {
        var mousePosition = Input.mousePosition;
        var ray = _camera.ScreenPointToRay(mousePosition);

        if (!Physics.Raycast(ray, out var hit, Distance)) return;

        RotateTower(hit.point);
        RotateGun(hit.point);


    }


}
