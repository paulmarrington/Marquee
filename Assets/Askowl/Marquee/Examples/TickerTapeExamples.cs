using Askowl;
using UnityEngine;

public sealed class TickerTapeExamples : MonoBehaviour {
  [SerializeField] private Tickertape tickertape;

  public void ShowButton() {
    tickertape.Show();
  }

  public void StopButton() {
    tickertape.Stop();
  }

  public void OnDisableButton() {
    tickertape.gameObject.SetActive(false);
  }

  public void OnEnableButton() {
    tickertape.gameObject.SetActive(true);
  }

  public void AddTextAssetButton() {
    tickertape.Add("quotes");
  }

  public void ShowSpecialButton() {
    tickertape.Show("A special message injected into the stream");
  }
}
