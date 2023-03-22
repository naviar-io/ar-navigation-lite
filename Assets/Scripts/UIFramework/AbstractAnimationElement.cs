using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractAnimationElement : ScriptableObject {

    public abstract void PlayAnimation(GameObject target);

    public abstract void RevertAnimation(GameObject target);
}
