using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMultipleTargets : MonoBehaviour
{
    public List<Transform> targets;
    public Vector3 offset;
    private Vector3 velocity;
    public float smoothTime;
    public float minZoom;
    public float maxZoom;
    public float zoomLimit;

    private Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (targets.Count == 0)
        {
            return;
        }

        Move();
        Zoom();
    }

    private void Zoom()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GreatestDistance() / zoomLimit);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
    }

    private void Move()
    {
        Vector3 center = GetCenterPoint();
        Vector3 newPos = center + offset;
        transform.position = Vector3.SmoothDamp(transform.position, newPos, ref velocity, smoothTime);
    }

    private float GreatestDistance()
    {
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }
        return bounds.size.x;
    }
    private Vector3 GetCenterPoint()
    {
        if (targets.Count == 1)
        {
            return targets[0].position;
        }

        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for(int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        return bounds.center;
    }
}
