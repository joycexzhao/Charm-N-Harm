using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{

    int totalWeapons = 1;
    public int currentWeaponIndex;

    public GameObject[] weapons;
    public GameObject weaponHolder;
    public GameObject currentWeapon;

    // Start is called before the first frame update
    void Start()
    {
        totalWeapons = weaponHolder.transform.childCount;
        weapons = new GameObject[totalWeapons];

        for(int i = 0; i < totalWeapons; i++)
        {
            weapons[i] = weaponHolder.transform.GetChild(i).gameObject;
            weapons[i].SetActive(false);
        }

        weapons[0].SetActive(true);
        currentWeapon = weapons[0];
        currentWeaponIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (weaponHolder.GetComponentInParent<WeaponParent>().attackBlocked == false)
        {
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                //next weapon
                if (currentWeaponIndex < totalWeapons - 1)
                {
                    weapons[currentWeaponIndex].SetActive(false);
                    currentWeaponIndex += 1;
                    weapons[currentWeaponIndex].SetActive(true);
                    currentWeapon = weapons[currentWeaponIndex];
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                //prev weapon
                if (currentWeaponIndex > 0)
                {
                    weapons[currentWeaponIndex].SetActive(false);
                    currentWeaponIndex -= 1;
                    weapons[currentWeaponIndex].SetActive(true);
                    currentWeapon = weapons[currentWeaponIndex];
                }
            }
        }
        
    }
}
