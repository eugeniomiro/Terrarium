//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                              
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OrganismBase;
using Terrarium.Hosting;

namespace Terrarium.Game
{
    /// <summary>
    ///  Rolls up and provides access to all actions requested by animals in a given tick.
    /// </summary>
    [Serializable]
    public class TickActions
    {
        /// <summary>
        ///  All of the attack actions that organisms have performed in this tick.
        /// </summary>
        private readonly Dictionary<String, AttackAction> _attackActions = new Dictionary<String, AttackAction>();

        /// <summary>
        ///  All of the defend actions that organisms have performed in this tick.
        /// </summary>
        private readonly Dictionary<String, DefendAction> _defendActions = new Dictionary<String, DefendAction>();

        /// <summary>
        ///  All of the eat actions that organisms have performed in this tick.
        /// </summary>
        private readonly Dictionary<String, EatAction>  _eatActions = new Dictionary<String, EatAction>();

        /// <summary>
        ///  All of the movement actions that organisms have performed in this tick.
        /// </summary>
        private readonly Dictionary<String, MoveToAction> _moveToActions = new Dictionary<String, MoveToAction>();

        /// <summary>
        ///  All of the reproduce actions that organisms have performed in this tick.
        /// </summary>
        private readonly Dictionary<String, ReproduceAction> _reproduceActions = new Dictionary<String, ReproduceAction>();

        /// <summary>
        ///  Provides access to the Movement actions.
        /// </summary>
        public IEnumerable<MoveToAction> MoveToActions
        {
            get { return _moveToActions.Values.ToArray(); }
        }

        /// <summary>
        ///  Provides access to the Attack actions.
        /// </summary>
        public IEnumerable<AttackAction> AttackActions
        {
            get { return _attackActions.Values.ToArray(); }
        }

        /// <summary>
        ///  Provides access to the Eat actions.
        /// </summary>
        public IEnumerable<EatAction> EatActions
        {
            get { return _eatActions.Values.ToArray(); }
        }

        /// <summary>
        ///  Provides access to the Reproduction actions.
        /// </summary>
        public IEnumerable<ReproduceAction> ReproduceActions
        {
            get { return _reproduceActions.Values.ToArray(); }
        }

        /// <summary>
        ///  Provides access to the Defend actions.
        /// </summary>
        public IDictionary<String, DefendAction> DefendActions
        {
            get
            {
                return _defendActions.ToDictionary(k => k.Key, v => v.Value);
            }
        }

        /// <summary>
        ///  Rips through all organisms and wraps their pending actions
        ///  up into the hashtables on a per action type basis.
        /// </summary>
        /// <param name="scheduler">The game scheduler that has all of the organisms.</param>
        internal void GatherActionsFromOrganisms(IGameScheduler scheduler)
        {
            foreach (Organism organism in scheduler.Organisms)
            {
                PendingActions pendingActions = organism.GetThenErasePendingActions();
                if (pendingActions.MoveToAction != null)
                {
                    _moveToActions[organism.ID] = pendingActions.MoveToAction;
                }

                if (pendingActions.AttackAction != null)
                {
                    _attackActions[organism.ID] = pendingActions.AttackAction;
                }

                if (pendingActions.EatAction != null)
                {
                    _eatActions[organism.ID] = pendingActions.EatAction;
                }

                if (pendingActions.ReproduceAction != null)
                {
                    _reproduceActions[organism.ID] = pendingActions.ReproduceAction;
                }

                if (pendingActions.DefendAction != null)
                {
                    _defendActions[organism.ID] = pendingActions.DefendAction;
                }
            }
        }
    }
}