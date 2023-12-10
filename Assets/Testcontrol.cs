using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testcontrol : MonoBehaviour
{
    public GameObject box1;
    public GameObject box2;
    public GameObject box3;
    public GameObject hand1;
    public GameObject hand2;
    public GameObject hand3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * 5 * Time.deltaTime);
        if (transform.position.x >= -28) 
        {
            Box1Off();
        }
        if (transform.position.x >= -24.8)
        {
            Box2Off();
        }
        if (transform.position.x >= -22)
        {
            Box3Off();
        }
    }
    private void Box1Off()
    {
        box1.SetActive(false);
        hand1.SetActive(true);
    }
    private void Box2Off()
    {
        box2.SetActive(false);
        hand2.SetActive(true);
    }
    private void Box3Off()
    {
        box3.SetActive(false);
        hand3.SetActive(true);
    }
}
