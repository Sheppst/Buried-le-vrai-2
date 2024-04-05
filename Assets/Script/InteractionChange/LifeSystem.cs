using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeSystem : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Slider slid;
    private float LifePlayer;
    private float maxLife;
    // Start is called before the first frame update
    void Start()
    {
        maxLife = player.GetComponent<PlayerMovement>().Life;
    }

    // Update is called once per frame
    void Update()
    {
        LifePlayer = player.GetComponent<PlayerMovement>().Life;
        slid.value = LifePlayer / maxLife;
    }
}
