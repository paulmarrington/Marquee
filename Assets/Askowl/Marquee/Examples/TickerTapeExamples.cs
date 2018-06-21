using System;
using CustomAsset.Constant;

#if Marquee && UNITY_EDITOR
namespace Askowl {
  using UnityEngine;

  public sealed class TickerTapeExamples : MonoBehaviour {
    [SerializeField] private Tickertape tickertape;

    [SerializeField] private Quotes andMoreQuotes;

    public void ShowButton() { tickertape.Show(); }

    public void StopButton() { tickertape.Stop(); }

    public void OnDisableButton() { tickertape.gameObject.SetActive(false); }

    public void OnEnableButton() { tickertape.gameObject.SetActive(true); }

    private void CheckCounts(Action adder) {
      string before = Csv<int>.Instance(tickertape.Counts).ToString();
      Debug.LogFormat("Before: {0}", before);

      adder();
      string after = Csv<int>.Instance(tickertape.Counts).ToString();

      if (before == after) {
        Debug.LogErrorFormat("No change in quotes");
      } else {
        Debug.LogFormat("After: {0}", after);
      }
    }

    public void AddQuotesButton()   { CheckCounts(() => tickertape.Add(andMoreQuotes)); }

    public void ClearQuotesButton() { CheckCounts(() => tickertape.Clear()); }

    public void ShowSpecialButton() {
      tickertape.Show("A special message injected into the stream");
    }

    public void FormattingButton() {
      string input  = "body (attribution text)";
      string output = Quotes.Rtf(input);

      if (output.Contains(">attribution text<")) {
        Debug.Log(output);
      } else {
        Debug.LogError(output);
      }

      input  = "body -- attribution";
      output = Quotes.Rtf(input);

      if (input == output) {
        Debug.Log(output);
      } else {
        Debug.LogError(output);
      }
    }
  }
}
#endif