using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ability
{
    public enum testEnum
    {
        Fire,
        Water,
        Wood
    }

    public testEnum test;
    public string abilityName;

    // enum별 다른 속성 예시
    public FireData fireData;
    public WaterData waterData;
    public WoodData woodData;
}

[System.Serializable]
public class FireData
{
    public float firePower;
}

[System.Serializable]
public class WaterData
{
    public float waterFlow;
}

[System.Serializable]
public class WoodData
{
    public float woodStrength;
}

public class AbilityHolder : MonoBehaviour
{
    // 확장 시 주의점:
    // 1. AbilityType enum에 새 항목 추가
    // 2. Ability 클래스에 새 기능 관련 데이터 추가
    // 3. UI나 기능 처리 코드에서 새 타입 처리 로직 추가
    // 4. 기존 배열이나 리스트에 새 Ability 타입 인스턴스 추가 가능
    
    public Ability[] abilities;

    private void OnValidate()
    {
        HashSet<Ability.testEnum> usedEnums = new HashSet<Ability.testEnum>();
        foreach (var ability in abilities)
        {
            if (usedEnums.Contains(ability.test))
            {
                Debug.LogError($"중복된 Ability 타입 발견: {ability.test}", this);
            }
            else
            {
                usedEnums.Add(ability.test);
            }
        }
    }
}