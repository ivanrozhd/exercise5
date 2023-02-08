using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = System.Object;

public class CameraControll : MonoBehaviour
{
    public LineRenderer lineRenderer;
    private float x;
    private float y;
    private float sensitivity = 10f;
    private Vector3 rotateValue;
    public Text text;
    public Text text_esc;
    private bool _selected = false;
    private bool _effects = false;
    private Collider target;
    private Vector3 default_position;
    private Vector3 default_rotation;
    private Quaternion default_angle;
    public GameObject particles;
    GameObject clone;
    


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        text.enabled = false;
        default_position = transform.position;
        default_rotation = transform.eulerAngles;
        default_angle = transform.rotation;
        text_esc.enabled = false;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, new Vector3(1,-1.5f, -2));
    }

    void Update()
    {
        if (!_selected)
        {
            communicateWithObject();
            x = Input.GetAxis("Mouse X");
            rotateValue = new Vector3(0, x * sensitivity, 0);
            transform.eulerAngles -= rotateValue;
            
        }
        
        if (_selected)
        {
            rotateAround(target);
            lineRenderer.enabled = false;
        }
        
        if (Input.GetKeyUp(KeyCode.Escape) && _selected)
        {
            _selected = false;
            transform.position = default_position;
            transform.eulerAngles = default_rotation;
            transform.rotation = default_angle;
            text_esc.enabled = false;
        }

    }

    // Detecting ray collision with the object and highlighting it if the collision has happened 
     void communicateWithObject()
     {
         Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(transform.position, transform.forward * 20f, Color.green);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {   
            text.enabled = true; 
            if (Input.GetKeyUp(KeyCode.E))
            {
                _selected = true;
                target = hit.collider;
                transform.Translate(sensitivity * Vector3.forward);
                text.enabled = false;
                text_esc.enabled = true;

            }
            rotateValue = new Vector3(0, 0.5f, 0);
            hit.transform.eulerAngles -= rotateValue;
            lineRenderer.SetPosition(1, new Vector3(hit.transform.position.x, hit.transform.position.y - 0.5f, hit.transform.position.z ));
            if (!_effects)
            {
                 clone = Instantiate(particles);
                clone.transform.position = hit.collider.transform.position + Vector3.up * 2;
                lineRenderer.enabled = true;
                _effects = true;
            }
        }
        else
        {
            text.enabled = false;
            lineRenderer.enabled = false;
            if (_effects)
            {
                Destroy(clone);    
            }
            _effects = false;
        }
        
    }
     
     // Orbiting the precised object
     void rotateAround(Collider other){
         transform.LookAt(other.transform);
         transform.Translate(sensitivity/6 * Vector3.right *  Time.deltaTime);
     }
}
