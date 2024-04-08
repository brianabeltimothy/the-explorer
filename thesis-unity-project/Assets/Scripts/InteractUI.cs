using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractUI : MonoBehaviour
{
    [Header("Tweaks")]
    [SerializeField] private Transform lookAt;
    [SerializeField] private Vector3 offset;

    [Header("Logic")]
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void FixedUpdate()
    {
        Vector3 pos = cam.WorldToScreenPoint(lookAt.position + offset);

        if(transform.position != pos)
        {
            transform.position = pos;
        }
    }
}
