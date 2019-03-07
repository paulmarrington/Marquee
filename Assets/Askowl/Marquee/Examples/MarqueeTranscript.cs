//- Good afternoon. Today I am going to introduce a new Unity3D package from Askowl. If you like it and find a use for it, please join my Patreon team from the link below.
//- Marquee is a simple package for showing a text message in a scrolling panel across the screen like this... [[run the sample marquee]]
//- Adding a marquee to your app or game is as simple as installing the package and running the wizard. [[replicate in a new project]]
//- Oh, and updating the data to something useful to you [[Open Tickertape and show the data fields]]
//- And you need not use pre-packaged messages. Just clear the entries from the Quotes custom asset and display messages interactively
using CustomAsset.Mutable;
using UnityEngine;

namespace Askowl.Transcripts {
  ///
  public class WhatsInAName : MonoBehaviour {
    //- This will be a reference to the same custom asset as used by Marquee
    [SerializeField] private String tickertapeMessage = default;
    //- health is a custom asset with an emitter that fires whenever the value changes
    [SerializeField] private Float health = default;

    // We can subscribe to changes in health
    private void Awake() => health.Emitter.Listen(HealthMessages);

    // and display a message if anything important occurs.
    private void HealthMessages(Emitter emitter) {
      if (health < 0.1) tickertapeMessage.Value = "You are nearly dead! Drink a potion or run away";
    }
  }
}