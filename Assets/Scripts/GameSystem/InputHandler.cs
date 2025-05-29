using UnityEngine;
using UnityEngine.InputSystem;


public class InputHandler : MonoBehaviour
{ 
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float launchForce = 10f;

    public void OnShoot(InputAction.CallbackContext context)
    {
        Vector3 direction = GetDirectionFromMouse();
        Vector3 origin = spawnPoint.position;


        if (context.started)
        {
            GameEventDispatcher.DispatchStartAiming(origin, direction * launchForce);
        }

        if (context.canceled)
        {
            GameEventDispatcher.DispatchShoot(direction);
        }
    }

    public void OnSwitchAmmo(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameEventDispatcher.DispatchAmmoSwitchRequested();
        }

    }
    private Vector3 GetDirectionFromMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        Plane plane = new Plane(Vector3.forward, spawnPoint.position);

        if (plane.Raycast(ray, out float distance))
        {
            Vector3 hitPoint = ray.GetPoint(distance);
            Vector3 direction = hitPoint - spawnPoint.position;
            direction.z = 0f;

            return direction;
        }

        return Vector3.right;
    }



}
