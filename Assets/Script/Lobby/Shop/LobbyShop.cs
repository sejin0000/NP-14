using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyShop : MonoBehaviour
{
    [Header("player")]
    public GameObject Player;

    [Header("ShopPanel")]
    public GameObject ShopPanel;

    private void OnCollisionEnter2D(UnityEngine.Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

        }
    }
}
