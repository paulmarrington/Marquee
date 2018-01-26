using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Runtime.CompilerServices;

public class Marquee : MonoBehaviour {
  public int charactersPerSecond = 20;
  public int repeats = 0;

  private Scroller scroller;
  private int repeat;
  private Text content;

  // Use this for initialization
  public void Start() {
    RectTransform viewport = GetComponent<RectTransform>();
    content = GetComponentInChildren<Text>();
    scroller = new Scroller (viewport, content.rectTransform);
    scroller.step.x = -1;
    scroller.step.y = 0;
  }

  public void OnDisable() {
    scroller.Reset();
    repeat = 0;
  }

  private IEnumerator displaying(string text) {
    yield return Hide();
    if (text != null && text.Length > 0) {
      repeat = repeats + 1;
      content.text = text;
      yield return null;
      content.resizeTextMaxSize = content.cachedTextGenerator.fontSizeUsedForBestFit;
      content.rectTransform.SetSizeWithCurrentAnchors(
        RectTransform.Axis.Horizontal,
        content.preferredWidth);
      scroller.Reset();

      Vector3[] corners = new Vector3[4];
      content.rectTransform.GetWorldCorners(corners);
      float pixelsWide = corners [2].x - corners [0].x;
      float pixelsPerCharacter = (pixelsWide / text.Length);
      float pixelsPerSecond = charactersPerSecond * pixelsPerCharacter;

      do {
        yield return null;
      } while (scroller.Step(pixelsPerSecond * Time.fixedUnscaledDeltaTime) || (--repeat > 0));
    }
  }

  public Coroutine Show(string text) {
    try {
      return StartCoroutine(displaying(text));
    } catch {
      Debug.LogError(this.GetType().Name + " prefab must be in the scene");
      return null;
    }
  }

  public IEnumerator Hide() {
    if (repeat != 0) {
      repeat = 1;
      while (repeat != 0) {
        yield return After.Realtime.ms(500);
      }
    }
  }
}
