using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HumanAttackintelligentmissile : MonoBehaviour
{
    bool targeting;
    GameObject target;
    Bullet _bullet;
    private float turningForce;
    public bool ready;
    int targetName;

    // Start is called before the first frame update
    //public void Start()
    //{
        //targeting = false;
        //_bullet = GetComponentInParent<Bullet>();
        //turningForce = 15f;
        //targetName = "Enemy";
    //}
    public void init(int i) 
    {
        if (i == 1)
        {
            targeting = false;
            _bullet = GetComponentInParent<Bullet>();
            turningForce = 15f;
            targetName = 7;
            ready= true;
        }
        else
        {
            transform.localScale = new Vector3(5,5,0);
            targeting = false;
            _bullet = GetComponentInParent<Bullet>();
            turningForce = 7f;
            targetName = 8;
            ready = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (targeting) 
        {
            if (target == null) { return; }
            Vector2 dir = (target.transform.position - transform.position).normalized;
            _bullet.gameObject.transform.right = Vector3.Slerp(_bullet.gameObject.transform.right.normalized, dir, turningForce * Time.deltaTime);
            //_bullet.gameObject.transform.right= transform.rotation;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (ready &&!targeting && collision.gameObject.layer == targetName) 
            //TODO ∏’» ≥Ø ∆¿≈≥ ∆–ƒ° «œ±‚
        {
            targeting = true;
            target= collision.gameObject;
        }

    }
}
