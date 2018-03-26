using Askowl;
using NUnit.Framework;
using UnityEngine;

public sealed class QuotesTest {
  private string process(string rtf) {
    Quotes quotes = new Quotes(quoteResourceName: "hints");
    return quotes.RTF(quote: rtf);
  }

  [Test]
  public void QuotesTestWithAttribution() {
    const string input  = "body (attribution text)";
    string       output = process(rtf: input);

    if (!output.Contains(">attribution text<")) {
      Debug.Log(output);
      Assert.Fail();
    }
  }

  [Test]
  public void QuotesTestWithoutAttribution() {
    const string input  = "body -- attribution";
    string       output = process(rtf: input);
    Assert.AreEqual(expected: input, actual: output);
  }
}