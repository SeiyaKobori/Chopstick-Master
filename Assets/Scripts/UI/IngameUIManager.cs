using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameUIManager : MonoBehaviour
{
    [SerializeField]
    private Chopsticks chopsticks = null;

    private Button liftButton = null;

    private void Awake()
    {
        liftButton = transform.Find("Lift").GetComponent<Button>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchLift()
    {
        var lift = !chopsticks.isLiftUp;
        chopsticks.SetLift(lift);
    }
}
