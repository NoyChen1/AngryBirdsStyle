using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TrajectoryTest : MonoBehaviour
{
    [SerializeField] private TrajectoryDrawer drawer;
    [SerializeField] private Vector3 velocity = new Vector3(5, 10, 0);
    [SerializeField] private Vector3 origin = new Vector3(0, 1, 0);

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
        //    drawer.DrawTrajectory(origin, velocity);
        }
    }
}