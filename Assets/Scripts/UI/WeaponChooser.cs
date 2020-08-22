using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class WeaponChooser : MonoBehaviour
{
    [SerializeField]
    private GameObject ChooseKnife;

    [SerializeField]
    private GameObject ChooseFist;

    [SerializeField]
    private GameObject ChooseGun;

    [SerializeField]
    private GameObject ChooseBat;

    [SerializeField]
    private GameObject[] Weapons;

    [SerializeField]
    private GameObject CurrentWeapon;

    [SerializeField]
    private int WeaponNumber = 0;

    public float ScrollSense = 0.01f;

    void Start()
    {
        CurrentWeapon = Weapons[0];
        WeaponNumber = 1;
        ChooseFist.SetActive(false);
        ChooseKnife.SetActive(true);
        ChooseGun.SetActive(false);
        ChooseBat.SetActive(false);
    }

    // Update is called once per frame aefef
    void Update()
    {
        
        if (Input.GetKeyDown("1"))
        {
            CurrentWeapon = Weapons[0];
            WeaponNumber = 1;
            ChooseFist.SetActive(false);
            ChooseKnife.SetActive(true);
            ChooseGun.SetActive(false);
            ChooseBat.SetActive(false);
        }
        if (Input.GetKeyDown("2"))
        {
            CurrentWeapon = Weapons[1];
            ChooseFist.SetActive(true);
            ChooseKnife.SetActive(false);
            ChooseGun.SetActive(false);
            ChooseBat.SetActive(false);
            WeaponNumber = 2;
        }
        if (Input.GetKeyDown("3"))
        {
            CurrentWeapon = Weapons[2];
            ChooseFist.SetActive(false);
            ChooseKnife.SetActive(false);
            ChooseGun.SetActive(true);
            ChooseBat.SetActive(false);
            WeaponNumber = 3;
        }
        if (Input.GetKeyDown("4"))
        {
            CurrentWeapon = Weapons[3];
            ChooseFist.SetActive(false);
            ChooseKnife.SetActive(false);
            ChooseGun.SetActive(false);
            ChooseBat.SetActive(true);
            WeaponNumber = 4;
        }

        if (Input.GetAxisRaw("Mouse ScrollWheel") == 0)
        {
            return;
        }
        if (Input.GetAxisRaw("Mouse ScrollWheel") > ScrollSense)
        {
            if (WeaponNumber < 4)
            {
                WeaponNumber += 1;
                CurrentWeapon = Weapons[WeaponNumber - 1];
            }
        }
        else if (Input.GetAxisRaw("Mouse ScrollWheel") < -ScrollSense)
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
