using System;
using Flux.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InTransition : MonoBehaviour
{
    public bool IsOperational => validity == dependencies.Length;
    
    [SerializeField] private string sceneName;
    [SerializeField] private Button button;
    [SerializeField] private DataField[] dependencies;

    private PlayerData[] data;
    private int validity;
    
    private bool isTransitioning;
    private AsyncOperation loadOperation;

    void Awake()
    {
        data = new PlayerData[dependencies.Length];
        Repository.Register(References.Data, data);
        
        foreach (var dependency in dependencies) dependency.OnChange += OnChange;
    }

    void OnChange(DataField field, bool hasFile)
    {
        if (hasFile)
        {
            var index = Array.IndexOf(dependencies, field);
            data[index] = field.Data;
            
            validity++;
            if (IsOperational) button.interactable = true;
        }
        else
        {
            validity--;
            button.interactable = false;
        }
    }

    public void Execute()
    {
        if (!IsOperational || isTransitioning) return;

        button.interactable = false;
        isTransitioning = true;
        
        loadOperation = SceneManager.LoadSceneAsync(sceneName);
    }
}