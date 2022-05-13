using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScreen : MonoBehaviour
{
    public GameObject Stats;
    public GameObject Announcement;
    public GameObject Controls;
    public Button Refresh;

    public void flipScreen()
    {
        if (gameObject.activeSelf == false)
        {
            gameObject.SetActive(true);
            Refresh.onClick.Invoke();
            Stats.SetActive(false);
            Announcement.SetActive(false);
            Controls.SetActive(false);
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            gameObject.SetActive(false);
            Stats.SetActive(false);
            Announcement.SetActive(false);
            Controls.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
