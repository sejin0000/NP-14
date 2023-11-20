using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class A0206 : MonoBehaviour
{
    private PlayerStatHandler playerStatHandler;
    private PlayerInput playerInput;
    private float curATKSpeed;
    private float goalATKSpeed;
    private float addedATKSpeed;
    private bool isLerping;

    private void Awake()
    {
        playerStatHandler = GetComponent<PlayerStatHandler>();
        playerInput = GetComponent<PlayerInput>();
        isLerping = false;
        addedATKSpeed = 0;
    }

    private void Update()
    {
        if (playerInput.actions["Attack"].ReadValue<float>() == 1 && !isLerping)
        {
            isLerping = true;
            StartCoroutine(LerpATKSpeed());
        }
    }

    private IEnumerator LerpATKSpeed()
    {
        GetATKSpeed();
        float elapsedTime = 0f;
        while (playerInput.actions["Attack"].ReadValue<float>() == 1)
        {
            if (playerStatHandler.CurAmmo == 0)
            {
                playerStatHandler.AtkSpeed.added -= addedATKSpeed;
                addedATKSpeed = 0;
                yield return null;
            }
            elapsedTime += Time.deltaTime;
            playerStatHandler.AtkSpeed.added += (goalATKSpeed - curATKSpeed) * elapsedTime;
            addedATKSpeed += (goalATKSpeed - curATKSpeed) * elapsedTime;
            yield return null;
        }
        isLerping = false;
        playerStatHandler.AtkSpeed.added -= addedATKSpeed;
        addedATKSpeed = 0;
    }

    private void GetATKSpeed()
    {
        curATKSpeed = playerStatHandler.AtkSpeed.total;
        goalATKSpeed = curATKSpeed * 1.0005f;
    }
}
