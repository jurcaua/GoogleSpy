using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.Linq;

public class DatabaseManager : MonoBehaviour {

    public string url = "https://uofthacks-7d047.firebaseio.com/dir/.json";
    public float fetchDelay = 0.5f;

    public TextToMovement ttm;

    [Header("Debug")]
    public bool fetchingEnabled = true;

    private string lastID = string.Empty;

    private string lastCommandType = string.Empty;
    private string lastCommand = string.Empty;
    private string lastDirection = string.Empty;
    private string lastColor = string.Empty;
    private string lastObject = string.Empty;
    private string lastSpeed = string.Empty;
    private int lastDelay = 0;

    void Start() {
        StartCoroutine(Fetch());
    }

    void UpdateValues(JSONNode json, string newID, bool execute = true) {

        // new id value
        lastID = newID;

        // set to default values
        lastCommandType = json[lastID][0]; // always a command type
        lastCommand = string.Empty;
        lastDirection = string.Empty;
        lastColor = string.Empty;
        lastObject = string.Empty;
        lastSpeed = string.Empty;
        lastDelay = 0;

        if (lastCommandType == "position") {
            lastCommand = json[lastID][1];
            lastDirection = json[lastID][2];
            lastObject = json[lastID][3];
            lastSpeed = json[lastID][4];
            lastDelay = int.Parse(json[lastID][5]);

            if (execute) {
                ttm.NewTranslate(lastCommandType, lastCommand, lastDirection, lastObject, lastSpeed, lastDelay);
            }

            Debug.Log("Updated Latest ID: " + lastID);
            Debug.Log("Updated Latest Command: " + lastCommand);
            Debug.Log("Updated Latest Direction: " + lastDirection);
            Debug.Log("Updated Latest Object: " + lastObject);
            Debug.Log("Updated Latest Speed: " + lastSpeed);
            Debug.Log("Updated Latest Delay: " + lastDelay);

        } else if (lastCommandType == "enemy") {
            lastCommand = json[lastID][1];
            lastDirection = json[lastID][2];
            lastColor = json[lastID][3];
            lastSpeed = json[lastID][4];
            lastDelay = int.Parse(json[lastID][5]);

            if (execute) {
                ttm.NewTranslate(lastCommandType, lastCommand, lastDirection, lastColor, lastSpeed, lastDelay);
            }

            Debug.Log("Updated Latest ID: " + lastID);
            Debug.Log("Updated Latest Command: " + lastCommand);
            Debug.Log("Updated Latest Direction: " + lastDirection);
            Debug.Log("Updated Latest Color: " + lastColor);
            Debug.Log("Updated Latest Speed: " + lastSpeed);
            Debug.Log("Updated Latest Delay: " + lastDelay);

        } else {
            Debug.LogWarning("Unknown command: " + lastCommandType);
            return;
        }
    }

    void Process(string data) {

        JSONNode json = JSON.Parse(data);

        List<string> ids = json.Keys.ToList();
        ids.Sort();
        string latestID = ids[ids.Count - 1];

        if (lastID == string.Empty) {

            UpdateValues(json, latestID, false);

        } else if (string.Compare(latestID, lastID, false) > 0) { // if we have a later id

            UpdateValues(json, latestID);

        }
    }

    IEnumerator Fetch() {

        while (true) {

            if (fetchingEnabled) {
                using (WWW www = new WWW(url)) {
                    yield return www;

                    if (www.error == null) {
                        Process(www.text);

                    } else {
                        Debug.Log("FETCH ERROR: " + www.error);
                    }
                }

                yield return new WaitForSeconds(fetchDelay);
            }
        }
    }
}
