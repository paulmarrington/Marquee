#if Marquee && UNITY_EDITOR
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Askowl {
  public class TickerTapeTests : PlayModeTests {
    private IEnumerator Setup() {
      yield return LoadScene("TickerTape Example");

      Tickertape tickertape = Components.Find<Tickertape>("Tickertape");
      tickertape.CharactersPerSecond = 100;
    }

    private IEnumerator Press(string buttonName) {
      yield return Setup();

      yield return PushButton(buttonName);
    }

    private IEnumerator TextDisplaying(float afterSeconds = 0.3f) {
      yield return IsDisplaying("Tickertape/Content", afterSeconds);
    }

    private IEnumerator TextNotDisplaying(float afterSeconds = 5) {
      yield return IsNotDisplaying("Tickertape/Content", afterSeconds);
    }

    private IEnumerator PressAndDisplay(string buttonName, float afterSeconds = 0.3f) {
      yield return Press(buttonName);
      yield return TextDisplaying(afterSeconds);
    }

    private IEnumerator PressAndNotDisplay(string buttonName, float afterSeconds = 2) {
      yield return Press(buttonName);
      yield return TextNotDisplaying(afterSeconds);
    }

    [UnityTest]
    public IEnumerator Show() {
      yield return PressAndDisplay("Enable");
      yield return PressAndDisplay("Show");
    }

    [UnityTest]
    public IEnumerator Stop() {
      yield return PressAndDisplay("Enable");
      yield return PressAndNotDisplay("Stop");
    }

    [UnityTest]
    public IEnumerator Enable() {
      yield return Press("Show");
      yield return PressAndDisplay("Enable");
    }

    [UnityTest]
    public IEnumerator Disable() {
      yield return Press("Show");
      yield return PressAndNotDisplay("Disable", 0.1f);
    }

    [UnityTest]
    public IEnumerator AddTextQuotes() { yield return Press("Add Text Quotes"); }

    [UnityTest]
    public IEnumerator AddQuotesAssets() { yield return Press("Add Quotes Asset"); }

    [UnityTest]
    public IEnumerator ShowImportantMessage() {
      yield return Press("Show Important Message");
      yield return new WaitForSeconds(0.1f);

      Text   component = Components.Find<Text>("Tickertape/Content");
      string text      = (component != null) ? component.text : "";
      Assert.AreEqual("A special message injected into the stream", text);
    }

    [UnityTest]
    public IEnumerator RtfFormatting() { yield return Press("RTF Formatting"); }
  }
}
#endif