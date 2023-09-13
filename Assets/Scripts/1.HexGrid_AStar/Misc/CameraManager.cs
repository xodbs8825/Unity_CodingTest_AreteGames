using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Camera cam;
    private float speed;

    private void Awake()
    {
        cam = Camera.main;
        speed = 5f;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            cam.transform.Translate(Vector3.right * Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            cam.transform.Translate(-Vector3.right * Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            cam.transform.Translate(Vector3.up * Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            cam.transform.Translate(-Vector3.up * Time.deltaTime * speed);
        }

        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        HexTile target;

        if (Physics.Raycast(ray, out hit))
        {
            Transform hitTransform = hit.transform;
            if (hitTransform.TryGetComponent<HexTile>(out target) && Input.GetMouseButtonDown(1))
            {
                target.OnSelectTile();
            }
        }

    }
}
