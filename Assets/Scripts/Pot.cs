using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pot : Interactable
{
    [SerializeField] private Image currentPot;

    public override void OnFocus() { }
    public override void OnLoseFocus() { }

    public override void OnInteract()
    {
        currentPot.gameObject.SetActive(true);
        gameObject.SetActive(false);

    }
}
