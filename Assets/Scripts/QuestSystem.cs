using UnityEngine;
using UnityEngine.UI;
using System;
using System.Xml;
using System.ComponentModel;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.Rendering;
using TMPro;
//using UnityEditor.SearchService;

public class QuestSystem: MonoBehaviour
{

    int currentStage;
    bool timerStarted;
    float timer;
    List<string> actions, targets, xps;
    List<bool> objectiveAchieved;

    string stageTitle, stageDescription, stageObjectives, startingPointForPlayer;
    bool panelDisplayed = true;
    float displayTimer;
    bool startDisplayTimer;

    int nbObjectivesAchieved = 0;
    int nbObjectivesToAchieve;
    int XPAchieved;

    public enum possibleActions { do_nothing = 0, talk_to = 1, acquire_a = 2, destroy_one = 3, enter_place_called = 4 };
    List<possibleActions> actionsForQuest;
    public GameObject stagePanel, stageTitleText, stageDescriptionText, stageObjectivesText;

    [SerializeField]
    GameObject player;

    private void Start()
    {

        Init();
        MovePlayerToStartingPoint();

    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.H))
        {

            panelDisplayed = !panelDisplayed;
            Display(panelDisplayed);

        }

        if (startDisplayTimer)
        {
            displayTimer += Time.deltaTime;
            if (displayTimer >= 2)
            {
                displayTimer = 0.0f;
                startDisplayTimer = false;
                GameObject.Find("notifyMessage").GetComponent<TextMeshProUGUI>().text = "";
            }
        }

    }

    public void Init() 
    {
        stageTitleText = GameObject.Find("stageTitle");
        stageObjectivesText = GameObject.Find("stageObjectives");
        stageDescriptionText = GameObject.Find("stageDescription");
        stagePanel = GameObject.Find("stagePanel");

        currentStage = GetComponent<GameManager>().GetStage();
        nbObjectivesAchieved = 0;
        nbObjectivesToAchieve = 0;
        actions = new List<string>();
        targets = new List<string>();
        xps = new List<string>();
        objectiveAchieved = new List<bool>();
        actionsForQuest = new List<possibleActions>();
        LoadQuest2();

        DisplayQuestInfo();
    }

    /*
    public void LoadQuest()
    {

        TextAsset textAsset = (TextAsset)Resources.Load("quest");
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(textAsset.text);
        stageObjectives = "For this stage, you need to:\n";

        foreach (XmlNode stage in doc.SelectNodes("quest/stage"))
        {
            if (stage.Attributes.GetNamedItem("id").Value == "" + currentStage)
            {
                stageTitle = stage.Attributes.GetNamedItem("name").Value;
                stageDescription = stage.Attributes.GetNamedItem("description").Value;
                foreach (XmlNode results in stage)
                {
                    print("For this stage, you need to:\n");
                    foreach (XmlNode result in results)
                    {

                        string action = result.Attributes.GetNamedItem("action").Value;
                        string target = result.Attributes.GetNamedItem("target").Value;
                        string xp = result.Attributes.GetNamedItem("xp").Value;
                        actions.Add(action);
                        targets.Add(target);
                        xps.Add(xp);
                        objectiveAchieved.Add(false);
                        print(action + " " + target + "[" + xp + " XP]");
                        stageObjectives += "\n -> " + action + " " + target + " [ " + xp + " XP]";
                        nbObjectivesToAchieve++;

                    }
                }
            }
        }
    }
    */

    public void LoadQuest2()
    {

        TextAsset textAsset = (TextAsset)Resources.Load("quest");
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(textAsset.text);
        stageObjectives = "For this stage, you need to:\n";

        foreach (XmlNode stage in doc.SelectNodes("quest/stage"))
        {
            if (stage.Attributes.GetNamedItem("id").Value == "" + currentStage)
            {
                stageTitle = stage.Attributes.GetNamedItem("name").Value;
                stageDescription = stage.Attributes.GetNamedItem("description").Value;
                foreach (XmlNode results in stage)
                {
                    //print("For this stage, you need to:\n");
                    foreach (XmlNode result in results)
                    {

                        string action = result.Attributes.GetNamedItem("action").Value;
                        string target = result.Attributes.GetNamedItem("target").Value;
                        string xp = result.Attributes.GetNamedItem("xp").Value;
                        possibleActions actionForQuest = possibleActions.do_nothing;

                        if (action.IndexOf("Acquire") >= 0)
                        {
                            actionForQuest = possibleActions.acquire_a;
                        }
                        else if (action.IndexOf("Talk") >= 0)
                        {
                            actionForQuest = possibleActions.talk_to;
                        }
                        else if (action.IndexOf("Destroy") >= 0 && action.IndexOf("one") >= 0)
                        {
                            actionForQuest = possibleActions.destroy_one;
                        }
                        else if (action.IndexOf("Enter") >= 0 && action.IndexOf("place") >= 0)
                        {
                            actionForQuest = possibleActions.enter_place_called;
                        }

                        actionsForQuest.Add(actionForQuest);
                        targets.Add(target);
                        xps.Add(xp);
                        objectiveAchieved.Add(false);
                        print(action + " " + target + "[" + xp + " XP]");
                        stageObjectives += "\n -> " + action + " " + target + " [ " + xp + " XP]";
                        nbObjectivesToAchieve++;
                        //print(nbObjectivesToAchieve);
                    }
                }
            }
        }
    }

    void MovePlayerToStartingPoint()
    {

        GameObject p;
        if (SceneManager.GetActiveScene().name == "level1")
        {
            p = Instantiate(player);
            p.name = "Player";
            p.transform.rotation = new Quaternion(0, 0, 0, 0);

            p.transform.position = GameObject.Find("startingPoint").transform.position;
            p.transform.parent = gameObject.transform;
        }
        else if (SceneManager.GetActiveScene().name == "level2" || SceneManager.GetActiveScene().name == "level3")
        {

            p = GameObject.Find("Player");
            p.transform.position = GameObject.Find("startingPoint").transform.position;

        }

    }

    void DisplayQuestInfo()
    {

        stageTitleText.GetComponent<TextMeshProUGUI>().text = stageTitle;
        stageDescriptionText.GetComponent<TextMeshProUGUI>().text = stageDescription;
        stageObjectivesText.GetComponent<TextMeshProUGUI>().text = stageObjectives + "\n Press H to Hide/Display this information";
    
    }

    void Display(bool display)
    {

        stagePanel.SetActive(display);

    }

    public void Notify(possibleActions action, string target)
    {

        print("Notified: Action=" + action + " Target:" + target);
        for (int i = 0; i < actionsForQuest.Count; i++)
        {
            if (action == actionsForQuest[i] && target == targets[i] && !objectiveAchieved[i])
            {
                DisplayMessage("+" + xps[i] + " XP");
                nbObjectivesAchieved++;
                //print(nbObjectivesAchieved);
                XPAchieved += Int32.Parse(xps[i]);
                objectiveAchieved[i] = true;
                break;
            }
        }

        if (nbObjectivesAchieved == nbObjectivesToAchieve)
        {
            DisplayMessage("Stage Complete");
            GetComponent<GameManager>().player.XP = CalculateTotalXPForLevel();
            Invoke("StageComplete", 2);

        }


    }

    void DisplayMessage(string message)
    {

        GameObject.Find("notifyMessage").GetComponent<TextMeshProUGUI>().text = message;
        startDisplayTimer = true;

    }

    int CalculateTotalXPForLevel()
    {

        int totalXP = 0;
        for (int i = 0; i < actionsForQuest.Count; i++)
        {
            totalXP += Int32.Parse(xps[i]);
        }
        return totalXP;

    }

    void StageComplete()
    {

        if (SceneManager.GetActiveScene().name != "level3")
        {
            SceneManager.LoadScene("levelComplete");
        }
        else
        {
            SceneManager.LoadScene("endScreen");
            Destroy(gameObject);
        }

        //SceneManager.LoadScene("levelComplete");

    }
}
