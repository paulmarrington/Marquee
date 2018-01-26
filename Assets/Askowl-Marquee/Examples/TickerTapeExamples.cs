using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickerTapeExamples : MonoBehaviour {
  public Tickertape tickertape;

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
