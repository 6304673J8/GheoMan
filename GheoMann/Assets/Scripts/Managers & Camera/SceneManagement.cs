using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneManagement : MonoBehaviour
{
    private int nextSceneToLoad;
    private int sceneToLoad;
    public static SceneManagement Instance { get; private set; }

    private GameObject activePlayer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Debug.Log("WARNING: multiple " + this + " in scene!");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        nextSceneToLoad = SceneManager.GetActiveScene().buildIndex + 1;
    }

    // Update is called once per frame
    void Update()
    {
        SwitchScenes();
    }

    private void SwitchScenes()
    {
        activePlayer = GameObject.FindGameObjectWithTag("Player");

        //Scene scene;
        sceneToLoad = SceneManager.GetActiveScene().buildIndex;
        switch (sceneToLoad)
        {
            case 0:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    LoadGameScene();
                }
                break;
            case 1:
                LoadLostScene();
                break;
            case 2:
                LoadTitleScene();
                break;
            case 3:
                LoadTitleScene();
                break;
        }
    }

    private void LoadTitleScene()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            SceneManager.LoadScene(0);
        }
    }

    private void LoadGameScene()
    {
        SceneManager.LoadScene(1);
    }

    private void LoadLostScene()
    {
        if (activePlayer == null)
        {
            SceneManager.LoadScene(2);
        }
    }

}
