using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAndEventsChecks : MonoBehaviour
{
    public Image img;
    public PlayerInput _playerInput;
    public PlayerDetection _PlayerDetection;
    //public Renderer rend;
    public void Start()
    {
        GameEvents.current.onDisableUI += DisableUI;
        GameEvents.current.onEnableUI += EnableUI;
        GameEvents.current.onNPCDialogue += OnNPCDialogue;
        GameEvents.current.onLockPlayerInputs += OnLockPlayerInputs;
        GameEvents.current.onUnLockPlayerInputs += OnUnLockPlayerInputs;
        /*GameEvents.current.onStopAttacking += OnStop;
        GameEvents.current.onContinueAttacking += OnContinue;*/

        img = this.GetComponent<Image>();
    }

    private void Awake()
    {
        _playerInput = FindObjectOfType<PlayerInput>();
        _PlayerDetection = FindObjectOfType<PlayerDetection>();
    }

    public void DisableUI()
    {
        if (this.img != null)
        {
            img.enabled = false;
        }
        gameObject.SetActive(false);
    }

    private void EnableUI()
    {
        if (this.img != null)
        {
            img.enabled = true;
        }
        gameObject.SetActive(true);
    }

    public void OnLockPlayerInputs()
    {
        _playerInput.enabled = false;
        print("the state of input is " + _playerInput.enabled);
    }

    public void OnUnLockPlayerInputs()
    {
        _playerInput.enabled = true;
    }

   /* public void OnStop()
    {
        FindObjectOfType<PlayerDetection>().GetComponent<PlayerDetection>().stopAttacking = false;
        print("functioning");
    }

    public void OnContinue()
    {
        FindObjectOfType<PlayerDetection>().stopAttacking = true;
    } */

    public void OnNPCDialogue()
    {
        GameEvents.current.LockPlayerInputs();
        GameEvents.current.StopAttacking();
    }
}
