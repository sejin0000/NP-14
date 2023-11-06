using Photon.Pun;
using System;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UIElements;

public class PlayerAnimatorController : MonoBehaviour
{
    [SerializeField] private GameObject PlayerSprite;
    [SerializeField] private GameObject weaponSprite;


    private TopDownCharacterController characterController;
    private PlayerStatHandler playerStatHandler;

    public SpriteRenderer weaponRenderer;
    [HideInInspector]public int isBack;
    private Animator _animation;
    private SpriteLibrary PlayerSpritelibrary;
    private SpriteLibrary WeaponSpritelibrary;

    private PhotonView pv;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        _animation = PlayerSprite.GetComponent<Animator>();
        PlayerSpritelibrary = PlayerSprite.GetComponent<SpriteLibrary>();
        WeaponSpritelibrary = weaponSprite.GetComponent<SpriteLibrary>();
        playerStatHandler = GetComponent<PlayerStatHandler>();
    }
    private void Start()
    {
        characterController = GetComponent<TopDownCharacterController>();
        PlayerSpritelibrary.spriteLibraryAsset = playerStatHandler.PlayerSprite;
        WeaponSpritelibrary.spriteLibraryAsset = playerStatHandler.WeaponSprite;
        characterController.OnMoveEvent += MoveAnimator;
        characterController.OnRollEvent += RPCRollAnimator;
        characterController.OnLookEvent += LookBack;
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
}
