// Copyright 2018,19 (C) paul@marrington.net http://www.askowl.net/unity-packages
#if AskowlTests
using System.Collections;
using CustomAsset;
using CustomAsset.Mutable;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

// ReSharper disable MissingXmlDoc

namespace Askowl {
  public class MarqueeTests : PlayModeTests {
    private readonly Integer charactersPerSecond = Manager.Load<Integer>("Askowl/Marquee/Characters per second.asset");
    private          int     cps;
    private          bool    sceneLoaded;

    private static string sceneName = "Marquee Example";
    #if UNITY_EDITOR
    [InitializeOnLoadMethod] private static void AddSceneToBuildSettings() => AddSceneToBuildSettings(sceneName);
    #endif

    private IEnumerator Setup() {
      cps                       = charactersPerSecond;
      charactersPerSecond.Value = 100;
      if (!sceneLoaded) yield return LoadScene(sceneName);
      sceneLoaded = true;
      yield return new WaitForSeconds(0.2f);
      yield return PushButton("Show Immediate");
      yield return new WaitForSeconds(0.2f);
    }

    private void TearDown() => charactersPerSecond.Value = cps;

    private IEnumerator TextDisplaying(bool visible = true) {
      yield return IsDisplayingInUi("Canvas/Marquee/Text", visible);
    }

    private IEnumerator PressAndNotDisplay(string buttonName) {
      yield return PushButton(buttonName);
      yield return new WaitForSeconds(0.3f);
      yield return TextDisplaying(false);
    }

    [UnityTest] public IEnumerator Show() {
      yield return Setup();
      yield return TextDisplaying();
      TearDown();
    }

    [UnityTest] public IEnumerator Stop() {
      yield return Setup();
      yield return PressAndNotDisplay("Stop");
      TearDown();
    }

    [UnityTest] public IEnumerator AddTextQuotes() {
      yield return Setup();
      yield return PushButton("Add Text Quotes");
      TearDown();
    }

    [UnityTest] public IEnumerator AddQuotesAssets() {
      yield return Setup();
      yield return PushButton("Add Quotes Asset");
      TearDown();
    }

    [UnityTest] public IEnumerator ShowImmediateMessage() {
      yield return Setup();
      yield return PushButton("Show Immediate");
      yield return new WaitForSeconds(0.1f);

      Text   component = Components.Find<Text>("Marquee/Text");
      string text      = (component != null) ? component.text : "";
      Assert.AreEqual("Drop everything for a special message", text);
      TearDown();
    }

    [UnityTest] public IEnumerator RtfFormatting() {
      yield return Setup();
      yield return PushButton("RTF Formatting");
      TearDown();
    }
  }
}
#endif