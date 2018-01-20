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
    private string lastAction = string.Empty;
    private int lastDelay = 0;
    private string lastColor = string.Empty;
    private string lastSpeed = string.Empty;
    private string lastObject = string.Empty;

    private string[] MovementWords = { "forwards", "backwards", "left", "right", "wait", "resume" };
    private string[] ActionWords = { "shoot", "sneak" };

    void Start() {
        StartCoroutine(Fetch());
    }

	void UpdateValues(JSONNode json, string newID, bool execute = true) {
        lastID = newID;              // always have an id
        lastAction = json[lastID][0];   // always have an action
        lastDelay = 0;                  // default value
        lastColor = string.Empty;       // default value
        lastSpeed = string.Empty;       // default value

        int jsonCount = json[lastID].Children.Count();
        if (jsonCount > 1 && json[lastID][1] != null) {
            lastDelay = json[lastID][1];
        } else {
            lastDelay = 0;
        }
        if (jsonCount > 2) {
            lastColor = json[lastID][2];
        }
        if (jsonCount > 3) {
            lastSpeed = json[lastID][3];
        }
        if (jsonCount > 4) {
            lastObject = json[lastID][4];
        }

        if (execute) {
            if (ActionWords.Contains(lastAction)) {
                //ttm.TranslateEnemy(lastAction, lastColor, lastSpeed);
				ttm.NewTranslate("enemy", lastAction, null, null, lastDelay, lastSpeed);
			} else if ((lastAction == "go to") || (lastAction == "run to")) {
              //  ttm.Translate(lastObject, lastDelay, lastSpeed);
            } else {
               // ttm.Translate(lastAction, lastDelay, lastSpeed);
            }
		}
    }

    void Process(string data) {

        JSONNode json = JSON.Parse(data);

        List<string> ids = json.Keys.ToList();
        ids.Sort();
        string latestID = ids[ids.Count-1];

        if (lastID == string.Empty) {

			UpdateValues(json, latestID, false);

            Debug.Log("First Latest ID: " + lastID);
            Debug.Log("First Latest Action: " + lastAction);
            Debug.Log("First Latest Delay: " + lastDelay);
            Debug.Log("First Latest Color: " + lastColor);
            Debug.Log("First Latest Speed: " + lastSpeed);

        } else if (string.Compare(latestID, lastID, false) > 0) { // if we have a later id

            UpdateValues(json, latestID);

            Debug.Log("Updated Latest ID: " + lastID);
            Debug.Log("Updated Latest Action: " + lastAction);
            Debug.Log("Updated Latest Delay: " + lastDelay);
            Debug.Log("Updated Latest Color: " + lastColor);
            Debug.Log("Updated Latest Speed: " + lastSpeed);
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
