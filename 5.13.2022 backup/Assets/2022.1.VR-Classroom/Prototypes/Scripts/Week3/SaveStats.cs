using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.XR;
using System.Text;

public class SaveStats : MonoBehaviour
{
    string path = "";
    public GameObject Player = null;
    public PersonalStats PlayerStats;
    public StatsManager statsManager;

    public List<string> StudentNames;
    public Dictionary<string, PersonalStats.BoothStats> checker;
    public int pos = 0;
    public int length = 0;

    // Update is called once per frame
    void Update()
    {
        if (Player == null)
        {
            Player = GameObject.Find("FirstPersonPlayer(Clone)");
            PlayerStats = Player.GetComponent<PersonalStats>();
        }
    }

    public void StatsSave()
    {
        var extension = new[]
        {
            new SFB.ExtensionFilter("Text", "txt")
        };
        if (XRSettings.isDeviceActive) Cursor.lockState = CursorLockMode.None;
        SFB.StandaloneFileBrowser.SaveFilePanelAsync("Save File", Application.dataPath, "Stats", extension, writeToSave);
    }

    public void writeToSave(string strs)
    {        
        StringBuilder writer = new StringBuilder();

        if (GameManager.AmTeacher)
        {
            foreach (KeyValuePair<string, PersonalStats> item in statsManager.studentStats)
            {
                writer.AppendLine("Stats of " + item.Key + "\n");
                PersonalStats student = item.Value;
                foreach (KeyValuePair<string, PersonalStats.BoothStats> subject in student.boothStats)
                {
                    writer.AppendLine("Booth : " + item.Key + " Score : " + PlayerStats.GetPercentageScore(item.Key) + " Completed : " + PlayerStats.GetCompletedState(item.Key) + "\n");
                }
                writer.AppendLine("\n");
            }
        }
        else
        {
            writer.AppendLine("Stats\n");
            foreach (KeyValuePair<string, PersonalStats.BoothStats> item in PlayerStats.boothStats)
            {
                writer.AppendLine("Booth : " + item.Key + " Score : " + PlayerStats.GetPercentageScore(item.Key) + " Completed : " + PlayerStats.GetCompletedState(item.Key) + "\n");
            }
        }

        File.WriteAllText(strs, writer.ToString());
    }
}
