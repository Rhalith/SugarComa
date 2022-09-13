using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HurtBoxType
{
    Player = 1 << 0,
    Meteor = 1 << 2,
}
[System.Flags]
public enum HurtBoxMask
{
    None = 0,
    Player = 1 << 0,
    Meteor = 1 << 2
}
