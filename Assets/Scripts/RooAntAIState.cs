using Anthill.AI;
using UnityEngine;


public class RooAntAIState : AntAIState
{
	public AntAIAgent antAiAgent;

	public override void Create(GameObject aGameObject)
	{
		base.Create(aGameObject);
		antAiAgent = aGameObject.GetComponent<AntAIAgent>();

		Create(aGameObject, antAiAgent);
	}

	public virtual void Create(GameObject ownerGameObject, AntAIAgent antAIAgnet)
	{

	}


}
