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
