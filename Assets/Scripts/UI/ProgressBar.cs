using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    private Slider slider;
    [SerializeField] float duration;
    [SerializeField] float updateInterval;
    [SerializeField] GameObject blockingPopup;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void OnEnable()
    {
        StartCoroutine(FillProgressBar());
    }

    private IEnumerator FillProgressBar()
    {
        float timer = 0.0f;

        while (timer < duration)
        {
            Gamemanager.instance.EnableDisableGameButtons(false);
            timer += updateInterval;
            slider.value = timer / duration;
            yield return new WaitForSeconds(updateInterval);
        }

        Gamemanager.instance.SetMessage("You got a Coin!");
        Gamemanager.instance.EnableDisableGameButtons(true);
        blockingPopup.SetActive(true);
        slider.value = 0;
        slider.gameObject.SetActive(false);
    }

}
