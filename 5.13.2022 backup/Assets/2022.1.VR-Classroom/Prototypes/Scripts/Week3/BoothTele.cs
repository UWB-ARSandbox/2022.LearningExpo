using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BoothTele : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> BoothTelePoints;
    public List<bool> BoothOpen;
    public List<GameObject> OpenBooths;
    public TMP_Dropdown list;
    public Dropdown listVR;
    public Button trigger;
    public Button refresh;

    public bool VR;

    private GameObject Player;
    private PersonalStats PlayerStats;
    private bool found;

    IEnumerator DelayedInit()
    {
        while (Player == null)
        {
            Player = GameObject.Find("FirstPersonPlayer(Clone)");
            if (Player != null)
            {
                PlayerStats = Player.GetComponent<PersonalStats>();
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    void Start()
    {
        var foundPoints = FindObjectsOfType<Telepoint>();
        foreach(var temp in foundPoints)
        {
            BoothTelePoints.Add(temp.gameObject);
        }

        foreach(var temp in BoothTelePoints)
        {
            BoothOpen.Add(temp.transform.parent.GetComponentInChildren<LockToggle>().locked);
        }


    }

    public void updateList()
    {
        if (VR)
        {
            listVR.ClearOptions();
        }
        else
        {
            list.ClearOptions();
        }
        OpenBooths.Clear();
        BoothOpen.Clear();

        foreach (var temp in BoothTelePoints)
        {
            BoothOpen.Add(temp.transform.parent.GetComponentInChildren<LockToggle>().locked);
        }

        for (int i = 0; i < BoothTelePoints.Count; i++)
        {
            if (!BoothOpen[i])
            {
                OpenBooths.Add(BoothTelePoints[i]);
            }
        }

        for (int i = 0; i < OpenBooths.Count; i++)
        {
            if (VR)
            {
                listVR.options.Add(new Dropdown.OptionData() { text = OpenBooths[i].transform.parent.GetComponentInParent<BoothManager>().boothName });
            }
            else
            {
                list.options.Add(new TMP_Dropdown.OptionData() { text = OpenBooths[i].transform.parent.GetComponentInParent<BoothManager>().boothName });
            }
        }
    }

    public void triggerTele()
    {
        if(Player != null && found != true)
        {
            found = true;
        }
        else
        {
            Player = GameObject.Find("FirstPersonPlayer(Clone)");
            if (Player != null)
            {
                found = true;
                PlayerStats = Player.GetComponent<PersonalStats>();
            }
        }
        

        if (Player != null)
        {
            //Debug.Log("value for tele : " + list.value);
            Player.transform.GetComponent<CharacterController>().enabled = false;
            if (VR)
            {
                Player.transform.position = OpenBooths[listVR.value].transform.position + (Vector3.up);
            }
            else
            {
                Player.transform.position = OpenBooths[list.value].transform.position + (Vector3.up);
            }
            Player.transform.GetComponent<CharacterController>().enabled = true;
        }

        if (VR)
        {
            listVR.ClearOptions();
        }
        else
        {
            list.ClearOptions();
        }
        OpenBooths.Clear();
    }
}
