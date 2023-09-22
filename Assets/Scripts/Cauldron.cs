using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Cauldron : Interactable
{
    [SerializeField] private Image greenPot;
    [SerializeField] private Image yellowPot;
    [SerializeField] private Image pinkPot;
    [SerializeField] private TextMeshProUGUI msg;
    [SerializeField] private Image panel;
    private float lastAlertTime;
    private float alertCooldown = 4;

    public override void OnFocus(){ }
    public override void OnLoseFocus() { }

    public override void OnInteract()
    {
        if (greenPot.gameObject.activeInHierarchy == true && yellowPot.gameObject.activeInHierarchy == true && pinkPot.gameObject.activeInHierarchy == true)
        {
            SceneManager.LoadScene("Victory");
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        else
        {
            lastAlertTime = Time.time;
            msg.gameObject.SetActive(true);
            panel.gameObject.SetActive(true);
            msg.text = "You don't have enough potions!";

        }

    }

    private void Update()
    {
        if (lastAlertTime!=0)
        {
            if (Time.time > (lastAlertTime + alertCooldown))
            {
                msg.gameObject.SetActive(false);
                panel.gameObject.SetActive(false);
                lastAlertTime = 0;
            }
        }

    }

    private void Start()
    {
        lastAlertTime = 0;
        msg.gameObject.SetActive(false);
        panel.gameObject.SetActive(false);
    }
}
