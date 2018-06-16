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

    private IEnumerator TextDisplaying(bool visible = true) {
      yield return IsDisplayingInUI("Canvas/Tickertape/Content", visible);
    }

    private IEnumerator PressAndDisplay(string buttonName, float afterSeconds = 0.3f) {
      yield return Press(buttonName);
      yield return TextDisplaying();
    }

    private IEnumerator PressAndNotDisplay(string buttonName, float afterSeconds = 2) {
      yield return Press(buttonName);
      yield return TextDisplaying(false);
    }

    [UnityTest]
    public IEnumerator Show() {
      yield return PressAndDisplay("Enable");
      yield return PressAndDisplay("Show");
    }

    [UnityTest]
    public IEnumerator Stop() { yield return PressAndNotDisplay("Stop", 5); }

    [UnityTest]
    public IEnumerator Enable() {
      yield return Press("Show");
      yield return PressAndDisplay("Enable");
    }

    [UnityTest]
    public IEnumerator Disable() {
      yield return Press("Show");
      yield return PressAndNotDisplay("Disable", 0.4f);
      yield return PressAndDisplay("Enable");
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