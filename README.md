# People Playground - SyringeMaker

## 🧪 Overview  
**SyringeMaker** is a utility for quickly and easily creating custom syringes in your People Playground mod. Just drop in the script and register your syringes with a few lines of code—no need to mess with complex boilerplate.

---

## 📦 Installation

To add SyringeMaker to your project:

1. Copy the `syringeMaker.cs` file into your mod project.
2. Add it to your `mod.json` file so it's compiled with the rest of your mod.

---

## 🚀 Usage Example

Here’s a simple example that adds a **True Knockout Syringe** to the game:

```csharp
using SyringeMaker; // Import the SyringeMaker namespace
using UnityEngine;

namespace Mod
{
    public class Mod
    {
        public static void Main()
        {
            // Function that sets a Person's consciousness to 0
            void knockout(LimbBehaviour limb) => limb.Person.Consciousness = 0f;

            // Create a new syringe with custom behavior
            new Syringe(
                name: "True Knockout Syringe",
                description: "Like a Knockout Syringe, but always works completely.",
                thumbnail: "test.png",
                serumID: "TRUE KNOCKOUT SERUM",
                serumColor: new Color(1f, 1f, 1f),
                effect: knockout
            );
        }
    }
}
```

---

## 🛠️ Syringe Parameters

| Parameter       | Type             | Description |
|----------------|------------------|-------------|
| `name`          | `string`         | Display name of the syringe. |
| `description`   | `string`         | Tooltip or UI description. |
| `thumbnail`     | `string`         | Path to the syringe’s icon (e.g., `"Assets/Icons/mySyringe.png"`). |
| `serumID`       | `string`         | Unique ID to identify the syringe. |
| `serumColor`    | `Color`          | Color of the serum inside the syringe. |
| `effect` | `Action<LimbBehaviour>` | Function to run once when the syringe pierces a limb. |
| `baseSyringe`   | `string` _(optional)_ | Name of a base syringe to inherit visuals/behavior from (defaults to `"Knockout Syringe"`). |
| `category`      | `string` _(optional)_ | Category shown in the UI (defaults to `"Chemistry"`). |

---

## 📁 Example Thumbnails

Make sure your `thumbnail` path points to a valid image file inside your mod directory. The file should be readable by Unity (e.g., PNG or JPG).

---

## 💬 Questions or Ideas?

### Feel free to open an issue or suggest improvements. SyringeMaker was designed to be lightweight, flexible, and easy to build on. Happy modding!
---
