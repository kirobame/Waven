using Flux.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class EntityPointerHandler : MonoBehaviour
{
    /*private GameObject prefab;
    private Transform group;
    private List<GameObject> prefabList = new List<GameObject>();
    private Vector3 position = new Vector3(-182, 1000, 0);

    private Damageable life;
    private Text healthText;

    [SerializeField] private float heightOffset;
    [SerializeField] private new Collider2D collider;

    private bool previousHit;
    Camera camera;

    private void Start()
    {
        prefab = Repository.Get<EntitiesHealthBar>(References.Healthbar).prefab;
        group = Repository.Get<EntitiesHealthBar>(References.Healthbar).group;

        life = GetComponent<Damageable>();

        camera = Repository.Get<Camera>(References.Camera);

        healthText = prefab.transform.GetChild(0).GetComponent<Text>();
    }

    private void Update()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        mousePos.z = -camera.transform.position.z;

        var hit = collider.OverlapPoint(camera.ScreenToWorldPoint(mousePos));
        if (previousHit == false && hit == true)
        {
            var instance = Instantiate(prefab, position, Quaternion.identity, group);
            prefabList.Add(instance);

            instance.GetComponent<Slider>().value = (float)life.Lives[0].actualValue / life.Lives[0].maxValue;
            instance.GetComponentInChildren<Text>().text = $"{life.Lives[0].actualValue} / {life.Lives[0].maxValue}";

            var camera = Repository.Get<Camera>(References.Camera);
            var entityPos = camera.WorldToScreenPoint(gameObject.transform.position + Vector3.up * heightOffset);

            instance.TryGetComponent<RectTransform>(out RectTransform tr);
            tr.position = entityPos;
        }
        else if (previousHit == true && hit == false)
        {
            foreach (var prefab in prefabList)
            {
                Destroy(prefab);
            }
        }
        previousHit = hit;

        /*Vector3 mousePos = Mouse.current.position.ReadValue();
        mousePos.z = -camera.transform.position.z;
        hit = Physics2D.Raycast(mousePos, mousePos - camera.ScreenToWorldPoint(mousePos), Mathf.Infinity);

        Debug.DrawRay(camera.ScreenToWorldPoint())

        Debug.DrawLine(Repository.Get<Camera>(References.Camera).ScreenToWorldPoint(mousePos), hit.point, Color.green, 3f);
        if (hit)
        {
            Debug.Log("pouet");
            Debug.DrawLine(Repository.Get<Camera>(References.Camera).ScreenToWorldPoint(mousePos), hit.point, Color.green, 3f);
            if (hit.transform.gameObject != gameObject)
            {
                if (prefabList.Count <= 0) return;

                foreach (var prefab in prefabList)
                {
                    Destroy(prefab);
                }
                return;
            }

            var instance = Instantiate(prefab, position, Quaternion.identity, group);
            prefabList.Add(instance);

            prefab.GetComponent<Slider>().value = life.Lives[0].actualValue / life.Lives[0].maxValue;

            var camera = Repository.Get<Camera>(References.Camera);
            var entityPos = camera.WorldToScreenPoint(gameObject.transform.position);

            instance.TryGetComponent<RectTransform>(out RectTransform tr);
            tr.position = entityPos + (Vector3.up * heightOffset);
        }
    }

 //private void OnMouseEnter()
 //{
 //    Debug.Log("pouet");
 //    var instance = Instantiate(prefab, position, Quaternion.identity, group);
 //    prefabList.Add(instance);
 //
 //    prefab.GetComponent<Slider>().value = life.Lives[0].actualValue / life.Lives[0].maxValue;
 //
 //    var camera = Repository.Get<Camera>(References.Camera);
 //    var entityPos = camera.WorldToScreenPoint(gameObject.transform.position);
 //
 //    instance.TryGetComponent<RectTransform>(out RectTransform tr);
 //    tr.position = entityPos + (Vector3.up * heightOffset);
 //}*/
}
