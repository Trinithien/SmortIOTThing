import numpy as np
import matplotlib.pyplot as plt

fig = plt.figure(figsize =(10, 10))
ax = fig.add_subplot()

t = np.linspace(0, 2*np.pi, 200)
x = np.cos(t)
y = t*np.sin(t)

ax.plot(x,y, label="A parametric curve")
ax.legend()
plt.show()


ax = plt.figure(figsize =(10, 10))\
.add_subplot(projection="3d")

z = np.sin(5*t)*3*np.cos(t)
ax.plot(x, y, z, label=" A 3d parametric curve")
ax.legend()
plt.show()
