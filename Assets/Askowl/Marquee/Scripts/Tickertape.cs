// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using CustomAsset;

namespace Askowl {
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using JetBrains.Annotations;
  using UnityEngine;

  public sealed class Tickertape : Marquee {
    [SerializeField] private bool     autoStart           = true;
    [SerializeField] private float    secondsBetweenFeeds = 5;
    [SerializeField] private Quotes[] quotes;

    private List<Quotes> allQuotes;
    private bool         running;

    private void Awake() { allQuotes = new List<Quotes>(quotes); }

    private IEnumerator TickertapeController() {
      running = true;

      while (running) {
        yield return Pick();
        yield return new WaitForSecondsRealtime(secondsBetweenFeeds);
      }
    }

    protected internal override void Start() {
      base.Start();
      if (autoStart) Show();
    }

    private void OnEnable() {
      if (running) Show(); // restart
    }

    public void Show() {
      if (!running) StartCoroutine(TickertapeController());
    }

    public void Stop() { running = false; }

    public void Add(Quotes moreQuotes) { allQuotes.Add(moreQuotes); }

    public void Add(TextAsset moreQuotes) { allQuotes.Add(Quotes.New.Add(moreQuotes)); }

    public Coroutine Pick() {
      Quotes quoter = allQuotes[Random.Range(0, allQuotes.Count)];
      return Show(quoter.Pick());
    }
  }
}