namespace Askowl {
  using System.Collections;
  using UnityEngine;
  using UnityEngine.UI;

  public class Marquee : MonoBehaviour {
    [SerializeField] private int charactersPerSecond = 20;
    [SerializeField] private int repeats             = 0;

    private Scroller scroller;
    private int      repeat;
    private Text     content;

    // Use this for initialization
    public void Start() {
      RectTransform viewport = GetComponent<RectTransform>();
      content = GetComponentInChildren<Text>();

      scroller = new Scroller(content: content.rectTransform, viewport: viewport) {
        StepSize = {x = -1, y = 0}
      };
    }

    public void OnDisable() {
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

    public Coroutine Show(string text) {
      try {
        return StartCoroutine(Displaying(text));
      } catch {
        Debug.LogError(GetType().Name + " prefab must be in the scene");
        return null;
      }
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public IEnumerator Hide() {
      if (repeat == 0) yield break;

      for (repeat = 1; repeat != 0;) {
        yield return new WaitForSecondsRealtime(0.5f);
      }
    }
  }
}