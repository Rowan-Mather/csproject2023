using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
    public GameObject mainMenuScreen;
    public GameObject helpScreen;
    // Start is called before the first frame update
    void Start()
    {
        showMenu();
    }

    public void activateHelp() {
        helpScreen.SetActive(true);
        mainMenuScreen.SetActive(false);

    }

    public void showMenu () {
        mainMenuScreen.SetActive(true);
        helpScreen.SetActive(false);
    }

    public void hideMenu () {
        mainMenuScreen.SetActive(false);
        helpScreen.SetActive(false);
    }
}
