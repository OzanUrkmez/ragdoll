using UnityEngine;
using System;

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
    private int WeaponNumber = 0;

    public float ScrollSense = 0.01f;

    void Start()
    {

        onNewWeaponChosen = null; //TODO terrible practice

        WeaponNumber = 0;
        ChooseFist.SetActive(true);
        ChooseKnife?.SetActive(false);
        ChooseGun?.SetActive(false);
        ChooseBat?.SetActive(false);
    }

    // Update is called once per frame aefef
    void Update()
    {
        
        if (Input.GetKeyDown("1"))
        {
            WeaponNumber = 0;
            ChooseFist.SetActive(false);
            ChooseKnife.SetActive(true);
            ChooseGun.SetActive(false);
            ChooseBat.SetActive(false);
        }
        if (Input.GetKeyDown("2"))
        {
            ChooseFist.SetActive(true);
            ChooseKnife.SetActive(false);
            ChooseGun.SetActive(false);
            ChooseBat.SetActive(false);
            WeaponNumber = 1;
        }
        if (Input.GetKeyDown("3"))
        {
            ChooseFist.SetActive(false);
            ChooseKnife.SetActive(false);
            ChooseGun.SetActive(true);
            ChooseBat.SetActive(false);
            WeaponNumber = 2;
        }
        if (Input.GetKeyDown("4"))
        {
            ChooseFist.SetActive(false);
            ChooseKnife.SetActive(false);
            ChooseGun.SetActive(false);
            ChooseBat.SetActive(true);
            WeaponNumber = 3;
        }

        if (Input.GetAxisRaw("Mouse ScrollWheel") > ScrollSense)
        {
            if (WeaponNumber < Weapons.Length - 1)
            {
                WeaponNumber += 1;
            }
        }
        else if (Input.GetAxisRaw("Mouse ScrollWheel") < -ScrollSense)
        {
            if (WeaponNumber > 0)
            {
                WeaponNumber = (WeaponNumber - 1);
            }
            
        }

        foreach (GameObject weapon in Weapons)
        {
            if (weapon == Weapons[WeaponNumber])
            {
                weapon.SetActive(true);
                onNewWeaponChosen?.Invoke((WeaponType)WeaponNumber);
            }
            else
            {
                weapon.SetActive(false);
            }
        }
    }

    public static Action<WeaponType> onNewWeaponChosen;


}

public enum WeaponType { Knife, Fist, Gun, Bat }