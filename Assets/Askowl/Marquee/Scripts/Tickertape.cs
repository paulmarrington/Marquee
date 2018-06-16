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

    public int[] Counts { get { return allQuotes.ConvertAll<int>(q => q.Count).ToArray(); } }

    private void Awake() {
      allQuotes = new List<Quotes>();

      foreach (Quotes quote in quotes) Add(quote);
    }

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
      if (running) { // restart
        Stop();
        Show();
      }
    }

    public void Show() {
      if (!running) StartCoroutine(TickertapeController());
    }

    public void Stop() { running = false; }

    public void Add(Quotes moreQuotes) {
      if (!loadedQuotes.Contains(moreQuotes.name)) {
        loadedQuotes.Add(moreQuotes.name);
        allQuotes.Add(moreQuotes);
      }
    }

    HashSet<object> loadedQuotes = new HashSet<object>();

    public void Add(TextAsset moreQuotes) {
      if (!loadedQuotes.Contains(moreQuotes)) {
        loadedQuotes.Add(moreQuotes);
        allQuotes.Add(Quotes.New.Add(moreQuotes));
      }
    }

    public Coroutine Pick() {
      Quotes quoter = allQuotes[Random.Range(0, allQuotes.Count)];
      return Show(quoter.Pick());
    }
  }
}