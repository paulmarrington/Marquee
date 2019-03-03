// Copyright 2019 (C) paul@marrington.net http://www.askowl.net/unity-packages

using UnityEditor;

namespace Askowl {
  /// <a href=""></a> //#TBD#//
  public class CreateNewMarquee {
    [MenuItem("Assets/Create/Marquee")] private void CreateMarquee() => AssetBuilder.CreateAssets("Marquee");
  }
}