using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchLight : MonoBehaviour
{
    [Range (0,1)]
    public float suspiciousThreshold = 0.5f;
    public float turnSmoothTime = 0.1f;
    public float intensityCalm = 2;
    public float intensityAlerted = 4;

    float turnSmoothVelocity;
    Light searchLight;
    Vision vision;
    LogicModel logicModel;
    // Start is called before the first frame update
    private void Start()
    {
        searchLight = GetComponent<Light>();
        vision = GetComponentInParent<Vision>();
        logicModel = GetComponentInParent<LogicModel>();

        searchLight.spotAngle = vision.viewAngle;

        logicModel.Alerted += HandleAlerted;
        logicModel.Calm += HandleCalm;
    }

	private void OnDestroy()
    {
        logicModel.Alerted -= HandleAlerted;
        logicModel.Calm -= HandleCalm;
    }

    void HandleAlerted()
	{
        searchLight.color = Color.yellow;
        searchLight.intensity = intensityAlerted;
        searchLight.range = vision.viewRadius * 2 * vision.alertedMultiplier;
	}
    void HandleCalm()
	{
        searchLight.color = Color.white;
        searchLight.intensity = intensityCalm;
        searchLight.range = vision.viewRadius * 2;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 returnedDirection = logicModel.GetClosestSuspicious(suspiciousThreshold);
        Vector3 direction = (returnedDirection == Vector3.zero) ? transform.parent.forward : returnedDirection - transform.position;

        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg; // get rotation required
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime); // smooth rotation
        transform.rotation = Quaternion.Euler(0f, angle, 0f); // rotate on y axis
    }
}
