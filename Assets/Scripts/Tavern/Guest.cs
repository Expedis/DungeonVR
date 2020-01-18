﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Guest : MonoBehaviour
{
    private NavMeshAgent agent;
    public float goldAmount;
    public Seat seat;
    public Transform handPosition;

    public TavernHandler tavern;
    private Transform mug;
    private bool canOrder;
    public SphereCollider coll;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        GuestHandler.instance.citizenList.Add(this);
        coll.enabled = false;
    }

    public void MoveTo(Vector3 position)
    {
        agent.SetDestination(position);
    }

    // TODO: Orders drink and wait for it, if didn't get then go out of tavern

    public void Order()
    {
        Debug.Log("Może zamówić");
        coll.enabled = true;
        Invoke("Unhandled", 30f);
        canOrder = true;
    }

    // TODO: Takes drink and starts to gossips

    public void TakeOrder(Mug mug)
    {
        this.mug = mug.transform;
        mug.transform.position = new Vector3(handPosition.position.x, handPosition.position.y, handPosition.position.z);
        mug.transform.rotation = handPosition.rotation;
        CancelInvoke("Unhandled");
        coll.enabled = false;
        Invoke("Served", 30f);
    }

    // TODO: Drink bear/wine then pay and exit from tavern

    private void Served()
    {
        LeaveMug();
        tavern.goldAmount += goldAmount;
        tavern.servedGuestAmount++;
        ExitTavern();
    }

    private void LeaveMug()
    {
        mug.parent = null;
        mug.GetComponent<Collider>().enabled = true;
        mug.GetComponent<Rigidbody>().isKinematic = false;
        mug.GetComponent<Mug>().DirtyMug();
        mug = null;
    }

    // TODO: Waiting for drink, if didn't get then go out of tavern

    public void Unhandled()
    {
        tavern.unhandledGuestAmount++;
        canOrder = false;
        ExitTavern();
    }

    // TODO: Exit tavern and go towards exit point then cleans variables for next time

    private void ExitTavern()
    {
        Debug.Log("Wychodzi");
        tavern.TryToSendGuest();
        seat.EmptySeat();
        seat = null;
        tavern = null;
        MoveTo(GuestHandler.instance.exitPoint.position);
    }

    // TODO: Checking if had collided with player, then take a drink

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && mug == null && canOrder == true)
        {
            InkeeperInventory inventory = other.GetComponent<InkeeperInventory>();
            if(inventory != null)
            {
                if (inventory.CheckTrey())
                {
                    var trey = inventory.trey.GetComponent<Trey>();
                    if(trey != null)
                    {
                        if (trey.mugs.Count != 0)
                        {
                            trey.GiveGuest(this);
                        }
                    }
                }
                else
                {
                    if(inventory.mugs.Count != 0)
                    {
                        inventory.GiveGuest(this);
                    }
                }                   
            }
        }
    }

    // TODO: Draw gizmo sphere for hand position

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(handPosition.position, 0.3f);
    }

}