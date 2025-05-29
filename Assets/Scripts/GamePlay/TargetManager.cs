using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TargetManager : MonoBehaviour
{
    public static TargetManager Instance { get; private set; }

    [SerializeField] private List<Target> _targets;

    private bool _gameEnded = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void CheckIfAllTargetsDestroyed()
    {
        if (_gameEnded) return;

        foreach (var target in _targets)
        {
            if (target != null && target.gameObject.activeInHierarchy)
                return;
        }

        _gameEnded = true;
        GameEventDispatcher.DispatchAllTargetsEliminated();
    }

    public bool HasActiveTargets()
    {
        return !_gameEnded;
    }
}
