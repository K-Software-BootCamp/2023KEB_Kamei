using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlot : MonoBehaviour
{
    
    [SerializeField] private Slot[] quickSlots;
    [SerializeField] private Transform tf_parent;

    private int selectedSlot;
    [SerializeField] private GameObject go_SelectedImage;

    [SerializeField] private TPSCharaterController theWeaponManager;
    [SerializeField] public GameObject Tools;

    GameObject axe;


    // Start is called before the first frame update
    void Start()
    {
        quickSlots = tf_parent.GetComponentsInChildren<Slot>();
        selectedSlot = 0;
    }

    // Update is called once per frame
    void Update()
    {
        TryInputNumber();
    }

    private void TryInputNumber()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            ChangeSlot(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            ChangeSlot(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            ChangeSlot(2);
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            ChangeSlot(3);
        else if (Input.GetKeyDown(KeyCode.Alpha5))
            ChangeSlot(4);
    }

     private void ChangeSlot(int _num)
    {
        SelectedSlot(_num);
        Execute();
    }

    private void SelectedSlot(int _num)
    {
        selectedSlot = _num;
        go_SelectedImage.transform.position = quickSlots[selectedSlot].transform.position;
    }

    private void Execute()
    {
        if (quickSlots[selectedSlot].item != null)
        {
            if (quickSlots[selectedSlot].item.itemName == "AxeItem")
            {
                theWeaponManager.punchReady = false;
                theWeaponManager.axeReady = true;
                theWeaponManager.pickaxeReady = false;
                theWeaponManager.animator.SetBool("isAxe", true);
                theWeaponManager.SetItem("axe", true);
                // 딕셔너리 Axe{ key( string Axe ) : value( bool isactive ) }  --> isactive 를 true로 만듬
                // SetItem( Axe.key, Axe.value ) --> Axe를 활성화 시킴
            }
            else if (quickSlots[selectedSlot].item.itemName == "PickAxeItem")
            {
                theWeaponManager.punchReady = false;
                theWeaponManager.axeReady = false;
                theWeaponManager.pickaxeReady = true;
                theWeaponManager.animator.SetBool("isPickAxe", true);
            }
            else theWeaponManager.punchReady = true;
        }
        else theWeaponManager.punchReady = true;
    }

    // public void IsActivatedQuickSlot(int _num)
    // {
    //     if (selectedSlot == _num)
    //     {
    //         Execute();
    //         return;
    //     }
    //     if (DragSlot.instance != null)
    //     {
    //         if (DragSlot.instance.dragSlot != null)
    //         {
    //             if (DragSlot.instance.dragSlot.GetQuickSlotNumber() == selectedSlot)
    //             {
    //                 Execute();
    //                 return;
    //             }
    //         }
    //     }     
    // }
}
