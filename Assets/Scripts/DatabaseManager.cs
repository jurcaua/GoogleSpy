using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.Linq;

public class DatabaseManager : MonoBehaviour {

    public string url = "https://uofthacks-7d047.firebaseio.com/dir/.json";     // insecure, but a work-around for now
    public float fetchDelay = 0.5f;                                             // how often we check the database for new entries

    public TextToMovement ttm;  // send all parsed queries to this script

    [Header("Debug")] // debug variables
    public bool fetchingEnabled = true; // disable fetching from database for testing

    private string lastID = null;   // used to know if new data has appeared

    // holds previous query data
    private string lastCommandType = null;
    private string lastCommand = null;
    private string lastDirection = null;
    private string lastColor = null;
    private string lastObject = null;
    private string lastSpeed = null;
    private int lastDelay = 0;

    void Start() {
        StartCoroutine(Fetch());
    }

    // fetches from database public .json object every "fetchingEnabled" seconds
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

    // turns json query result into usable object and checks if we have new data
    void Process(string data) {

        JSONNode json = JSON.Parse(data);

        List<string> ids = json.Keys.ToList();
        ids.Sort();
        string latestID = ids[ids.Count - 1];

        // on startup, do not do the most recent query
        if (lastID == null) {

            UpdateValues(json, latestID, false);

        } else if (string.Compare(latestID, lastID, false) > 0) { // if we have a later id, update data

            UpdateValues(json, latestID);

        }
    }

    void UpdateValues(JSONNode json, string newID, bool execute = true) {

        lastID = newID; // new id value

        // set to default values in the beginning
        lastCommandType = json[lastID][0]; // always a command type
        lastCommand = null;
        lastDirection = null;
        lastColor = null;
        lastObject = null;
        lastSpeed = null;
        lastDelay = 0;

        if (lastCommandType == "position") { // positional command [type = "position", command, direction, object, speed, delay]
            lastCommand = json[lastID][1];
            lastDirection = json[lastID][2];
            lastObject = json[lastID][3];
            lastSpeed = json[lastID][4];
            lastDelay = int.Parse(json[lastID][5]);

            // translate commands to player
            if (execute) {
                ttm.NewTranslate(lastCommandType, lastCommand, lastDirection, lastObject, lastSpeed, lastDelay);
            }

            Debug.Log("Updated Latest ID: " + lastID);
            Debug.Log("Updated Latest Command: " + lastCommand);
            Debug.Log("Updated Latest Direction: " + lastDirection);
            Debug.Log("Updated Latest Object: " + lastObject);
            Debug.Log("Updated Latest Speed: " + lastSpeed);
            Debug.Log("Updated Latest Delay: " + lastDelay);

        } else if (lastCommandType == "enemy") { // enemy command [type = "enemy", command, direction, color, speed, delay]
            lastCommand = json[lastID][1];
            lastDirection = json[lastID][2];
            lastColor = json[lastID][3];
            lastSpeed = json[lastID][4];
            lastDelay = int.Parse(json[lastID][5]);

            // translate commands to player
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
}
