// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using System.Collections.Generic;
using CustomAsset.Constant;
using UnityEngine;

namespace Askowl {
  /// <a href=""></a> //#TBD#//
  public sealed class Tickertape : Marquee {
    [SerializeField] private bool     autoStart           = true;
    [SerializeField] private float    secondsBetweenFeeds = 5;
    [SerializeField] private Quotes[] quotes              = default;

    private List<Quotes> allQuotes;

    /// <a href=""></a> //#TBD#//
    public int[] Counts => allQuotes.ConvertAll(q => q.Count).ToArray();

    private void Awake() {
      allQuotes = new List<Quotes>();

      foreach (Quotes quote in quotes) Add(quote);
    }

    /// <a href=""></a> //#TBD#//
    protected internal override void Start() {
      base.Start();
      if (autoStart) Show();
    }

    private void OnEnable() => Show();

    /// <a href=""></a> //#TBD#//
    public void Show() {
      Stop();
      void pick(Fiber fiber) {
        if (allQuotes.Count != 0) Show(allQuotes[Random.Range(0, allQuotes.Count)].Pick());
      }
      showFiber = Fiber.Start.Begin.Do(pick).WaitFor(secondsBetweenFeeds).Again;
    }
    private Fiber showFiber;

    /// <a href=""></a> //#TBD#//
    public void Stop() => showFiber.Exit();

    /// <a href=""></a> //#TBD#//
    // ReSharper disable once UnusedMethodReturnValue.Global
    public Tickertape Add(Quotes moreQuotes) {
      if ((moreQuotes.Count == 0) || loadedQuotes[moreQuotes.name].Found) return this;

      loadedQuotes.Add(moreQuotes.name);
      allQuotes.Add(moreQuotes);
      return this;
    }

    /// <a href=""></a> //#TBD#//
    // ReSharper disable once UnusedMethodReturnValue.Global
    public Tickertape Clear() {
      loadedQuotes.Clear();
      allQuotes.Clear();
      return this;
    }

    private readonly Map loadedQuotes = new Map();
  }
}