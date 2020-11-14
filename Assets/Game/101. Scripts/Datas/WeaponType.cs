using UnityEngine;

public enum WeaponType
{
    None = 0,
    [InspectorName("한손 검")]
    OneHanded = 1,
    [InspectorName("레이피어")]
    Rapier = 2
}