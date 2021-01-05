using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HandleGameEnd : MonoBehaviour {
    public float TransitionTime;
    public Animator CrossFade;

    public void EndGame() {
        StartCoroutine(Transition());
    }

    IEnumerator Transition() {
        CrossFade.SetTrigger("EndLevel");
        yield return new WaitForSeconds(TransitionTime);
        SceneManager.LoadScene(0);
    }
}

