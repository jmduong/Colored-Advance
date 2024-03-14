using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockShift : MonoBehaviour
{
    public GameMaster master;
    
    /*private void OnMouseDown()
    {
        if(!master.lost)
        {
            master.Shift(name);
            Debug.Log(name + " Game Object Clicked!");
        }
    }*/

    public void Shift(string side)
    {
        Debug.Log(side + " Game Object Clicked!");
        master.Shift(side);
    }

    /*private void Update()
    {
        if (!master.lost && Input.touchCount > 0)
        {
            for(int i = 0; i < Input.touchCount; i++)
            {
                if (Input.GetTouch(i).phase == TouchPhase.Began)
                {
                    Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
                    RaycastHit raycastHit;
                    if (Physics.Raycast(raycast, out raycastHit))
                    {
                        Debug.Log("Something Hit");
                        if (raycastHit.collider.name == name)
                            master.Shift(name);
                    }
                }
            }
        }
    }*/
}
