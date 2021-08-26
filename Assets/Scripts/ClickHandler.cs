using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickHandler : Movement
{

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            //NavMeshHit navHit; 
            bool hitSomething = false;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // set ray from camera to mouse position 
            hitSomething = Physics.Raycast(ray, out hit, float.MaxValue);

            if (hitSomething) MoveAgent(hit.point);
        }
    }
}
