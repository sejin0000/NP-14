using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class PlayerAnimatorController : MonoBehaviour
{
    [SerializeField] private GameObject mainSprite;
    private TopDownCharacterController characterController;
    private PlayerStatHandler playerStatHandler;

    private SpriteRenderer _mainRenderer;
    private Animator _animation;
    private SpriteLibrary _library;
    SpriteLibraryAsset sprite;

    private void Awake()
    {
        characterController = GetComponent<TopDownCharacterController>();
        _animation = mainSprite.GetComponent<Animator>();
        _mainRenderer = mainSprite.GetComponent<SpriteRenderer>();
        _library = mainSprite.GetComponent<SpriteLibrary>();
        playerStatHandler = GetComponent<PlayerStatHandler>();
    }
    private void Start()
    {
        _library.spriteLibraryAsset = playerStatHandler.Sprite;
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
