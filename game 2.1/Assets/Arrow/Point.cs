using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    public Transform enemy;
    public Transform self;
    public Transform player;
    public Transform playerFlat;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 point = player.InverseTransformPoint(enemy.position).normalized;
        self.localRotation = Quaternion.FromToRotation(playerFlat.up, point);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 point = player.InverseTransformPoint(enemy.position).normalized;
        self.localRotation = Quaternion.FromToRotation(playerFlat.up, point);
    }
}
