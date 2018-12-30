using System;
using UnityEngine;
using UnityEngine.UI;

namespace Askowl {
  public class Marquee : MonoBehaviour {
    [SerializeField] private int charactersPerSecond = 20;
    [SerializeField] private int repeats             = 0;

    public int CharactersPerSecond { get { return charactersPerSecond; } set { charactersPerSecond = value; } }

    private Scroller scroller;
    private int      repeat;
    private Text     content;

    /// Use this for initialization
    protected internal virtual void Start() {
      RectTransform viewport = GetComponent<RectTransform>();
      content = GetComponentInChildren<Text>();

      scroller = new Scroller(content: content.rectTransform, viewport: viewport) {
        StepSize = {x = -1, y = 0}
      };
    }

    /// Stop scrolling
    protected internal virtual void OnDisable() {
      scroller.Reset();
      repeat = 0;
    }

    private IEnumerator Displaying(string text) {
      yield return Hide();

      if (string.IsNullOrEmpty(text)) yield break;

      repeat       = repeats + 1;
      content.text = text;
      yield return null;

      content.resizeTextMaxSize = content.cachedTextGenerator.fontSizeUsedForBestFit;

      content.rectTransform.SetSizeWithCurrentAnchors(
        axis: RectTransform.Axis.Horizontal,
        size: content.preferredWidth);

      scroller.Reset();

      Vector3[] corners = new Vector3[4];
      content.rectTransform.GetWorldCorners(corners);
      float pixelsWide         = corners[2].x - corners[0].x;
      float pixelsPerCharacter = (pixelsWide / text.Length);
      float pixelsPerSecond    = charactersPerSecond * pixelsPerCharacter;

      do {
        yield return null;
      } while (scroller.Step(pixelsPerSecond * Time.fixedUnscaledDeltaTime) || (--repeat > 0));
    }

    /// <a href=""></a> //#TBD#//
    public void Show(string text) {
      if (string.IsNullOrEmpty(text)) return;
      if (firstShow) {
        firstShow  = false;
        repeatOnce = (fiber) => repeat = 1;
      }

      Fiber.Start
           .Begin.BreakIf(repeat == 0).Do(repeatOnce).WaitFor(displayComplete).End
           .Finish();

      try {
        return StartCoroutine(Displaying(text));
      } catch {
        Debug.LogError(GetType().Name + " prefab must be in the scene");
        return null;
      }
    }
    private          Fiber.Action repeatOnce;
    private          Boolean      firstShow       = true;
    private readonly Emitter      displayComplete = Emitter.Instance;

    // ReSharper disable once MemberCanBePrivate.Global
    public IEnumerator Hide() {
      if (repeat == 0) yield break;

      for (repeat = 1; repeat != 0;) {
        yield return new WaitForSecondsRealtime(0.5f);
      }
    }
  }
}