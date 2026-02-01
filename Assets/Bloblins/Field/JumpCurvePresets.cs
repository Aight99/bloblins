using UnityEngine;

public static class JumpCurvePresets
{
    public static AnimationCurve Arc()
    {
        AnimationCurve curve = new AnimationCurve();
        curve.AddKey(0f, 0f);
        curve.AddKey(0.5f, 1f);
        curve.AddKey(1f, 0f);
        
        for (int i = 0; i < curve.keys.Length; i++)
        {
            curve.SmoothTangents(i, 0.5f);
        }
        
        return curve;
    }

    public static AnimationCurve DoubleJump()
    {
        AnimationCurve curve = new AnimationCurve();
        curve.AddKey(0f, 0f);
        curve.AddKey(0.25f, 0.8f);
        curve.AddKey(0.5f, 0.3f);
        curve.AddKey(0.75f, 1f);
        curve.AddKey(1f, 0f);
        
        for (int i = 0; i < curve.keys.Length; i++)
        {
            curve.SmoothTangents(i, 0.5f);
        }
        
        return curve;
    }

    public static AnimationCurve Hop()
    {
        AnimationCurve curve = new AnimationCurve();
        curve.AddKey(0f, 0f);
        curve.AddKey(0.15f, 0.5f);
        curve.AddKey(0.3f, 0f);
        curve.AddKey(0.45f, 0.5f);
        curve.AddKey(0.6f, 0f);
        curve.AddKey(0.75f, 0.5f);
        curve.AddKey(1f, 0f);
        
        for (int i = 0; i < curve.keys.Length; i++)
        {
            curve.SmoothTangents(i, 0.3f);
        }
        
        return curve;
    }

    public static AnimationCurve Flat()
    {
        return AnimationCurve.Linear(0f, 0f, 1f, 0f);
    }
}
