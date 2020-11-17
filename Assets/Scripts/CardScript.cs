using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardScript : ScriptableObject
{
    //On update is called once every phase. Characteristic-defining abilities can be declared here, such as Changeling or Devoid.
    public abstract void OnUpdate();

    //The untap phase.
    public virtual void OnUntap() {}

    //The upkeep phase.
    public virtual void OnUpkeep() {}

    //The draw phase.
    public virtual void OnDraw() {}

    //The precombat main phase.
    public virtual void OnPreCombatMain() {}
}
