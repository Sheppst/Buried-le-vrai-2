using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBar : MonoBehaviour
{
    private PlayerLifeSystem playerLifeSystem;
    [SerializeField] private GameObject player;
    [SerializeField] private Image barImage;
    
    // Start is called before the first frame update
    void Start()
    {
        barImage = transform.Find("LifeBar").GetComponent<Image>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerLifeSystem = player.GetComponent<PlayerLifeSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        playerLifeSystem.Update();
        barImage.fillAmount = playerLifeSystem.LifeNormalized();
    }
}
