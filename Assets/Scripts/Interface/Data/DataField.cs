using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DataField : MonoBehaviour
{
    #region Nested Types

    private enum State
    {
        HasNone,
        HasPath,
        HasFile
    }

    #endregion

    public event Action<DataField, bool> OnChange;

    public bool HasFile => hasFile;

    public PlayerData Data { get; private set; }
    
    [SerializeField] private TMP_InputField field;
    [SerializeField] private Validation validation;
    [SerializeField] private Button button;

    private bool hasFile;
    private State state;
    private string input;

    public void OnUserInput(string input)
    {
        input = input.Replace('"'.ToString(), string.Empty);
        field.SetTextWithoutNotify(input);
        this.input = input;
        
        var lastIndex = input.LastIndexOf('.');
        if (lastIndex == -1)
        {
            SetState(State.HasNone);
            return;
        }
        
        lastIndex++;
        var extension = input.Substring(lastIndex, input.Length - lastIndex);

        if (extension != "cbk")
        {
            SetState(State.HasNone);
            return;
        }

        if (File.Exists(input))
        {
            Data = new PlayerData(input, File.ReadAllLines(input));
            SetState(State.HasFile);
        }
        else SetState(State.HasPath);
    }

    private void SetState(State state)
    {
        if (this.state == state) return;
        this.state = state;
        
        switch (state)
        {
            case State.HasNone:
                if (hasFile) OnChange?.Invoke(this, false);
                hasFile = false;
                
                validation.Set(false);
                button.interactable = false;
                break;
            
            case State.HasPath:
                if (hasFile) OnChange?.Invoke(this, false);
                hasFile = false;
                
                validation.Set(false);
                button.interactable = true;
                break;
            
            case State.HasFile:
                if (!hasFile) OnChange?.Invoke(this, true);
                hasFile = true;
                
                validation.Set(true);
                button.interactable = false;
                break;
        }
    }

    public void CreateFile()
    {
        if (state != State.HasPath) return;

        var lines = new string[] { "Empty" };
        
        var stream = File.CreateText(input);
        foreach (var line in lines) stream.WriteLine(line);
        stream.Close();

        Data = new PlayerData(input, lines);

        SetState(State.HasFile);
    }
}