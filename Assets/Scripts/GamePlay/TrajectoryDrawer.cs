using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


[RequireComponent(typeof(TrajectoryMeshRenderer))]
public class TrajectoryDrawer : MonoBehaviour
{
    [SerializeField] private TrajectoryMeshRenderer meshRenderer;
    [SerializeField] private int resolution = 30;
    [SerializeField] private float stepTime = 0.1f;

    private float delayTime = 1.5f;
    private Task _clearTask;
    private bool _isClearing = false;

    private List<Vector3> _lastTrajectory = new();

    private void OnEnable()
    {
        GameEventDispatcher.OnStartAiming += DrawTrajectory;
        GameEventDispatcher.OnShoot += dir => ScheduleClearAsync(delayTime);
    }

    private void OnDisable()
    {
        GameEventDispatcher.OnStartAiming -= DrawTrajectory;
        GameEventDispatcher.OnShoot -= dir => ScheduleClearAsync(delayTime);
    }


    public void DrawTrajectory(Vector3 origin, Vector3 velocity)
    {
        _lastTrajectory.Clear();
        for (int i = 0; i < resolution; i++)
        {
            float t = i * stepTime;
            Vector3 pos = origin + velocity * t + 0.5f * Physics.gravity * t * t;
            _lastTrajectory.Add(pos);
        }

        if (_lastTrajectory.Count < 2) return;

        meshRenderer.RenderPath(_lastTrajectory);
        GameEventDispatcher.DispatchTrajectoryUpdated(_lastTrajectory);

    }

    private async void ScheduleClearAsync(float delayTime)
    {
        if (_isClearing)
        {
            return; 
        }

        _isClearing = true;
        await Task.Delay((int)(delayTime * 1000));
        Clear();
        _isClearing = false;
    }
    private void Clear()
    {
        meshRenderer.RenderPath(new Vector3[2] { Vector3.zero, Vector3.zero });
    }
}
