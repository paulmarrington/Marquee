using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Tickertape : Marquee {

  public bool autoStart = false;
  public int timeBetweenFeeds = 5;
  public TextAsset[] tickertapeAssets;

  private class Feeds : Dictionary<string, Pick<string>> {
  }

  private static Feeds feeds = new Feeds ();
  private static string[] feedNames;

  private Pick<string> Feed(string name) {
    if (!feeds.ContainsKey(name)) {
      Add(name, new Quotes (name));
    }
    return feeds [name];
  }

  IEnumerator TickertapeController() {
    while (autoStart) {
      yield return Pick();
      yield return After.Realtime.seconds(timeBetweenFeeds);
    }
  }

  public new void Start() {
    base.Start();
    StartCoroutine(TickertapeController());
  }

  public void Add(string name, Pick<string> picker) {
    feeds.Add(name, picker);
    feedNames = feeds.Keys.ToArray();
  }

  public Coroutine Pick(string name) {
    return Show(Feed(name).Pick());
  }

  private System.Random random = new System.Random ();

  public Coroutine Pick(params string[] names) {
    if (names.Length == 0) {
      if (Tickertape.feedNames == null) {
        foreach (TextAsset asset in tickertapeAssets) {
          Add(asset.name, new Quotes (asset.text.Split('\n')));
        }
      }
      names = Tickertape.feedNames;
    }
    return Pick(names [random.Next(0, names.Length)]);
  }
}
