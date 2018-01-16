using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

/*
 * Quotes quotes = Quotes.Singleton();
 * ...
 * string quote = Quotes.Pick();
 */
public class Quotes : CustomAsset<Quotes> {
  private Randomiser<string> quotes;

  public TextAsset quoteAsset;

  public override void OnEnable() {
    base.OnEnable();
    if (quoteAsset == null) {
      quoteAsset = (Resources.Load("quotes") as TextAsset);
    }
    quotes = new Randomiser<string> (quoteAsset.text.Split('\n'));
  }

  public string Pick() {
    return quotes.Pick();
  }

  public string RTF(string quote) {
    return Regex.Replace(quote, @"^(.*?)\s*\((.*)\)$", m =>
      string.Format("<b>\"</b><i>{0}</i><b>\"</b>      <color=grey>{1}</color>",
        m.Groups [1].Value, m.Groups.Count > 1 ? m.Groups [2].Value : "")
    );
  }

  public string PickRTF() {
    return RTF(Pick());
  }
}
