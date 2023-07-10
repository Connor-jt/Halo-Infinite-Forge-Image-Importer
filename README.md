# Halo-Image-Tool
a macro (& mem editing) tool to import images into halo infinite forge via thousands of little blocks

due to 343 removing full RGB color assignment, this tool utilizes the color intensity to expand the very limited color palette

this means that the images ingame will have very poor accuracy. and for god knows why, the maths does not add up when interpolating between base texture color and overlay color, so its even worse if you aren't using the plastic techsuit base texture

this tool will probably not have a standard built release, as its highly dependent on your hardware. so knowing how to open, modify & run the code will be essential to getting this tool working on your device. 
not only that, but this tool was not tested in release mode, meaning the timings may be off

# usage
recommended image sizes: 32x32 - 96x96 (best to go in the middle with 64x64)

it takes roughly 1.6 seconds per pixel (excluding fully transparent ones) so a full 32x32 image will take around 27 minutes.
removing the background on images is a good way to half the pixel count, and creation time.


a lot of the settings are for testing purposes, but the ideal settings would be using observed colors checked, and techsuit plastic as the base texture. color intensity you'd need to play around with.


MAKE SURE YOUR MENUS ARE CONFIGURED CORRECTLY FOR THE TOOL, OR ELSE IT WILL NOT WORK
- 'r' menu should be the object properties tab
- there should be NO collapsed ui elements in the object properties tab
- position x,y,z, texture 1 swatch & intensity all have to be visible upon opening the object properties menu (you have to scroll down, select color intensity and then close the menu)
- a template pixel MUST be selected before running
- object properties menu MUST be closed before running

ideal operating specs for this to work: 60 FPS, <100ms ping (these were my operational stats during testing)

# credits
- Z (xxZxx) for his work on the initial version of this tool, proving feasibility & providing a foundation to work off of
