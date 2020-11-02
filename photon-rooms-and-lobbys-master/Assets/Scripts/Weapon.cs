using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviourPun
{
    public GameObject weapon;
    public GameObject positionRef;
    public GameObject bulletRef;
    public float coldDown = 1.5f, bulletForce = 18f;
    bool canShoot = true;
    
    void Update()
    {
        if(photonView.IsMine && PhotonNetwork.IsConnected)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {                
                photonView.RPC("ShootOnDelay", RpcTarget.All);
            }
        }
    }
        
    public void Shoot()
    {
        if (canShoot)
        {
            GameObject bulletClon = Instantiate(bulletRef, positionRef.transform.position, weapon.transform.rotation);
            bulletClon.GetComponent<Bullet>().tankOwner = gameObject.GetComponent<GameObject>();
            Rigidbody bulletBody = bulletClon.GetComponent<Rigidbody>();
            bulletBody.AddForce(transform.forward * bulletForce, ForceMode.Impulse);
        }
    }

    [PunRPC]
    public IEnumerator ShootOnDelay()
    {
        Shoot();
        canShoot = false;
        yield return new WaitForSeconds(coldDown);
        Debug.Log("Can shoot");
        canShoot = true;
    }
    
}
