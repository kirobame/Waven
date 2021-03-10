using System;
using System.Collections;
using System.Collections.Generic;
using Flux.Event;
using Flux.Feedbacks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Banner : MonoBehaviour
{
    [SerializeField] private Sequencer entry;

    [Space, SerializeField] private TMP_Text titleMesh;
    [SerializeField] private TMP_Text challengeMesh;
    [SerializeField] private TMP_Text textMesh;
    [SerializeField] private Image icon;

    void Awake()
    {
        Events.Open(InterfaceEvent.OnChallengeUpdate);
        Events.Open(InterfaceEvent.OnChallengeCompleted);
        Events.Open(InterfaceEvent.OnChallengeFailed);
        
        Events.RelayByValue<Challenge>(InterfaceEvent.OnChallengeUpdate, Display);
    }
    void OnDestroy() => Events.BreakValueRelay<Challenge>(InterfaceEvent.OnChallengeUpdate, Display);

    public void Display(Challenge challenge)
    {
        titleMesh.text = $"Au tour du joueur 0{Player.Active.Index} !";
        challengeMesh.text = $"Challenge : {challenge.Title}";
        textMesh.text = challenge.GetDescription();
        icon.sprite = challenge.Icon;
        
        entry.Play(EventArgs.Empty);
    }
}
