using UnityEngine;

public class Rifle : Pistol
{
    public override Vector3 Spread()
    {
        Vector3 forwardVector = Vector3.forward;
        float deviation;
        float angle;
        if (spread < spreadPattern.Length - 1)
        {
            deviation = spreadPattern[spread].x;
            angle = spreadPattern[spread].y;
        }
        else
        {
            int random = spreadPattern.Length;
            deviation = Random.Range(spreadPattern[random / 2].x, spreadPattern[random - 1].x);
            angle = Random.Range(0, 360f);
        }
        forwardVector = Quaternion.AngleAxis(deviation, Vector3.up) * forwardVector;
        forwardVector = Quaternion.AngleAxis(angle, Vector3.forward) * forwardVector;
        forwardVector = player.cam.transform.rotation * forwardVector;

        /*Vector3 gunPosition = gameObject.transform.localPosition;
        gunPosition.y += 0.05f;
        gunPosition.z -= 0.1f;

        gameObject.transform.localPosition = gunPosition;*/

        gameObject.transform.localPosition = new Vector3(
            gameObject.transform.localRotation.x,
            gameObject.transform.localRotation.y + 0.05f,
            gameObject.transform.localRotation.z - 0.01f
        );

        if (spread < spreadPattern.Length - 1)
            spread++;

        return forwardVector;
    }
}
