using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputInGame : MonoBehaviour
{
    [SerializeField] private GameObject OptionPanel;

    private void Awake()
    {
        Time.timeScale = 1f;
        OptionPanel.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
       if (Input.GetKeyDown(KeyCode.Escape))
       {
           Pause();
       } 
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        OptionPanel.SetActive(true);
    }
    public void UnPause ()
    {
        Time.timeScale = 1f;
        OptionPanel.SetActive(false);
    }
}
