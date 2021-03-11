using Flux.Data;
using Flux.Event;
using System.Collections;
using System.Collections.Generic;
using Flux;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    private bool isOpen = false;

    private InputAction escapeAction;

    private void Start()
    {
        var inputs = Repository.Get<InputActionAsset>(References.Inputs);
        escapeAction = inputs["Core/Escape"];
        escapeAction.performed += OnEscapePressed;
    }

    void OnEscapePressed(InputAction.CallbackContext context)
    {
        if (isOpen) CloseMenu();
        else OpenMenu(); 
    }

    public void OpenMenu()
    {
        isOpen = true;
        menu.SetActive(true);
    }

    public void CloseMenu()
    {
        isOpen = false;
        menu.SetActive(false);
    }

    public void NewGame()
    {
        Buffer.hasStopped = true;
        Time.timeScale = 0;
        
        Routines.Clear();
        Events.Clear();
        Repository.Clear();

        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void Leave()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
