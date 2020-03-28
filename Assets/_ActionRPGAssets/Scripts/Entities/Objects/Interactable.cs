
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    //public float interactionRadius;
    [SerializeField]
    public CharacterControls Player;
    public Item item; 
    public GameObject itemPrefab;
    


    public void Awake()
    {
        Player = FindObjectOfType<CharacterControls>();
        GameObject itemPrefab = Instantiate(item.itemModel);
        itemPrefab.transform.SetParent(this.transform);
        itemPrefab.transform.localPosition = Vector3.zero;
        itemPrefab.transform.rotation = this.transform.rotation;
        this.gameObject.name = item.itemName; 
    }


    public virtual void Interact()
    {

    }

    

    private void OnDrawGizmosSelected()
    {
        //Gizmos.color = Color.green;
       // Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }

    private void Update()
    {
       // float distanceFromPlayer = Vector3.Distance(transform.position, Player.transform.position);

       // if (distanceFromPlayer <= interactionRadius) 
        //{
         //   Player.canPickUp = true;
        
       // }
      //  else 
       // {
       //     Player.canPickUp = false;
        
       // }

       
       // print("Can pick up" + Player.canPickUp);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Player.canPickUp = true;
            Debug.Log("Can Pick Up!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Player.canPickUp = false;
            Debug.Log("Can not Pick Up!");
        }
    }



}
