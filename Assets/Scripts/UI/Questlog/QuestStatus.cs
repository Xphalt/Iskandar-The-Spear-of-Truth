/*
 * Dominique 08-12-2021
 * Set the quest log status sprite to a tick when completed
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestStatus : MonoBehaviour
{
    [SerializeField] private Sprite completeSprite;
    [SerializeField] private Image statusImage;

    public void SetComplete()
    {
        statusImage.sprite = completeSprite;
    }
}