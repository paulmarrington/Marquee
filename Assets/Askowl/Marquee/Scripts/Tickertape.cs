// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using CustomAsset.Constant;

namespace Askowl {
  using System.Collections;
  using System.Collections.Generic;
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

    public Tickertape Add(Quotes moreQuotes) {
      if ((moreQuotes.Count == 0) || loadedQuotes.Contains(moreQuotes.name)) return this;

      loadedQuotes.Add(moreQuotes.name);
      allQuotes.Add(moreQuotes);
      return this;
    }

    public Tickertape Clear() {
      loadedQuotes.Clear();
      allQuotes.Clear();
      return this;
    }

    HashSet<object> loadedQuotes = new HashSet<object>();

    public Coroutine Pick() {
      if (allQuotes.Count == 0) return null;

      Quotes quoter = allQuotes[Random.Range(0, allQuotes.Count)];
      return Show(quoter.Pick());
    }
  }
}