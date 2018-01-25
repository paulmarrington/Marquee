using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class QuotesTest {

  private string process(string rtf) {
    Quotes quotes = new Quotes ("hints");
    return quotes.RTF(rtf);
  }

  [Test]
  public void QuotesTestWithAttribution() {
    string input = "body (attribution text)";
    string output = process(input);
    if (!output.Contains(">attribution text<")) {
      Debug.Log(output);
      Assert.Fail();
    }
  }

  [Test]
  public void QuotesTestWithoutAttribution() {
    string input = "body -- attribution";
    string output = process(input);
    Assert.AreEqual(input, output);
  }
}
