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
        /// <param name="onEnterLimb">
        /// Called when this liquid enters a limb. Note that this may be called quite often for the same container as liquid quickly moves in and out of it.
        /// The action receives the affected <see cref="LimbBehaviour"/> as its parameter.
        /// </param>
        /// <param name="onUpdate">
        /// Called every second by every container for every liquid it contains.
        /// The action receives the affected <see cref="BloodContainer container"/> as its parameter.
        /// </param>
        ///<param name="onEnterContainer">
        /// Called when this liquid enters a container. Limbs are also containers. Note that this may be called quite often for the same container as liquid quickly moves in and out of it.
        /// The action receives the affected <see cref="BloodContainer container"/> as its parameter.
        /// </param>
        ///<param name="onExitContainer">
        ///Called when this liquid exits a container. Note that this may be called quite often for the same container as liquid quickly moves in and out of it.
        /// The action receives the affected <see cref="BloodContainer container"/> as its parameter.
        /// </param>
        /// <param name="baseSyringe">The name of an existing syringe to use as a base template. Defaults to "Knockout Syringe".</param>
        /// <param name="category">The category this syringe appears under in the UI. Defaults to "Chemistry".</param>
        public Syringe(
            string name,
            string description,
            string thumbnail,
            string serumID,
            UnityEngine.Color serumColor,
            Action<LimbBehaviour> onEnterLimb = null,
            Action<BloodContainer> onUpdate = null,
            Action<BloodContainer> onEnterContainer = null,
            Action<BloodContainer> onExitContainer = null,
            string baseSyringe = "Knockout Syringe",
            string category = "Chemistry"
        )
        {
            ModAPI.RegisterLiquid(
                serumID,
                new SyringeItem.Serum(
                    serumID,
                    serumColor,
                    onEnterLimb,
                    onUpdate,
                    OnEnterContainer,
                    OnExitContainer
                )
            );
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
                public Action<LimbBehaviour> _onEnterLimb;
                public Action<BloodContainer> _onUpdate;
                public Action<BloodContainer> _onEnterContainer;
                public Action<BloodContainer> _onExitContainer;

                public Serum(
                    string serumID,
                    UnityEngine.Color serumColor,
                    Action<LimbBehaviour> onEnterLimb,
                    Action<BloodContainer> onUpdate,
                    Action<BloodContainer> OnEnterContainer,
                    Action<BloodContainer> OnExitContainer
                )
                {
                    ID = serumID;
                    Color = serumColor;
                    _onEnterLimb = onEnterLimb;
                    _onUpdate = onUpdate;
                    _onEnterContainer = OnEnterContainer;
                    _onExitContainer = OnExitContainer;
                }

                public override void OnEnterLimb(LimbBehaviour limb)
                {
                    _onEnterLimb?.Invoke(limb);
                }

                public override void OnUpdate(BloodContainer container)
                {
                    _onUpdate?.Invoke(container);
                }

                public override void OnEnterContainer(BloodContainer container)
                {
                    _onEnterContainer?.Invoke(container);
                }

                public override void OnExitContainer(BloodContainer container)
                {
                    _onExitContainer?.Invoke(container);
                }
            }
        }
    }
}
