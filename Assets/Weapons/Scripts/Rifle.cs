using UnityEngine;

public class Rifle : Pistol
{
    public override Vector3 Spread()
    {
        Vector3 forwardVector = Vector3.forward;
        float deviation = spreadPattern[spread].x;
        float angle = spreadPattern[spread].y;
        forwardVector = Quaternion.AngleAxis(deviation, Vector3.up) * forwardVector;
        forwardVector = Quaternion.AngleAxis(angle, Vector3.forward) * forwardVector;
        forwardVector = player.cam.transform.rotation * forwardVector;

        if (spread < spreadPattern.Length - 1)
            spread++;
        else
            spread = 1;

        StartCoroutine(shaker.Shake(fireRatePrimary, 0.15f));

        return forwardVector;
    }
}
