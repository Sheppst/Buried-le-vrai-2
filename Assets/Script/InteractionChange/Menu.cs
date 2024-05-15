using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject OptPanel;
    void Start()
    {
        OptPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    public void Play()
    {
        SceneManager.LoadScene(1);
    }
    public void Quit() 
    {
        Application.Quit();
    }
    public void Options()
    {
        OptPanel.SetActive(true);
    }
    public void OptionOff()
    {
        OptPanel.SetActive(false);
    }
}
