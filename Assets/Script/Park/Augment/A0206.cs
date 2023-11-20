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
    private bool isLerping;

    private void Awake()
    {
        playerStatHandler = GetComponent<PlayerStatHandler>();
        playerInput = GetComponent<PlayerInput>();
        isLerping = false;
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
            elapsedTime += Time.deltaTime;
            playerStatHandler.AtkSpeed.added = (Mathf.Lerp(curATKSpeed, goalATKSpeed, elapsedTime) - curATKSpeed);
            yield return null;
        }
        isLerping = false;
        float addedATKSpeed = playerStatHandler.AtkSpeed.total - curATKSpeed;
        playerStatHandler.AtkSpeed.added -= addedATKSpeed;
    }

    private void GetATKSpeed()
    {
        curATKSpeed = playerStatHandler.AtkSpeed.total;
        goalATKSpeed = curATKSpeed * 2.5f;
    }
}
