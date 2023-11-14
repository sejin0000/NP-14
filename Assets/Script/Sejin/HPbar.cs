using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPbar : MonoBehaviour
{

    private PlayerStatHandler playerStat;
    public Slider HP;

    private void Awake()
    {
        playerStat = GetComponent<PlayerStatHandler>();
    }

    private void Start()
    {
        playerStat.OnChangeCurHPEvent += UiHpUpdate;
        HP.value = 1;
    }

    private void UiHpUpdate()
    {
        HP.value =  playerStat.CurHP / playerStat.HP.total;
    }
}