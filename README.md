# People-Playground-SyringeMaker
do you want to more easily make custom syringes?
well i spent like 3 hours so you can do that!
meet SyringeMaker!
all you have to do is add the SyringeMaker file to your project and add it to the mod.json
then you can just do
```c#
using SyringeMaker;
using UnityEngine;
namespace Mod
{
    public class Mod
    {
        public static void Main()
        {
            void knockout(LimbBehaviour limb) => limb.Person.Consciousness = 0f;
            new Syringe(
                name: "True Knockout Syringe",
                description: "like a Knockout Syringe but always works Completly",
                thumbnail: "test.png",
                serumID: "TRUE KNOCKOUT SERUM",
                serumColor: new UnityEngine.Color(1f, 1f, 1f),
                effects: knockout
            );
        }
    }
}
```
have fun!
