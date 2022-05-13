using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using ASL;

public class ChatManager : MonoBehaviour
{
    #region variables
    TMP_InputField inputField;
    Text historyText;
    bool inputActive => PlayerController.IsTypingInput;
    bool inputEmpty => inputField.text == "";

    GameObject historyObject;
    GameObject messageInput;
    Image historyBg;
    Color originalHistoryBg;
    Coroutine HideHistory;
    
    ASLObject m_ASLObject;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        messageInput = transform.Find("Background").gameObject;
        inputField = transform.Find("Background/Input").GetComponent<TMP_InputField>();
        inputField.enabled = false;
        
        historyObject = transform.Find("ChatHistory").gameObject;
        historyText = historyObject.transform.Find("Background/History").GetComponent<Text>();
        historyBg = historyObject.transform.Find("Background").GetComponent<Image>();
        originalHistoryBg = new Color(historyBg.color.r, historyBg.color.g, historyBg.color.b, historyBg.color.a);
        
        historyBg.color = new Color(originalHistoryBg.r, originalHistoryBg.g, originalHistoryBg.b, 0);
        historyText.GetComponent<CanvasRenderer>().SetAlpha(0f);

        m_ASLObject = GetComponent<ASLObject>();
        m_ASLObject._LocallySetFloatCallback(FloatReceive);
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current[Key.Enter].wasPressedThisFrame) {
            StartCoroutine(TrySendMessage());
        }
        
        if (inputField.enabled) {
            PlayerController.IsTypingInput = inputField.isFocused;
        }
    }

    IEnumerator TrySendMessage() {
        if (inputActive) {
            if (!inputEmpty) {
                string message = inputField.text;
                int playerID = GameManager.MyID;
                float[] messageToSend = new float[message.Length + 2];

                messageToSend[0] = 100;
                messageToSend[1] = playerID;
                for (int i = 2; (i - 2) < message.Length; i++) {
                    messageToSend[i] = (float)(int)message[i - 2];
                }
                
                foreach (ChatManager chatManager in FindObjectsOfType<ChatManager>()) {
                    chatManager.SendMessage(messageToSend);
                }
                
                inputField.text = "";
                
                yield return new WaitForEndOfFrame();
                inputField.ActivateInputField();
            } 
            HideChat();
            Cursor.lockState = CursorLockMode.Locked;
            inputField.enabled = false;
            PlayerController.IsTypingInput = false;
            inputField.DeactivateInputField();
            transform.Find("Background").GetComponent<Image>().enabled = false;
        }
        yield return null;
    }

    public void SendMessage(float[] messageToSend) {
        m_ASLObject.SendAndSetClaim(() => {
            m_ASLObject.SendFloatArray(messageToSend);
        });
    }

    public void OpenInput() {
        if (!inputActive) {
            if (HideHistory != null) {StopCoroutine(HideHistory);}
            ShowHistory();

            messageInput.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            inputField.enabled = true;
            PlayerController.IsTypingInput = true;
            inputField.ActivateInputField();
            transform.Find("Background").GetComponent<Image>().enabled = true;
        }
    }

    public void OnDeselect() {
        PlayerController.IsTypingInput = false;
        HideChat();
    }

    public void ShowHistory() {
        historyBg.color = originalHistoryBg;
        historyText.GetComponent<CanvasRenderer>().SetAlpha(1f);
        HideChat();
    }

    public void HideChat() {
        if (inputEmpty) {
            messageInput.SetActive(false);
            if (HideHistory != null) {StopCoroutine(HideHistory);}
            HideHistory = StartCoroutine(HideAfterTime());
        }
    }

    IEnumerator HideAfterTime() {
        yield return new WaitForSeconds(5f);
        if (!inputActive && inputEmpty) {  
            for (float i = 1; i >= 0; i -= Time.deltaTime) {
                float newAlpha = originalHistoryBg.a * i;
                Color newColor = new Color(originalHistoryBg.r, originalHistoryBg.g, originalHistoryBg.b, newAlpha);
                historyText.GetComponent<CanvasRenderer>().SetAlpha(i);
                historyBg.color = newColor;
                yield return null;
            }
        }
    }
    
    #region Receiving
    void FloatReceive(string _id, float[] _f)
    {
        int opcode = (int)_f[0];
        switch(opcode) {
            case 100:
                int playerID = (int)_f[1];
                string message = "";
                for (int i = 2; i < _f.Length; i++) {
                    message += (char)(int)_f[i];
                }
                
                message = FormatText(playerID, message);
                historyText.text += message;
                ShowHistory();
                break;
        }
    }

    string FormatText(int playerID, string message) {
        string playerName = GameLiftManager.GetInstance().m_Players[playerID];
        playerName = "<b>" + playerName + "</b>"; //bold name
        string color = playerID == 1 ? "<color=yellow>" : "<color=red>";
        playerName = color + playerName + "</color>"; //set color

        message = "\n" + playerName + "<b>:</b> " + message;
        return message;
    }
    #endregion
}