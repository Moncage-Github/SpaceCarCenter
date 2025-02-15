using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tuning
{
    public class Car : MonoBehaviour
    {
        [SerializeField] private List<PartsSlot> _partsSlots;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if(transform.position.x < -26.0)
            {
                SceneManager.LoadScene("StartScene");
            }
        }
    }
}