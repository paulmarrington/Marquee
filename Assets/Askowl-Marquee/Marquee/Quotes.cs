using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class Quotes : Pick<string> {
  
  public TextAsset quoteAsset;

  private Selector<string> selector = new Selector<string> ();

  public Quotes(string quoteResourceName = "quotes") {
    if (quoteAsset == null) {
      quoteAsset = (Resources.Load(quoteResourceName) as TextAsset);
    }
    init(quoteAsset.text.Split('\n'));
  }

  public Quotes(string[] listOfQuotes) {
    init(listOfQuotes);
  }

  void init(string[] listOfQuotes) {
    selector.Choices = listOfQuotes;
    if (listOfQuotes.Length < 100) {
      selector.Exhaustive();
    }
  }

  public string Pick() {
    return RTF(selector.Pick());
  }

  public string RTF(string quote) {
    return Regex.Replace(quote, @"^(.*?)\s*\((.*)\)$", m =>
      string.Format("<b>\"</b><i>{0}</i><b>\"</b>      <color=grey>{1}</color>",
        m.Groups [1].Value, m.Groups.Count > 1 ? m.Groups [2].Value : "")
    );
  }
}
