using UnityEngine;
//Ajouter using static + le nom du script
using static MyInterface;

//Monobehaviour, Nom de l'interface
public class Candle : MonoBehaviour, Interactable
{


    //Recopier le nom de ma fonction
    public void Interact()
    {
        if (transform.GetChild(0).gameObject.activeSelf == true)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
        else if (transform.GetChild(0).gameObject.activeSelf == false)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }

}
