using UnityEngine;
using UnityEngine.UI;

namespace Askowl {
  /// <a href=""></a> //#TBD#//
  public class Marquee : MonoBehaviour {
    [SerializeField] private int charactersPerSecond = 20;
    [SerializeField] private int repeats             = 0;

    /// <a href=""></a> //#TBD#//
    public int CharactersPerSecond { get => charactersPerSecond; set => charactersPerSecond = value; }

    private Text     content;
    private Fiber    display;
    private int      repeat;
    private Scroller scroller;
    private string   textToDisplay;

    /// <a href=""></a> //#TBD#//
    protected virtual void Awake() {
      float pixelsPerSecond = 0;

      void prepare(Fiber fiber) {
        if (string.IsNullOrEmpty(textToDisplay)) {
          fiber.Break();
          return;
        }
        repeat                    = repeats + 1;
        content.text              = textToDisplay;
        content.resizeTextMaxSize = content.cachedTextGenerator.fontSizeUsedForBestFit;
        content.rectTransform.SetSizeWithCurrentAnchors(
          axis: RectTransform.Axis.Horizontal, size: content.preferredWidth);
        scroller.Reset();
        Vector3[] corners = new Vector3[4];
        content.rectTransform.GetWorldCorners(corners);
        float pixelsWide         = corners[2].x - corners[0].x;
        float pixelsPerCharacter = (pixelsWide / textToDisplay.Length);
        pixelsPerSecond = charactersPerSecond * pixelsPerCharacter;
      }

      void step(Fiber fiber) {
        if (!scroller.Step(pixelsPerSecond * Time.fixedUnscaledDeltaTime) || (repeat == 0)) fiber.Break();
      }

      display = Fiber.Instance.Do(prepare).Begin.Begin.Do(step).Again.Until(_ => --repeat <= 0);
    }

    private void OnDestroy() => display.Dispose();

    /// Use this for initialization - specifically to prepare the scroller
    protected internal virtual void Start() {
      RectTransform viewport = GetComponent<RectTransform>();
      content = GetComponentInChildren<Text>();

      scroller = new Scroller(content: content.rectTransform, viewport: viewport) {
        StepSize = {x = -1, y = 0}
      };
    }

    /// Stop scrolling
    protected internal virtual void OnDisable() {
      repeat = 0;
      scroller.Reset();
      content.text = null;
    }

    /// <a href=""></a> //#TBD#//
    public Fiber Show(string text) {
      if (string.IsNullOrEmpty(text)) return display;
      textToDisplay = text;
      repeat        = 0;
      return display.Go();
    }

    /// <a href=""></a> //#TBD#//
    public bool Displaying => display.Running;
  }
}