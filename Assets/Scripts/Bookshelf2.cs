using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Bookshelf2 : Interactable
{
    [SerializeField] private TextMeshProUGUI msg;
    [SerializeField] private Image panel;
    [SerializeField] private TextMeshProUGUI inter;
    private float lastAlertTime;
    private float alertCooldown = 4;

    public override void OnFocus()
    {
        inter.gameObject.SetActive(true);
    }

    public override void OnInteract()
    {
        lastAlertTime = Time.time;
        msg.gameObject.SetActive(true);
        panel.gameObject.SetActive(true);
        msg.text = "In the room where coffee is milled, I am in a place aligned with human eyes";
    }

    public override void OnLoseFocus()
    {
        inter.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (lastAlertTime != 0)
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
        inter.gameObject.SetActive(false);
    }
}
