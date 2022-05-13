using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatScreen : MonoBehaviour
{
    public GameObject List;
    public GameObject BoothObject;
    public GameObject TitleObject;
    public GameObject Player;
    public PersonalStats PlayerStats;
    public StatsManager statsManager;

    public Button Left;
    public Button Right;

    private bool Started = false;
    private bool Teacher = GameManager.AmTeacher;
    public List<string> StudentNames;
    public Dictionary<string, PersonalStats.BoothStats> checker;
    public int pos = 0;
    public int length = 0;

    void Start()
    {
        StartCoroutine(DelayedInit());
    }

    IEnumerator DelayedInit()
    {
        while (Player == null)
        {
            Player = GameObject.Find("FirstPersonPlayer(Clone)");
            if (Player != null) {
                PlayerStats = Player.GetComponent<PersonalStats>();
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void createList()
    {
        if(Started != true)
        {
            Player = GameObject.Find("FirstPersonPlayer(Clone)");
            PlayerStats = Player.GetComponent<PersonalStats>();
            Started = true;
        }

        if(Teacher == true)
        {
            Left.gameObject.SetActive(true);
            Right.gameObject.SetActive(true);
            foreach(KeyValuePair<string, PersonalStats> item in statsManager.studentStats)
            {
                if (!StudentNames.Contains(item.Key))
                {
                    StudentNames.Add(item.Key);
                }
            }
            length = StudentNames.Count;
        }
        else
        {
            Left.gameObject.SetActive(false);
            Right.gameObject.SetActive(false);
            foreach (KeyValuePair<string, PersonalStats.BoothStats> item in PlayerStats.boothStats)
            {
                var newBoothStat = Instantiate(BoothObject, List.transform, false) as GameObject;
                newBoothStat.GetComponent<BoothStatsContainer>().SetStats(item.Key, PlayerStats.GetTimeInBooth(item.Key), PlayerStats.GetPercentageScore(item.Key), PlayerStats.GetTimeTaken(item.Key), PlayerStats.GetCompletedState(item.Key), PlayerStats.GetNumQuestionsTimedOut(item.Key));
            }
        }

        var TitleStat = Instantiate(TitleObject, List.transform, false) as GameObject;
    }

    public void updateList(bool val)
    {
        if (val)
        {
            resetList();
            
            pos++;
            if (pos == length)
            {
                pos = 0;
            }
            string temp = StudentNames[pos];
            PersonalStats student = statsManager.studentStats[temp];
            checker = student.boothStats;
            foreach (KeyValuePair<string, PersonalStats.BoothStats> item in student.boothStats)
            {
                var newBoothStat = Instantiate(BoothObject, List.transform, false) as GameObject;
                newBoothStat.GetComponent<BoothStatsContainer>().SetStats(item.Key, student.GetTimeInBooth(item.Key), student.GetPercentageScore(item.Key), student.GetTimeTaken(item.Key), student.GetCompletedState(item.Key), student.GetNumQuestionsTimedOut(item.Key));
            }

            var TitleStat = Instantiate(TitleObject, List.transform, false) as GameObject;
        }
        else
        {
            resetList();
            
            pos--;
            if (pos < 0)
            {
                pos = length-1;
            }
            string temp = StudentNames[pos];
            PersonalStats student = statsManager.studentStats[temp];
            checker = student.boothStats;
            foreach (KeyValuePair<string, PersonalStats.BoothStats> item in student.boothStats)
            {
                var newBoothStat = Instantiate(BoothObject, List.transform, false) as GameObject;
                newBoothStat.GetComponent<BoothStatsContainer>().SetStats(item.Key, student.GetTimeInBooth(item.Key), student.GetPercentageScore(item.Key), student.GetTimeTaken(item.Key), student.GetCompletedState(item.Key), student.GetNumQuestionsTimedOut(item.Key));
            }

            var TitleStat = Instantiate(TitleObject, List.transform, false) as GameObject;
        }
    }

    public void resetList()
    {
        foreach (Transform child in List.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public void flipStatScreen()
    {
        if (gameObject.activeSelf == false)
        {
            Cursor.lockState = CursorLockMode.Confined;
            gameObject.SetActive(true);
            resetList();
            createList();
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            gameObject.SetActive(false);
        }
    }
}
