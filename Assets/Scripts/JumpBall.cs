using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBall : MonoBehaviour
{
    Rigidbody BallRB;
    void Start()
    {
        BallRB = GetComponent<Rigidbody>();
    }

    // La pelota se impulsa hacia arriba si toca un objeto
    void OnCollisionEnter(Collision collision)
    {
        BallRB.linearVelocity = Vector3.zero;
        BallRB.AddForce(transform.up * 300f);
    }
}
