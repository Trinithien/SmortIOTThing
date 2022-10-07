import numpy as np
import matplotlib.pyplot as plt

def f(x, y):
    return (x**3 - 3*x*y**2)

x = np.linspace(-2, 2, 40)
y = np.linspace(-2, 2, 40)

x, y = np.meshgrid(x, y)
X = x
Y = y
Z = f(x, y)

fig = plt.figure(figsize = (10, 10))

ax = plt.axes(projection = '3d')
ax.plot_surface(X, Y, Z, cmap = 'plasma', \
    edgecolor = 'none')

plt.show()
