using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats
{
    public float total       { get { return (basic + added) * coefficient; } }   //  ÃÑ ½ºÅÈ °ª
    public float basic       { get; private set; }                               //±âº» ½ºÅÈ °ª
    public float added       { get; set; } = 0;                                  //Ãß°¡ ½ºÅÈ °ª
    public float coefficient { get; set; } = 1;                          //½ºÅÈ °è¼ö °ª

    public Stats(float basic) 
    {
        this.basic = basic;
    }
}
