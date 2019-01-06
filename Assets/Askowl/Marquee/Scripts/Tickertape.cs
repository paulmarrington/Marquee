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
  /// <a href="http://bit.ly/2R8doWe">Manager Custom Asset to serve messages to the marquee</a>
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

    /// <a href="http://bit.ly/2R8doWe">Combined total of all messages that can be served</a>
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

    /// <a href="http://bit.ly/2RuJR8B">Start showing messages from the currently loaded Quote custom assets</a>
    public void Show() => showFiber.Exit().Go();
    private Fiber showFiber;

    /// <a href="http://bit.ly/2RcaBew">Inject a message to be displayed after the current one has finished</a>
    public void ShowNext(string text) => texts.Push(text);

    /// <a href="http://bit.ly/2R9IJb5">Inject a message to show right now - removing any half-finished message first</a>
    public void ShowImmediate(string text) => showing.Value = text;

    /// <a href="http://bit.ly/2RvUjg9">Stop displaying messages after the current one is done</a>
    public void Stop() => showFiber.Exit();

    /// <a href="http://bit.ly/2REqmux">Add more messages contained in a Quote custom asset</a>
    // ReSharper disable once UnusedMethodReturnValue.Global
    public Tickertape Add(Quotes moreQuotes) {
      if ((moreQuotes.Count == 0) || loadedQuotes[moreQuotes.name].Found) return this;

      loadedQuotes.Add(moreQuotes.name);
      allQuotes.Add(moreQuotes);
      return this;
    }

    /// <a href="http://bit.ly/2GVrcP7">Remove all messages from the list to display</a>
    // ReSharper disable once UnusedMethodReturnValue.Global
    public Tickertape Clear() {
      loadedQuotes.Clear();
      allQuotes.Clear();
      return this;
    }
  }
}