// Copyright 2019 (C) paul@marrington.net http://www.askowl.net/unity-packages

using CustomAsset.Constant;
using CustomAsset.Mutable;
using UnityEditor;
using UnityEngine;
using Integer = CustomAsset.Mutable.Integer;
using String = CustomAsset.Mutable.String;

namespace Askowl {
  /// Wizard to create a new Marquee in the current scene
  public class CreateNewMarquee : AssetWizard {
    [MenuItem("Assets/Create/Marquee")] private static void CreateMarquee() =>
      new CreateNewMarquee().CreateAssets("Marquee");

    protected override void OnScriptReload() {
      CreateAssetDictionary(
        ("Tickertape Manager", typeof(Tickertape))
      , ("Characters per second", typeof(Integer))
      , ("Contents", typeof(Quotes))
      , ("Display complete", typeof(Trigger))
      , ("Now showing", typeof(String))
      , ("Repeats per message", typeof(Integer))
      , ("Marquee Canvas", typeof(Marquee))
      , ("quotes", typeof(TextAsset)));

      SetField("Tickertape Manager", "quotes",          Asset("Contents"));
      SetField("Tickertape Manager", "showing",         Asset("Now showing"));
      SetField("Tickertape Manager", "showingComplete", Asset("Display complete"));

      SetField("Marquee Canvas", "charactersPerSecond", Asset("Characters per second"));
      SetField("Marquee Canvas", "repeats",             Asset("Repeats per message"));
      SetField("Marquee Canvas", "showing",             Asset("Now showing"));
      SetField("Marquee Canvas", "showingComplete",     Asset("Display complete"));

      Field("Characters per second").intValue = 16;

      SetActiveObject("Contents");
    }
  }
}