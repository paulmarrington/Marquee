using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Tickertape : Marquee {

  public bool autoStart = true;
  public int timeBetweenFeeds = 5;
  public TextAsset[] tickertapeAssets;

  private class Feeds : Dictionary<string, Pick<string>> {
  }

  private static Feeds feeds = new Feeds ();

  private Selector<Pick<string>> selector = new Selector<Pick<string>> ();

  private bool running = false;

  IEnumerator TickertapeController() {
    running = true;
    while (running) {
      yield return Pick();
      yield return After.Realtime.seconds(timeBetweenFeeds);
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

  public void Stop() {
    running = false;
  }

  public void Add(string name, Pick<string> picker) {
    if (!feeds.ContainsKey(name)) {
      feeds.Add(name, picker);
      selector.Choices = feeds.Values.ToArray();
      selector.Exhaustive();
    }
  }

  public void Add(params TextAsset[] textAssets) {
    foreach (TextAsset asset in tickertapeAssets) {
      Add(asset.name, new Quotes (asset.text.Split('\n')));
    }
  }

  public void Add(params string[] textAssetNames) {
    foreach (string name in textAssetNames) {
      TextAsset asset = Resources.Load<TextAsset>(name);
      Add(name, new Quotes (asset.text.Split('\n')));
    }
  }

  public Coroutine Pick() {
    return Show(selector.Pick().Pick());
  }
}
