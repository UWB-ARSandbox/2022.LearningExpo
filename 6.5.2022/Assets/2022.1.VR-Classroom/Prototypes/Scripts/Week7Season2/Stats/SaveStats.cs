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
            //new SFB.ExtensionFilter("Text", "txt"),
            new SFB.ExtensionFilter("CSV", "csv")
        };
        if (XRSettings.isDeviceActive) Cursor.lockState = CursorLockMode.None;
        SFB.StandaloneFileBrowser.SaveFilePanelAsync("Save File", Application.dataPath, "Stats", extension, writeToSave);
    }

    public void writeToSave(string strs)
    {        
        StringBuilder writer = new StringBuilder();

        if (GameManager.AmTeacher)
        {
            foreach (var item in statsManager.studentStats)
            {
                //top line: name, stats categories
                writer.AppendLine($"{item.Key},Score,Completed");
                PersonalStats studentStats = item.Value;

                foreach (var boothStats in studentStats.boothStats)
                {
                    //each booth line: Name, score, completed
                    writer.AppendLine($"{boothStats.Key},{studentStats.GetPercentageScore(boothStats.Key)},{studentStats.GetCompletedState(boothStats.Key)}");
                }
                writer.AppendLine();
            }
        }
        else
        {
            writer.AppendLine($",Score,Completed");
            foreach (var item in PlayerStats.boothStats)
            {
                writer.AppendLine($"{item.Key},{PlayerStats.GetPercentageScore(item.Key)},{PlayerStats.GetCompletedState(item.Key)}");
            }
        }

        File.WriteAllText(strs, writer.ToString());
    }
}
