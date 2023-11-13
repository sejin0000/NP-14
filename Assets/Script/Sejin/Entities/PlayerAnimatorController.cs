using Photon.Pun;
using System;
using System.Drawing;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UIElements;

public class PlayerAnimatorController : MonoBehaviour
{
    [SerializeField] private GameObject playerSprite;
    [SerializeField] private GameObject weaponSprite;

    private TopDownCharacterController characterController;
    private PlayerStatHandler playerStatHandler;

    private SpriteRenderer playerRenderer;
    private SpriteRenderer weaponRenderer;
    [HideInInspector]public int isBack;
    private Animator _animation;
    private Animator weaponAnimator;
    private SpriteLibrary PlayerSpritelibrary;
    private SpriteLibrary WeaponSpritelibrary;

    private PhotonView pv;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        _animation = playerSprite.GetComponent<Animator>();
        weaponAnimator = weaponSprite.GetComponent<Animator>();
        PlayerSpritelibrary = playerSprite.GetComponent<SpriteLibrary>();
        WeaponSpritelibrary = weaponSprite.GetComponent<SpriteLibrary>();
        playerStatHandler = GetComponent<PlayerStatHandler>();

        playerRenderer = playerSprite.GetComponentInChildren<SpriteRenderer>();
        weaponRenderer = weaponSprite.GetComponentInChildren<SpriteRenderer>();
    }
    private void Start()
    {
        characterController = GetComponent<TopDownCharacterController>();
        PlayerSpritelibrary.spriteLibraryAsset = playerStatHandler.PlayerSprite;
        WeaponSpritelibrary.spriteLibraryAsset = playerStatHandler.WeaponSprite;
        characterController.OnMoveEvent += MoveAnimator;
        characterController.OnRollEvent += RPCRollAnimator;
        characterController.OnLookEvent += LookBack;
        characterController.OnAttackEvent += Fire;
        playerStatHandler.OnDieEvent += Die;
        playerStatHandler.OnRegenEvent += Regen;
        playerStatHandler.HitEvent += ColorTeen;
    }


    private void Fire()
    {
        weaponAnimator.SetTrigger("IsFire");
    }


    private void LookBack(Vector2 direction)
    {
        float rotY = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;

        if (Mathf.Abs(rotY) > 90f)
        {
            _animation.SetFloat ("IsLookBack", 0);
            pv.RPC("WSO", RpcTarget.AllBuffered, rotY);
        }
        else
        {
            _animation.SetFloat("IsLookBack", 1);
            pv.RPC("WSO", RpcTarget.AllBuffered, rotY);

        }
    }

    private void MoveAnimator(Vector2 direction)
    {
        if(direction != Vector2.zero)
        {
            _animation.SetBool("IsRun",true);
        }
        else
        {
            _animation.SetBool("IsRun", false);
        }
    }


    private void RPCRollAnimator()
    {
        pv.RPC("RollAnimator", RpcTarget.AllBuffered);
    }
    [PunRPC]

    private void RollAnimator()
    {
        _animation.SetTrigger("IsRoll");
        weaponRenderer.color = new Vector4(255,255,255,0);
        Invoke("EndRollAnimator", 0.7f);
    }

    private void EndRollAnimator()
    {
        weaponRenderer.color = new Vector4(255, 255, 255, 255);
    }
    private void ColorTeen()
    {
        
    }

    [PunRPC]
    void WSO(float rotY)//WeaponSortingOrder
    {
        if (Mathf.Abs(rotY) > 90f)
        {
            weaponRenderer.sortingOrder = 6;
        }
        else
        {
            weaponRenderer.sortingOrder = 4;
        }
    }

    private void Die()
    {
        Debug.Log("Á×À½¿ä");
        _animation.SetTrigger("IsDie");
        pv.RPC("PunDie", RpcTarget.OthersBuffered);
    }

    [PunRPC]
    private void PunDie()
    {
        _animation.SetTrigger("IsDie");
    }


    private void Regen()
    {
        _animation.SetTrigger("IsRegen");
        pv.RPC("PunRegen", RpcTarget.OthersBuffered);
    }

    [PunRPC]
    private void PunRegen()
    {
        _animation.SetTrigger("IsRegen");
    }

}
