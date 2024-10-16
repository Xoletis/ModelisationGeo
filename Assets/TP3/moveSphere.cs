using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveSphere : MonoBehaviour
{
    private Camera mainCamera;
    private bool isDragging = false;
    private Vector3 offset;
    private Plane dragPlane;

    public int force = 10;
    public bool earase = true;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        transform.position += mainCamera.transform.forward * scroll;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == transform)
                {
                    isDragging = true;
                    dragPlane = new Plane(mainCamera.transform.forward, hit.point);
                    offset = hit.point - transform.position;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            float distance;
            if (dragPlane.Raycast(ray, out distance))
            {
                Vector3 hitPoint = ray.GetPoint(distance);
                transform.position = hitPoint - offset;
            }
        }
    }
}
