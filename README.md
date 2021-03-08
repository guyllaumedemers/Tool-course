# 420-J05-SU-OUTILS-2D-3D
 
Each homework has its own repo. Usefull scripts are kept inside the Scripts folder and Editor Components are kept inside the Editor Folder.

*=> ColorWindow isnt made the way it was asked. The color palette gets filed by looking its neighbor that are not equal to the color of the erased color compare to what was asked, which was to fill only the neighbors of the same color. (the algo is a bit different and can only expand to a limit which is set by the initial position click <Initial neighbor>).
 
 *=> Instead of using a recursive pattern to fill neighbor of the same color by checking if the color of the neighbor is equal to the previous color,
 we fill in the grid with a AStar pattern, looking for neighbors until the Array gets emptied and update all tile colors. 
