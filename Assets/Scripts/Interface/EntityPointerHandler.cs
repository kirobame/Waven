using Flux.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class EntityPointerHandler : MonoBehaviour
{
    private GameObject prefab;
    private Transform group;
    private List<GameObject> prefabList = new List<GameObject>();
    private Vector3 position = new Vector3(-182, 1000, 0);

    private Damageable life;

    [SerializeField] private float heightOffset;

    RaycastHit2D hit;
    Camera camera;

    private void Start()
    {
        prefab = Repository.Get<EntitiesHealthBar>(References.Healthbar).prefab;
        group = Repository.Get<EntitiesHealthBar>(References.Healthbar).group;

        life = gameObject.GetComponent<Damageable>();

        camera = Repository.Get<Camera>(References.Camera);
    }

    private void Update()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        hit = Physics2D.Raycast(mousePos, mousePos - camera.ScreenToWorldPoint(mousePos), Mathf.Infinity);
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

    private void OnMouseEnter()
    {
        Debug.Log("pouet");
        var instance = Instantiate(prefab, position, Quaternion.identity, group);
        prefabList.Add(instance);

        prefab.GetComponent<Slider>().value = life.Lives[0].actualValue / life.Lives[0].maxValue;

        var camera = Repository.Get<Camera>(References.Camera);
        var entityPos = camera.WorldToScreenPoint(gameObject.transform.position);

        instance.TryGetComponent<RectTransform>(out RectTransform tr);
        tr.position = entityPos + (Vector3.up * heightOffset);
    }
}
