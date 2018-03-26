namespace Askowl {
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using JetBrains.Annotations;
  using UnityEngine;

  public sealed class Tickertape : Marquee {
    [SerializeField] private bool        autoStart           = true;
    [SerializeField] private float       secondsBetweenFeeds = 5;
    [SerializeField] private TextAsset[] tickertapeAssets;

    private sealed class FeedsClass : Dictionary<string, IPick<string>> { }

    private static readonly FeedsClass Feeds = new FeedsClass();

    private readonly Selector<IPick<string>> selector = new Selector<IPick<string>>();

    private bool running;

    private IEnumerator TickertapeController() {
      running = true;

      while (running) {
        yield return Pick();
        yield return new WaitForSecondsRealtime(secondsBetweenFeeds);
      }
    }

    public new void Start() {
      base.Start();
      Add(tickertapeAssets);

      if (autoStart) {
        Show();
      }
    }

    public void OnEnable() {
      if (running) {
        StartCoroutine(TickertapeController());
      }
    }

    public void Show() {
      if (!running) {
        StartCoroutine(TickertapeController());
      }
    }

    public void Stop() { running = false; }

    public void Add([NotNull] string feedName, IPick<string> picker) {
      if (!Feeds.ContainsKey(feedName)) {
        Feeds.Add(feedName, picker);
        selector.Choices = Feeds.Values.ToArray();
        selector.Exhaustive();
      }
    }

    public void Add(params TextAsset[] textAssets) {
      foreach (TextAsset asset in tickertapeAssets) {
        Add(asset.name, new Quotes(asset.text.Split('\n')));
      }
    }

    public void Add([NotNull] params string[] textAssetNames) {
      foreach (string textAssetName in textAssetNames) {
        TextAsset asset = Resources.Load<TextAsset>(textAssetName);
        Add(textAssetName, new Quotes(asset.text.Split('\n')));
      }
    }

    public Coroutine Pick() { return Show(selector.Pick().Pick()); }
  }
}