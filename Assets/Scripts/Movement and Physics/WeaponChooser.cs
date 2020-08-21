using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class WeaponChooser : MonoBehaviour
{
    public GameObject ChooseKnife;

    public GameObject ChooseFist;

    public GameObject ChooseGun;

    public GameObject ChooseBat;

    public GameObject[] Weapons;

    public GameObject CurrentWeapon;

    public int WeaponNumber = 0;

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown("1"))
        {
            ChooseFist.SetActive(true);
            ChooseKnife.SetActive(false);
            ChooseGun.SetActive(false);
            ChooseBat.SetActive(false);
        }
        if (Input.GetKeyDown("2"))
        {
            ChooseFist.SetActive(false);
            ChooseKnife.SetActive(true);
            ChooseGun.SetActive(false);
            ChooseBat.SetActive(false);
        }
        if (Input.GetKeyDown("3"))
        {
            ChooseFist.SetActive(false);
            ChooseKnife.SetActive(false);
            ChooseGun.SetActive(true);
            ChooseBat.SetActive(false);
        }
        if (Input.GetKeyDown("4"))
        {
            ChooseFist.SetActive(false);
            ChooseKnife.SetActive(false);
            ChooseGun.SetActive(false);
            ChooseBat.SetActive(true);
        }

        if (Input.GetAxisRaw("Mouse ScrollWheel") == 0)
        {
            return;
        }
        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
        {
            if (WeaponNumber < 4)
            {
                WeaponNumber += 1;
                CurrentWeapon = Weapons[WeaponNumber - 1];
            }
        }
        else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
        {
            if (WeaponNumber > 1)
            {
                WeaponNumber = (WeaponNumber - 1);
                CurrentWeapon = Weapons[WeaponNumber - 1];
                Debug.Log(WeaponNumber);
            }
            
        }

        foreach (GameObject weapon in Weapons)
        {
            if (weapon == CurrentWeapon)
            {
                weapon.SetActive(true);
            }
            else
            {
                weapon.SetActive(false);
            }
        }
    }
}
