using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Redcode.Extensions;
using UnityEngine;

public class ParentingSearcher : MonoBehaviour
{
	[SerializeField] private PhotonView phview;
	[SerializeField] private ParentingType parentType;
	[SerializeField] private ParentingType needsToConnectTo;
	[SerializeField] private GameObject contentParenting;

	public PhotonView accessPhotonView { get { return phview; } }
	public ParentingType parent_Type { get { return parentType; } }
	public GameObject parent_Object { get { return contentParenting; } }
	public void EnableParentingSystem()
	{
		phview.RPC("SearchParent", RpcTarget.AllBuffered);
	}

	[PunRPC]
	private void SearchParent()
	{
		ParentingSearcher[] objectsWithScript = FindObjectsOfType<ParentingSearcher>();


		foreach (var item in objectsWithScript)
		{
			if (item != this && item.accessPhotonView.OwnerActorNr == phview.OwnerActorNr && item.parent_Type == needsToConnectTo)
			{

				if (item.parent_Type == ParentingType.controllerCar)
				{
					this.gameObject.transform.parent = item.parent_Object.transform;
					this.gameObject.transform.transform.localPosition = Vector3.zero;
					this.gameObject.transform.SetLocalPositionY(-0.1f);
			

				}
				else
				{
					this.gameObject.transform.parent = item.transform;
					this.gameObject.transform.transform.localPosition = Vector3.zero;
				}

			}
		}
	}
}

public enum ParentingType
{
	carModel,
	accesories,
	controllerCar
}
