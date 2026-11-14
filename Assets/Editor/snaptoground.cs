using UnityEngine;
using UnityEditor;

/// <summary>
/// Editor tool that snaps the selected objects onto the Terrain below them.
/// Only the Y position is modified: X, Z and rotation are left untouched.
/// Must be placed in a folder named "Editor" (e.g. Assets/Editor).
/// </summary>
public static class SnapToGround
{
    [MenuItem("Tools/Snap To Ground")]
    private static void Snap()
    {
        // Only the objects that are explicitly selected (not their children)
        Transform[] selection = Selection.GetTransforms(
            SelectionMode.Editable | SelectionMode.ExcludePrefab);

        if (selection.Length == 0)
        {
            Debug.LogWarning("Snap To Ground: no object selected.");
            return;
        }

        int movedCount = 0;
        int skippedCount = 0;

        foreach (Transform t in selection)
        {
            Vector3 position = t.position;

            // Look for the Terrain height at the object's X/Z position
            if (TryGetGroundHeight(position.x, position.z, out float groundHeight))
            {
                // Makes the operation undoable with Ctrl+Z
                Undo.RecordObject(t, "Snap To Ground");

                // Only Y changes: X and Z stay the same
                position.y = groundHeight;
                t.position = position;

                movedCount++;
            }
            else
            {
                skippedCount++;
            }
        }

        Debug.Log("Snap To Ground: " + movedCount + " object(s) snapped to the ground, "
                  + skippedCount + " skipped (no Terrain below).");
    }

    /// <summary>
    /// Gets the world-space height of the Terrain at the given X/Z position.
    /// Returns false if no active Terrain covers that position.
    /// </summary>
    private static bool TryGetGroundHeight(float x, float z, out float height)
    {
        height = 0f;

        foreach (Terrain terrain in Terrain.activeTerrains)
        {
            Vector3 origin = terrain.transform.position;
            Vector3 size = terrain.terrainData.size;

            // Is the (x, z) point inside this Terrain's footprint?
            bool isInsideTerrain =
                x >= origin.x && x <= origin.x + size.x &&
                z >= origin.z && z <= origin.z + size.z;

            if (isInsideTerrain)
            {
                // SampleHeight returns a height relative to the Terrain,
                // so the Terrain's own Y position is added to get world height
                height = terrain.SampleHeight(new Vector3(x, 0f, z)) + origin.y;
                return true;
            }
        }

        return false;
    }
}
