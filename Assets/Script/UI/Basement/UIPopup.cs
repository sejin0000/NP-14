using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIPopup : UIBase
{
    private TMP_Text text;
    private Animator animator;
    [SerializeField] private float lifetime = 3f;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
