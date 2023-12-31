using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mixer : MonoBehaviour, IMakeMenu
{
    [SerializeField] Animator animator;

    public GameObject juiceObject;

    [SerializeField] private Machines machines;

    [SerializeField] private bool isWorking = false;
    [SerializeField] private float timer;

    private void Awake()
    {
        timer = UpgradeManager.Instance.GetUpgrade("Mixer:Time");
    }
    
    private void OnMouseDown()
    {
        if(!isWorking)
        {
            MakeMenu();
        }
    }

    public void MakeMenu()
    {
        SoundManager.Instance.PlayEffect("mixer");
        StartCoroutine(JuiceGenerate());
    }

    IEnumerator JuiceGenerate()
    {
        animator.SetInteger("State", 1);
        isWorking = true;
        yield return new WaitForSeconds(1); // 애니메이션 기다리기

        animator.SetInteger("State", 2);
        yield return new WaitForSeconds(timer);

        animator.SetInteger("State", 3);
        yield return new WaitForSeconds(1); // 애니메이션 기다리기

        List<Holder> emptyList = GetEmptyList();
        if(emptyList.Count != 0)
        {         
            foreach(var holder in emptyList)
            {
                GameObject menu = Instantiate(juiceObject, holder.transform);
                holder.Object = menu;
            }
        }

        animator.SetInteger("State", 0);

        isWorking = false;
    }

    public List<Holder> GetEmptyList()
    {
        List<Holder> emptyList = new List<Holder>();
        List<GameObject> holders = machines.MachinesList;
        for(int i=0;i<machines.MachineCount;i++) // 활성화된 홀더 중에
        {
            Holder holder = holders[i].GetComponent<Holder>();
            if(holder.Object == null) // 홀더가 비어있는
            {
                emptyList.Add(holder);
            }
        }
        return emptyList;
    }
}
