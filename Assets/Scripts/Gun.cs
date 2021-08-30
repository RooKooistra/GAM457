using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    
    public Transform bulletSpawnPosition;
    public GameObject prefabBullet;
    public float firerate = 0.3f;
    public float bulletForce = 50;

    bool isFiring = false;
    GameObject playerGameObject;
    float firePauseVariable = 0;
    float turnSmoothVelocity;


    // Start is called before the first frame update
    void Start()
    {
        LogicModel.FireGuns += HandleFireGuns;
        LogicModel.StopGuns += HandleStopGuns;
    }

	private void OnDestroy()
	{
        LogicModel.FireGuns -= HandleFireGuns;
        LogicModel.StopGuns -= HandleStopGuns;
    }


    void HandleFireGuns(GameObject playerGO)
	{
        isFiring = true;
        playerGameObject = playerGO;
	}

    void HandleStopGuns()
    {
        isFiring = false;
    }

    // Update is called once per frame
    void Update()
    {
        firePauseVariable += Time.deltaTime;
        if (!isFiring) return;

        Quaternion lookOnLook = 
            Quaternion.LookRotation((playerGameObject.transform.position + 
            (playerGameObject.transform.forward * 
            Random.Range(0, playerGameObject.GetComponent<RoosBetterCharacterController>().speed))) - transform.position); // randomise the leading shot

        transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, Time.deltaTime * 5f);

        if (firePauseVariable < firerate) return;

        GameObject bullet = Instantiate(prefabBullet, bulletSpawnPosition.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().AddForce(bulletSpawnPosition.forward * bulletForce, ForceMode.Impulse);

        firePauseVariable = 0;
    }
}
