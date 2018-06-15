using Askowl;
using CustomAsset;
using UnityEngine;

public sealed class TickerTapeExamples : MonoBehaviour {
  [SerializeField] private Tickertape tickertape;
  [SerializeField] private TextAsset  moreQuotes;
  [SerializeField] private Quotes     andMoreQuotes;

  public void ShowButton() { tickertape.Show(); }

  public void StopButton() { tickertape.Stop(); }

  public void OnDisableButton() { tickertape.gameObject.SetActive(false); }

  public void OnEnableButton() { tickertape.gameObject.SetActive(true); }

  public void AddTextAssetButton() {
    tickertape.Add(moreQuotes);
  }

  public void AddQuotesButton() {
    tickertape.Add(andMoreQuotes);
  }

  public void ShowSpecialButton() { tickertape.Show("A special message injected into the stream"); }

  public void FormattingButton() {
    string input  = "body (attribution text)";
    string output = Quotes.RTF(input);

    if (output.Contains(">attribution text<")) {
      Debug.Log(output);
    } else {
      Debug.LogError(output);
    }

    input  = "body -- attribution";
    output = Quotes.RTF(input);

    if (input == output) {
      Debug.Log(output);
    } else {
      Debug.LogError(output);
    }
  }
}