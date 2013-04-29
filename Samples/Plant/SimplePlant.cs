using OrganismBase;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Plant
{
    // It's an herbivore
    [Carnivore(false)]
    // This value must be between 24 and 48, 24 means faster reproduction
    // while 48 would give more defense and attack power
    // Make it smaller for reproduction
    [MatureSize(25)]
    // AnimalSkin = AnimalSkinFamilyEnum.Beetle, you can be a Beetle
    // an Ant, a Scorpion, an Inchworm, or a Spider
    // MarkingColor = KnownColor.Red, you can choose to mark your
    // creature with a color.  This does not affect appearance in
    // the game.
    [PlantSkin(PlantSkinFamily.Plant)]
    [MarkingColor(KnownColor.Green)]
    // Point Based Attributes
    // You get 100 points to distribute among these attributes to define
    // what your organism can do.  Choose them based on the strategy your organism
    // will use.  This organism hides and has good eyesight to find plants.
    [MaximumEnergyPoints(100)]
    [SeedSpreadDistance(300)]
    public class SimplePlant : OrganismBase.Plant
    {
        public override void DeserializePlant(System.IO.MemoryStream m)
        {
        }

        public override void SerializePlant(System.IO.MemoryStream m)
        {
        }
    }
}
