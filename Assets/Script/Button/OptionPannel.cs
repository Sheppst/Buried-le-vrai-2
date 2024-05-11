using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionPannel : MonoBehaviour
{
    [SerializeField] private GameObject InfoBulleGM;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void GModOn()
    {

    }
    public void GModOff()
    {

    }
    public void GModInfoOn()
    {
        InfoBulleGM.SetActive(true);
    }
    public void GModInfoOff()
    {
        InfoBulleGM.SetActive(false);
    }
}
