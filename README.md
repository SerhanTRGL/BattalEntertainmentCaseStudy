Using Unity 6000.3.10f1

Controls:
- Click on enemies with left mouse click to kill them.
- Scroll with mouse wheel to zoom in and out from the grid.
- Hold right mouse button and move the mouse to change view position
- Double click on placed entities to destroy them
- Select a building from the buildings dropdown with left mouse button. Move mouse to a valid grid cell and click left mouse button to place an entity.

Potential Improvements:
- There is no audio.
- Enemies move very flat, and directly towards a target. It could be improved, such as avoiding non-target grid entities.
- Build/Destroy animations are flat.
- Enemies have no animation.
- The camera always focuses on the grid center. Clicking on a grid entity can make the camera focus on that cell.
- Grid entities do have a danger zone. If an enemy is within that zone, an exclamation mark can be shown over the enemy to show the player that game over is imminent.
- Trees are spawned as separate objects, though when painting the terrain, trees can be added to the terrain as well.

Known Issues:
- The game can only be played properly in editor mode. This is because game and UI is separated into different scenes. So the scenes *have to be* loaded in the editor. This can be fixed with a custom scene loader.
- Missing icons in the buildings dropdown. 


Other Notes:
- The grid size is completely customizable in the editor.
- All grid rings respond to the changes in the grid size in the editor.
