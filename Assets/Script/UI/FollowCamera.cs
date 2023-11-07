using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private GameObject player;

    private void Awake()
    {

    }

    public void SetFocus(GameObject target)
    {
        player = target;
    }

    // Update is called once per frame
    void Update()
    {
        //if (player != null)
        //{
        //    Vector3 temp = player.transform.position;
        //    Camera.main.transform.position = new Vector3(temp.x, temp.y, -10f);
        //}
    }
}
