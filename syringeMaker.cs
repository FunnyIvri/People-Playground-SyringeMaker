using System;
using UnityEngine;

namespace SyringeMaker
{
    public class Syringe
    {
        /// <summary>
        /// Registers a custom syringe with defined appearance and behavior.
        /// </summary>
        /// <param name="name">The display name of the syringe.</param>
        /// <param name="description">A brief description of the syringe, shown in the UI.</param>
        /// <param name="thumbnail">The file path for the syringe's thumbnail image.</param>
        /// <param name="serumID">A unique string identifier for the serum.</param>
        /// <param name="serumColor">The color used to represent the serum inside the syringe.</param>
        /// <param name="instantEffect">
        /// An action that is triggered immediately when the syringe pierces a limb.
        /// The action receives the affected <see cref="LimbBehaviour"/> as its parameter.
        /// </param>
        /// <param name="baseSyringe">The name of an existing syringe to use as a base template. Defaults to "Knockout Syringe".</param>
        /// <param name="category">The category this syringe appears under in the UI. Defaults to "Chemistry".</param>
        public Syringe(
            string name,
            string description,
            string thumbnail,
            string serumID,
            UnityEngine.Color serumColor,
            Action<LimbBehaviour> effect,
            string baseSyringe = "Knockout Syringe",
            string category = "Chemistry"
        )
        {
            ModAPI.RegisterLiquid(serumID, new SyringeItem.Serum(serumID, serumColor, effect));
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable(baseSyringe), //item to derive from
                    NameOverride = name, //new item name with a suffix to assure it is globally unique
                    DescriptionOverride = description, //new item description
                    CategoryOverride = ModAPI.FindCategory(category), //new item category
                    ThumbnailOverride = ModAPI.LoadSprite(thumbnail), //new item thumbnail (relative path)
                    AfterSpawn = (Instance) => //all code in the AfterSpawn delegate will be executed when the item is spawned
                    {
                        UnityEngine.Object.Destroy(Instance.GetComponent<SyringeBehaviour>());
                        var syringe = Instance.GetOrAddComponent<SyringeItem>();
                        syringe.ID = serumID;
                    },
                }
            );
        }

        public class SyringeItem : SyringeBehaviour
        {
            // provide the liquid ID for this syringe
            public string ID;

            public override string GetLiquidID() => ID;

            public class Serum : Liquid
            {
                public string ID;
                public Action<LimbBehaviour> effect;

                public Serum(
                    string serumID,
                    UnityEngine.Color serumColor,
                    Action<LimbBehaviour> effect
                )
                {
                    ID = serumID;
                    Color = serumColor;
                    this.effect = effect;
                }

                public override void OnEnterLimb(LimbBehaviour limb)
                {
                    effect(limb);
                }

                public override void OnEnterContainer(BloodContainer container)
                {
                    //
                }

                public override void OnExitContainer(BloodContainer container)
                {
                    //
                }
            }
        }
    }
}
