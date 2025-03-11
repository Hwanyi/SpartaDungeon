using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f;
    private float lastCheckTime;
    public float maxCheckDistance;

    public LayerMask layerMask;

    public GameObject itemObject;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDescription;

    Camera _camera;
    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                if (hit.collider.gameObject != itemObject)
                {
                    itemObject = hit.collider.gameObject;
                    SetText();
                }
            }
            else
            {
                itemObject = null;
                itemName.gameObject.SetActive(false);
                itemDescription.gameObject.SetActive(false);
            }
        }

    }

    void SetText()
    {
        itemName.text = itemObject.GetComponent<ItemObject>().itemData.name;
        itemDescription.text = itemObject.GetComponent<ItemObject>().itemData.description;
        itemName.gameObject.SetActive(true);
        itemDescription.gameObject.SetActive(true);
    }

    public void OnInterAction(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && itemObject != null)
        {
            //캐릭터 인벤토리 따로 구현
            Destroy(itemObject);
            itemObject = null;
            itemName.gameObject.SetActive(false);
            itemDescription.gameObject.SetActive(false);
        }
    }
}
