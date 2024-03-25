using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Script to load to rocket so that it can process different types of collisions
public class CollisionHandler : MonoBehaviour
{

    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] AudioClip successClip;
    [SerializeField] AudioClip crashClip;
    [SerializeField] ParticleSystem sucessParticles;
    [SerializeField] ParticleSystem explosionParticles;

    AudioSource myAudioSource;

    bool isTransitioning=false;
    bool collisionDisable = false;

    void Start() 
    {
        myAudioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        processDebugKeys();
    }

    //Keys used for easily debugging different parts of the game. 
    //Would remove/hide this for production
    private void processDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionDisable = !collisionDisable; //toggle collision
        }
    }

    //If the rocket is still eligible for collisions then
    //check which type of collision and handle appropratiely
    void OnCollisionEnter(Collision other) 
    {
        if(!isTransitioning && !collisionDisable)
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

    //Handle crash
    IEnumerator CrashSequence()
    {
        //Once it is entering this state we don't want it to trigger other collisions
        isTransitioning = true;
        GetComponent<Movement>().enabled = false;
        explosionParticles.Play();
        myAudioSource.Stop();
        myAudioSource.PlayOneShot(crashClip);
        yield return new WaitForSecondsRealtime(levelLoadDelay);
        ReloadLevel();
    }

    //Handle successful landing
    IEnumerator FinishSequence()
    {
        //Once it is entering this state we don't want it to trigger other collisions
        isTransitioning = true;
        GetComponent<Movement>().enabled = false;
        sucessParticles.Play();
        myAudioSource.Stop();
        myAudioSource.PlayOneShot(successClip);
        yield return new WaitForSecondsRealtime(levelLoadDelay);
        LoadNextLevel();
    }


/* Should move these to a separate script handling level loads and trigger using events
*/
    //Loads current level on fail
    void ReloadLevel()
    {
        int currSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currSceneIndex);
    }

    //Loads next level on success. 
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
