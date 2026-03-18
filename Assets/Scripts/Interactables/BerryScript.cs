using UnityEngine;

public class BerryScript : MonoBehaviour
{
    [SerializeField] private PlayerSanity _playerSanity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void Eaten() 
    {
        _playerSanity.FlatSanityIncrease(10f);
        Debug.Log("Eaten");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
