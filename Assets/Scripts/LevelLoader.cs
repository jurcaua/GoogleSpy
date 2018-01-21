using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {

    public Animator a;
    public GameObject explosion;

    public string[] sceneNames;
    public Transform[] explosionSpawns; 

	public void LoadScene(int num) {
        StartCoroutine(Load(sceneNames[num - 1]));
        Instantiate(explosion, explosionSpawns[num - 1]);
    }

    IEnumerator Load(string sceneName) {
        FadeOut();
        yield return new WaitForSeconds(a.GetCurrentAnimatorStateInfo(0).normalizedTime);
        SceneManager.LoadScene(sceneName);
    }

    void FadeOut() {
        a.SetTrigger("fade");
    }
}
