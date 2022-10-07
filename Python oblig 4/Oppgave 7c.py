import numpy as np
import matplotlib.pyplot as plt

delta_z = 0.5 # step length in z-direction
x = np.arange(-1, 1, 0.15) #Decides how many points and the range to distribute them on.
y = np.arange(-1, 1, 0.15)
z = np.arange(-1, 1, delta_z)

# 3d point mesh
x, y, z = np.meshgrid(x, y, z)

# the vector field
u = np.sin(x)*np.cos(y) # array with x-coord.
v = np.cos(x+z) # array with y-coord.
w = np.sin(y+z) # array with z-coord.

# set up coordinate system
fig = plt.figure(figsize =(7, 7))
ax = fig.add_subplot(projection="3d")

# plot vector field
ax.quiver(x, y, z, u, v, w, color = "blue", length = 0.2)

# determine interval for plot
ax.axis([-1, 1, -1, 1])
plt.show()