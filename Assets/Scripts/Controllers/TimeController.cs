using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeController : MonoBehaviour
{
    float seconds;
    int minutes;
    int hours;
    int days;
    [SerializeField]
    TextMeshProUGUI timeUI;
    private void FixedUpdate()
    {
        seconds  += Time.fixedDeltaTime * 120;
        Mathf.RoundToInt(seconds);
        if (seconds >= 60)
        {
            seconds = 0;
            minutes++;
        }
        if (minutes >= 60)
        {
            minutes = 0;
            GameManager.MyInstance.IncreaseMoney(100);
            hours++;
        }
        if (hours >= 24)
        {
            hours = 0;
            days++;
        }

        string time;
        time = string.Format("{0}{1} {2}{3}{4}", 
            "Day ", days.ToString(), hours.ToString(), ":", minutes.ToString());
        timeUI.text = time;
    }
}
