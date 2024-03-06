//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class DynamicDepth : MonoBehaviour
//{
//    // Start is called before the first frame update
//    void Start()
//    {

//    }

//    // Update is called once per frame
//    void Update()
//    {

//    }
//}
using UnityEngine;

public class DynamicDepth : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Adjust the Order in Layer based on the object's Y position
        // This example assumes lower Y values should be drawn in front (higher Order in Layer)
        spriteRenderer.sortingOrder = Mathf.RoundToInt(transform.position.y * -100);
    }
}