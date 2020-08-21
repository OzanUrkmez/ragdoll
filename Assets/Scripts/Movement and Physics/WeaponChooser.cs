using UnityEngine;

public class WeaponChooser : MonoBehaviour
{
    public GameObject ChooseKnife;

    public GameObject ChooseFist;

    public GameObject ChooseGun;

    public GameObject ChooseBat;

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
    }
}
