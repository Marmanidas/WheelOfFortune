using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WheelButton : MonoBehaviour
{
    private WheelOfFortune wheelOfFortune;
    private Button thisButton;

    private void Awake()
    {
        thisButton = GetComponent<Button>();
        wheelOfFortune = GetComponentInParent<WheelOfFortune>();
    }

    private void Start()
    {
        thisButton.onClick.AddListener(SetButton);
    }

    void SetButton()
    {
        int number = int.Parse(this.gameObject.name);
        wheelOfFortune.ButonNumber(number);
    }


}
