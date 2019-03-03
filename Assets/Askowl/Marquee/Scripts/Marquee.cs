// Copyright 2018,19 (C) paul@marrington.net http://www.askowl.net/unity-packages
using CustomAsset.Mutable;
using UnityEngine;
using UnityEngine.UI;

namespace Askowl {
  /// <a href="http://bit.ly/2CSCmQR">Visual object to display a message scrolling across the screen</a><inheritdoc/>
  public class Marquee : MonoBehaviour {
    [SerializeField] private Integer charactersPerSecond = default;
    [SerializeField] private Integer repeats             = default;
    [SerializeField] private String  showing             = default;
    [SerializeField] private Trigger showingComplete     = default;

    private Text     content;
    private Fiber    display;
    private int      repeat;
    private Scroller scroller;
    private string   textToDisplay;

    /// hook into emitters for displaying text and recording completion then prepare the visual
    protected void Awake() {
      void show(Emitter emitter) {
        var text = showing.Value;
        if (string.IsNullOrEmpty(text)) return;
        textToDisplay = text;
        repeat        = 0;
        display.Go();
      }

      float pixelsPerSecond = 0;

      void prepare(Fiber fiber) {
        if (string.IsNullOrEmpty(textToDisplay)) {
          fiber.Break();
          return;
        }
        repeat                    = repeats + 1;
        content.text              = textToDisplay;
        content.resizeTextMaxSize = content.cachedTextGenerator.fontSizeUsedForBestFit;
        var rect = content.rectTransform;
        rect.SetSizeWithCurrentAnchors(axis: RectTransform.Axis.Horizontal, size: content.preferredWidth);
        scroller.Reset();
        Vector3[] corners = new Vector3[4];
        rect.GetWorldCorners(corners);
        float pixelsWide         = corners[2].x - corners[0].x;
        float pixelsPerCharacter = (pixelsWide / textToDisplay.Length);
        pixelsPerSecond = charactersPerSecond * pixelsPerCharacter;
      }

      void step(Fiber fiber) {
        if (!scroller.Step(pixelsPerSecond * Time.fixedUnscaledDeltaTime) || (repeat == 0)) fiber.Break();
      }

      display = Fiber.Instance.Do(prepare).Begin.Begin.Do(step).Again.Until(_ => --repeat <= 0);
      display.OnComplete.Listen(_ => showingComplete.Fire());
      showing.Emitter.Listen(show);
      if (showing.Emitter.Firings > 0) show(showing.Emitter);
    }

    private void OnDestroy() => display.Dispose();

    /// Use this for initialization - specifically to prepare the scroller
    protected internal void Start() {
      RectTransform viewport = GetComponent<RectTransform>();
      content = GetComponentInChildren<Text>();

      scroller = new Scroller(content: content.rectTransform, viewport: viewport) {
        StepSize = {x = -1, y = 0}
      };
    }

    /// Stop scrolling
    protected internal void OnDisable() {
      repeat = 0;
      scroller.Reset();
      content.text = null;
    }
  }
}