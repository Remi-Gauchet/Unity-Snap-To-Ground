# Snap To Ground

A small Unity editor tool that snaps selected objects onto the Terrain below them.

Props placed by hand or in groups on uneven terrain often end up floating or
sunk into the ground. **Snap To Ground** fixes this in one click: each selected
object gets the same Y position as the Terrain surface at its location.

## Features

- Sets the **Y position** of each selected object to the Terrain height at its X/Z.
- Leaves **X, Z and rotation** unchanged.
- Works on many objects at once.
- Fully **undoable** with `Ctrl+Z`.
- Logs how many objects were moved and how many were skipped.

## Requirements

- Unity 6.3 LTS (should work on any recent version).
- The ground must be a Unity **Terrain** component.

## Installation

1. Create an `Editor` folder inside `Assets` if it does not exist.
   > The name matters: Unity treats scripts in an `Editor` folder as editor-only,
   > so they are never included in a build.
2. Copy `SnapToGround.cs` into `Assets/Editor`.
3. Wait for Unity to recompile. A **Tools > Snap To Ground** entry appears in the menu bar.

## Usage

1. In the **Hierarchy**, select the objects to snap (`Shift+Click` / `Ctrl+Click` for several).
2. Click **Tools > Snap To Ground**.
3. Check the **Console** for a summary.
4. Not happy? Press `Ctrl+Z`.

**Tip:** test on one or two objects first, then run on the whole selection.

## How it works

For each selected object, the tool reads its X/Z position, finds the Terrain
covering that point, samples the height there (`Terrain.SampleHeight` + the
Terrain's own Y), and sets `position.y` accordingly.

The object's **pivot** (its Transform position) is what lands on the ground,
not the bottom of its mesh.

## Limitations

⚠️ **Read before using on a large selection.**

- **Everything selected is reset to ground level.** Deliberate offsets
  (half-sunk walls, houses on foundations, floating platforms) are lost.
  Select only what you want to snap; `Ctrl+Z` is your friend.
- **The pivot is snapped, not the visible bottom.** If an asset's pivot is at
  its center, the model will appear sunk or floating. Fix on the asset side
  (pivot placement, or an empty parent used as pivot).
- **Children are not moved individually.** Selecting a parent snaps the parent
  only; children follow keeping their relative heights. Select individual
  objects instead.
- **Terrain only.** Mesh-based ground (Mesh Collider) is not supported.
- **Objects outside the Terrain are skipped** and counted in the Console.
- **Overlapping Terrains:** the first one found is used.
- **Heightmap vs. rendered surface.** Small gaps possible on steep slopes
  depending on the Terrain's *Pixel Error* setting.
- **One object, one point.** Large objects (houses, long walls) are snapped
  from their pivot; one side may float or sink on a slope.
- **Terrain objects must be active** in the scene.
- **Editor only.** Does not run in play mode or in builds.

## Contributing

Issues and suggestions are welcome. Possible improvements:
- Per-object vertical offset.
- Snap to lowest/highest ground point under large objects.
- Support for mesh-based ground.

## License

MIT