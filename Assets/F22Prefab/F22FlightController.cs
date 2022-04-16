using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Core;
using Assets.F22Prefab;

public class F22FlightController : MonoBehaviour
{
    public F22Aircraft state;
    public Rigidbody body;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        state = new F22Aircraft(body);
    }

    // Update is called once per frame
    void Update()
    {
        state.Update();
    }
}
