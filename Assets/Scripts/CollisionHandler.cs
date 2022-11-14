using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{

    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] AudioClip successClip;
    [SerializeField] AudioClip crashClip;
    [SerializeField] ParticleSystem sucessParticles;
    [SerializeField] ParticleSystem explosionParticles;

    AudioSource myAudioSource;

    bool isTransitioning=false;

    void Start() {
        myAudioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision other) {
        if(!isTransitioning)
        {
            switch(other.gameObject.tag)
            {
                case "Friendly":
                    Debug.Log("Starting pad");
                    break;
                case "Finish":
                    Debug.Log("Reached the finish");
                    StartCoroutine(FinishSequence());
                    break;
                default:
                    Debug.Log("Destroyed rocket");
                    StartCoroutine(CrashSequence());
                    break;
            }
        }
    }

    IEnumerator CrashSequence()
    {
        isTransitioning = true;
        GetComponent<Movement>().enabled = false;
        explosionParticles.Play();
        myAudioSource.Stop();
        myAudioSource.PlayOneShot(crashClip);
        yield return new WaitForSecondsRealtime(levelLoadDelay);
        ReloadLevel();
    }

    IEnumerator FinishSequence()
    {
        isTransitioning = true;
        GetComponent<Movement>().enabled = false;
        sucessParticles.Play();
        myAudioSource.Stop();
        myAudioSource.PlayOneShot(successClip);
        yield return new WaitForSecondsRealtime(levelLoadDelay);
        LoadNextLevel();
    }

    void ReloadLevel()
    {
        int currSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currSceneIndex);
    }

    void LoadNextLevel()
    {
        int currSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currSceneIndex + 1;
        if(nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
}
