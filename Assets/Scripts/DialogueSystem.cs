using UnityEngine;
using System.Xml;
using UnityEngine.Scripting;
using UnityEngine.UI;
using TMPro;


public class DialogueSystem : MonoBehaviour 
{

    string nameOfCharacter;
    Dialogue[] dialogues;
    int nbDialogues;
    int currentDialogueIndex = 0;
    bool waitingForUserInput = false;
    bool dialogueIsActive = false;
    GameObject dialogueBox, dialoguePanel;

    void Start()
    {
        dialogueBox = GameObject.Find("textBox");
        dialoguePanel = GameObject.Find("dialoguePanel");
        GameObject.Find("dialogueImage").GetComponent<RawImage>().texture = Resources.Load<Texture2D>(gameObject.name) as Texture2D;
        dialoguePanel.SetActive(false);

        nameOfCharacter = gameObject.name;
        nbDialogues = CalculateNbDialogues();
        dialogues = new Dialogue[nbDialogues];

        LoadDialogue();
        nbDialogues = CalculateNbDialogues();

        /*
        for (int i = 0; i < nbDialogues; i++)
        {
            print("Message:" + dialogues[i].message);
            print("- Answer A:" + dialogues[i].response[0]);
            print("- Answer B:" + dialogues[i].response[0]);
        }
        */

        //StartDialogue();
    }

    private void Update()
    {
        if (dialogueIsActive)
        {
            if (!waitingForUserInput)
            {
                dialoguePanel.SetActive(true);

                if (currentDialogueIndex != -1) DisplayDialogue2();
                else
                {
                    dialogueIsActive = false;
                    dialoguePanel.SetActive(false);
                    waitingForUserInput = false;
                    currentDialogueIndex = 0;

                    GameObject.Find("Player").GetComponent<ControlPlayer>().EndTalking();

                    GameObject.Find("GameManager").GetComponent<QuestSystem>().Notify(QuestSystem.possibleActions.talk_to, nameOfCharacter);

                }

                waitingForUserInput = true;
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    currentDialogueIndex = dialogues[currentDialogueIndex].targetForResponse[0];
                    waitingForUserInput = false;
                }
                else if (Input.GetKeyDown(KeyCode.B))
                {
                    currentDialogueIndex = dialogues[currentDialogueIndex].targetForResponse[1];
                    waitingForUserInput = false;
                }
            }
        }
    }

    public void LoadDialogue()
    {
        TextAsset textAsset = (TextAsset)Resources.Load("dialogues");
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(textAsset.text);
        int dialogueIndex = 0;

        foreach (XmlNode character in doc.SelectNodes("dialogues/character"))
        {
            if(character.Attributes.GetNamedItem("name").Value == nameOfCharacter)
            {
                dialogueIndex = 0;

                foreach (XmlNode dialogueFromXML in doc.SelectNodes("dialogues/character/dialogue"))
                {
                    dialogues[dialogueIndex] = new Dialogue();
                    dialogues[dialogueIndex].message = dialogueFromXML.Attributes.GetNamedItem("content").Value;
                    int choiceIndex = 0;

                    dialogues[dialogueIndex].response = new string[2];
                    dialogues[dialogueIndex].targetForResponse = new int[2];

                    foreach (XmlNode choice in dialogueFromXML)
                    {
                        dialogues[dialogueIndex].response[choiceIndex] = choice.Attributes.GetNamedItem("content").Value;
                        dialogues[dialogueIndex].targetForResponse[choiceIndex] = int.Parse(choice.Attributes.GetNamedItem("target").Value);
                        choiceIndex++;
                    }

                    dialogueIndex++;
                }
            }
        }
    }

    public int CalculateNbDialogues()
    {
        TextAsset textAsset = (TextAsset)Resources.Load("dialogues");
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(textAsset.text);
        int dialogueIndex = 0;
        foreach (XmlNode character in doc.SelectNodes("dialogues/character"))
        {
            if (character.Attributes.GetNamedItem("name").Value == nameOfCharacter)
            {
                foreach (XmlNode dialogueFromXml in doc.SelectNodes("dialogues/character/dialogue"))
                {
                    dialogueIndex++;
                }
            }
        }

        return dialogueIndex;
    }

    public void DisplayDialogue1()
    {
        print(dialogues[currentDialogueIndex].message);
        print("[A]>" + dialogues[currentDialogueIndex].response[0]);
        print("[B]>" + dialogues[currentDialogueIndex].response[1]);
    }

    public void DisplayDialogue2()
    {
        string textToDisplay = "[" + gameObject.name + "]  " + dialogues[currentDialogueIndex].message + "\n[A]> " + dialogues[currentDialogueIndex].response[0] + "\n[B]> " + dialogues[currentDialogueIndex].response[1];

        GameObject.Find("textBox").GetComponent<TextMeshProUGUI>().text = textToDisplay;
    }
    public void StartDialogue()
    {
        waitingForUserInput = false;

        dialogueIsActive = true;
    }
}
