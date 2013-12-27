using OrganismBase;
using System;
using System.Collections;
using System.Drawing;
using System.IO;

namespace Carnivore
{
    // It's an carnivore
    [Carnivore(true)]
    // This value must be between 24 and 48, 24 means faster reproduction
    // while 48 would give more defense and attack power
    // Make it smaller for reproduction
    [MatureSize(30)]
    // AnimalSkin = AnimalSkinFamilyEnum.Scorpion, you can be a Beetle
    // an Ant, a Scorpion, an Inchworm, or a Spider
    // MarkingColor = KnownColor.Red, you can choose to mark your
    // creature with a color.  This does not affect appearance in
    // the game.
    [AnimalSkin(AnimalSkinFamily.Scorpion)]
    [MarkingColor(KnownColor.Chocolate)]
    // Point Based Attributes
    // You get 100 points to distribute among these attributes to define
    // what your organism can do.  Choose them based on the strategy your organism
    // will use.  This organism hides and has good eyesight to find plants.
    [MaximumEnergyPoints(0)] // Don't need to increase this as it just sits next to plants
    [EatingSpeedPoints(0)] // Ditto
    [AttackDamagePoints(0)] // Doesn't ever attack
    [DefendDamagePoints(0)] // Doesn't even defend
    [MaximumSpeedPoints(20)] // Doesn't need to move quickly
    [CamouflagePoints(30)] // Try to remain hidden
    [EyesightPoints(50)] // Need this to find plants better
    public class SimpleCarnivore : Animal
    {
        private AnimalState _targetAnimal;

        protected override void Initialize()
        {
            Load += SampleCarnivore_Load;
            Idle += SampleCarnivore_Idle;
        }

        void SampleCarnivore_Load(object sender, LoadEventArgs e)
        {
            try
            {
                if (_targetAnimal == null)
                {
                    _targetAnimal = (AnimalState) LookFor(_targetAnimal);
                }
            }
            catch (Exception exc)
            {
                WriteTrace(exc.ToString());
            }
        }

        void SampleCarnivore_Idle(object sender, IdleEventArgs e)
        {
            try
            {
                if (CanReproduce)
                {
                    BeginReproduction(null);
                }
                if (CanEat && !IsEating)
                {
                    if (_targetAnimal != null)
                    {
                        if (WithinEatingRange(_targetAnimal))
                        {
                            BeginEating(_targetAnimal);
                            if (IsMoving)
                                StopMoving();
                        }
                        else
                        {
                            if (!IsMoving)
                                BeginMoving(new MovementVector(_targetAnimal.Position, 2));
                        }
                    }
                    else
                    {
                        if (!ScanForTargetAnimal())
                        {
                            if (!IsMoving)
                            {
                                int randomX = OrganismRandom.Next(0, WorldWidth - 1);
                                int randomY = OrganismRandom.Next(0, WorldHeight - 1);
                                BeginMoving(new MovementVector(new Point(randomX, randomY), 2));
                            }
                        }
                    }
                }
                else
                {
                    if (IsMoving)
                        StopMoving();
                }
            }
            catch (Exception exc)
            {
                WriteTrace(exc.ToString());
            }
        }

        private bool ScanForTargetAnimal()
        {
            try
            {
                ArrayList foundAnimals = Scan();
                if (foundAnimals.Count > 0)
                {
                    foreach (OrganismState organism in foundAnimals)
                    {
                        if (!(organism is AnimalState)) continue;
                        _targetAnimal = (AnimalState) organism;
                        BeginMoving(new MovementVector(_targetAnimal.Position, 2));
                        return true;
                    }
                }
            }
            catch (Exception exc)
            {
                WriteTrace(exc.ToString());
            }
            return false;
        }

        public override void DeserializeAnimal(MemoryStream m)
        {
        }

        public override void SerializeAnimal(MemoryStream m)
        {
        }
    }
}
