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
    private Fifo<string> texts = Fifo<string>.Instance;

    /// <a href=""></a> //#TBD#//
    public int[] Counts => allQuotes.ConvertAll(q => q.Count).ToArray();

    /// <a href=""></a> //#TBD#// <inheritdoc />
    protected override void Awake() {
      base.Awake();
      allQuotes = new List<Quotes>();

      foreach (Quotes quote in quotes) Add(quote);

      string pick() => texts.Pop() ?? allQuotes[Random.Range(0, allQuotes.Count)].Pick();

      showFiber = Fiber.Instance.Begin.If(_ => allQuotes.Count > 0)
                       .WaitFor(_ => Show(pick())).Then.WaitFor(secondsBetweenFeeds).Again;
    }

    private void OnEnable() {
      if (autoStart) Show();
    }

    /// <a href=""></a> //#TBD#//
    public void Show() => showFiber.Exit().Go();
    private Fiber showFiber;

    /// <a href=""></a> //#TBD#//
    public void NextMessage(string text) => texts.Push(text);

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