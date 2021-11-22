using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAndEventsChecks : MonoBehaviour
{
    [HideInInspector] public Image img;
    [HideInInspector] public PlayerInput _playerInput;
    [HideInInspector] public EnemyBase[] _enemies;
    private PlayerMovement_Jerzy _playerMovement;
    private GameObject _dialogue;
    //public Renderer rend;
    public void Start()
    {
        GameEvents.current.onDisableUI += DisableUI;
        GameEvents.current.onEnableUI += EnableUI;
        GameEvents.current.onNPCDialogue += OnNPCDialogue;
        GameEvents.current.onLockPlayerInputs += OnLockPlayerInputs;
        GameEvents.current.onUnLockPlayerInputs += OnUnLockPlayerInputs;
        GameEvents.current.onStopAttacking += OnStop;
        GameEvents.current.onContinueAttacking += OnContinue;

        img = this.GetComponent<Image>();
    }

    private void Awake()
    {
        _playerInput = FindObjectOfType<PlayerInput>();
        _playerMovement = FindObjectOfType<PlayerMovement_Jerzy>();
        _dialogue = GameObject.Find("DialoguePanel");
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
        _playerInput.TogglePlayerInteraction(false);
        _playerMovement.LockPlayerMovement();
        print("the state of input is " + _playerInput.enabled);
    }

    public void OnUnLockPlayerInputs()
    {
        _playerInput.TogglePlayerInteraction(!_dialogue.activeSelf);

    }

    public void OnStop()
    {
        _enemies = FindObjectsOfType<EnemyBase>();
        foreach (EnemyBase enemy in _enemies) enemy.enabled = false;
        print("functioning");
    }

    public void OnContinue()
    {
        _enemies = FindObjectsOfType<EnemyBase>();
        foreach (EnemyBase enemy in _enemies) enemy.enabled = true;
    }

    public void OnNPCDialogue()
    {
        GameEvents.current.LockPlayerInputs();
        GameEvents.current.StopAttacking();
    }


}
