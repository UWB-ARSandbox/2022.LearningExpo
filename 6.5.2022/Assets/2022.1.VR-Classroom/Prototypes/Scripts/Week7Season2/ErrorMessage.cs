using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorMessage : MonoBehaviour
{
    public void trigger()
    {
        ClosePopup();
    }

    IEnumerator ClosePopup()
    {
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }
}
