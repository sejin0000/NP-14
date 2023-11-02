using UnityEngine;
using UnityEngine.U2D.Animation;

public class PlayerAnimatorController : MonoBehaviour
{
    [SerializeField] private GameObject PlayerSprite;
    [SerializeField] private GameObject weaponSprite;


    private TopDownCharacterController characterController;
    private PlayerStatHandler playerStatHandler;

    private SpriteRenderer PlayerRenderer;
    private Animator _animation;
    private SpriteLibrary PlayerSpritelibrary;
    private SpriteLibrary WeaponSpritelibrary;

    private void Awake()
    {
        characterController = GetComponent<TopDownCharacterController>();
        _animation = PlayerSprite.GetComponent<Animator>();
        PlayerRenderer = PlayerSprite.GetComponent<SpriteRenderer>();
        PlayerSpritelibrary = PlayerSprite.GetComponent<SpriteLibrary>();
        WeaponSpritelibrary = weaponSprite.GetComponent<SpriteLibrary>();
        playerStatHandler = GetComponent<PlayerStatHandler>();
    }
    private void Start()
    {
        PlayerSpritelibrary.spriteLibraryAsset = playerStatHandler.PlayerSprite;
        WeaponSpritelibrary.spriteLibraryAsset = playerStatHandler.WeaponSprite;
        characterController.OnMoveEvent += MoveAnimator;
        characterController.OnRollEvent += RollAnimator;
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

    private void RollAnimator()
    {
        _animation.SetTrigger("IsRoll");
    }
}
