using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoosBullet : MonoBehaviour
{
	public int selfDestructTime = 3;
	public float damage = -5;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("selfDestruct", selfDestructTime);
    }

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.GetComponent<RoosBullet>()) return;

		if (collision.gameObject.GetComponent<RoosBetterCharacterController>() || collision.gameObject.tag =="Player")
		{
			collision.gameObject.GetComponent<Health>().healthLevel += damage;
			Debug.Log("Hit Player");
		}

		selfDestruct();
	}

	void selfDestruct()
	{
        Destroy(gameObject);
	}
}
