import numpy as np
import matplotlib.pyplot as plt

x = np.linspace(-4.0, 4.0, 100)
y = np.linspace(-4.0, 4.0, 100)
u, v = np.meshgrid(x, y)

w = 1 -u*2 -v*3 -u + 3*v -u*v #Calculates "Z" values ploting 

fig = plt.figure(figsize =(9, 9))
ax = fig.add_subplot()
levels = np.arange(-5, 5, 0.5) # Decides where to draw level lines 0.5 is spacing -5,5 is the range
cont = ax.contour(u, v, w, levels, colors = "black")
plt.show()

