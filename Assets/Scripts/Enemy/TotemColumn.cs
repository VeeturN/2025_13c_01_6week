using System.Collections;
using UnityEngine;

public class TotemColumn : MonoBehaviour
{
    [Tooltip("Minimalna odległość między totemami (w jednostkach)")]
    public float spacing = 0.1f;

    // publiczny żeby można było wymusić układ z inspektora podczas testów
    public void ArrangeTotems()
    {
        int count = transform.childCount;
        if (count == 0) return;

        // pierwszy totem = baza
        Transform baseTotem = transform.GetChild(0);
        BoxCollider2D baseCol = baseTotem.GetComponent<BoxCollider2D>();
        if (baseCol == null)//zabezpieczenie
        {
            Debug.LogWarning($"{baseTotem.name} nie ma BoxCollider2D – przerwano ustawianie.");
            return;
        }

        
        Bounds baseBounds = baseCol.bounds;
        float baseCenterX_world = baseBounds.center.x;
        float currentTopY_world = baseBounds.max.y;
        
        for (int i = 1; i < count; i++)
        {
            Transform child = transform.GetChild(i);
            BoxCollider2D childCol = child.GetComponent<BoxCollider2D>();

            // aktalne colidery dziecka srodek i  wysokość
            Bounds childBounds = childCol.bounds;
            Vector3 childCenter_world = childBounds.center;

            // nowy środek dla colidera i łaczona wysokość + odstęp
            float desiredCenterX_world = baseCenterX_world;
            float halfHeight_world = childBounds.extents.y;
            float desiredCenterY_world = currentTopY_world + halfHeight_world + spacing;

            Vector3 desiredCenter_world = new Vector3(desiredCenterX_world, desiredCenterY_world, childCenter_world.z);

            // aby przesunąć do nowego środaka
            Vector3 delta = desiredCenter_world - childCenter_world;
            child.position = child.position + delta;

            //aktualizacja nowej wysokości dla kolejnego dziecka
            Bounds newChildBounds = childCol.bounds;
            currentTopY_world = newChildBounds.max.y;
        }
    }

    private IEnumerator Start()
    {
        // Poczekaj jedną klatkę aby colidery się zaktualizowały
        yield return null;
        ArrangeTotems();
    }
}
