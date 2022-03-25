using UnityEngine;

public class DamageList : MonoBehaviour
{
    [Header("Lists")]
    [SerializeField] string[] collidersList;
    [SerializeField] int[] damagesList;

    public int GetDamageFromCollisionWith(string objectTag)
    {
        if (objectTag.Equals(null) || objectTag.Equals("Untagged")) { return 0; } // if there's no tag, get out of here

        for (int i = 0; i < collidersList.Length; i++)
        {
            if (objectTag.Equals(collidersList[i]))
            {
                return damagesList[i];
            }
        }
        return 0;
    }
}
