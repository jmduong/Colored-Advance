using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ObjRotation : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    bool onImage = false;
    Vector3 mPrevPos = Vector3.zero, mPosDelta = Vector3.zero;
    public AudioSource sound;

    public GameObject baseGraph, graph;

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        onImage = false;
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        if (name == "CubeSyncDisplay")
        {
            onImage = true;
            mPosDelta = pointerEventData.position;
            mPrevPos = Input.mousePosition;
            sound.Play();
        }
    }

    void Update()
    {
        if(onImage)
        {
            mPosDelta = Input.mousePosition - mPrevPos;
            if(Vector3.Dot(transform.up, Vector3.up) >= 0)
            {
                //baseGraph.transform.Rotate(transform.up, -Vector3.Dot(mPosDelta, Camera.main.transform.right), Space.World);
                graph.transform.Rotate(transform.up, -Vector3.Dot(mPosDelta, Camera.main.transform.right), Space.World);
            }
            else
            {
                //baseGraph.transform.Rotate(transform.up, Vector3.Dot(mPosDelta, Camera.main.transform.right), Space.World);
                graph.transform.Rotate(transform.up, Vector3.Dot(mPosDelta, Camera.main.transform.right), Space.World);
            }
            //baseGraph.transform.Rotate(Camera.main.transform.right, Vector3.Dot(mPosDelta, Camera.main.transform.up), Space.World);
            graph.transform.Rotate(Camera.main.transform.right, Vector3.Dot(mPosDelta, Camera.main.transform.up), Space.World);
            mPrevPos = Input.mousePosition;
        }
    }
}
