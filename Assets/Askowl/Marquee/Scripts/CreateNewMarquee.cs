// Copyright 2019 (C) paul@marrington.net http://www.askowl.net/unity-packages

using CustomAsset.Constant;
using CustomAsset.Mutable;
using UnityEditor;
using Integer = CustomAsset.Mutable.Integer;
using String = CustomAsset.Mutable.String;

namespace Askowl {
  /// <a href=""></a> //#TBD#//
  public class CreateNewMarquee : AssetBuilder {
    [MenuItem("Assets/Create/Marquee")] private static void CreateMarquee() =>
      new CreateNewMarquee().CreateAssets("Marquee");

    protected override void OnScriptReload() {
      CreateAssetDictionary(
        ("TickertapeManager", typeof(Tickertape))
      , ("Characters per second", typeof(Integer))
      , ("Contents", typeof(Quotes))
      , ("Display complete", typeof(Trigger))
      , ("Now showing", typeof(String))
      , ("Repeats per message", typeof(Integer)));

      SetField("TickertapeManager", "quotes",          Asset("Contents"));
      SetField("TickertapeManager", "showing",         Asset("Now showing"));
      SetField("TickertapeManager", "showingComplete", Asset("Display complete"));

      SetField("Marquee", "charactersPerSecond", Asset("Characters per second"));
      SetField("Marquee", "repeats",             Asset("Repeats per message"));
      SetField("Marquee", "showing",             Asset("Now showing"));
      SetField("Marquee", "showingComplete",     Asset("Display complete"));
    }
  }
}