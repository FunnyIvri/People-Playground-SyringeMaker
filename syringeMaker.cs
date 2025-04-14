using System;
using UnityEngine;

namespace SyringeMaker
{
    public class Syringe
    {
        public Syringe(
            string name,
            string description,
            string thumbnail,
            string serumID,
            UnityEngine.Color serumColor,
            Action<LimbBehaviour> effects,
            string baseSyringe = "Knockout Syringe",
            string category = "Chemistry"
        )
        {
            ModAPI.RegisterLiquid(serumID, new SyringeItem.Serum(serumID, serumColor, effects));
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
                public Action<LimbBehaviour> effects;

                public Serum(
                    string serumID,
                    UnityEngine.Color serumColor,
                    Action<LimbBehaviour> effects
                )
                {
                    ID = serumID;
                    Color = serumColor;
                    this.effects = effects;
                }

                public override void OnEnterLimb(LimbBehaviour limb)
                {
                    effects(limb);
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
