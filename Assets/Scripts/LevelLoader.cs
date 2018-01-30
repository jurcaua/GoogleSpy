using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {

    public Animator a;              // fade out animator
    public GameObject explosion;    // explosion prefab to instantiate

    // index number means level number (+ 1)
    public string[] sceneNames;             // for loading a certain scene
    public Transform[] explosionSpawns;     // for spawning explosion effect

    void Start() {
        // sanity check
        if (sceneNames.Length != explosionSpawns.Length) {
            Debug.LogWarning("Not the same amount of scene names and explosion spawns entered!");
        }
    }

	public void LoadScene(int num) {
        StartCoroutine(Load(sceneNames[num - 1]));
        Instantiate(explosion, explosionSpawns[num - 1]);
    }

    // fades out, waits for end of fade out animation, and loads new scene
    IEnumerator Load(string sceneName) {
        FadeOut();
        yield return new WaitForSeconds(a.GetCurrentAnimatorStateInfo(0).normalizedTime);
        SceneManager.LoadScene(sceneName);
    }

    void FadeOut() {
        a.SetTrigger("fade");
    }
}
