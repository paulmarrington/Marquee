// Copyright 2018,19 (C) paul@marrington.net http://www.askowl.net/unity-packages

using System;
using System.Collections.Generic;
using CustomAsset;
using CustomAsset.Constant;
using CustomAsset.Mutable;
using UnityEngine;
using Random = UnityEngine.Random;
using String = CustomAsset.Mutable.String;

namespace Askowl {
  /// <a href=""></a> //#TBD#//
  [CreateAssetMenu(menuName = "Managers/Tickertape"), Serializable]
  public sealed class Tickertape : Manager {
    [SerializeField] private bool    autoStart           = true;
    [SerializeField] private float   secondsBetweenFeeds = 5;
    [SerializeField] private Quotes  quotes              = default;
    [SerializeField] private String  showing             = default;
    [SerializeField] private Trigger showingComplete     = default;

    private readonly Fifo<string> texts = Fifo<string>.Instance;
    private          List<Quotes> allQuotes;
    private readonly Map          loadedQuotes = new Map();

    /// <a href=""></a> //#TBD#//
    public int[] Counts => allQuotes.ConvertAll(q => q.Count).ToArray();

    protected override void Initialise() {
      base.Initialise();
      allQuotes = new List<Quotes>();
      Add(quotes);

      string pick() => texts.Pop() ?? allQuotes[Random.Range(0, allQuotes.Count)].Pick();

      showFiber = Fiber.Instance.Begin.If(_ => allQuotes.Count > 0)
                       .Do(_ => showing.Value = pick()).WaitFor(showingComplete.Emitter)
                       .Then.WaitFor(secondsBetweenFeeds).Again;
      if (autoStart) Show();
    }

    /// <a href=""></a> //#TBD#//
    public void Show() => showFiber.Exit().Go();
    private Fiber showFiber;

    /// <a href=""></a> //#TBD#//
    public void ShowNext(string text) => texts.Push(text);

    /// <a href=""></a> //#TBD#//
    public void ShowImmediate(string text) => showing.Value = text;

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
  }
}