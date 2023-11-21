using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.ParticleSystem;

public class ResourceNode : MonoBehaviour
{
    public ItemData Item;
    public int ItemDropAmount;
    public int NodeHealth;
    public int ToolLevelRequired;
    public float MiningRange;
    public ParticleSystem Particles;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Player.Instance.InRange(transform.position, MiningRange) && IsMouseOverObject())
        {
            spriteRenderer.color = Color.gray;
            if (Input.GetKeyDown(KeyCode.Mouse0)) Mine();
        }
        else
        {
            spriteRenderer.color = Color.white;
        }
    }

    [ContextMenu("DropItems")]
    public void DropItems()
    {
        for (int item = 0; item < ItemDropAmount; item++)
        {
            float angle = item * (360f / ItemDropAmount); // Calculate the angle for each object.

            // Calculate the position based on the angle and radius.
            float x = transform.position.x + 0.5f * Mathf.Cos(angle * Mathf.Deg2Rad);
            float z = transform.position.z + 0.5f * Mathf.Sin(angle * Mathf.Deg2Rad);
            Vector3 spawnPosition = new Vector3(x, transform.position.y, z);

            Item.Drop(spawnPosition);
        }
    }

    bool IsMouseOverObject()
    {
        // Cast a ray from the mouse position into the scene
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero, 100.0f,LayerMask.GetMask("ResourceNode"));

        // Check if the ray hits a collider
        if (hit.collider != null)
        {
            // Check if the collider belongs to the desired GameObject
            if (hit.collider.gameObject == gameObject)
            {
                return true;
            }
        }

        return false;
    }

    void Mine() 
    {
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        ToolData tool = InventoryController.Instance.SelectedItem as ToolData;
        if (tool == null)
        {
            tool = ScriptableObject.CreateInstance<ToolData>();
            tool.ToolLevel = 0;
            tool.Damage = 1;
        }
        if (tool.ToolLevel >= ToolLevelRequired)
        {
            ParticleSystem particle = Instantiate(Particles, worldPoint, Particles.transform.rotation);
            Destroy(particle.gameObject, particle.main.duration);
            NodeHealth -= tool.Damage;
            if (NodeHealth <= 0)
            {
                DropItems();
                Destroy(gameObject);
            }
        }
    }
}