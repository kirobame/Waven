using Flux.Data;
using Flux.Event;
using System.Collections;
using System.Collections.Generic;
using Flux;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject blockingPanel;
    [SerializeField] private GameObject menu;

    [Space, SerializeField] private GameObject victoryPanel;
    [SerializeField] private Animator banner;
    [SerializeField] private TMP_Text title;
    
    [Space, SerializeField] private TMP_FontAsset redFont;
    [SerializeField] private TMP_FontAsset blueFont;
    
    private bool isOpen = false;
    private bool isLocked;

    private InputAction escapeAction;

    void Start()
    {
        var inputs = Repository.Get<InputActionAsset>(References.Inputs);
        escapeAction = inputs["Core/Escape"];

        escapeAction.performed += OnEscapePressed;

        Events.RelayByValue<Player>(GameEvent.OnPlayerDeath, Interrupt);
    }
    void OnDestroy() => Events.BreakValueRelay<Player>(GameEvent.OnPlayerDeath, Interrupt);
    
    void OnEscapePressed(InputAction.CallbackContext context)
    {
        if (isLocked) return;
        
        if (isOpen) CloseMenu();
        else OpenMenu(); 
    }

    public void OpenMenu()
    {
        isOpen = true;
        
        blockingPanel.SetActive(true);
        menu.SetActive(true);
    }

    public void CloseMenu()
    {
        if (isLocked) return;
        
        isOpen = false;
        
        blockingPanel.SetActive(false);
        menu.SetActive(false);
    }

    void Interrupt(Player player)
    {
        blockingPanel.SetActive(true);
        
        int index;
        if (player.Index == 1)
        {
            index = 0;
            title.font = blueFont;
        }
        else
        {
            index = 1;
            title.font = redFont;
        }

        title.text = $"Le joueur 0{index + 1} est victorieux !";

        isLocked = true;
        
        var inputs = Repository.Get<InputActionAsset>(References.Inputs);
        inputs.Disable();
        
        StartCoroutine(Routines.DoAfter(() =>
        {
            Time.timeScale = 0;
            victoryPanel.SetActive(true);
            
            banner.SetTrigger("In");
            
        }, 1.0f));
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
