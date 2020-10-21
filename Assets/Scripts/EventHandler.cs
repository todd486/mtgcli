using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class Events {
        #region Beginning Phase

        public static event Action OnUntap;
        public static void Untap() => OnUntap?.Invoke();

        public static event Action OnUpkeep;
        public static void Upkeep() => OnUpkeep?.Invoke();

        public static event Action OnDraw;
        public static void Draw() => OnDraw?.Invoke();

        #endregion

        public static event Action OnPreCombatMain;
        public static void PreCombatMain() => OnPreCombatMain?.Invoke();

        #region Combat Phase 

        public static event Action OnBeginCombat;
        public static void BeginCombat() => OnBeginCombat?.Invoke();

        public static event Action OnDeclareAttackers;
        public static void DeclareAttackers() => OnDeclareAttackers?.Invoke();

        public static event Action OnDeclareBlockers;
        public static void DeclareBlockers() => OnDeclareBlockers?.Invoke();

        public static event Action OnCombatDamage;
        public static void CombatDamage() => OnCombatDamage?.Invoke();

        public static event Action OnEndCombat;
        public static void EndCombat() => OnEndCombat?.Invoke();

        #endregion

        public static event Action OnPostCombatMain;
        public static void PostCombatMain() => OnPostCombatMain?.Invoke();

        #region End Phase

        public static event Action OnEndStep;
        public static void EndStep() => OnEndStep?.Invoke();

        public static event Action OnCleanup;
        public static void Cleanup() => OnCleanup?.Invoke();

        #endregion

        #region Generic Events

        public static event Action OnCreatureDeath;
        public static void CreatureDeath() => OnCreatureDeath?.Invoke();

        #endregion

        #region etb, ltb

        public static event Action OnEnterTheBattlefield;
        public static void EnterTheBattlefield() => OnEnterTheBattlefield?.Invoke();

        public static event Action OnLeaveTheBattlefield;
        public static void LeaveTheBattlefield() => OnLeaveTheBattlefield?.Invoke();

        #endregion

        public static event Action OnPriorityCheck;
        public static void PriorityCheck() => OnPriorityCheck?.Invoke();
    }

public class EventHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
