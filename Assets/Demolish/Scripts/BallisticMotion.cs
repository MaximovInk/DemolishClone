using UnityEngine;
using System.Collections;

public class BallisticMotion : MonoBehaviour
{

    // Private fields
    Vector3 lastPos;
    Vector3 impulse;
    float gravity;

    public void Initialize(Vector3 pos, float gravity)
    {
        transform.position = pos;
        lastPos = transform.position;
        this.gravity = gravity;
    }

    void FixedUpdate()
    {
        // Simple verlet integration
        float dt = Time.fixedDeltaTime;
        Vector3 accel = -gravity * Vector3.up;

        Vector3 curPos = transform.position;
        Vector3 newPos = curPos + (curPos - lastPos) + impulse * dt + accel * dt * dt;
        lastPos = curPos;
        transform.position = newPos;
        transform.forward = newPos - lastPos;

        impulse = Vector3.zero;

        // Z-kill
        if (transform.position.y < -5f)
            Destroy(gameObject);
    }

    public void AddImpulse(Vector3 impulse)
    {
        this.impulse += impulse;
    }
}