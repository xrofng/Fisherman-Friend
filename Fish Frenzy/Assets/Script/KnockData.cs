using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockData : MonoBehaviour {
    
    public enum KnockScale
    {
        verySmall =0,
        small,
        medium,
        far,
        veryFar,
    }
    [Header("Force Applied for each level")]
    [Help(" 0:very small\n 1:small \n 2:medium \n 3:far \n 4:very far \n 5:FIND ONE PIECE")]
    public bool ___;

    public enum xyComponent
    {
        force = 0,
        upMultiplier
    }
    /// x = launching force , y = upMultiplier. use for Player.damageRecieve
    public Vector2[] force;

    [Space(2)]
    [Header("Tier of Damage/Charge")]
    [Help("4 Damages interval")]
    public bool Apply;
    /// check if damage between x and y. return index
    public Vector2[] slapDamage;
    public Vector2[] throwCharge;
    public enum AtPercent
    {
        zero = 0,
        twoFive,
        fiveZero,
        sevenFive,
        oneZeroZero,
        oneFiveZero,
        twoZeroZero,
        threeZeroZero
    }

    [Space(2)]
    [Header("% to reach each level of knocking")]
    public float[] percentList = { 0, 25, 50, 75, 100, 150, 200, 300, 1000 };
    public Color[] colorLevel  = new Color[8];
    [Header("Knock Level due to %")]
    [Help("0:At 0 %\n1:At 25 %\n2:At 50 %\n3:At 75 %\n4:At 100 %\n5:At 150 %\n6:At 200 %\n7:At 300 % ++")]
    public bool Knock;
    /// 
    public KnockScale[] SlapScale_tier1;
    public KnockScale[] SlapScale_tier2;
    public KnockScale[] SlapScale_tier3;
    public KnockScale[] SlapScale_tier4;
    protected List<KnockScale[]> SlapScaleList = new List<KnockScale[]>();
    public KnockScale[] ThrowScale_tier1;
    public KnockScale[] ThrowScale_tier2;
    public KnockScale[] ThrowScale_tier3;
    public KnockScale[] ThrowScale_tier4;
    protected List<KnockScale[]> ThrowScaleList = new List<KnockScale[]>();

    [Header("Vertical")]
    public float verticalInstaKillPercent = 300;
    public float vertiForceMin = 1;
    public float vertiForceMax = 50;

    // Use this for initialization
    void Start () {
        SlapScaleList.Add(SlapScale_tier1);
        SlapScaleList.Add(SlapScale_tier2);
        SlapScaleList.Add(SlapScale_tier3);
        SlapScaleList.Add(SlapScale_tier4);

        ThrowScaleList.Add(ThrowScale_tier1);
        ThrowScaleList.Add(ThrowScale_tier2);
        ThrowScaleList.Add(ThrowScale_tier3);
        ThrowScaleList.Add(ThrowScale_tier4);
    }
	

    int getDamageApplyIndex_Charge(int charge, Vector2[] chargeArr)
    {
        for (int i = 0; i < chargeArr.Length; i++)
        {
            if (charge >= chargeArr[i].x && charge <= chargeArr[i].y)
            {
                return i;
            }
        }
        return 0;
    }
    int getDamageApplyIndex_Attack(int damage , Vector2[] damageArr)
    {
        for (int i = 0; i < damageArr.Length; i++)
        {
            if (damage >= damageArr[i].x && damage <= damageArr[i].y)
            {
                return i;
            }
        }
        return 0;
    }
    int getKnockScaleIndex(int percent, float[] percentArr)
    {
        for (int i = 0; i < percentArr.Length - 1; i++)
        {
            if (percent >= percentArr[i] && percent <= percentArr[i+1])
            {
                return i;
            }
        }
        return 0;
    }
    public float GetVerticalKnockForce(int percent)
    {
        return Mathf.Lerp(vertiForceMin, vertiForceMax, percent / verticalInstaKillPercent);
    }
    public Vector2 GetSlapKnockForce(int damage, int percent)
    {
        // check which damage set
        int ApplyDamageIndex = getDamageApplyIndex_Attack(damage, slapDamage);
        int KnockScaleIndex = getKnockScaleIndex(percent, percentList);
        KnockScale knockLevel = SlapScaleList[ApplyDamageIndex][KnockScaleIndex];
        return force[(int)knockLevel];
    }
    public Vector2 getThrowKnockForce(int chargePercent, int percent)
    {
        // check which carge set
        int ApplyDamageIndex = getDamageApplyIndex_Charge(chargePercent, throwCharge);
        int KnockScaleIndex = getKnockScaleIndex(percent, percentList);
        //int tier = ApplyDamageIndex + 1;
       // print("tier" + tier);
       // print("at element" + KnockScaleIndex);
        KnockScale knockLevel = ThrowScaleList[ApplyDamageIndex][KnockScaleIndex];
       
        return force[(int)knockLevel];
    }

    public Color GetColor(int percent)
    {
        int colorIndex = 0;
        for(int i=0;i<percentList.Length;i++)
        {
            if (percent >= percentList[i])
            {
                colorIndex = i; 
            }
            if(percent < percentList[i])
            {
                return colorLevel[colorIndex];
            }
        }
       return colorLevel[colorIndex];
    }
}
