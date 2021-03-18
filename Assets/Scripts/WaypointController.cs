using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointController : MonoBehaviour
{
    public float size = 1f;
    //Make the waypoint visible in the editor
    void OnDrawGizmos(){
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(transform.position, size);
    }
}
