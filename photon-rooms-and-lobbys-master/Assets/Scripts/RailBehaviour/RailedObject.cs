using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

public class RailedObject : MonoBehaviourPun
{
    private RailNode currentNearestRailPoint = null;
    private Sequence mRotationSequence = null;
    private Vector3 movementDirection = Vector3.zero;
    private float distanceToCurrentNode = 0;
    private float movementSpeed = 0.5f;
    private bool switchedToEndpoint = false;
    private bool canTurn = true;

    private Action desiredProjectedTurn = null;

    public void SetNearestRailNode(RailNode newNearestNode)
    {
        this.switchedToEndpoint = true;
        this.currentNearestRailPoint = newNearestNode;
    }

    public void SetSpeed(float speed) => this.movementSpeed = speed;

    public float GetDistanceToCurrentNode() => this.distanceToCurrentNode;

    private void Start()
    {
        this.SnapToNearestRailPoint();
        this.FindRailwayOnDirection();
        if (photonView.IsMine && PhotonNetwork.IsConnected) {
            RailManager.localPlayerRailedObject = this;
        }

        if (!photonView.IsMine) {
            Destroy(this);
        }
    }

    void Update()
    {
        this.UpdateCurrentRailNodeDistance();
        this.CalculateBrakeDistance();

        if (!this.canTurn) {
            return;
        }

        if (Input.GetKeyDown(KeyCode.W)) {
            this.RotationInput(0);
        } else if (Input.GetKeyDown(KeyCode.S)) {
            this.RotationInput(-180);
        } else if (Input.GetKeyDown(KeyCode.A)) {
            this.RotationInput(-90);
        } else if (Input.GetKeyDown(KeyCode.D)) {
            this.RotationInput(-270);
        }
    }

    private void FixedUpdate()
    {
        this.Move();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            if(this.movementSpeed == 0) {
                return;
            }

            this.KillRotationSequence();
            this.SetSpeed(0);
            this.mRotationSequence = DOTween.Sequence();
            this.mRotationSequence.Join(this.transform.DORotate(new Vector3(0, this.transform.eulerAngles.y - 180), 0.35f));
            this.mRotationSequence.OnComplete(() =>
            {
                this.movementDirection = this.transform.forward;
                this.SetSpeed(0.1f);
                this.canTurn = false;
                this.switchedToEndpoint = false;
            });
        }
    }

    private void UpdateCurrentRailNodeDistance()
    {
        if(this.currentNearestRailPoint != null) {
            this.distanceToCurrentNode = Vector3.Distance(this.currentNearestRailPoint.transform.position, this.transform.position);
        }
    }
    
    private void CalculateBrakeDistance()
    {
        if (this.switchedToEndpoint && Vector3.Distance(this.currentNearestRailPoint.transform.position, this.transform.position) < 0.15f) {
            this.canTurn = true;
            this.SetSpeed(0);
            if (this.desiredProjectedTurn != null) {
                this.desiredProjectedTurn.Invoke();
                this.desiredProjectedTurn = null;
            } else {
                this.FindRailwayOnDirection();
            }
        } else if(this.switchedToEndpoint && Vector3.Distance(this.currentNearestRailPoint.transform.position, this.transform.position) < 5f) {
            if (Input.GetKeyDown(KeyCode.W)) {
                this.desiredProjectedTurn = () => this.RotationInput(0);
            } else if (Input.GetKeyDown(KeyCode.S)) {
                this.desiredProjectedTurn = () => this.RotationInput(-180);
            } else if (Input.GetKeyDown(KeyCode.A)) {
                this.desiredProjectedTurn = () => this.RotationInput(-90);
            } else if (Input.GetKeyDown(KeyCode.D)) {
                this.desiredProjectedTurn = () => this.RotationInput(-270);
            }
        }
    }

    private void SnapToNearestRailPoint()
    {
        List<(RailNode, RailNode)> currentRails = RailManager.GetRails();
        if(currentRails == null) {
            return;
        }

        float distanceRecord = Vector3.Distance(currentRails[0].Item1.transform.position, this.transform.position);
        RailNode closestRail = currentRails[0].Item1;

        foreach ((RailNode, RailNode) rail in currentRails) {
            if(Vector3.Distance(rail.Item1.transform.position, this.transform.position) < distanceRecord) {
                closestRail = rail.Item1;
            }
            if(Vector3.Distance(rail.Item2.transform.position, this.transform.position) < distanceRecord) {
                closestRail = rail.Item2;
            }
        }

        if(closestRail == null) {
            return;
        }

        //this.transform.position = closestRail.transform.position;
        this.currentNearestRailPoint = closestRail;
    }

    private void FindRailwayOnDirection()
    {
        List<(RailNode, RailNode)> currentRails = RailManager.GetRails();
        if (currentRails == null) {
            return;
        }

        foreach ((RailNode, RailNode) rail in currentRails) {
            if (rail.Item1.Equals(this.currentNearestRailPoint)) {
                if((rail.Item2.transform.position - rail.Item1.transform.position).normalized == this.transform.forward) {
                    this.movementDirection = (rail.Item2.transform.position - rail.Item1.transform.position).normalized;
                    this.SetSpeed(0.1f);
                    this.canTurn = false;
                    this.switchedToEndpoint = false;
                }
            } else if (rail.Item2.Equals(this.currentNearestRailPoint)) {
                if ((rail.Item1.transform.position - rail.Item2.transform.position).normalized == this.transform.forward) {
                    this.movementDirection = (rail.Item1.transform.position - rail.Item2.transform.position).normalized;
                    this.SetSpeed(0.1f);
                    this.canTurn = false;
                    this.switchedToEndpoint = false;
                }
            }
        }
    }

    private void Move()
    {
        this.transform.position += this.movementDirection * this.movementSpeed;
    }

    private void KillRotationSequence()
    {
        if (this.mRotationSequence != null && this.mRotationSequence.IsActive()) {
            this.mRotationSequence.Kill();
        }
    }

    private void RotationInput(float yBodyRotation)
    {
        if (this.currentNearestRailPoint == null) {
            this.SnapToNearestRailPoint();
        }

        this.KillRotationSequence();
        this.mRotationSequence = DOTween.Sequence();
        this.mRotationSequence.Join(this.transform.DORotate(new Vector3(0, yBodyRotation, 0), 0.15f));
        this.mRotationSequence.Join(this.transform.DOMove(this.currentNearestRailPoint.transform.position, 0.1f));
        this.mRotationSequence.OnComplete(() => FindRailwayOnDirection());
    }
}
