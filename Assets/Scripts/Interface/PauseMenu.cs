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
    [SerializeField] private GameObject reprendre;
    
    private bool isOpen = false;
    private bool isLocked;

    private InputAction escapeAction;

    void Start()
    {
        var inputs = Repository.Get<InputActionAsset>(References.Inputs);
        escapeAction = inputs["Core/Escape"];

        escapeAction.performed += OnEscapePressed;

        Events.RelayByVoid(GameEvent.OnPlayerDeath, Interrupt);
    }
    void OnDestroy() => Events.BreakVoidRelay(GameEvent.OnPlayerDeath, Interrupt);
    
    void OnEscapePressed(InputAction.CallbackContext context)
    {
        if (isLocked) return;
        
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
        if (isLocked) return;
        
        isOpen = false;
        menu.SetActive(false);
    }

    void Interrupt()
    {
        isLocked = true;
        StartCoroutine(Routines.DoAfter(OpenMenu, 1.0f));
    }
    
    public void NewGame()
    {
        Buffer.hasStopped = true;
        Time.timeScale = 0;
        
        Routines.Clear();
        
        Events.Clear();
        Events.ResetFlagTranslation();
        
        Repository.Clear();

        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadSceneAsync(scene.name);
    }

    public void Leave()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
